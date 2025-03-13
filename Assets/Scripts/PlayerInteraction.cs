using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public GameObject dialogBox;
    public GameObject openingDialogue;
    public GameObject sawMomDialogue;
    public GameObject momFirstDialogue;
    public GameObject finishBlackFogDialogue;
    public GameObject momSecondDialogue;
    public GameObject jackFirstDialogue;
    public GameObject jackSecondDialogue;
    public GameObject fenceOpenDialogue;
    public GameObject cowMissingLegPanel;
    public GameObject momThirdDialogue;
    public GameObject fenceUnlockedPanel;
    public GameObject blackFogPuzzlePanel;
    public GameObject pausePanel;
    public Sprite newCowSprite;

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
                StartCoroutine(TriggerDialogue(momFirstDialogue, () => hasTalkedToMomFirst = true));
            }
            else if (hasCompletedBlackFog && !hasTalkedToMomSecond)
            {
                StartCoroutine(TriggerDialogue(momSecondDialogue, () => hasTalkedToMomSecond = true));
            }
            else if (hasTalkedToJackSecond && hasCompletedCowMissingLeg && !hasTalkedToMomThird)
            {
                StartCoroutine(TriggerDialogue(momThirdDialogue, () => hasTalkedToMomThird = true));
            }
        }

        if (isCollidingWithBlackFog && Input.GetKeyDown(KeyCode.E) && hasTalkedToMomFirst && !hasCompletedBlackFog)
        {
            StartCoroutine(ShowBlackFogPuzzlePanel());
        }

        if (isCollidingWithJack && Input.GetKeyDown(KeyCode.E) && hasTalkedToMomSecond && !hasTalkedToJack)
        {
            StartCoroutine(TriggerDialogue(jackFirstDialogue, () => hasTalkedToJack = true));
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
            StartCoroutine(TriggerDialogue(sawMomDialogue));
            hasSeenMom = true;
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
    }

    public void CompleteBlackFog()
    {
        hasCompletedBlackFog = true;
        StartCoroutine(TriggerDialogue(finishBlackFogDialogue));
    }

    public void CompleteFencePuzzle()
    {
        hasCompletedFencePuzzle = true;
        StartCoroutine(TriggerDialogue(fenceOpenDialogue, () => TogglePanel(cowMissingLegPanel, true)));
    }

    public void CompleteCowMissingLegPuzzle()
    {
        hasCompletedCowMissingLeg = true;
        StartCoroutine(CloseCowMissingLegPanelAfterDelay(2f, () => StartCoroutine(TriggerDialogue(jackSecondDialogue, () => hasTalkedToJackSecond = true))));
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
        yield return new WaitForSeconds(1f); // Small delay before triggering
        yield return TriggerDialogue(openingDialogue);
    }

    private IEnumerator TriggerDialogue(GameObject dialogue, System.Action onComplete = null)
    {
        if (dialogue != null)
        {
            Time.timeScale = 0f; // Stop time
            dialogue.SetActive(true);
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            dialogue.SetActive(false);
            Time.timeScale = 1f; // Resume time
            onComplete?.Invoke();
        }
    }

    public IEnumerator CloseCowMissingLegPanelAfterDelay(float delay, System.Action onComplete = null)
    {
        yield return new WaitForSecondsRealtime(delay);
        if (cowMissingLegPanel != null)
        {
            cowMissingLegPanel.SetActive(false);
            Time.timeScale = 0f;
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
            Time.timeScale = state ? 0f : 1f; // Stop time when opening, resume when closing
        }
    }
}
