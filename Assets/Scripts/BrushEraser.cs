using UnityEngine;

public class BrushEraser : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject eraserPrefab;
    public Transform eraserParent;
    public float eraseSpacing = 0.1f;
    public float eraserSize = 0.3f; // Default size
    public float minSize = 0.1f, maxSize = 1f; // Eraser size limits
    public float sizeChangeSpeed = 0.1f; // How fast size changes

    private Vector2 lastErasePosition;

    void Update()
{
    if (!enabled) return; // Stop erasing if the script is disabled

    if (Input.GetMouseButton(1)) // Hold Right Click
    {
        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        if (Vector2.Distance(lastErasePosition, mousePos) > eraseSpacing)
        {
            CreateEraserAt(mousePos);
            lastErasePosition = mousePos;
        }
    }
}

    void CreateEraserAt(Vector2 position)
    {
        GameObject eraser = Instantiate(eraserPrefab, position, Quaternion.identity, eraserParent);
        eraser.transform.localScale = Vector3.one * eraserSize; // Set dynamic eraser size
    }
}
