using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public GameObject dialogBox;
    public GameObject momDialog;
    public GameObject pausePanel;
    public GameObject interactText;
    public GameObject pensoDialog;
    public GameObject jackDialog;
    public GameObject cowMissingLegPanel;
    public GameObject fenceUnlockedPanel;
    public Sprite newCowSprite;

    private bool isCollidingWithMomDialog = false;
    private bool isCollidingWithJackDialog = false;
    private bool isCollidingWithCowMissingLeg = false;
    private bool isCollidingWithFenceUnlocked = false;
    private bool hasInteractedWithCowPanel = false;
    private bool hasInteractedWithFence = false;
    private bool isMomDialogFinished = false; // Ensures MomDialog only appears once
    private Transform puzzleObjectTransform;

    private PlayerMovement playerMovement;
    private PlayerDrawing playerDrawing;
    private bool forceDialogTriggered = false;
    private bool momDialogTriggered = false;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerDrawing = GetComponent<PlayerDrawing>();
    }

    private void Update()
    {
        if (isCollidingWithMomDialog && Input.GetKeyDown(KeyCode.E) && !momDialogTriggered && !isMomDialogFinished)
        {
            StartCoroutine(HandleMomDialog());
        }

        if (isCollidingWithJackDialog && Input.GetKeyDown(KeyCode.E))
        {
            ToggleJackDialog();
        }

        if (isCollidingWithCowMissingLeg && Input.GetKeyDown(KeyCode.E))
        {
            ToggleCowMissingLegPanel();
        }

        if (isCollidingWithFenceUnlocked && Input.GetKeyDown(KeyCode.E) && !hasInteractedWithFence)
        {
            ToggleFenceUnlockedPanel();
            hasInteractedWithFence = true; // Mark interaction as completed
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("MomDialog") && !isMomDialogFinished)
        {
            isCollidingWithMomDialog = true;
            puzzleObjectTransform = collision.transform;
            interactText?.SetActive(true);
            UpdateTextPosition(); // Now updates position correctly
        }

        if (collision.CompareTag("FenceUnlocked") && !hasInteractedWithFence)
        {
            if (!isMomDialogFinished)
            {
                Debug.Log("You must finish the MomDialog before interacting with the fence.");
                return;
            }
            isCollidingWithFenceUnlocked = true;
            puzzleObjectTransform = collision.transform;
            interactText?.SetActive(true);
            UpdateTextPosition();
        }

        if (collision.CompareTag("CowMissingLeg") && !hasInteractedWithCowPanel)
        {
            if (!isMomDialogFinished)
            {
                Debug.Log("You must finish the MomDialog before interacting with the cow.");
                return;
            }
            isCollidingWithCowMissingLeg = true;
            puzzleObjectTransform = collision.transform;
            interactText?.SetActive(true);
            UpdateTextPosition(); // Now updates position correctly
        }

        if (collision.CompareTag("ForceDialog") && !forceDialogTriggered)
        {
            forceDialogTriggered = true;
            pensoDialog?.SetActive(true);
            StartCoroutine(DisableMovementForSeconds(3f, true));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("MomDialog"))
        {
            isCollidingWithMomDialog = false;
            interactText?.SetActive(false);
        }

        if (collision.CompareTag("JackDialog"))
        {
            isCollidingWithJackDialog = false;
            interactText?.SetActive(false);
        }

        if (collision.CompareTag("CowMissingLeg"))
        {
            isCollidingWithCowMissingLeg = false;
            interactText?.SetActive(false);
        }

        if (collision.CompareTag("FenceUnlocked"))
        {
            isCollidingWithFenceUnlocked = false;
            interactText?.SetActive(false);
        }
    }

    private IEnumerator HandleMomDialog()
    {
        momDialogTriggered = true;
        playerMovement?.SetMovement(false);
        interactText?.SetActive(false);
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
        momDialog?.SetActive(true);
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
        momDialog?.SetActive(false);
        playerMovement?.SetMovement(true);
        isMomDialogFinished = true;
        momDialogTriggered = false;
    }

    private IEnumerator DisableMovementForSeconds(float seconds, bool isForceDialog)
    {
        playerMovement?.SetMovement(false);
        yield return new WaitForSeconds(seconds);
        playerMovement?.SetMovement(true);
        if (isForceDialog)
        {
            pensoDialog?.SetActive(false);
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

    public void ToggleDialogBox()
    {
        if (dialogBox != null)
        {
            bool isActive = dialogBox.activeSelf;
            dialogBox.SetActive(!isActive);
        }
    }

    public void TogglePausePanel()
    {
        if (pausePanel != null)
        {
            bool isActive = pausePanel.activeSelf;
            pausePanel.SetActive(!isActive);
            Time.timeScale = isActive ? 1 : 0;
        }
    }

    private void UpdateTextPosition()
    {
        if (puzzleObjectTransform != null && interactText != null)
        {
            interactText.transform.position = puzzleObjectTransform.position + new Vector3(0, 1.3f, 0);
        }
    }

    public void ToggleJackDialog()
    {
        if (jackDialog != null)
        {
            bool isActive = jackDialog.activeSelf;
            jackDialog.SetActive(!isActive);
        }
    }

    public void ToggleCowMissingLegPanel()
    {
        if (!hasInteractedWithCowPanel && cowMissingLegPanel != null)
        {
            bool isActive = cowMissingLegPanel.activeSelf;
            cowMissingLegPanel.SetActive(!isActive);
            Time.timeScale = isActive ? 1 : 0;
        }
    }
    public void ToggleFenceUnlockedPanel()
    {
        if (fenceUnlockedPanel != null)
        {
            bool isActive = fenceUnlockedPanel.activeSelf;
            fenceUnlockedPanel.SetActive(!isActive); // Toggle panel visibility
        }
    }
    public IEnumerator CloseCowMissingLegPanelAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        if (cowMissingLegPanel != null)
        {
            cowMissingLegPanel.SetActive(false);
            Time.timeScale = 1;
            hasInteractedWithCowPanel = true;
        }
    }
    private IEnumerator FadeOutAndDestroy(GameObject obj)
    {
        SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            float fadeDuration = 1f;
            Color color = spriteRenderer.color;

            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                float alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
                spriteRenderer.color = new Color(color.r, color.g, color.b, alpha);
                yield return null;
            }

            spriteRenderer.color = new Color(color.r, color.g, color.b, 0f);
        }

        CloseFenceUnlockedPanel(); // Close panel before destruction
        Destroy(obj);
    }
    public void CloseFenceUnlockedPanel()
    {
        if (fenceUnlockedPanel != null)
        {
            fenceUnlockedPanel.SetActive(false); // Close the panel
        }
    }
}
