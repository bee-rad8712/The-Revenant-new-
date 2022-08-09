using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaPotion : MonoBehaviour
{
    [SerializeField] CharacterManager cm;
    [SerializeField] float ManaAmount;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ghost" && cm.mana < cm.maxMana)
        {
            cm.mana += ManaAmount;
            Destroy(gameObject);
        }
    }
}
