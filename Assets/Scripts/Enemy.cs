using System.Threading.Tasks;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    private float location;
    public float turnDist;
    private Rigidbody rb;
    private Animator animator;
    [SerializeField] int enemyHP;
    [SerializeField] int attackCooldowntime;
    [SerializeField] int attackCooldown;
    [SerializeField] GameObject enemy;
    [SerializeField] GameObject player;
    [SerializeField] CharacterManager cm;
    private void Awake()
    {
        rb = enemy.GetComponent<Rigidbody>();
        animator = enemy.GetComponent<Animator>();  
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == ("Projectile")) Debug.Log("Damage!");
        //Debug.Log("Damage!");
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
        if (Input.GetKey(KeyCode.Mouse1) && canAttack())
        {
            
            Debug.Log("Hit!");
            animator.SetTrigger("EnemyHit");
            attackCooldown = attackCooldowntime;
            enemyHP--;
        }
        if(attackCooldown > 0) attackCooldown--;
        if (enemyHP <= 0)
        {
            animator.SetTrigger("EnemyKilled");
            Task.Delay(1300);
            Destroy(enemy);
        }     
    }
    private bool canAttack()
    {
        if (attackCooldown > 0) return false;
        if(Vector3.Distance(player.transform.position, enemy.transform.position) > 5) return false;
        if(cm.isAlive == false) return false;
        return true;
    }
}
