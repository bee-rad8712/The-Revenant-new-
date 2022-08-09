using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    [SerializeField] Health healthbar;
    [SerializeField] float HealthAmount;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && healthbar.currentHealth < healthbar.startingHealth)
        {
            healthbar.currentHealth += HealthAmount;
            Destroy(gameObject);
        }       
    }
}
