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

    private bool isCollidingWithMomDialog = false;
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

        // Handle dialog box and pause panel toggling
        if (Input.GetMouseButtonDown(0) && CompareTag("DialogButton"))
        {
            ToggleDialogBox();
        }

        if (Input.GetMouseButtonDown(0) && CompareTag("PauseButton"))
        {
            TogglePausePanel();
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
            StartCoroutine(DisableMovementForSeconds(10f));

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
            interactText.transform.position = puzzleObjectTransform.position + new Vector3(0, 1.2f, 0);
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
}
