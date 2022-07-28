using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] Health healthbar;
    public Image img;
    public float fillamount;
    private void Update()
    {
        fillamount = (healthbar.currentHealth) / healthbar.startingHealth;
        img.fillAmount = fillamount;
    }
}

