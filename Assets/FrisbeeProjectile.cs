using UnityEngine;

public class FrisbeeProjectile : MonoBehaviour
{
    public float speed = 1.0f;    // Speed of the frisbee
    public float torque = 10.0f;    // Torque applied to the frisbee
    public float lift = 1.0f;    // Lift force applied to the frisbee
    public float gravity = -9.81f;    // Gravity force applied to the frisbee

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
        float liftAngle = Mathf.Clamp(relativeVelocity.y / speed, -1.0f, 1.0f) * Mathf.PI / 2.0f;
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
        Vector3 movement = transform.forward * speed * Time.deltaTime;
        rb.MovePosition(rb.position + movement);
    }
}