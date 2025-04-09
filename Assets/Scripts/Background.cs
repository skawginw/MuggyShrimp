using UnityEngine;

public class Background : MonoBehaviour
{
    private float startPos;
    public GameObject cam;
    public float parallaxEffect;

    void Start()
    {
        startPos = transform.position.x;
    }

    void Update()
    {
        float distance = cam.transform.position.x * parallaxEffect;
        float newX = Mathf.Round((startPos + distance) * 100f) / 100f; // Optional rounding
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }
}
