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
    public Sprite newSprite; // Assign the new sprite in the Inspector for the "SwapUncle" objects

    private bool isCollidingWithMomDialog = false;
    private bool isCollidingWithJackDialog = false;
    private Transform puzzleObjectTransform; // Transform of the object the player interacts with

    private PlayerMovement playerMovement; // Reference to PlayerMovement script
    private bool forceDialogTriggered = false; // One-time use flag for ForceDialog

    private void Start()
    {
        // Get the PlayerMovement component attached to this GameObject
        playerMovement = GetComponent<PlayerMovement>();

        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement script not found on this GameObject!");
        }
    }

    private void Update()
    {
        // Check if the player can interact with the MomDialog
        if (isCollidingWithMomDialog && Input.GetKeyDown(KeyCode.E))
        {
            ToggleMomDialog();
        }

        if (isCollidingWithJackDialog && Input.GetKeyDown(KeyCode.E))
        {
            ToggleJackDialog();
        }

        // Handle dialog box and pause panel toggling
        if (Input.GetMouseButtonDown(0) && CompareTag("DialogButton"))
        {
            ToggleDialogBox();
        }

        if (Input.GetMouseButtonDown(0) && CompareTag("PauseButton"))
        {
            TogglePausePanel();
        }

        // Handle right-click actions
        if (Input.GetMouseButtonDown(1))
        {
            HandleRightClick();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("MomDialog"))
        {
            isCollidingWithMomDialog = true;
            puzzleObjectTransform = collision.transform;

            // Show the "Hold E to interact" message
            if (interactText != null)
            {
                interactText.SetActive(true);
                UpdateTextPosition();
            }

            Debug.Log("Press 'E' to interact.");
        }

        if (collision.CompareTag("JackDialog"))
        {
            isCollidingWithJackDialog = true;
            puzzleObjectTransform = collision.transform;

            // Show the "Hold E to interact" message
            if (interactText != null)
            {
                interactText.SetActive(true);
                UpdateTextPosition();
            }

            Debug.Log("Press 'E' to interact.");
        }

        if (collision.CompareTag("ForceDialog") && !forceDialogTriggered)
        {
            // Set the one-time use flag
            forceDialogTriggered = true;

            puzzleObjectTransform = collision.transform;

            // Show the penso dialog
            if (pensoDialog != null)
            {
                pensoDialog.SetActive(true);
            }

            // Disable player movement for 10 seconds
            StartCoroutine(DisableMovementForSeconds(3f));

            Debug.Log("ForceDialog triggered.");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("MomDialog"))
        {
            isCollidingWithMomDialog = false;

            // Hide the "Hold E to interact" message
            if (interactText != null)
            {
                interactText.SetActive(false);
            }
        }

        if (collision.CompareTag("JackDialog"))
        {
            isCollidingWithJackDialog = false;

            // Hide the "Hold E to interact" message
            if (interactText != null)
            {
                interactText.SetActive(false);
            }
        }
    }

    private void ToggleMomDialog()
    {
        if (momDialog != null)
        {
            bool isActive = momDialog.activeSelf;
            momDialog.SetActive(!isActive); // Toggle the dialog box visibility
        }
        else
        {
            Debug.LogWarning("MomDialog is not assigned!");
        }
    }

    private void ToggleJackDialog()
    {
        if (jackDialog != null)
        {
            bool isActive = jackDialog.activeSelf;
            jackDialog.SetActive(!isActive); // Toggle the dialog box visibility
        }
        else
        {
            Debug.LogWarning("JackDialog is not assigned!");
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
            Time.timeScale = isActive ? 1 : 0; // Toggle time scale for pausing/unpausing
        }
    }

    private void UpdateTextPosition()
    {
        if (puzzleObjectTransform != null && interactText != null)
        {
            // Position the text above the puzzle object (adjust offset as needed)
            interactText.transform.position = puzzleObjectTransform.position + new Vector3(0, 1.3f, 0);
        }
    }

    private IEnumerator DisableMovementForSeconds(float seconds)
    {
        if (playerMovement != null)
        {
            playerMovement.SetMovement(false); // Disable player movement
        }

        Debug.Log($"Movement disabled for {seconds} seconds.");
        yield return new WaitForSeconds(seconds);

        if (playerMovement != null)
        {
            playerMovement.SetMovement(true); // Re-enable player movement
        }

        Debug.Log("Movement re-enabled.");

        if (pensoDialog != null)
        {
            pensoDialog.SetActive(false);
        }
    }

    private void HandleRightClick()
    {
        // Get mouse position in world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Use Physics2D.OverlapPoint to detect the object under the mouse cursor
        Collider2D hitCollider = Physics2D.OverlapPoint(new Vector2(mousePosition.x, mousePosition.y));

        if (hitCollider != null)
        {
            if (hitCollider.CompareTag("Shadow"))
            {
                StartCoroutine(FadeOutAndDestroy(hitCollider.gameObject));
                Debug.Log("Shadow object fading out and destroyed.");
            }
            else if (hitCollider.CompareTag("SwapUncle"))
            {
                ChangeSprite(hitCollider.gameObject);
                Debug.Log("SwapUncle object sprite changed.");
            }
        }
        else
        {
            Debug.Log("No object found under the mouse.");
        }
    }

    private void ChangeSprite(GameObject obj)
    {
        SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();

        if (spriteRenderer != null && newSprite != null)
        {
            spriteRenderer.sprite = newSprite;
        }
        else
        {
            Debug.LogWarning("SpriteRenderer or newSprite is missing!");
        }
    }

    private IEnumerator FadeOutAndDestroy(GameObject obj)
    {
        SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            // Gradually reduce the alpha value of the object's color
            float fadeDuration = 1f; // Duration of fade in seconds
            Color color = spriteRenderer.color;

            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                float alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
                spriteRenderer.color = new Color(color.r, color.g, color.b, alpha);
                yield return null; // Wait until the next frame
            }

            // Ensure the object is fully transparent
            spriteRenderer.color = new Color(color.r, color.g, color.b, 0f);
        }

        // Destroy the object
        Destroy(obj);
    }
}
