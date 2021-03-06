using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField]private float speed;
	[SerializeField]private float jumpPower;
	[SerializeField]private LayerMask groundLayer;
	[SerializeField]private LayerMask wallLayer;
	private Rigidbody body;
	private Animator anim;
	private BoxCollider boxCollider;
	private float wallJumpCooldown;
	private float horizontalInput;

	private void Awake()
	{
		//Grab references for rigidbody and animator from object
		body = GetComponent<Rigidbody>();
		anim = GetComponent<Animator>();
		boxCollider = GetComponent<BoxCollider>();
	}
	
	private void Update()
	{
		Debug.Log("Character Updating");

		horizontalInput = Input.GetAxis("Horizontal");

		//Flip player when moving left-right
		if(horizontalInput > 0.01f)
			transform.localScale = Vector3.one;
		else if(horizontalInput < -0.01f)
			transform.localScale = new Vector3(-1,1,1);
	
		//Set animator parameters
		anim.SetBool("Running", horizontalInput != 0);
		anim.SetBool("Grounded", isGrounded());
		
		//Wall jump logic
		if(wallJumpCooldown > 0.2f)
		{
			Debug.Log("Preparing for Moving");
			body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
			if(onWall() && !isGrounded())	
			{
				body.useGravity = false;
				body.velocity = Vector2.zero;
			}
			else
				body.useGravity = true;

			Debug.Log("Preparing for Jumping");
			if (Input.GetKey(KeyCode.Space))
			{
				Debug.Log("Jumping");
				Jump();
			}
		}
		else
			wallJumpCooldown += Time.deltaTime;
	}
	private void Jump()
	{
		if(isGrounded())
		{
			body.velocity = new Vector2(body.velocity.x, jumpPower);
			anim.SetTrigger("Jump");
		}
		else if (onWall() && !isGrounded())
		{
			if (horizontalInput == 0)
			{
				body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x)*10, 0);	
				transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);			
			}
			else
				body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x)*3, 6);
					
			wallJumpCooldown = 0;
		}
	}
	private bool isGrounded()
	{
		//RaycastHit raycastHit = Physics.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, Vector3.down);
		//		RaycastHit raycastHit = Physics.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, Vector3.down, Quaternion.identity, 0.1f, groundLayer);
		//return raycastHit.collider != null;
		return true;
	}
	private bool onWall()
	{
		RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
		return raycastHit.collider != null;
	}
	public bool canAttack()
	{
		return horizontalInput == 0 && isGrounded() && !onWall();
	}
}
