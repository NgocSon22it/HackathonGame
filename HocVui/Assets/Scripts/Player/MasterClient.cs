using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterClient : MonoBehaviour
{
    private bool isMoving = false; // Flag to check if the object is currently moving
    private float minX = -14f; // Minimum X position
    private float maxX = 9f;  // Maximum X position
    private float targetX;     // Stores the target X position for the object

    [SerializeField] GameObject MasterCamera;

    GameObject CameraInstance;
    private void Start()
    {    
        CameraInstance = Instantiate(MasterCamera);
        CameraInstance.GetComponent<CinemachineVirtualCamera>().m_Follow = gameObject.transform;
        CameraInstance.GetComponent<CinemachineConfiner2D>().m_BoundingShape2D = GameManager.Instance.CameraBox;
    }

    private void Update()
    {
        // Check if right mouse button is pressed
        if (Input.GetMouseButtonDown(1))
        {
            // Get the mouse position in screen space
            Vector3 mousePos = Input.mousePosition;

            // Convert the mouse position to world space (2D)
            targetX = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, 0, 0)).x;
            targetX = Mathf.Clamp(targetX, minX, maxX); // Clamp targetX within the desired range
            isMoving = true; // Set the moving flag to true
        }

        // Move the object towards the target X position smoothly
        if (isMoving)
        {
            Vector3 currentPosition = transform.position;
            Vector3 targetPosition = new Vector3(targetX, currentPosition.y, currentPosition.z);
            transform.position = Vector3.MoveTowards(currentPosition, targetPosition, 10f * Time.deltaTime);

            // Check if the object has reached the target X position
            if (Mathf.Approximately(currentPosition.x, targetX))
            {
                isMoving = false; // Set the moving flag to false when reached
            }
        }
    }
}
