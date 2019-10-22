using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public float force = 5;
    public Rigidbody rb;
    public Transform hand;
    public LayerMask handLayer, ignoreLayer;

    // Start is called before the first frame update
    void Start()
    {
        Physics.IgnoreLayerCollision(11, 12);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        rb.position = hand.position;
        rb.rotation = hand.rotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Interactable"))
        {
            print("Punch!");
            Vector3 dir = (collision.GetContact(0).point - transform.position).normalized;
            collision.rigidbody.AddForceAtPosition(dir * force, collision.GetContact(0).point, ForceMode.Impulse);
        }
        else
            print("notPunch");
    }
}
