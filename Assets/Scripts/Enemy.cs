using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    private float location;
    public float turnDist;
    [SerializeField] GameObject enemy;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == ("Projectile")) Debug.Log("Damage!");
        Debug.Log("Damage!");
    }
    private void Update()
    {
        enemy.transform.Translate(speed, 0, 0);
        location += speed;
        if (location >= turnDist || location <= 0)
        {
            speed = speed * -1;
            enemy.transform.Translate(2 * speed, 0, 0);
            
        }
        if (speed < 0) enemy.transform.localScale = Vector3.one;
        else enemy.transform.localScale = new Vector3(-1,1,1);
    }
}
