using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraObject : MonoBehaviour
{
    [SerializeField] GameObject securityCamera;
    [SerializeField] GameObject camObject;
    [SerializeField] CharacterManager cm;
    public float xAngle, yAngle, zAngle;
    public float lowerConstraint, upperConstraint;
    private float camAngle;
    private bool isActive = true;

    private void Update()
    {
        
        if(isActive)camObject.transform.Rotate(xAngle, yAngle, zAngle, Space.Self);
        camAngle = camObject.transform.localRotation.eulerAngles.x;

        if (Mathf.Abs(camAngle - upperConstraint) < 2 || Mathf.Abs(camAngle - lowerConstraint) < 2) xAngle = xAngle * -1;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Ghost" && Input.GetKey(KeyCode.R) && cm.isGhost)
        {
            cm.destroyCamera();
            Destroy(securityCamera);
            isActive = false;
        }
    }
}
