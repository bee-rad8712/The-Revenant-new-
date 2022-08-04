using UnityEngine;

public class StartingBarrier : MonoBehaviour
{
    [SerializeField] CharacterManager cm;
    [SerializeField] GameObject IWalls;
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Ghost" && Input.GetKey(KeyCode.R) && cm.isGhost)
        {
            Destroy(IWalls);
        }
    }
}
