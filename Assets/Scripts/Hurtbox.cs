using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    [SerializeField] Health health;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            health.TakeDamage(20);
        }
    }
}
