using UnityEngine;
using System.Collections;

public class PlatformController : MonoBehaviour {

	[HideInInspector] public bool facingRight = true;
	[HideInInspector] public bool jump = false;
	public float moveForce = 365f;
	public float maxSpeed = 5f;
	public float jumpForce = 110f;
	public Transform groundCheck;
	private float trackspeed = 20f;
	private float currentYVel;
	private bool playerDeath = false;
	private bool respawned = false;

	private bool grounded = false;
	private Animator anim;
	private Rigidbody2D rb2d;
	private Vector2 startPosition; 

	// Use this for initialization
	void Awake () 
	{
		anim = GetComponent<Animator>();
		rb2d = GetComponent<Rigidbody2D>();
		currentYVel = rb2d.velocity.y;
		startPosition = transform.position;
	}

	// Update is called once per frame
	void Update () 
	{
		grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

		for (int i = 0; i < Input.touchCount; ++i) {
			if (Input.GetTouch (i).phase == TouchPhase.Began) {
				if (respawned) {
					playerStart ();
				} else {
					if(grounded && !playerDeath){
						jump = true;
					}
				}
			}
		}
		if (Input.GetKeyDown("space"))
		{
			if (respawned) {
				playerStart ();
			} else {
				if(grounded && !playerDeath){
					jump = true;
				}
			}
		}
		//rb2d.AddForce(Vector2.right * 1 * trackspeed);
		if (grounded && !playerDeath) {
			if (jump) {
				Jump ();
			} else {
				rb2d.velocity = new Vector2 (15, rb2d.velocity.y);
			}
		}
		if (playerDeath && !respawned) {
			respawnPlayer ();
		}
	}
	public void setDeath(bool dead){
		playerDeath = dead;
	}

	public void respawnPlayer(){
		transform.position = startPosition;
		rb2d.velocity = new Vector2 (0, 0);
		rb2d.gravityScale = 0;
		setDeath (false);
		respawned = true;
	}

	public void playerStart(){
		rb2d.gravityScale = 1;
		respawned = false;
	}

	void FixedUpdate()
	{
		float h = Input.GetAxis("Horizontal");

		anim.SetFloat("Speed", Mathf.Abs(h));

		if (h > 0 && !facingRight)
			Flip ();
		else if (h < 0 && facingRight)
			Flip ();

		if (grounded && !jump) {
			//print (currentYVel);
			if (rb2d.velocity.y < -0.5) {
				//print ("down");
			}
			if (rb2d.velocity.y > 0.5) {
				//print ("up");
				//rb2d.velocity = new Vector2 (20, 20);
			}
		}
		currentYVel = rb2d.velocity.y;
	}
	void Jump(){
		anim.SetTrigger ("Jump");
		rb2d.velocity = new Vector2(rb2d.velocity.x, 15f);
		jump = false;
	}

	void Flip()
	{
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}