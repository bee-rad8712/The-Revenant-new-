using UnityEngine;
using UnityEngine.UI;

public class Manabar : MonoBehaviour
{
    [SerializeField] CharacterManager cm;
    public Image img;
    public double manafill;
    private void Update()
    {
        manafill = cm.mana/cm.maxMana;
        img.fillAmount = (float)manafill;
    }
}
