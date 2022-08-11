using UnityEngine;

public class Health : MonoBehaviour
{
	[SerializeField] CharacterManager cm;	
	public float startingHealth;
	public float currentHealth;

	private void Awake()
	{
		currentHealth = startingHealth;
	}
    private void Update()
    {
        if(currentHealth > startingHealth) currentHealth = startingHealth;
    }
    public void TakeDamage(float _damage)
	{
		currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
		
		if (currentHealth > 0)
		{
			Debug.Log("Hurt");
			cm.cAnim.SetTrigger("VicHurt");
		}
		else
		{
			cm.onDeath();
		}
	}
}
