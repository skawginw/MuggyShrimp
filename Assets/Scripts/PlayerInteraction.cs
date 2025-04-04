using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInteraction : MonoBehaviour
{ // Stage 01 Dialogues and Panels
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

    // Stage 02 Dialogues and Panels
    public Dialogue stage02Dialogue;
    public Dialogue brokenShelfDialogue;
    public Dialogue finishBrokenShelfDialogue;
    public Dialogue uncleFirstDialogue;

    public GameObject brokenShelfPuzzlePanel;

    // Common Panels
    public GameObject pausePanel;
    public GameObject settingPanel;

    public Sprite newCowSprite;
    public string nextSceneName;

    // Stage 01 Flags
    private bool hasSeenMom;
    private bool hasTalkedToMomFirst;
    private bool hasCompletedBlackFog = false;
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

    // Stage 02 Flags
    private bool hasSeenBrokenShelfDialogue = false;
    private bool hasCompletedBrokenShelfPuzzle = false;
    private bool isCollidingWithBrokenShelf = false;
    private bool isCollidingWithUncle = false;
    private bool hasTalkedToUncle = false;

    public DialogueManager dialogueManager; // Reference to DialogueManager

    private void Start()
    {
        // If Stage02Dialogue is assigned, assume we're in Stage 02; otherwise, run Stage 01 flow.
        if (stage02Dialogue != null)
        {
            StartCoroutine(TriggerStage02Dialogue());
        }
        else
        {
            StartCoroutine(TriggerOpeningDialogue());
        }
    }

    private void Update()
    {
        // --- Stage 01 interactions ---
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

        // --- Stage 02 interactions ---
        if (isCollidingWithBrokenShelf && Input.GetKeyDown(KeyCode.E) && hasSeenBrokenShelfDialogue && !hasCompletedBrokenShelfPuzzle)
        {
            TogglePanel(brokenShelfPuzzlePanel, true);
        }

        if (isCollidingWithUncle && Input.GetKeyDown(KeyCode.E) && hasCompletedBrokenShelfPuzzle && !hasTalkedToUncle)
        {
            StartDialogue(uncleFirstDialogue, () => hasTalkedToUncle = true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // --- Stage 01 triggers ---
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

        // --- Stage 02 triggers ---
        if (collision.CompareTag("BrokenShelfDialogue") && !hasSeenBrokenShelfDialogue)
        {
            StartDialogue(brokenShelfDialogue, () => hasSeenBrokenShelfDialogue = true);
        }
        if (collision.CompareTag("BrokenShelf"))
        {
            isCollidingWithBrokenShelf = true;
        }
        if (collision.CompareTag("Uncle"))
        {
            isCollidingWithUncle = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // --- Stage 01 exit triggers ---
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

        // --- Stage 02 exit triggers ---
        if (collision.CompareTag("BrokenShelf"))
        {
            isCollidingWithBrokenShelf = false;
        }
        if (collision.CompareTag("Uncle"))
        {
            isCollidingWithUncle = false;
        }
    }

    // --- Stage 01 methods ---
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
        Time.timeScale = 1f;
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
    }

    public void CheckBlackFogPuzzleCompletion()
    {
        if (!hasCompletedBlackFog)
        {
            CompleteBlackFog();
        }
    }

    private IEnumerator TriggerOpeningDialogue()
    {
        yield return new WaitForSeconds(1f);
        StartDialogue(openingDialogue);
    }

    // --- Stage 02 methods ---
    private IEnumerator TriggerStage02Dialogue()
    {
        yield return new WaitForSeconds(1f);
        StartDialogue(stage02Dialogue);
    }

    // Call this from your BrokenShelf puzzle script when the puzzle is finished.
    public void CompleteBrokenShelfPuzzle()
    {
        hasCompletedBrokenShelfPuzzle = true;
        StartDialogue(finishBrokenShelfDialogue);
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

    public void ToggleSettingPanel()
    {
        TogglePanel(settingPanel, !settingPanel.activeSelf);
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
        if (!hasTalkedToMomThird)
        {
            Debug.Log("You must finish MomThirdDialogue before leaving the scene!");
            return;
        }
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