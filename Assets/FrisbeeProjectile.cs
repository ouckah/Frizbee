using UnityEngine;
using Unity.Netcode;

public class FrisbeeProjectile : MonoBehaviour
{
    public float collisionSpeedReduction = 0.3f; // Rate in which the frisbee loses speed on collision
    public float torque = 10.0f;    // Torque applied to the frisbee
    public float lift = 1.0f;    // Lift force applied to the frisbee
    public float gravity = -9.81f;    // Gravity force applied to the frisbee

    public bool isGrounded = false;
    public float timeToDestroy = 2f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;    // Disable gravity so we can apply our own
    }

    void FixedUpdate()
    {
        // Apply lift force to the frisbee based on its velocity
        Vector3 relativeVelocity = transform.InverseTransformDirection(rb.velocity);
        float liftAngle = Mathf.Clamp(relativeVelocity.y, -1.0f, 1.0f) * Mathf.PI / 2.0f;
        Vector3 liftVector = Vector3.up * Mathf.Cos(liftAngle) + transform.forward * Mathf.Sin(liftAngle);
        rb.AddForce(lift * liftVector);

        // Apply gravity force to the frisbee
        rb.AddForce(gravity * Vector3.up);

        // Apply torque to the frisbee
        // float hInput = Input.GetAxis("Horizontal");
        // float vInput = Input.GetAxis("Vertical");
        // Vector3 torqueVector = Vector3.up * hInput - Vector3.right * vInput;
        // rb.AddTorque(Vector3.up * torque);
    }

    void Update()
    {
        // Move the frisbee forward
        Vector3 movement = transform.forward * Time.deltaTime;
        rb.MovePosition(rb.position + movement);
    }

    private void OnTriggerEnter(Collider collision)
    {
        // Reduce velocity of the projectile upon collision
        rb.velocity *= collisionSpeedReduction;

        // Check if the projectile has collided with the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            // set grounded property to true
            isGrounded = true;

            // Destroy the projectile object after a delay
            Destroy(gameObject, timeToDestroy);
        }

        // Check if the projectile has collided with an AI (and hasn't already touched the ground)
        if (collision.gameObject.GetComponent<AI>() != null && !isGrounded)
        {
            AI ai = collision.gameObject.GetComponent<AI>();

            // Destroy the projectile object instantly
            Destroy(gameObject);

            // Trigger "catch" method
            ai.Catch();
        }

        // Check if the projectile has collided with an Player (and hasn't already touched the ground)
        if (collision.gameObject.GetComponent<Player>() != null && !isGrounded)
        {
            Player player = collision.gameObject.GetComponent<Player>();

            Debug.Log("A frisbee has collided with a player!");

            // Trigger "catch" method
            player.Catch();

            Debug.Log("A player has caught a frisbee!");

            // Destroy the projectile object instantly
            GetComponent<NetworkObject>().Despawn();
        }
    }
}