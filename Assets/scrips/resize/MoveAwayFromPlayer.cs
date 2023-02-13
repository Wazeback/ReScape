using UnityEngine;

public class MoveAwayFromPlayer : MonoBehaviour
{
    public float speed = 1.0f;

    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            float distance = Vector3.Distance(transform.position, hit.point) - 0.1f;
            transform.position = transform.position + transform.forward * distance;
        }
        else
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }
    }
}