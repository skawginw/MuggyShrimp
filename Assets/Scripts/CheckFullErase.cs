using UnityEngine;

public class CheckFullErase : MonoBehaviour
{
    public Transform eraserParent; // Assign the "EraserParent"
    public int eraseThreshold = 900; // Adjust based on how many eraser marks are needed
    public Sprite newFenceSprite; // Assign the new sprite in the Inspector
    public BrushEraser eraserScript; // Assign the BrushEraser script (on EraserParent)

    void Update()
    {
        if (eraserParent.childCount >= eraseThreshold)
        {
            ChangeFenceSprites(); // Change all FenceUnlocked sprites
            DestroyEraserMarks(); // Remove all eraser marks
            DisableEraser(); // Disable the eraser script
            Destroy(gameObject); // Destroy the erased sprite

            // Call PlayerInteraction to trigger FenceOpenDialogue automatically
            PlayerInteraction playerInteraction = FindObjectOfType<PlayerInteraction>();
            if (playerInteraction != null)
            {
                playerInteraction.CheckFencePuzzleCompletion();
            }
            else
            {
                Debug.LogWarning("PlayerInteraction script not found in the scene!");
            }
        }
    }

    void ChangeFenceSprites()
    {
        GameObject[] fences = GameObject.FindGameObjectsWithTag("FenceUnlocked");

        foreach (GameObject fence in fences)
        {
            SpriteRenderer fenceRenderer = fence.GetComponent<SpriteRenderer>();
            if (fenceRenderer != null)
            {
                fenceRenderer.sprite = newFenceSprite; // Change sprite
            }
        }
    }

    void DestroyEraserMarks()
    {
        foreach (Transform child in eraserParent)
        {
            Destroy(child.gameObject); // Remove all eraser marks
        }
    }

    void DisableEraser()
    {
        if (eraserScript != null)
        {
            eraserScript.enabled = false; // Disable the eraser script
        }
    }
}
