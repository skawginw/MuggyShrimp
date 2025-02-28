using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer), typeof(EdgeCollider2D))]
public class DrawPhysicsLine : MonoBehaviour
{
    [SerializeField] private GameObject linePrefab; // Prefab containing LineRenderer, EdgeCollider2D, and Rigidbody2D
    private LineRenderer currentLineRenderer;
    private EdgeCollider2D currentEdgeCollider;
    private Rigidbody2D currentRigidbody;
    private List<Vector2> points = new List<Vector2>();

    [SerializeField] private float minDistance = 0.1f; // Minimum distance between points
    [SerializeField] private float lineWidth = 0.1f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CreateNewLine();
        }

        if (Input.GetMouseButton(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (points.Count == 0 || Vector2.Distance(points[points.Count - 1], mousePos) > minDistance)
            {
                points.Add(mousePos);
                currentLineRenderer.positionCount = points.Count;
                currentLineRenderer.SetPosition(points.Count - 1, mousePos);
                UpdateCollider();
            }
        }
    }

    void CreateNewLine()
    {
        // Instantiate a new line GameObject from the prefab
        GameObject newLine = Instantiate(linePrefab);
        currentLineRenderer = newLine.GetComponent<LineRenderer>();
        currentEdgeCollider = newLine.GetComponent<EdgeCollider2D>();
        currentRigidbody = newLine.GetComponent<Rigidbody2D>();

        points.Clear();
        currentLineRenderer.positionCount = 0;

        // Set LineRenderer properties
        currentLineRenderer.startWidth = lineWidth;
        currentLineRenderer.endWidth = lineWidth;

        // Set Rigidbody2D properties to allow falling
        currentRigidbody.bodyType = RigidbodyType2D.Dynamic;
        currentRigidbody.gravityScale = 1f;
        currentRigidbody.mass = 1f;
    }

    void UpdateCollider()
    {
        if (points.Count > 1)
        {
            currentEdgeCollider.SetPoints(points);
        }
    }
}
