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
    public GameObject goHereObject;
    public GameObject skyObject;
    public GameObject beanTreeObject;
    public GameObject cageObject;
    public GameObject markObject; // Assign in the Inspector
    public GameObject tutorialPanel;  // Assign in inspector

    // === Opening Scene ===
    public Dialogue openingSceneDialogue;
    public Dialogue metPensoDialogue;

    public GameObject bookPanel;
    public Animator bookshelfAnimator;

    public string stage01SceneName;

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
    public Dialogue deadEndStage02;

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

    // === Stage 04 ===
    public Dialogue stage04Dialogue;
    public Dialogue sawCastleDialogue;

    // === Stage 05 ===
    public Dialogue enterGiantHouseDialogue;
    public Dialogue holeOnTheWallDialogue;

    public string subStage05SceneName;

    // === Sub_Stage_05 ===
    public Dialogue storageDialogue;
    public Dialogue sawChickenDialogue;
    public Dialogue finishEraseCageDialogue;
    public Dialogue bagOfFogDialogue;

    public GameObject eraseCagePanel;
    public GameObject bagOfFogPanel;

    public string scene06Name;

    // === Stage 06 ===
    public GameObject eraseBagOfFogPanel;
    public Dialogue finishEraseBagOfFogDialogue;

    public GameObject escapeFromGiantPanel;
    public Dialogue escapeFromGiantDialogue;

    public GameObject climbingBeanTreePanel;
    public Dialogue climbingBeanTreeDialogue;

    public GameObject chopBeanTreePanel;
    public Dialogue chopBeanTreeDialogue;

    public string stage07SceneName;

    // === Stage 07 ===
    public Dialogue stage07Dialogue;
    public Dialogue momSixthDialogue;
    public Dialogue momSeventhDialogue;
    public GameObject goldenChickenPanel;
    public string endingSceneName;

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
    private bool isCollidingWithDeadEnd = false;
    private bool hasStartedDeadEndDialogue = false;
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
    private bool isCollidingWithBeanTree = false;


    // === Stage 04 Flags ===
    private bool hasFinishedStage04Dialogue = false;
    private bool hasSeenCastle = false;
    private bool hasFinishedSawCastleDialogue = false;

    // === Stage 05 Flags ===
    private bool hasFinishedGiantHouseDialogue = false;
    private bool hasSeenHoleOnTheWall = false;
    private bool isCollidingWithHole = false;

    // === Sub Stage 05 Flags ===
    private bool hasFinishedStorageDialogue = false;
    private bool hasSeenChicken = false;
    private bool hasFinishedEraseCage = false;
    private bool hasOpenedBagOfFog = false;

    // === Stage 06 Flags ===
    private bool hasFinishedEraseBagOfFog = false;
    private bool hasPlayedEscapeFromGiant = false;

    private bool isCollidingWithBagOfFog = false;

    // === Stage 07 Flags ===
    private bool hasStartedStage07 = false;
    private bool hasTalkedToMomSixth = false;
    private bool hasSeenGoldenChicken = false;
    private bool hasHeardMomSeventh = false;

    private bool isCollidingWithMomStage07 = false;

    // === Repeatable NPC Dialogue ===
    public Dialogue npcDialogue;
    private bool isCollidingWithNPC;
    private bool isNPCDialogueOpen = false;

    private void Start()
    {
        if (fadeOverlay != null)
        {
            fadeOverlay.gameObject.SetActive(true);

            StartCoroutine(FadeFromBlack(() =>
            {
                // === Tutorial Panel (Stage_01 only) ===
                if (tutorialPanel != null)
                {
                    tutorialPanel.SetActive(true);
                    StartCoroutine(HideTutorialThenTriggerOpening());
                }
                else if (openingSceneDialogue != null)
                {
                    StartCoroutine(BeginOpeningSceneFlow());
                }
                else if (stage07Dialogue != null)
                {
                    StartCoroutine(TriggerStage07Dialogue());
                }
                else if (storageDialogue != null)
                {
                    StartCoroutine(TriggerStorageDialogue());
                }
                else if (enterGiantHouseDialogue != null)
                {
                    StartCoroutine(TriggerEnterGiantHouse());
                }
                else if (stage04Dialogue != null)
                {
                    StartCoroutine(TriggerStage04Dialogue());
                }
                else if (stage03Dialogue != null)
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
            }));
        }
        else
        {
            // === Fallback if fadeOverlay is missing ===
            if (tutorialPanel != null)
            {
                tutorialPanel.SetActive(true);
                StartCoroutine(HideTutorialThenTriggerOpening());
            }
            else if (openingSceneDialogue != null)
            {
                StartCoroutine(BeginOpeningSceneFlow());
            }
            else if (enterGiantHouseDialogue != null)
            {
                StartDialogue(enterGiantHouseDialogue, () =>
                {
                    hasFinishedGiantHouseDialogue = true;
                });
            }
            else if (stage04Dialogue != null)
            {
                StartCoroutine(TriggerStage04Dialogue());
            }
            else if (stage03Dialogue != null)
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
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePanel(pausePanel, !pausePanel.activeSelf);
        }
        // === Stage 01 ===
        if (isCollidingWithMom && Input.GetKeyDown(KeyCode.E))
        {
            if (!hasTalkedToMomFirst && hasSeenMom)
            {
                StartDialogue(momFirstDialogue, () =>
                {
                    hasTalkedToMomFirst = true;
                    ShowGoHereAtTarget(GameObject.FindWithTag("BlackFog")?.transform);
                });
            }
            else if (hasCompletedBlackFog && !hasTalkedToMomSecond)
            {
                StartDialogue(momSecondDialogue, () =>
                {
                    hasTalkedToMomSecond = true;
                    GameObject jack = GameObject.FindGameObjectWithTag("Jack");
                    if (jack != null)
                    {
                        ShowGoHereAtTarget(jack.transform);
                    }
                    else
                    {
                        Debug.LogWarning("Jack not found with tag!");
                    }
                });
            }
            else if (hasTalkedToJackSecond && hasCompletedCowMissingLeg && !hasTalkedToMomThird)
            {
                StartDialogue(momThirdDialogue, () =>
                {
                    hasTalkedToMomThird = true;
                    ShowGoHereAtTarget(GameObject.FindWithTag("SceneChangeTrigger")?.transform);
                });
            }
        }

        if (isCollidingWithBlackFog && Input.GetKeyDown(KeyCode.E) && hasTalkedToMomFirst && !hasCompletedBlackFog)
        {
            StartCoroutine(ShowBlackFogPuzzlePanel());
        }

        if (isCollidingWithJack && Input.GetKeyDown(KeyCode.E) && hasTalkedToMomSecond && !hasTalkedToJack)
        {
            StartDialogue(jackFirstDialogue, () =>
            {
                hasTalkedToJack = true;
                ShowGoHereAtTarget(GameObject.FindWithTag("Fence")?.transform);
            });
        }

        if (isCollidingWithFence && Input.GetKeyDown(KeyCode.E) && hasTalkedToJack && !hasCompletedFencePuzzle)
        {
            TogglePanel(fenceUnlockedPanel, true);
        }

        // === Stage 02 ===
        if (isCollidingWithUncle && Input.GetKeyDown(KeyCode.E) && hasCompletedBrokenShelfPuzzle && !hasTalkedToUncle)
        {
            StartDialogue(uncleFirstDialogue, () =>
            {
                hasTalkedToUncle = true;
                ShowGoHereAtTarget(GameObject.FindWithTag("SceneChangeTrigger")?.transform);
            });
        }

        if (isCollidingWithDeadEnd && hasTalkedToUncle && !hasStartedDeadEndDialogue)
        {
            hasStartedDeadEndDialogue = true;
            StartDialogue(deadEndStage02);
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

        if (isCollidingWithBeanTree && Input.GetKeyDown(KeyCode.E) && hasHeardMomFifth)
        {
            ChangeScene();
        }

        // === Stage 05 ===
        if (isCollidingWithHole && Input.GetKeyDown(KeyCode.E) && hasSeenHoleOnTheWall)
        {
            ChangeToSubStage05();
        }

        // === Stage 06 ===
        if (isCollidingWithBagOfFog && Input.GetKeyDown(KeyCode.E) && !hasFinishedEraseBagOfFog)
        {
            if (markObject != null)
                markObject.SetActive(false);
            TogglePanel(eraseBagOfFogPanel, true);
        }

        // === Stage 07 ===
        if (isCollidingWithMomStage07 && Input.GetKeyDown(KeyCode.E) && hasStartedStage07 && !hasTalkedToMomSixth)
        {
            StartDialogue(momSixthDialogue, () =>
            {
                hasTalkedToMomSixth = true;
                TriggerGoldenChickenPanel();
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
            hasSeenMom = true;
            Destroy(collision.gameObject);
            StartDialogue(sawMomDialogue);
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
        if (collision.CompareTag("DeadEnd")) isCollidingWithDeadEnd = true;

        // === Stage 03 ===
        if (collision.CompareTag("Mom")) isCollidingWithMomStage03 = true;

        if (collision.CompareTag("BeanTree"))
        {
            isCollidingWithBeanTree = true;
        }

        // === Stage 04 ===
        if (collision.CompareTag("SawCastle") && !hasSeenCastle)
        {
            if (hasFinishedStage04Dialogue)
            {
                hasSeenCastle = true;
                StartDialogue(sawCastleDialogue, () =>
                {
                    hasFinishedSawCastleDialogue = true;
                });
            }
            else
            {
                Debug.Log("Must finish Stage04Dialogue before triggering castle.");
            }
        }

        // === Stage 05 ===
        if (collision.CompareTag("SawHole") && !hasSeenHoleOnTheWall && hasFinishedGiantHouseDialogue)
        {
            hasSeenHoleOnTheWall = true;
            StartDialogue(holeOnTheWallDialogue);
        }

        if (collision.CompareTag("HoleOnTheWall"))
        {
            isCollidingWithHole = true;
        }

        // === Sub Stage 05 ===
        if (collision.CompareTag("SawChicken") && !hasSeenChicken && hasFinishedStorageDialogue)
        {
            hasSeenChicken = true;
            StartDialogue(sawChickenDialogue, () =>
            {
                if (eraseCagePanel != null)
                {
                    TogglePanel(eraseCagePanel, true);
                }
            });
        }

        // === Stage 06 ===
        if (collision.CompareTag("BagOfFog"))
        {
            isCollidingWithBagOfFog = true;
        }

        if (collision.CompareTag("Mom"))
        {
            isCollidingWithMom = true;
            isCollidingWithMomStage07 = true;
        }

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

        if (collision.CompareTag("BeanTree"))
        {
            isCollidingWithBeanTree = false;
        }
        if (collision.CompareTag("HoleOnTheWall"))
        {
            isCollidingWithHole = false;
        }
        if (collision.CompareTag("BagOfFog"))
        {
            isCollidingWithBagOfFog = false;
        }
        if (collision.CompareTag("Mom"))
        {
            isCollidingWithMom = false;
            isCollidingWithMomStage07 = false;
        }
    }
    private IEnumerator HideTutorialThenTriggerOpening()
{
    yield return new WaitForSecondsRealtime(3f);

    if (tutorialPanel != null)
        tutorialPanel.SetActive(false);

    yield return new WaitForSecondsRealtime(0.3f); 
    StartCoroutine(TriggerOpeningDialogue());
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
        yield return new WaitForSeconds(0.5f);
        StartDialogue(openingDialogue, () =>
        {
            ShowGoHereAtTarget(GameObject.FindWithTag("Mom")?.transform);
        });
    }

    private IEnumerator TriggerStage02Dialogue()
    {
        yield return new WaitForSeconds(0.5f);
        StartDialogue(stage02Dialogue, () =>
        {
            ShowGoHereAtTarget(GameObject.FindWithTag("BrokenShelf")?.transform);
        });
    }

    private IEnumerator TriggerStage03Dialogue()
    {
        yield return new WaitForSeconds(0.5f);
        StartDialogue(stage03Dialogue, () =>
        {
            hasFinishedStage03Dialogue = true;
            ShowGoHereAtTarget(GameObject.FindWithTag("Mom")?.transform);
        });
    }
    private IEnumerator TriggerStage04Dialogue()
    {
        yield return new WaitForSeconds(0.5f);
        StartDialogue(stage04Dialogue, () =>
        {
            hasFinishedStage04Dialogue = true;
            ShowGoHereAtTarget(GameObject.FindWithTag("GiantCastle")?.transform);
        });
    }
    private IEnumerator TriggerEnterGiantHouse()
    {
        yield return new WaitForSeconds(0.5f);
        StartDialogue(enterGiantHouseDialogue, () =>
        {
            hasFinishedGiantHouseDialogue = true;
            ShowGoHereAtTarget(GameObject.FindWithTag("HoleOnTheWall")?.transform);
        });
    }
    private IEnumerator TriggerStorageDialogue()
    {
        yield return new WaitForSeconds(0.5f);
        StartDialogue(storageDialogue, () =>
        {
            hasFinishedStorageDialogue = true;
            ShowGoHereAtTarget(GameObject.FindWithTag("Chicken")?.transform);
        });
    }
    private IEnumerator TriggerStage07Dialogue()
    {
        yield return new WaitForSeconds(0.5f);
        StartDialogue(stage07Dialogue, () =>
        {
            hasStartedStage07 = true;
            ShowGoHereAtTarget(GameObject.FindWithTag("Mom")?.transform);
        });
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
                ShowGoHereAtTarget(GameObject.FindWithTag("BeanTree")?.transform);
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
    public void CompleteEraseBagOfFogPuzzle()
    {
        hasFinishedEraseBagOfFog = true;

        if (eraseBagOfFogPanel != null)
            eraseBagOfFogPanel.SetActive(false);

        StartDialogue(finishEraseBagOfFogDialogue, () =>
        {
            StartCoroutine(FadeBetweenPanels(eraseBagOfFogPanel, escapeFromGiantPanel, () =>
            {
                StartDialogue(escapeFromGiantDialogue, () =>
                {
                    TriggerClimbingBeanTree();
                });
            }));
        });
    }
    private void TriggerEscapeFromGiant()
    {
        if (!hasPlayedEscapeFromGiant && escapeFromGiantPanel != null)
        {
            hasPlayedEscapeFromGiant = true;

            StartCoroutine(FadeInThenShowPanel(escapeFromGiantPanel, () =>
            {
                Time.timeScale = 0f;

                StartDialogue(escapeFromGiantDialogue, () =>
                {
                    StartCoroutine(FadeOutPanelThenBack(escapeFromGiantPanel, () =>
                    {
                        Time.timeScale = 1f;
                        TriggerClimbingBeanTree();
                    }));
                });
            }));
        }
    }

    private void TriggerClimbingBeanTree()
    {
        StartCoroutine(FadeBetweenPanels(escapeFromGiantPanel, climbingBeanTreePanel, () =>
        {
            StartDialogue(climbingBeanTreeDialogue, () =>
            {
                TriggerChopBeanTree();
            });
        }));
    }

    private void TriggerChopBeanTree()
    {
        StartCoroutine(FadeBetweenPanels(climbingBeanTreePanel, chopBeanTreePanel, () =>
        {
            StartDialogue(chopBeanTreeDialogue, () =>
            {
                StartCoroutine(FadeToBlack(() =>
                {
                    SceneManager.LoadScene(stage07SceneName);
                }));
            });
        }));
    }
    private void TriggerGoldenChickenPanel()
    {
        if (!hasSeenGoldenChicken && goldenChickenPanel != null)
        {
            hasSeenGoldenChicken = true;

            StartCoroutine(FadeInThenShowPanel(goldenChickenPanel, () =>
            {
                StartCoroutine(WaitAndCloseGoldenChickenPanel());
            }));
        }
    }

    private IEnumerator WaitAndCloseGoldenChickenPanel()
    {
        yield return new WaitForSecondsRealtime(2f);

        StartCoroutine(FadeOutPanelThenBack(goldenChickenPanel, () =>
        {
            TriggerMomSeventhDialogue();
        }));
    }
    private void TriggerMomSeventhDialogue()
    {
        if (!hasHeardMomSeventh)
        {
            StartDialogue(momSeventhDialogue, () =>
            {
                hasHeardMomSeventh = true;
                StartCoroutine(FadeToBlack(() =>
                {
                    SceneManager.LoadScene(endingSceneName);
                }));
            });
        }
    }


    public void CompleteEraseCagePuzzle()
    {
        hasFinishedEraseCage = true;

        if (eraseCagePanel != null)
            eraseCagePanel.SetActive(false);

        if (cageObject != null)
            cageObject.SetActive(false); 

        StartDialogue(finishEraseCageDialogue, () =>
        {
            TriggerBagOfFogPanel();
        });
    }

    private void TriggerBagOfFogPanel()
    {
        if (hasFinishedEraseCage && !hasOpenedBagOfFog && bagOfFogPanel != null)
        {
            hasOpenedBagOfFog = true;

            StartCoroutine(FadeInThenShowPanel(bagOfFogPanel, () =>
            {
                Time.timeScale = 0f;

                StartDialogue(bagOfFogDialogue, () =>
                {
                    Time.timeScale = 1f;

                    StartCoroutine(FadeToBlack(() =>
                    {
                        LoadScene06(); 
                    }));
                });
            }));
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
        ShowGoHereAtTarget(GameObject.FindWithTag("Uncle")?.transform);
    }

    public void CompleteBlackFog()
    {
        hasCompletedBlackFog = true;
        StartDialogue(finishBlackFogDialogue, () =>
        {
            GameObject mom = GameObject.FindGameObjectWithTag("Mom");
            ShowGoHereAtTarget(mom?.transform);
        });
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
        {
            StartDialogue(jackSecondDialogue, () =>
            {
                hasTalkedToJackSecond = true;
                GameObject mom = GameObject.FindGameObjectWithTag("Mom");
                ShowGoHereAtTarget(mom?.transform);
            });
        }));
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
        if (stage04Dialogue != null && !hasFinishedSawCastleDialogue)
        {
            Debug.Log("You must finish SawCastleDialogue before leaving the scene!");
            return;
        }

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

        if (stage02Dialogue == null && stage03Dialogue == null && stage04Dialogue == null && !hasTalkedToMomThird)
        {
            Debug.Log("You must finish MomThirdDialogue before leaving the scene!");
            return;
        }

        if (stage03Dialogue != null && !hasHeardMomFifth)
        {
            Debug.Log("You must finish MomFifthDialogue before leaving the scene!");
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

        fadeOverlay.gameObject.SetActive(true);
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
    private IEnumerator BeginOpeningSceneFlow()
    {

        if (bookPanel != null)
            bookPanel.SetActive(true);

        StartDialogue(openingSceneDialogue, () =>
        {
            StartCoroutine(FadeOutBookPanel()); 
        });

        yield return null;
    }


    private IEnumerator FadeOutBookPanel()
    {
        yield return new WaitForSecondsRealtime(0.2f);

        if (fadeOverlay != null)
            fadeOverlay.gameObject.SetActive(true);

        float time = 0f;
        Color baseColor = fadeOverlay.color;

        while (time < fadeDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, time / fadeDuration);
            fadeOverlay.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        if (bookPanel != null)
            bookPanel.SetActive(false);

        StartCoroutine(FadeFromBlack(() =>
        {
            StartBookshelfAnimation();
        }));
    }
    private void StartBookshelfAnimation()
    {
        if (bookshelfAnimator != null)
        {
            bookshelfAnimator.gameObject.SetActive(true);
            bookshelfAnimator.Play("BookShelfOpen"); // Make sure this matches your animation name

            // Wait for animation to finish, then trigger next
            StartCoroutine(WaitForBookshelfAnimation());
        }
    }
    private IEnumerator WaitForBookshelfAnimation()
    {
        yield return new WaitForSeconds(bookshelfAnimator.GetCurrentAnimatorStateInfo(0).length + 0.1f);
        yield return new WaitForSecondsRealtime(2f);

        StartDialogue(metPensoDialogue, () =>
        {
            StartCoroutine(GoToStage01());
        });
    }
    private IEnumerator GoToStage01()
    {
        if (fadeOverlay != null)
        {
            fadeOverlay.gameObject.SetActive(true);
            Color color = fadeOverlay.color;
            float time = 0f;

            while (time < fadeDuration)
            {
                float alpha = Mathf.Lerp(0f, 1f, time / fadeDuration);
                fadeOverlay.color = new Color(color.r, color.g, color.b, alpha);
                time += Time.unscaledDeltaTime;
                yield return null;
            }
        }

        SceneManager.LoadScene(stage01SceneName);
    }
    private void ChangeToSubStage05()
    {
        if (!string.IsNullOrEmpty(subStage05SceneName))
        {
            SceneManager.LoadScene(subStage05SceneName);
        }
        else
        {
            Debug.LogWarning("Sub_Stage_05 scene name is not set!");
        }
    }
    private void LoadScene06()
    {
        if (!string.IsNullOrEmpty(scene06Name))
        {
            SceneManager.LoadScene(scene06Name);
        }
        else
        {
            Debug.LogWarning("Scene_06 name is not set!");
        }
    }
    private IEnumerator FadeBetweenPanels(GameObject panelToClose, GameObject panelToOpen, System.Action afterFade = null)
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

       
        if (panelToClose != null) panelToClose.SetActive(false);
        if (panelToOpen != null) panelToOpen.SetActive(true);

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
    public void ShowGoHereAtTarget(Transform target)
    {
        if (goHereObject != null && target != null)
        {
            goHereObject.SetActive(true);

            GoHereFollower follower = goHereObject.GetComponent<GoHereFollower>();
            if (follower != null)
            {
                follower.SetTarget(target);
            }
            else
            {
                Debug.LogWarning("GoHereFollower script not found on GoHereObject.");
            }
        }
    }

    public void HideGoHere()
    {
        if (goHereObject != null)
        {
            GoHereFollower follower = goHereObject.GetComponent<GoHereFollower>();
            if (follower != null)
            {
                follower.ClearTarget();
            }
            else
            {
                goHereObject.SetActive(false);
            }
        }
    }
}
