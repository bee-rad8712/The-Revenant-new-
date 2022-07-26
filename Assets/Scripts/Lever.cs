using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] CharacterManager cm;
    [SerializeField] Door door;
    private void OnTriggerStay(Collider other)
    {
        if(!cm.isGhost && Input.GetKey(KeyCode.R))
        {
            Debug.Log("Open door!");
            door.Opendoor();
        }
    }
}
