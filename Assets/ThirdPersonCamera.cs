using UnityEngine;
using Unity.Netcode;

public class ThirdPersonCamera : NetworkBehaviour
{
    [SerializeField] private Camera thisCamera;

    public Transform target;    // Target to follow
    public float distance = 10.0f;    // Distance from target
    public float height = 5.0f;    // Height above target
    public float smoothSpeed = 1.0f;    // Smoothness of camera movement
    public float lookSpeed = 2.0f;    // Speed of camera rotation

    private Vector3 offset;    // Offset from target
    private float yaw = 0.0f;    // Yaw angle of camera (left/right)
    private float pitch = 0.0f;    // Pitch angle of camera (up/down)

    void Start()
    {
        // Calculate initial camera offset from target
        offset = transform.position - target.position;

        // Lock cursor to screen and hide it
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        // Calculate desired camera position and rotation based on input
        yaw += lookSpeed * Input.GetAxis("Mouse X");
        pitch += lookSpeed * Input.GetAxis("Mouse Y");
        pitch = Mathf.Clamp(pitch, -45.0f, 9.0f);    // Limit pitch angle to prevent flipping

        Vector3 desiredPosition = target.position + Quaternion.Euler(pitch, yaw, 0.0f) * -offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        transform.LookAt(target);
    }
}