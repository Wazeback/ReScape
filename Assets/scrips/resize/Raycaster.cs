using UnityEngine;

public class Raycaster : MonoBehaviour
{
    public float maxDistance = 10.0f;
    public Transform objectTransform;
    private Vector3[] rayDirections = new Vector3[] { 
        Vector3.forward,
        Vector3.back,
        Vector3.left,
        Vector3.right,
        Vector3.up,
        Vector3.down
    };

    // Update is called once per frame
    void Update()
    {
        foreach (Vector3 direction in rayDirections)
        {
            Ray ray = new Ray(objectTransform.position, direction);
            Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.red);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, maxDistance))
            {
                Debug.DrawRay(hit.point, Vector3.up, Color.green);
            }
        }
    }
}