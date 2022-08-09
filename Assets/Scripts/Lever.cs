using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] CharacterManager cm;
    [SerializeField] Door door;
    [SerializeField] int direction;
    [SerializeField] bool ghostLever;
    private void OnTriggerStay(Collider other)
    {
        if (!ghostLever)
        {
            if (!cm.isGhost && Input.GetKey(KeyCode.R))
            {
                door.Opendoor(direction);
            }
        }
        else
        {
            if (cm.isGhost && Input.GetKey(KeyCode.R))
            {
                door.Opendoor(direction);
            }
        }
    }
}
