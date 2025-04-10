using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDrawing : MonoBehaviour
{
    public GameObject[] dots; // Ordered dots (first to last)
    public GameObject filledSprite; // Final completed sprite
    public LineRenderer lineRenderer; // Add a LineRenderer to the player object

    private int currentDotIndex = 0; // Progress tracker
    private bool isDrawing = false;
    private bool hasStartedCoroutine = false;
    private List<Vector3> drawPoints = new List<Vector3>(); // To hold the drawn points
    private PlayerInteraction playerInteraction; // Reference to PlayerInteraction script

    void Start()
    {
        playerInteraction = FindObjectOfType<PlayerInteraction>();

        if (playerInteraction == null)
        {
            Debug.LogError("PlayerInteraction script not found!");
        }

        lineRenderer.positionCount = 0;
        lineRenderer.useWorldSpace = true;

        // Set default material to prevent transparency issues
        if (lineRenderer.material == null)
        {
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        }

        // Set color & opacity
        lineRenderer.startColor = new Color(1f, 0f, 0f, 1f); // Red
        lineRenderer.endColor = new Color(1f, 0f, 0f, 1f);
        lineRenderer.material.color = Color.black; // Ensure material uses color

        // Adjust width and sorting order
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.sortingOrder = 10; // Bring it to the front
    }

    void Update()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        if (Input.GetMouseButtonDown(0))
        {
            if (currentDotIndex == 0 && IsMouseOverDot(mouseWorldPos, 0))
            {
                StartDrawing(mouseWorldPos);
            }
        }

        if (Input.GetMouseButton(0) && isDrawing)
        {
            if (currentDotIndex < dots.Length && IsMouseOverDot(mouseWorldPos, currentDotIndex))
            {
                // Correctly reached the next dot
                AddPointToLine(dots[currentDotIndex].transform.position);
                HideDot(currentDotIndex);
                currentDotIndex++;
            }
            else if (TouchedWrongDot(mouseWorldPos))
            {
                // Player touched a wrong dot — reset
                ResetDrawing();
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            isDrawing = false;

            if (currentDotIndex >= dots.Length)
            {
                CompleteDrawing();
            }
            else
            {
                ResetDrawing(); // Restart the puzzle if incomplete
            }
        }

    }

    void StartDrawing(Vector2 startPos)
    {
        isDrawing = true;
        drawPoints.Clear();
        lineRenderer.positionCount = 0;
        lineRenderer.enabled = true; // Make sure line appears when starting
        AddPointToLine(dots[0].transform.position); // Start line at first dot
        HideDot(0); // Hide first dot
        currentDotIndex = 1; // Move to next
    }

    void AddPointToLine(Vector3 point)
    {
        point.z = 0f;
        if (drawPoints.Count == 0 || Vector3.Distance(drawPoints[drawPoints.Count - 1], point) > 0.1f)
        {
            drawPoints.Add(point);
            lineRenderer.positionCount = drawPoints.Count;
            lineRenderer.SetPositions(drawPoints.ToArray());
        }
    }

    bool IsMouseOverDot(Vector2 mousePos, int dotIndex)
    {
        Collider2D col = dots[dotIndex].GetComponent<Collider2D>();
        return col != null && col.OverlapPoint(mousePos);
    }

    bool TouchedWrongDot(Vector2 mousePos)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            if (i != currentDotIndex && dots[i].activeSelf && IsMouseOverDot(mousePos, i))
            {
                return true; // Player touched out of order
            }
        }
        return false;
    }

    void HideDot(int index)
    {
        dots[index].SetActive(false);
    }

    void ResetDrawing()
    {
        // Show all dots again
        foreach (GameObject dot in dots)
        {
            dot.SetActive(true);
        }

        // Reset line
        lineRenderer.positionCount = 0;
        lineRenderer.enabled = false; // Hide line on reset
        drawPoints.Clear();

        currentDotIndex = 0;
        isDrawing = false;
    }

    void CompleteDrawing()
    {
        if (currentDotIndex >= dots.Length && !hasStartedCoroutine)
        {
            filledSprite.SetActive(true);
            lineRenderer.enabled = false; // Hide the line when complete
            foreach (GameObject dot in dots)
            {
                dot.SetActive(false); // Hide all dots after completion
            }

            Debug.Log("Drawing complete, changing CowMissingLeg sprite and closing panel.");

            hasStartedCoroutine = true;

            // Change sprite on 'CowMissingLeg'
            if (playerInteraction != null)
            {
                playerInteraction.ChangeCowMissingLegSprite();
                playerInteraction.CompleteCowMissingLegPuzzle();
            }
            else
            {
                Debug.LogWarning("PlayerInteraction reference is missing!");
            }

            // Start coroutine to close panel after delay
            StartCoroutine(playerInteraction.CloseCowMissingLegPanelAfterDelay(2f));
        }
    }
    public bool IsDrawingComplete()
    {
        return currentDotIndex >= dots.Length; // Returns true if all dots are connected
    }
}