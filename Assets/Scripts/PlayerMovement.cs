using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;    // Player movement speed
    public float rotationSpeed = 5.0f;    // Speed of the player rotation
    public float jumpForce = 5.0f;    // Player jump force
    public float raycastDistance = 1.0f;    // Distance to cast ray for ground detection
    public LayerMask groundLayer;    // Layer mask for ground objects

    private Rigidbody rb;
    private Transform cameraTransform;

    public float holdTime = 0.0f;  // time that user held down click (for frisbee power)
    private bool isHolding = false;
    public GameObject frisbeePrefab;    // Prefab for the projectile
    public float launchForce = 10.0f;    // Force applied to the projectile when launched
    public Transform throwingHand;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        // Jump if player is on the ground and space bar is pressed
        if (IsGrounded() && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        if (Input.GetMouseButtonDown(0))
        {
            isHolding = true;
        }

        if (Input.GetMouseButton(0))
        {
            if (isHolding)
            {
                holdTime += 3.0f * Time.deltaTime;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isHolding = false;

            // Spawn a new projectile at the position of the script object
            GameObject newProjectile = Instantiate(frisbeePrefab, throwingHand.position, Quaternion.identity);

            // Get the forward direction of the camera
            Vector3 cameraForward = Camera.main.transform.forward;

            // Set the forward direction of the projectile to the camera forward direction
            newProjectile.transform.forward = cameraForward;

            // Apply a force to the projectile in the direction of the camera forward direction
            Rigidbody projectileRb = newProjectile.GetComponent<Rigidbody>();
            holdTime = Mathf.Clamp(holdTime, 0.0f, 2.0f);
            projectileRb.AddForce(cameraForward * (holdTime * holdTime) * launchForce, ForceMode.Impulse);

            holdTime = 0.0f;
        }
    }

    void FixedUpdate()
    {
        // Get the movement input axis
        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");

        // Move the player object in the direction of the camera
        Vector3 cameraForward = Vector3.Scale(cameraTransform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 movement = (vInput * cameraForward + hInput * cameraTransform.right).normalized * moveSpeed;
        rb.MovePosition(rb.position + movement * Time.fixedDeltaTime);

        // Rotate the player object to face the direction of the camera
        if (movement.magnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
        else
        {
            Quaternion targetRotation = Quaternion.LookRotation(cameraForward, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }

    bool IsGrounded()
    {
        // Cast a ray down from player to check for ground
        return Physics.Raycast(transform.position, Vector3.down, raycastDistance, groundLayer);
    }
}