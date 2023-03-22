using UnityEngine;

public class DoorController : MonoBehaviour
{
    private HingeJoint hingeJointDoor;
    private bool isOpen = false;
    public Transform mainCam;
    public int dist;
    
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            RaycastHit hit; // Cast a ray from the cursor position using the camera's forward direction.
            if (Physics.Raycast(mainCam.position, mainCam.TransformDirection(Vector3.forward), out hit, dist) && hit.transform.CompareTag("ToggleDoor")) { // Check if the ray hits an object within the maximum distance.
                hingeJointDoor = hit.transform.GetComponent<HingeJoint>();
                JointMotor motor = hingeJointDoor.motor;
                motor.force = 1000f;
                motor.targetVelocity = isOpen ? -100f : 100f;
                hingeJointDoor.motor = motor;
                isOpen = !isOpen;
            }
        }
    }

}