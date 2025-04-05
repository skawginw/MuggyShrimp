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

    public GameObject brokenShelfPuzzlePanel;

    public float fadeDuration = 2f;

    private bool isFencePuzzle = false;
    private bool isBlackFogPuzzle = false;
    private bool isBrokenShelfPuzzle = false;
    private bool puzzleCompleted = false;
    private int initialEraseCount;

    void Start()
    {
        if (gameObject.CompareTag("ErasableSprite"))
        {
            if (transform.parent != null)
            {
                if (transform.parent.CompareTag("LockedBlackFog"))
                {
                    isBlackFogPuzzle = true;
                }
                else if (transform.parent.CompareTag("LockedBrokenShelf"))
                {
                    isBrokenShelfPuzzle = true;
                }
                else
                {
                    isFencePuzzle = true;
                }
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
            else if (isBrokenShelfPuzzle)
            {
                CompleteBrokenShelfPuzzle();
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
    }

    void CompleteBlackFogPuzzle()
    {
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
    }

    void CompleteBrokenShelfPuzzle()
    {
        DestroyEraserMarks();
        DisableEraser();
        puzzleCompleted = true;

        if (brokenShelfPuzzlePanel != null)
        {
            brokenShelfPuzzlePanel.SetActive(false);
        }

        
        if (gameObject != null)
        {
            gameObject.SetActive(false); // or use Destroy(gameObject) if you prefer
        }

        PlayerInteraction playerInteraction = FindObjectOfType<PlayerInteraction>();
        if (playerInteraction != null)
        {
            playerInteraction.CompleteBrokenShelfPuzzle(); 
        }
        else
        {
            Debug.LogWarning("PlayerInteraction not found!");
        }
    }


    IEnumerator FadeOutAndDestroy(GameObject obj, float duration)
    {
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();

        if (sr != null)
        {
            float startAlpha = sr.color.a;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                float alpha = Mathf.Lerp(startAlpha, 0f, elapsed / duration);
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
                elapsed += Time.deltaTime;
                yield return null;
            }

            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0f);
        }

        Destroy(obj);
    }

    void ChangeFenceSprites()
    {
        GameObject[] fences = GameObject.FindGameObjectsWithTag("FenceUnlocked");
        foreach (GameObject fence in fences)
        {
            SpriteRenderer sr = fence.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.sprite = newFenceSprite;
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
