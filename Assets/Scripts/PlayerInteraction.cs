using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInteraction : MonoBehaviour
{
    public DialogueManager dialogueManager; // Reference to DialogueManager

    public Dialogue openingDialogue;
    public Dialogue sawMomDialogue;
    public Dialogue momFirstDialogue;
    public Dialogue finishBlackFogDialogue;
    public Dialogue momSecondDialogue;
    public Dialogue jackFirstDialogue;
    public Dialogue jackSecondDialogue;
    public Dialogue fenceOpenDialogue;
    public Dialogue momThirdDialogue;

    public GameObject cowMissingLegPanel;
    public GameObject fenceUnlockedPanel;
    public GameObject blackFogPuzzlePanel;
    public GameObject pausePanel;
    public Sprite newCowSprite;
    public string nextSceneName;

    private bool hasSeenMom;
    private bool hasTalkedToMomFirst;
    private bool hasCompletedBlackFog;
    private bool hasTalkedToMomSecond;
    private bool hasTalkedToJack;
    private bool hasCompletedFencePuzzle;
    private bool hasCompletedCowMissingLeg;
    private bool hasTalkedToMomThird;
    private bool hasTalkedToJackSecond;
    private bool isCollidingWithMom;
    private bool isCollidingWithBlackFog;
    private bool isCollidingWithJack;
    private bool isCollidingWithFence;
    private bool hasTriggeredSceneChange = false;

    private void Start()
    {
        StartCoroutine(TriggerOpeningDialogue());
    }

    private void Update()
    {
        if (isCollidingWithMom && Input.GetKeyDown(KeyCode.E))
        {
            if (!hasTalkedToMomFirst && hasSeenMom)
            {
                StartDialogue(momFirstDialogue, () => hasTalkedToMomFirst = true);
            }
            else if (hasCompletedBlackFog && !hasTalkedToMomSecond)
            {
                StartDialogue(momSecondDialogue, () => hasTalkedToMomSecond = true);
            }
            else if (hasTalkedToJackSecond && hasCompletedCowMissingLeg && !hasTalkedToMomThird)
            {
                StartDialogue(momThirdDialogue, () => hasTalkedToMomThird = true);
            }
        }

        if (isCollidingWithBlackFog && Input.GetKeyDown(KeyCode.E) && hasTalkedToMomFirst && !hasCompletedBlackFog)
        {
            StartCoroutine(ShowBlackFogPuzzlePanel());
        }

        if (isCollidingWithJack && Input.GetKeyDown(KeyCode.E) && hasTalkedToMomSecond && !hasTalkedToJack)
        {
            StartDialogue(jackFirstDialogue, () => hasTalkedToJack = true);
        }

        if (isCollidingWithFence && Input.GetKeyDown(KeyCode.E) && hasTalkedToJack && !hasCompletedFencePuzzle)
        {
            TogglePanel(fenceUnlockedPanel, true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("SawMomDialogue") && !hasSeenMom)
        {
            StartDialogue(sawMomDialogue, () => hasSeenMom = true);
        }
        if (collision.CompareTag("Mom"))
        {
            isCollidingWithMom = true;
        }
        if (collision.CompareTag("BlackFog"))
        {
            isCollidingWithBlackFog = true;
        }
        if (collision.CompareTag("Jack"))
        {
            isCollidingWithJack = true;
        }
        if (collision.CompareTag("Fence"))
        {
            isCollidingWithFence = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Mom"))
        {
            isCollidingWithMom = false;
        }
        if (collision.CompareTag("BlackFog"))
        {
            isCollidingWithBlackFog = false;
        }
        if (collision.CompareTag("Jack"))
        {
            isCollidingWithJack = false;
        }
        if (collision.CompareTag("Fence"))
        {
            isCollidingWithFence = false;
        }
        if (collision.CompareTag("SceneChangeTrigger") && !hasTriggeredSceneChange)
        {
            hasTriggeredSceneChange = true;
            ChangeScene();
        }
    }

    public void CompleteBlackFog()
    {
        hasCompletedBlackFog = true;
        StartDialogue(finishBlackFogDialogue);
    }

    public void CompleteFencePuzzle()
    {
        hasCompletedFencePuzzle = true;
        StartDialogue(fenceOpenDialogue, () => TogglePanel(cowMissingLegPanel, true));
    }

    public void CompleteCowMissingLegPuzzle()
    {
        hasCompletedCowMissingLeg = true;
        StartCoroutine(CloseCowMissingLegPanelAfterDelay(2f, () => StartDialogue(jackSecondDialogue, () => hasTalkedToJackSecond = true)));
        ChangeCowMissingLegSprite();
    }

    public void CheckFencePuzzleCompletion()
    {
        if (!hasCompletedFencePuzzle)
        {
            CompleteFencePuzzle();
        }
    }

    private IEnumerator ShowBlackFogPuzzlePanel()
    {
        TogglePanel(blackFogPuzzlePanel, true);
        yield return new WaitForSecondsRealtime(3f);
        TogglePanel(blackFogPuzzlePanel, false);
        CompleteBlackFog();
    }

    private IEnumerator TriggerOpeningDialogue()
    {
        yield return new WaitForSeconds(1f);
        StartDialogue(openingDialogue);
    }

    private void StartDialogue(Dialogue dialogue, System.Action onComplete = null)
    {
        if (dialogueManager != null && dialogue != null)
        {
            dialogueManager.StartDialogue(dialogue, onComplete);
        }
    }

    public IEnumerator CloseCowMissingLegPanelAfterDelay(float delay, System.Action onComplete = null)
    {
        yield return new WaitForSecondsRealtime(delay);
        if (cowMissingLegPanel != null)
        {
            cowMissingLegPanel.SetActive(false);
            onComplete?.Invoke();
        }
    }

    public void ChangeCowMissingLegSprite()
    {
        GameObject[] cowObjects = GameObject.FindGameObjectsWithTag("CowMissingLeg");
        foreach (GameObject cowObject in cowObjects)
        {
            SpriteRenderer spriteRenderer = cowObject.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null && newCowSprite != null)
            {
                spriteRenderer.sprite = newCowSprite;
            }
        }
    }

    public void TogglePausePanel()
    {
        TogglePanel(pausePanel, !pausePanel.activeSelf);
    }

    private void TogglePanel(GameObject panel, bool state)
    {
        if (panel != null)
        {
            panel.SetActive(state);
            Time.timeScale = state ? 0f : 1f;
        }
    }

    private void ChangeScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("Next scene name is not set!");
        }
    }
}
