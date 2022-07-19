using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraObject : MonoBehaviour
{
    [SerializeField] SecurityCamera cam;
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Ghost" && Input.GetKey(KeyCode.R)) Destroy(cam);
    }
}
