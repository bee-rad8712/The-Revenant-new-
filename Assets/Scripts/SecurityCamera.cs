using UnityEngine;
public class SecurityCamera : MonoBehaviour
{
    [SerializeField] CharacterManager cm;
    private bool isactive;

    private void Awake()
    {
        isactive = true;
    }
    private void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") cm.onDeath();
    }
    
}
