using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothMouseLook : MonoBehaviour
{
    public Camera cam;
    public float zoomDefault = -10;

    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityX = 15F;
    public float sensitivityY = 15F;

    public float minimumX = -360F;
    public float maximumX = 360F;

    public float minimumY = -60F;
    public float maximumY = 60F;

    float rotationX = 0F;
    float rotationY = 0F;

    private List<float> rotArrayX = new List<float>();
    float rotAverageX = 0F;

    private List<float> rotArrayY = new List<float>();
    float rotAverageY = 0F;

    public float frameCounter = 20;

    Quaternion originalRotation;

    public LayerMask obstacleMask;
    private float y;
    private bool sliding;
    private float dist;
    RaycastHit hit;
    Vector3 viablePosition;

    void Update()
    {
        FindViablePosition();
        

        if (Physics.Raycast(cam.transform.position, Vector3.down, out hit, Mathf.Infinity))
        {
            dist = Vector3.Distance(cam.transform.position, hit.point);
        }

        if (axes == RotationAxes.MouseXAndY)
        {
            rotAverageY = 0f;
            rotAverageX = 0f;

            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationX += Input.GetAxis("Mouse X") * sensitivityX;

            rotArrayY.Add(rotationY);
            rotArrayX.Add(rotationX);

            if (rotArrayY.Count >= frameCounter)
            {
                rotArrayY.RemoveAt(0);
            }
            if (rotArrayX.Count >= frameCounter)
            {
                rotArrayX.RemoveAt(0);
            }

            for (int j = 0; j < rotArrayY.Count; j++)
            {
                rotAverageY += rotArrayY[j];
            }
            for (int i = 0; i < rotArrayX.Count; i++)
            {
                rotAverageX += rotArrayX[i];
            }

            rotAverageY /= rotArrayY.Count;
            rotAverageX /= rotArrayX.Count;

            rotAverageY = ClampAngle(rotAverageY, minimumY, maximumY);
            rotAverageX = ClampAngle(rotAverageX, minimumX, maximumX);

            Quaternion yQuaternion = Quaternion.AngleAxis(rotAverageY, Vector3.left);
            Quaternion xQuaternion = Quaternion.AngleAxis(rotAverageX, Vector3.up);

            //if (dist < cam.transform.localPosition.y + 0.1f)
            //{
            //    if (!sliding)
            //    {
            //        sliding = true;
            //        y = rotAverageY;                    
            //    }
            //    else
            //    {
            //        if (rotAverageY > y)
            //        {
            //            float p = Mathf.Clamp((rotAverageY - y) / (maximumY - y), 0, 0.35f);
            //            if (p < 0)
            //            {
            //                cam.transform.localPosition = new Vector3(1, cam.transform.localPosition.y, -10);
            //                y = 0;
            //                sliding = false;
            //            }
            //            else
            //            {
            //                cam.transform.localPosition = new Vector3(1, cam.transform.localPosition.y, Mathf.Lerp(-10, -2f, p * 3f));
            //            }
            //        }
            //        else
            //        {
            //            cam.transform.localPosition = new Vector3(1, cam.transform.localPosition.y, -10);
            //            y = 0;
            //            sliding = false;
            //        }
            //    }
            //}
            //else
            //{
            //    cam.transform.localPosition = new Vector3(1, cam.transform.localPosition.y, -10);
            //    y = 0;
            //    sliding = false;
            //}

            transform.localRotation = originalRotation * xQuaternion * yQuaternion;
        }
        else if (axes == RotationAxes.MouseX)
        {
            rotAverageX = 0f;

            rotationX += Input.GetAxis("Mouse X") * sensitivityX;

            rotArrayX.Add(rotationX);

            if (rotArrayX.Count >= frameCounter)
            {
                rotArrayX.RemoveAt(0);
            }
            for (int i = 0; i < rotArrayX.Count; i++)
            {
                rotAverageX += rotArrayX[i];
            }
            rotAverageX /= rotArrayX.Count;

            rotAverageX = ClampAngle(rotAverageX, minimumX, maximumX);

            Quaternion xQuaternion = Quaternion.AngleAxis(rotAverageX, Vector3.up);
            transform.localRotation = originalRotation * xQuaternion;
        }
        else
        {
            rotAverageY = 0f;

            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;

            rotArrayY.Add(rotationY);

            if (rotArrayY.Count >= frameCounter)
            {
                rotArrayY.RemoveAt(0);
            }
            for (int j = 0; j < rotArrayY.Count; j++)
            {
                rotAverageY += rotArrayY[j];
            }
            rotAverageY /= rotArrayY.Count;

            rotAverageY = ClampAngle(rotAverageY, minimumY, maximumY);

            Quaternion yQuaternion = Quaternion.AngleAxis(rotAverageY, Vector3.left);
            transform.localRotation = originalRotation * yQuaternion;
        }
    }

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb)
            rb.freezeRotation = true;
        originalRotation = transform.localRotation;
    }

    private void LateUpdate()
    {
        cam.transform.localPosition = new Vector3(1, cam.transform.localPosition.y, viablePosition.z);
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        angle = angle % 360;
        if ((angle >= -360F) && (angle <= 360F))
        {
            if (angle < -360F)
            {
                angle += 360F;
            }
            if (angle > 360F)
            {
                angle -= 360F;
            }
        }
        return Mathf.Clamp(angle, min, max);
    }

    void FindViablePosition()
    {
        Debug.DrawRay(transform.position, -transform.forward * 10);
        RaycastHit hit1;
        if (Physics.Raycast(transform.position, -transform.forward, out hit1, -zoomDefault, obstacleMask))
        {
            Vector3 hitDir = (hit1.point - transform.position).normalized;
            float hitDst = -Vector3.Distance(hit1.point, transform.position);
            hitDst = Mathf.Clamp(hitDst, zoomDefault, -2);
            viablePosition = new Vector3(0, 0, hitDst + 1);
        }
        else
        {
            viablePosition = new Vector3(0, 0, zoomDefault);
        }
    }

    private void OnValidate()
    {
        cam.transform.localPosition = new Vector3(1, 0, zoomDefault);
    }
}
