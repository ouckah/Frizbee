using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public GameObject frisbeePrefab;    // Prefab for the projectile
    public float launchForce = 20.0f;    // Force applied to the projectile when launched
    public float upwardForce = 0.12f;

    public GameObject frisbeeVisual;
    public Transform throwingHand;
    public bool hasFrisbee = false;

    public float waitBeforeThrowing = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hasFrisbee) frisbeeVisual.transform.position = throwingHand.transform.position;
        if (hasFrisbee)
        {
            Throw();
        }
    }

    public void Throw()
    {
        // Spawn a new projectile at the position of the script object
        GameObject newProjectile = Instantiate(frisbeePrefab, throwingHand.position, Quaternion.identity);

        // Get the forward direction of the camera
        Vector3 throwDirection = transform.forward + Vector3.up * upwardForce;

        // Set the forward direction of the projectile to the camera forward direction
        newProjectile.transform.forward = transform.forward;

        // Apply a force to the projectile in the direction of the camera forward direction
        Rigidbody projectileRb = newProjectile.GetComponent<Rigidbody>();
        projectileRb.AddForce(throwDirection * launchForce, ForceMode.Impulse);

        // Remove frisbee graphics + status
        hasFrisbee = false;
        frisbeeVisual.SetActive(false);
    }

    public void Catch()
    {
        hasFrisbee = true;
        frisbeeVisual.SetActive(true);
    }
}
