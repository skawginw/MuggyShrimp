using UnityEngine;
using System.Collections;

public class CheckFullErase : MonoBehaviour
{
    public Transform eraserParent;
    public int eraseThreshold = 900;
    public Sprite newFenceSprite;
    public BrushEraser eraserScript;

    public GameObject blackFogPuzzlePanel;
    public GameObject blackFogObject;
    public float fadeDuration = 2f;

    private bool isFencePuzzle = false;
    private bool isBlackFogPuzzle = false;
    private bool puzzleCompleted = false;
    private int initialEraseCount;

    void Start()
    {
        if (gameObject.CompareTag("ErasableSprite"))
        {
            if (transform.parent != null && transform.parent.CompareTag("LockedBlackFog"))
            {
                isBlackFogPuzzle = true;
            }
            else
            {
                isFencePuzzle = true;
            }
        }

        initialEraseCount = eraserParent.childCount;
    }

    void Update()
    {
        if (!puzzleCompleted && (eraserParent.childCount - initialEraseCount) >= eraseThreshold)
        {
            puzzleCompleted = true;

            if (isFencePuzzle)
            {
                CompleteFencePuzzle();
            }
            else if (isBlackFogPuzzle)
            {
                CompleteBlackFogPuzzle();
            }
        }
    }

    void CompleteFencePuzzle()
    {
        ChangeFenceSprites();
        DestroyEraserMarks();
        DisableEraser();
        Destroy(gameObject);

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

    void CompleteBlackFogPuzzle()
    {
        Debug.Log("BlackFog Puzzle Erased! Fading out BlackFog...");

        DestroyEraserMarks();
        DisableEraser();
        Destroy(gameObject); 

        if (blackFogPuzzlePanel != null)
        {
            blackFogPuzzlePanel.SetActive(false);
        }

        PlayerInteraction playerInteraction = FindObjectOfType<PlayerInteraction>();
        if (playerInteraction != null)
        {
            if (blackFogObject != null)
            {
                playerInteraction.StartCoroutine(FadeOutAndDestroy(blackFogObject, fadeDuration));
            }

            playerInteraction.CompleteBlackFog();
        }
        else
        {
            Debug.LogWarning("PlayerInteraction script not found in the scene!");
        }
    }

    IEnumerator FadeOutAndDestroy(GameObject obj, float duration)
    {
        SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            float startAlpha = spriteRenderer.color.a;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float alpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / duration);
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0f);
        }

        Debug.Log("BlackFog completely faded out, destroying now...");
        Destroy(obj); 
    }

    void ChangeFenceSprites()
    {
        GameObject[] fences = GameObject.FindGameObjectsWithTag("FenceUnlocked");
        foreach (GameObject fence in fences)
        {
            SpriteRenderer fenceRenderer = fence.GetComponent<SpriteRenderer>();
            if (fenceRenderer != null)
            {
                fenceRenderer.sprite = newFenceSprite;
            }
        }
    }

    void DestroyEraserMarks()
    {
        foreach (Transform child in eraserParent)
        {
            Destroy(child.gameObject);
        }
    }

    void DisableEraser()
    {
        if (eraserScript != null)
        {
            eraserScript.enabled = false;
        }
    }
}
