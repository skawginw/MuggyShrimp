using UnityEngine;

public class BrushEraser : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject eraserPrefab; // Prefab for eraser marks
    public Transform eraserParent; // Parent for eraser marks
    public float eraseSpacing = 0.1f; // Spacing between eraser marks
    public float eraserSize = 0.3f; // Size of eraser
    public LayerMask erasableLayer; // Layer containing erasable objects

    private Vector2 lastErasePosition;

    void Update()
    {
        if (Input.GetMouseButton(1)) // Right Click to erase
        {
            Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            // Raycast to detect only objects in the "Erasable" layer
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, erasableLayer);

            if (hit.collider != null)
            {
                // Check if the hit object has the correct tag
                if (hit.collider.gameObject.CompareTag("ErasableSprite"))
                {
                    Debug.Log("Erasable Object Hit: " + hit.collider.gameObject.name);

                    if (Vector2.Distance(lastErasePosition, mousePos) > eraseSpacing)
                    {
                        CreateEraserAt(mousePos);
                        lastErasePosition = mousePos;
                    }
                }
                else
                {
                    Debug.Log("Hit an object, but it's not erasable: " + hit.collider.gameObject.name);
                }
            }
            else
            {
                Debug.Log("No erasable object detected.");
            }
        }
    }

    void CreateEraserAt(Vector2 position)
    {
        GameObject eraser = Instantiate(eraserPrefab, position, Quaternion.identity, eraserParent);
        eraser.transform.localScale = Vector3.one * eraserSize; // Adjust eraser size
    }
}
