using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 2;
    public Transform cameraHolder;
    public Rigidbody rb;
    Vector3 newFwd, newLeft;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        newFwd = cameraHolder.forward;
        newLeft = Vector3.Cross(newFwd, Vector3.up);
        rb.position += (newFwd * Input.GetAxis("Vertical") + newLeft * -Input.GetAxis("Horizontal")) * moveSpeed * Time.fixedDeltaTime;
    }
}
