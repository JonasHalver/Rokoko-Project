using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookatPivot : MonoBehaviour
{
    public Transform cam, player;

    public float dstFromFace = 1;
    Vector3 oppositePos, playerPos, dir;
    float dst;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = player.position + (Vector3.up * 3.4f);
        dir = (playerPos - cam.position).normalized;
        dst = Vector3.Distance(playerPos, cam.position);
        oppositePos = cam.position + (dir * (dst + dstFromFace));
    }

    private void LateUpdate()
    {
        transform.position = oppositePos;
    }
}
