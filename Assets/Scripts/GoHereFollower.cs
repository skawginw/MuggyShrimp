using UnityEngine;

public class GoHereFollower : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 1f, 0);

    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void ClearTarget()
    {
        target = null;
        gameObject.SetActive(false);
    }
}
