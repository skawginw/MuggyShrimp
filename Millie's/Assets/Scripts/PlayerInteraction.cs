using UnityEngine;
using TMPro; // Use TextMeshPro for better text rendering

public class PlayerInteraction : MonoBehaviour
{
    public GameObject dialogBox;
    public GameObject puzzlePopup; 
    public GameObject interactText; 
    private bool isCollidingWithPuzzleObject = false; // Track collision status
    private Transform puzzleObjectTransform; // The transform of the puzzle object

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && CompareTag("DialogButton"))
        {
            ToggleDialogBox();
        }
        
        if (isCollidingWithPuzzleObject && Input.GetKeyDown(KeyCode.E))
        {
            TogglePuzzlePopup();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collided object is the puzzle object
        if (collision.CompareTag("PuzzleObject"))
        {
            isCollidingWithPuzzleObject = true;
            puzzleObjectTransform = collision.transform;

            // Show the "Hold E to interact" message
            if (interactText != null)
            {
                interactText.SetActive(true);
                UpdateTextPosition();
            }

            // Optional: Debug log for collision
            Debug.Log("Press 'E' to interact.");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Reset when the player moves away from the puzzle object
        if (collision.CompareTag("PuzzleObject"))
        {
            isCollidingWithPuzzleObject = false;

            // Hide the "Hold E to interact" message when leaving the collision area
            if (interactText != null)
            {
                interactText.SetActive(false);
            }
        }
    }

    private void TogglePuzzlePopup()
    {
        if (puzzlePopup != null)
        {
            bool isActive = puzzlePopup.activeSelf;
            puzzlePopup.SetActive(!isActive); // Toggle the popup visibility
        }
        else
        {
            Debug.LogWarning("Puzzle popup is not assigned!");
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

        // Update the position of the interact text above the puzzle object
        private void UpdateTextPosition()
    {
        if (puzzleObjectTransform != null && interactText != null)
        {
            // Position the text above the puzzle object (adjust offset as needed)
            interactText.transform.position = puzzleObjectTransform.position + new Vector3(0, 0.7f, 0);
        }
    }
}
