using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class PlayerInteraction : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public Image fadeOverlay;
    public float fadeDuration = 1f;
    public GameObject skyObject;
    public GameObject beanTreeObject;

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

    // === Stage 03 ===
    public Dialogue stage03Dialogue;
    public Dialogue momFourthDialogue;
    public Dialogue throwingBeansDialogue;
    public Dialogue jackThirdDialogue;
    public Dialogue jackFourthDialogue;
    public Dialogue momFifthDialogue;

    public GameObject throwingBeansPanel;
    public GameObject skipTheDayPanel;
    public GameObject skipToDayPanel;

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

    // === Stage 03 Flags ===
    private bool hasFinishedStage03Dialogue = false;
    private bool hasTalkedToMomFourth = false;
    private bool hasSeenThrowingBeans = false;
    private bool hasFinishedThrowingBeansDialogue = false;
    private bool hasHeardJackThird = false;
    private bool hasSkippedTheDay = false;
    private bool hasHeardJackFourth = false;
    private bool hasHeardMomFifth = false;
    private bool isCollidingWithMomStage03;

    // === Repeatable NPC Dialogue ===
    public Dialogue npcDialogue;
    private bool isCollidingWithNPC;
    private bool isNPCDialogueOpen = false;

    private void Start()
    {
        if (stage03Dialogue != null)
        {
            StartCoroutine(TriggerStage03Dialogue());
        }
        else if (stage02Dialogue != null)
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

        // === Stage 03 ===
        if (isCollidingWithMomStage03 && Input.GetKeyDown(KeyCode.E) && hasFinishedStage03Dialogue && !hasTalkedToMomFourth)
        {
            StartDialogue(momFourthDialogue, () =>
            {
                hasTalkedToMomFourth = true;
                TriggerThrowingBeansPanel();
            });
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

        // === Stage 03 ===
        if (collision.CompareTag("Mom")) isCollidingWithMomStage03 = true;

        // === NPC ===
        if (collision.CompareTag("NPC")) isCollidingWithNPC = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Mom"))
        {
            isCollidingWithMom = false;
            isCollidingWithMomStage03 = false;
        }
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

    private IEnumerator TriggerStage03Dialogue()
    {
        yield return new WaitForSeconds(1f);
        StartDialogue(stage03Dialogue, () => hasFinishedStage03Dialogue = true);
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

    private void TriggerThrowingBeansPanel()
    {
        if (throwingBeansPanel != null && !hasSeenThrowingBeans)
        {
            hasSeenThrowingBeans = true;

            StartCoroutine(FadeInThenShowPanel(throwingBeansPanel, () =>
            {
                Time.timeScale = 0f;

                StartDialogue(throwingBeansDialogue, () =>
                {
                    StartCoroutine(FadeOutPanelThenBack(throwingBeansPanel, () =>
                    {
                        Time.timeScale = 1f;
                        hasFinishedThrowingBeansDialogue = true;
                        TriggerJackThirdDialogue();
                    }));
                });
            }));
        }
    }


    private void TriggerJackThirdDialogue()
    {
        if (!hasHeardJackThird && hasFinishedThrowingBeansDialogue)
        {
            StartDialogue(jackThirdDialogue, () =>
            {
                hasHeardJackThird = true;
                ShowSkipTheDayPanel();
            });
        }
    }

    private void ShowSkipTheDayPanel()
    {
        if (skipTheDayPanel != null && !hasSkippedTheDay)
        {
            hasSkippedTheDay = true;

            StartCoroutine(FadeInThenShowPanel(skipTheDayPanel, () =>
            {
                StartCoroutine(DelayedActivateSkipToDayPanel());

                StartCoroutine(CloseSkipPanelThenFadeToSkipToDay());
            }));
        }
    }
    private IEnumerator DelayedActivateSkipToDayPanel()
    {
        yield return new WaitForSecondsRealtime(1.5f);

        if (skipToDayPanel != null)
        {
            skipToDayPanel.SetActive(true);
        }
    }

    private void TriggerSkipToDayPanel()
    {
        if (skipToDayPanel != null)
        {
            skipToDayPanel.SetActive(true);

            ActivateSkyObject(); 

            StartCoroutine(CloseSkipToDayPanelSequence());
        }
    }
    private IEnumerator CloseSkipToDayPanelSequence()
    {
        yield return new WaitForSecondsRealtime(2f);

        StartCoroutine(FadeOutPanelThenBack(skipToDayPanel, () =>
        {
            ActivateSkyObject();

            StartDialogue(jackFourthDialogue, () =>
            {
                hasHeardJackFourth = true;
                TriggerMomFifthDialogue();
            });
        }));
    }

    private IEnumerator CloseSkipPanelThenFadeToSkipToDay()
    {
        yield return new WaitForSecondsRealtime(2f);

        StartCoroutine(FadeToBlack(() =>
        {
            skipTheDayPanel.SetActive(false);

            if (skipToDayPanel != null) skipToDayPanel.SetActive(true);
            ActivateSkyObject();

            StartCoroutine(FadeFromBlack(() =>
            {
                StartCoroutine(CloseSkipToDayPanelSequence());
            }));
        }));
    }


    private void TriggerMomFifthDialogue()
    {
        if (!hasHeardMomFifth && hasHeardJackFourth)
        {
            StartDialogue(momFifthDialogue, () =>
            {
                hasHeardMomFifth = true;
            });
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
        if (stage03Dialogue != null && !hasHeardMomFifth)
        {
            Debug.Log("You must finish MomFifthDialogue before leaving the scene!");
            return;
        }

        if (stage02Dialogue != null && !hasTalkedToUncle)
        {
            Debug.Log("You must finish UncleFirstDialogue before leaving the scene!");
            return;
        }

        if (stage02Dialogue == null && stage03Dialogue == null && !hasTalkedToMomThird)
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
    private IEnumerator FadeOutPanelThenBack(GameObject panelToClose, Action afterFade = null)
    {
        if (fadeOverlay == null) yield break;

        fadeOverlay.gameObject.SetActive(true);
        Color baseColor = fadeOverlay.color;

        // Fade to black
        float time = 0f;
        while (time < fadeDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, time / fadeDuration);
            fadeOverlay.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
            time += Time.unscaledDeltaTime;
            yield return null;
        }
        fadeOverlay.color = new Color(baseColor.r, baseColor.g, baseColor.b, 1f);

        // Now close the panel while screen is black
        if (panelToClose != null)
            panelToClose.SetActive(false);

        // Optional pause at full black
        yield return new WaitForSecondsRealtime(0.1f);

        // Fade back to clear
        time = 0f;
        while (time < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, time / fadeDuration);
            fadeOverlay.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        fadeOverlay.color = new Color(baseColor.r, baseColor.g, baseColor.b, 0f);
        fadeOverlay.gameObject.SetActive(false);

        afterFade?.Invoke();
    }
    private IEnumerator FadeToBlack(System.Action onFadeComplete = null)
    {
        if (fadeOverlay == null) yield break;

        fadeOverlay.gameObject.SetActive(true);
        Color baseColor = fadeOverlay.color;

        float time = 0f;
        while (time < fadeDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, time / fadeDuration);
            fadeOverlay.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        fadeOverlay.color = new Color(baseColor.r, baseColor.g, baseColor.b, 1f);
        onFadeComplete?.Invoke();
    }
    private void ActivateSkyObject()
    {
        if (skyObject != null)
        {
            skyObject.SetActive(true);
            Debug.Log("Sky activated!");
        }
        else
        {
            Debug.LogWarning("Sky object not assigned.");
        }

        if (beanTreeObject != null)
        {
            beanTreeObject.SetActive(true);
            Debug.Log("BeanTree activated!");
        }
        else
        {
            Debug.LogWarning("BeanTree object not assigned.");
        }
    }

    private IEnumerator FadeFromBlack(System.Action afterFade = null)
    {
        if (fadeOverlay == null) yield break;

        Color baseColor = fadeOverlay.color;
        float time = 0f;

        while (time < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, time / fadeDuration);
            fadeOverlay.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        fadeOverlay.color = new Color(baseColor.r, baseColor.g, baseColor.b, 0f);
        fadeOverlay.gameObject.SetActive(false);

        afterFade?.Invoke();
    }
    private IEnumerator FadeInThenShowPanel(GameObject panelToShow, Action afterReveal = null)
    {
        if (fadeOverlay == null) yield break;

        fadeOverlay.gameObject.SetActive(true);
        Color baseColor = fadeOverlay.color;

        // Fade to black
        float time = 0f;
        while (time < fadeDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, time / fadeDuration);
            fadeOverlay.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        fadeOverlay.color = new Color(baseColor.r, baseColor.g, baseColor.b, 1f);

        // Activate panel while screen is black
        if (panelToShow != null)
        {
            panelToShow.SetActive(true);
        }

        yield return new WaitForSecondsRealtime(0.1f); // brief pause

        // Fade out to reveal the panel
        time = 0f;
        while (time < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, time / fadeDuration);
            fadeOverlay.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        fadeOverlay.color = new Color(baseColor.r, baseColor.g, baseColor.b, 0f);
        fadeOverlay.gameObject.SetActive(false);

        afterReveal?.Invoke();
    }

}
