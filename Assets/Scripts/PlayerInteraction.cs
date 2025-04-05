using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInteraction : MonoBehaviour
{
    public DialogueManager dialogueManager;

    // === Stage 01 ===
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

    // === Stage 02 ===
    public Dialogue stage02Dialogue;
    public Dialogue brokenShelfDialogue;
    public Dialogue finishBrokenShelfDialogue;
    public Dialogue uncleFirstDialogue;

    public GameObject brokenShelfPuzzlePanel;

    // === Common Panels ===
    public GameObject pausePanel;
    public GameObject settingPanel;

    public Sprite newCowSprite;
    public string nextSceneName;

    // === Stage 01 Flags ===
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

    // === Stage 02 Flags ===
    private bool hasSeenBrokenShelfDialogue = false;
    private bool hasCompletedBrokenShelfPuzzle = false;
    private bool isCollidingWithUncle = false;
    private bool hasTalkedToUncle = false;
    public GameObject currentBrokenShelfObject;

    // === Repeatable NPC Dialogue ===
    public Dialogue npcDialogue;
    private bool isCollidingWithNPC;
    private bool isNPCDialogueOpen = false;

    private void Start()
    {
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
        // === Stage 01 ===
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

        // === Stage 02 ===
        if (isCollidingWithUncle && Input.GetKeyDown(KeyCode.E) && hasCompletedBrokenShelfPuzzle && !hasTalkedToUncle)
        {
            StartDialogue(uncleFirstDialogue, () => hasTalkedToUncle = true);
        }

        // === NPC Repeatable ===
        if (isCollidingWithNPC && Input.GetKeyDown(KeyCode.E))
        {
            if (!isNPCDialogueOpen)
            {
                StartDialogue(npcDialogue, () => isNPCDialogueOpen = false);
                isNPCDialogueOpen = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // === Stage 01 ===
        if (collision.CompareTag("SawMomDialogue") && !hasSeenMom)
        {
            StartDialogue(sawMomDialogue, () => hasSeenMom = true);
        }
        if (collision.CompareTag("Mom")) isCollidingWithMom = true;
        if (collision.CompareTag("BlackFog")) isCollidingWithBlackFog = true;
        if (collision.CompareTag("Jack")) isCollidingWithJack = true;
        if (collision.CompareTag("Fence")) isCollidingWithFence = true;

        // === Stage 02 ===
        if (collision.CompareTag("BrokenShelfDialogue") && !hasSeenBrokenShelfDialogue)
        {
            hasSeenBrokenShelfDialogue = true;
            StartDialogue(brokenShelfDialogue, () =>
            {
                TriggerBrokenShelfPuzzle();
                collision.gameObject.SetActive(false);
            });
        }

        if (collision.CompareTag("BrokenShelf"))
        {
            currentBrokenShelfObject = collision.gameObject;
        }

        if (collision.CompareTag("Uncle")) isCollidingWithUncle = true;

        // === NPC ===
        if (collision.CompareTag("NPC")) isCollidingWithNPC = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Mom")) isCollidingWithMom = false;
        if (collision.CompareTag("BlackFog")) isCollidingWithBlackFog = false;
        if (collision.CompareTag("Jack")) isCollidingWithJack = false;
        if (collision.CompareTag("Fence")) isCollidingWithFence = false;

        if (collision.CompareTag("SceneChangeTrigger") && !hasTriggeredSceneChange)
        {
            hasTriggeredSceneChange = true;
            ChangeScene();
        }

        if (collision.CompareTag("Uncle")) isCollidingWithUncle = false;
        if (collision.CompareTag("NPC")) isCollidingWithNPC = false;
    }

    private void StartDialogue(Dialogue dialogue, System.Action onComplete = null)
    {
        if (dialogueManager != null && dialogue != null)
        {
            dialogueManager.StartDialogue(dialogue, onComplete);
        }
    }

    private IEnumerator TriggerOpeningDialogue()
    {
        yield return new WaitForSeconds(1f);
        StartDialogue(openingDialogue);
    }

    private IEnumerator TriggerStage02Dialogue()
    {
        yield return new WaitForSeconds(1f);
        StartDialogue(stage02Dialogue);
    }

    private IEnumerator ShowBlackFogPuzzlePanel()
    {
        TogglePanel(blackFogPuzzlePanel, true);
        yield return new WaitForSecondsRealtime(3f);
    }

    private void TriggerBrokenShelfPuzzle()
    {
        if (brokenShelfPuzzlePanel != null)
        {
            TogglePanel(brokenShelfPuzzlePanel, true);
        }
    }

    public void CompleteBrokenShelfPuzzle()
    {
        hasCompletedBrokenShelfPuzzle = true;

        if (brokenShelfPuzzlePanel != null)
        {
            brokenShelfPuzzlePanel.SetActive(false);
        }

        if (currentBrokenShelfObject != null)
        {
            currentBrokenShelfObject.SetActive(false); 
        }

        StartDialogue(finishBrokenShelfDialogue);
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
        Time.timeScale = 1f;
        StartCoroutine(CloseCowMissingLegPanelAfterDelay(2f, () =>
            StartDialogue(jackSecondDialogue, () => hasTalkedToJackSecond = true)));
        ChangeCowMissingLegSprite();
    }

    public void CheckFencePuzzleCompletion()
    {
        if (!hasCompletedFencePuzzle)
        {
            CompleteFencePuzzle();
        }
    }

    public void CheckBlackFogPuzzleCompletion()
    {
        if (!hasCompletedBlackFog)
        {
            CompleteBlackFog();
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
            SpriteRenderer sr = cowObject.GetComponent<SpriteRenderer>();
            if (sr != null && newCowSprite != null)
            {
                sr.sprite = newCowSprite;
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
        if (stage02Dialogue == null)
        {
            if (!hasTalkedToMomThird)
            {
                Debug.Log("You must finish MomThirdDialogue before leaving the scene!");
                return;
            }
        }
        else
        {
            if (!hasTalkedToUncle)
            {
                Debug.Log("You must finish UncleFirstDialogue before leaving the scene!");
                return;
            }
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
