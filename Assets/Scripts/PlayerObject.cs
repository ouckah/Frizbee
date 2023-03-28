using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : MonoBehaviour
{

    private PlayerMovement controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = gameObject.GetComponent<PlayerMovement>();    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Throw()
    {
        controller.Throw();
    }

    public void Catch()
    {
        controller.Catch();
    }
}
