using UnityEngine;

public class EdgeSnapGPT : MonoBehaviour
{
    public float maxDistance = 0.01f;
    public LayerMask layerMask;
    private Vector3 closestPoint;
    private float closestDistance = Mathf.Infinity;

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance, layerMask))
        {
            float distance = Vector3.Distance(transform.position, hit.point);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPoint = hit.point;
            }
        }

        transform.position = closestPoint;
    }
}
