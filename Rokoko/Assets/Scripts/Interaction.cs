using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public float force = 5, upForce = 1;
    public Rigidbody rb;
    public Transform hand;
    public LayerMask handLayer, ignoreLayer;

    AudioSource aS;
    public AudioClip whack;

    // Start is called before the first frame update
    void Start()
    {
        aS = GetComponent<AudioSource>();
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
        if (PlayerControllerV2.isPunching)
        {
            if (collision.gameObject.CompareTag("Interactable"))
            {
                Vector3 dir = (collision.GetContact(0).point - transform.position).normalized;
                collision.rigidbody.AddForceAtPosition(dir * force + Vector3.up * upForce, collision.GetContact(0).point, ForceMode.Impulse);
                aS.pitch += Random.Range(-0.2f, 0.2f);
                aS.PlayOneShot(whack);
                aS.pitch = 1;
            }
        }
    }
}
