﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class PlayerControllerV2: MonoBehaviour {
 
    public float floorOffsetY;
    public float moveSpeed = 6f;
    public float rotateSpeed = 10f;
 
    Rigidbody rb;
    Animator anim;
    float vertical;
    float horizontal;
    Vector3 moveDirection;
    float inputAmount;
    Vector3 raycastFloorPos;
    Vector3 floorMovement;
    Vector3 gravity;
    Vector3 CombinedRaycast;

    public Transform lookAtThis;

    public AnimLibrary animations;
    List<string> leftPunches = new List<string>();
    List<string> rightPunches = new List<string>();
    List<string> specials = new List<string>();
    public static bool isPunching;

    public Collider foot;

    AudioSource aS;
    public AudioClip woosh;

    // Use this for initialization
    void Start () {
        aS = GetComponent<AudioSource>();
        leftPunches.AddRange(animations.leftPunches);
        rightPunches.AddRange(animations.rightPunches);
        specials.AddRange(animations.specials);
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        Cursor.visible = false;
    }
 
    private void Update()
    {
        Cursor.visible = PauseMenu.paused;
        // reset movement
        moveDirection = Vector3.zero;
        // get vertical and horizontal movement input (controller and WASD/ Arrow Keys)
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");
 
        // base movement on camera
        Vector3 correctedVertical = vertical * Camera.main.transform.forward;
        Vector3 correctedHorizontal = horizontal * Camera.main.transform.right;
 
        Vector3 combinedInput = correctedHorizontal + correctedVertical;
        // normalize so diagonal movement isnt twice as fast, clear the Y so your character doesnt try to
        // walk into the floor/ sky when your camera isn't level
         moveDirection = new Vector3((combinedInput).normalized.x, 0, (combinedInput).normalized.z);
       
        // make sure the input doesnt go negative or above 1;
        float inputMagnitude = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
        inputAmount = Mathf.Clamp01(inputMagnitude);
 
        // handle animation blendtree for walking
        anim.SetFloat("forward", vertical, .05f, Time.deltaTime);
        anim.SetFloat("turn", horizontal, .05f, Time.deltaTime);

        if (Input.GetMouseButtonDown(0))
        {
            int l = Random.Range(0, leftPunches.Count);
            ResetTriggers(leftPunches[l]);
            anim.SetTrigger(leftPunches[l]);
        }

        if (Input.GetMouseButtonDown(1))
        {
            int r = Random.Range(0, rightPunches.Count);
            ResetTriggers(rightPunches[r]);
            anim.SetTrigger(rightPunches[r]);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int s = Random.Range(0, specials.Count);
            ResetTriggers(specials[s]);
            anim.SetTrigger(specials[s]);
        }
    }

    void ResetTriggers(string trigger)
    {
        foreach (string s in leftPunches)
        {
            if (s != trigger)
                anim.ResetTrigger(s);
        }
        foreach (string s in rightPunches)
        {
            if (s != trigger)
                anim.ResetTrigger(s);
        }
        foreach(string s in specials)
        {
            if (s != trigger)
                anim.ResetTrigger(s);
        }
    }

    public void StartPunch(string type)
    {
        aS.pitch += Random.Range(-0.2f, 0.2f);
        aS.PlayOneShot(woosh);
        aS.pitch = 1;
        isPunching = true;
        if (type == "kick")
            foot.enabled = true;
    }

    public void EndPunch(string type)
    {
        isPunching = false;
        if (type == "kick")
            foot.enabled = false;
    }

    private void OnAnimatorIK(int layerIndex)
    {
        float distanceFaceObject = Vector3.Distance(anim.GetBoneTransform(HumanBodyBones.Head).position, lookAtThis.position);

        anim.SetLookAtPosition(lookAtThis.position);

        anim.SetLookAtWeight(Mathf.Clamp01(5 - distanceFaceObject), Mathf.Clamp01(1 - distanceFaceObject));
    }
   
   private void FixedUpdate () {
        // if not grounded , increase down force
        if(FloorRaycasts(0,0,0.6f) == Vector3.zero)
        {
            gravity += Vector3.up * Physics.gravity.y * Time.fixedDeltaTime;
        }
       
        // actual movement of the rigidbody + extra down force
        rb.velocity = (moveDirection * moveSpeed * inputAmount) + gravity;
 
        // find the Y position via raycasts
        floorMovement =  new Vector3(rb.position.x, FindFloor().y + floorOffsetY, rb.position.z);
 
        // only stick to floor when grounded
        if(FloorRaycasts(0,0, 0.6f) != Vector3.zero && floorMovement != rb.position)
        {
            // move the rigidbody to the floor
            rb.MovePosition(floorMovement);
            gravity.y = 0;
        }
       
             // rotate player to movement direction
        Quaternion rot = Quaternion.LookRotation(moveDirection);
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, rot, Time.fixedDeltaTime * inputAmount * rotateSpeed);
        transform.rotation = targetRotation;

    }
 
    Vector3 FindFloor()
    {
        // width of raycasts around the centre of your character
        float raycastWidth = 0.25f;
        // check floor on 5 raycasts   , get the average when not Vector3.zero  
         int floorAverage = 1;
 
        CombinedRaycast = FloorRaycasts(0, 0, 1.6f);
        floorAverage += (getFloorAverage(raycastWidth, 0) + getFloorAverage(-raycastWidth, 0) + getFloorAverage(0, raycastWidth) + getFloorAverage(0, -raycastWidth));
     
        return CombinedRaycast / floorAverage;
    }
 
    // only add to average floor position if its not Vector3.zero
    int getFloorAverage(float offsetx, float offsetz)
    {
       
        if (FloorRaycasts(offsetx, offsetz, 1.6f) != Vector3.zero)
        {
            CombinedRaycast += FloorRaycasts(offsetx, offsetz, 1.6f);
            return 1;
        }
        else { return 0; }
    }
 
 
    Vector3 FloorRaycasts(float offsetx, float offsetz, float raycastLength)
    {
        RaycastHit hit;
        // move raycast
        raycastFloorPos = transform.TransformPoint(0 + offsetx, 0 + 0.5f, 0 + offsetz);
 
        Debug.DrawRay(raycastFloorPos, Vector3.down, Color.magenta);
        if (Physics.Raycast(raycastFloorPos, -Vector3.up, out hit, raycastLength))
        {
            return hit.point;
        }
        else return Vector3.zero;
    }
}