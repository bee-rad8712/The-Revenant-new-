using System.Threading.Tasks;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public GameObject avatar;
    public GameObject ghost;
    public GameObject enemy;
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] RespawnManager respawnManager;
    [SerializeField] Health healthbar;

    public double possessManaCost;
    public double ghostManaCost;
    public double transformCost;
    public double manaRegen;
    public double mana;
    public double maxMana;
    public float gravityScale;
    public double teleportCost;
    bool canSwitch = true;
    public bool isGhost = false;
    public bool isPossessing = false;
    bool isCrouching = false;
    bool toggleCrouch = true;
    private float horizontalInput;
    private Rigidbody bodyAvatar;
    private BoxCollider boxColliderAvatar;
    private Animator animAvatar;
    private Rigidbody bodyGhost;
    private BoxCollider boxColliderGhost;
    private Animator animGhost;
    private Rigidbody cBody;
    private BoxCollider cBoxCollider;
    private Animator cAnim;
    private Rigidbody bodyEnemy;
    private BoxCollider boxColliderEnemy;
    private Animator animEnemy;

    private float wallJumpCooldown;
    public bool isAlive;
    private void Awake()
    {
        //Grab references for rigidbody and animator from object
        bodyAvatar = avatar.GetComponent<Rigidbody>();
        animAvatar = avatar.GetComponent<Animator>();
        boxColliderAvatar = avatar.GetComponent<BoxCollider>();
        bodyGhost = ghost.GetComponent<Rigidbody>();
        animGhost = ghost.GetComponent<Animator>();
        boxColliderGhost = ghost.GetComponent<BoxCollider>();
        bodyEnemy = enemy.GetComponent<Rigidbody>();
        animEnemy = enemy.GetComponent<Animator>();
        boxColliderEnemy = enemy.GetComponent<BoxCollider>();
        cBody = bodyAvatar;
        cBoxCollider = boxColliderAvatar;
        cAnim = animAvatar;
        isAlive = true;
    }
    void Update()
    {
        if(isGhost && mana <= 0)
        {           
            onDeath();
        }
        //Check whether the player is releasing the left alt key
        if (Input.GetKeyUp(KeyCode.LeftAlt)) canSwitch = true;

        if (Input.GetKey(KeyCode.F) && isGhost && mana > teleportCost)
        {
            mana -= teleportCost;
            Debug.Log("Teleport!");
            isGhost = !isGhost;
            bodyAvatar.transform.position = bodyGhost.position + new Vector3(0, 0, 5);
            cBody = bodyAvatar;
            cBoxCollider = boxColliderAvatar;
            cAnim = animAvatar;
            ghost.SetActive(false);
            isGhost = false;
        }

        //Switch between ghost and human form if able
        if (canSwitch && Input.GetKey(KeyCode.LeftAlt) && isAlive && bodyAvatar.velocity.magnitude <= 1)
        {
            isGhost = !isGhost;
            if (isPossessing)
            {
                cAnim.SetTrigger("Project");
                cBody = bodyGhost;
                cBoxCollider = boxColliderGhost;
                cAnim = animGhost;
                bodyGhost.transform.position = bodyEnemy.position + new Vector3(0, 0, -5);
                ghost.SetActive(true);
                isPossessing = false;
                isGhost = true;
            }
            else if (!isGhost)
            {
                if (Vector3.Distance(bodyGhost.transform.position, bodyAvatar.transform.position) < 8)
                {                 
                    cBody = bodyAvatar;
                    cBoxCollider = boxColliderAvatar;
                    cAnim = animAvatar;
                    ghost.SetActive(false);
                    isGhost = false;
                }
                else
                {
                    isGhost = true;
                }
            }
            else if (mana > transformCost)
            {
                //Set position of the ghost to right in front of the avatar
                mana -= transformCost;
                Debug.Log(mana);
                cBody = bodyGhost;
                cBoxCollider = boxColliderGhost;
                cAnim = animGhost;
                bodyGhost.transform.position = bodyAvatar.position + new Vector3(0, 0, -5);
                ghost.SetActive(true);
                isGhost = true;

            }
            canSwitch = false;
        }

        if (!isGhost)
        {
            characterController(avatar.transform);
        }
        else
        {
            characterController(ghost.transform);
            mana -= ghostManaCost;
        }
        if (!isPossessing && !isGhost)
        {
            Vector3 gravity = gravityScale * Vector3.up;
            bodyAvatar.AddForce(gravity, ForceMode.Acceleration);
            if (mana < maxMana) mana += manaRegen;
        }
        if (Input.GetKey(KeyCode.E) && canPossess())
        {
            mana -= possessManaCost;
            isGhost = false;
            ghost.SetActive(false);
            isPossessing = true;
            cBody = bodyEnemy;
            cBoxCollider = boxColliderEnemy;
            cAnim = animEnemy;
        }
    }

    void characterController(Transform character)
    {

        //read the inputs and make character.transform.position change accordingly
        horizontalInput = Input.GetAxis("Horizontal");

        //Flip player when moving left-right
        if (!isPossessing && !isGhost)
        {
            if (horizontalInput > 0.01f)
                character.localScale = Vector3.one;
            else if (horizontalInput < -0.01f)
                character.localScale = new Vector3(-1, 1, 1);
        }
        if(isGhost)
        {
            if (horizontalInput > 0.01f)
                character.localScale = Vector3.one;
            else if (horizontalInput < -0.01f)
                character.localScale = new Vector3(-1, 1, 1);
        }

        //Set animator parameters
        cAnim.SetBool("Running", horizontalInput != 0);
        cAnim.SetBool("Grounded", isGrounded());

        //Wall jump logic
        if (wallJumpCooldown > 0.2f && isAlive)
        {
            cBody.velocity = new Vector2(horizontalInput * speed, cBody.velocity.y);
            if (onWall() && !isGrounded())
            {
                cBody.useGravity = false; //FIXME?
                cBody.velocity = Vector2.zero;
            }
            else
            {
                if (!isGhost)
                {
                    cBody.useGravity = true; //FIXME?
                }
            }

            if (Input.GetKey(KeyCode.Space) && !isCrouching)
            {
                Jump(character);
            }
        }
        else
            wallJumpCooldown += Time.deltaTime;

        //Ghost Movement
        if (isGhost)
        {
            if (Input.GetKey(KeyCode.W) && isAlive)
                bodyGhost.velocity = new Vector2(bodyGhost.velocity.x, jumpPower*2);
            
            if (Input.GetKeyUp(KeyCode.W) && isAlive) 
                bodyGhost.velocity = new Vector2(bodyGhost.velocity.x, 0);

            if (Input.GetKey(KeyCode.S) && isAlive)
                bodyGhost.velocity = new Vector2(bodyGhost.velocity.x, -jumpPower*2);
            
            if (Input.GetKeyUp(KeyCode.S) && isAlive)
                bodyGhost.velocity = new Vector2(bodyGhost.velocity.x, 0);

           
        }
        
        //Crouching and kill bind
        if(Input.GetKey(KeyCode.P))
        {
            onDeath();
        }
        if(Input.GetKey(KeyCode.C) && !isCrouching && toggleCrouch)
        {
            speed = 3;
            toggleCrouch = false;
            isCrouching = !isCrouching;          
        }
        if(Input.GetKey(KeyCode.C) && isCrouching && toggleCrouch)
        {
            speed = 10;
            toggleCrouch = false;
            isCrouching = !isCrouching;
        }
        if(Input.GetKeyUp(KeyCode.C))
        {
            toggleCrouch = true;
        }
    }

    bool canSwitchCharacter()
    {
        //Have to be stationary to switch to ghost form
        if (bodyAvatar.velocity.magnitude != 0)
            return false;
        else
            return true;
    }
    bool canPossess()
    {
        if (mana < possessManaCost) return false;
        if (!isGhost || isPossessing) return false;
        float distance = Vector3.Distance(bodyGhost.transform.position, bodyEnemy.transform.position);
        if (distance > 10) return false;
        return true;
    }

    private void Jump(Transform character)
    {
        cBody.velocity = new Vector2(cBody.velocity.x, jumpPower);
        cAnim.SetTrigger("Jump");
        if (isGrounded())
        {
            Debug.Log("Grounded");
        }
        /*else if (onWall() && !isGrounded())
        {
            if (horizontalInput == 0)
            {
                //cBody.velocity = new Vector2(-Mathf.Sign(character.localScale.x) * 10, 0);
                //character.localScale = new Vector3(-Mathf.Sign(character.localScale.x), character.localScale.y, character.localScale.z);
            }
            else
                //cBody.velocity = new Vector2(-Mathf.Sign(character.localScale.x) * 3, 6);

            wallJumpCooldown = 0;
        }*/
    }
    public async void onDeath()
    {
        cBody = bodyAvatar;
        cBoxCollider = boxColliderAvatar;
        cAnim = animAvatar;
        ghost.SetActive(false);
        isGhost = false;
        cAnim.SetTrigger("Death");
        bodyAvatar.velocity = Vector3.zero;
        ghost.SetActive(false);
        await Task.Delay(2500);
        isAlive = false;
        avatar.SetActive(false);
        
        //GameObject rp = respawnManager.GetRespawnPoint();
        await Task.Delay(2500);
        healthbar.currentHealth = 100;
        mana = maxMana;
        //avatar.transform.position = rp.transform.position;
        avatar.transform.position = new Vector3(0, 0, 0);
        avatar.SetActive(true);
        isAlive = true;
        characterController(avatar.transform);
    }
    public void destroyCamera()
    {
        cAnim.SetTrigger("GhostInteract");
    }
    private bool isGrounded()
    {
        //RaycastHit raycastHit = Physics.BoxCast(cBoxCollider.bounds.center, cBoxCollider.bounds.size, Vector3.down, Quaternion , 0.1f, groundLayer);
        //return raycastHit.collider != null;
        return true;
    }
    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(cBoxCollider.bounds.center, cBoxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
    public bool canAttack()
    {
        return horizontalInput == 0 && isGrounded() && !onWall();
    }
}
