using UnityEngine;
using System.Collections;

public class SimplePlatformController : MonoBehaviour {

	[HideInInspector] public bool facingRight = true;
	[HideInInspector] public bool jump = false;

	public float moveForce = 365f;
	public float maxSpeed = 5f;
	public float jumpForce = 1000f;
	public Transform groundCheck;
    private Vector3 startPosition;

	private bool grounded = false;
	private Animator anim;
	private Rigidbody2D rb2d;

	// Use this for initialization
	void Awake() 
	{
        startPosition = GetComponent<Transform>().position;
		anim = GetComponent<Animator>();
		rb2d = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update() 
	{
        Vector3 screenCoords = Camera.main.WorldToViewportPoint(this.transform.position);
        if (screenCoords.x < 0.0f || screenCoords.x > 1.0f || screenCoords.y < 0.0f || screenCoords.y > 1.0f)
        {
            rb2d.velocity = new Vector3(0, 0, 0);
            rb2d.position = startPosition;
        }

		//grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        grounded = Physics2D.BoxCast(transform.position, this.GetComponent<Collider2D>().bounds.extents, 270.0f, Vector2.down, 1 << LayerMask.NameToLayer("Ground"));

		if (Input.GetButtonDown ("Jump") && grounded == true) 
		{
			jump = true;
		}
	}

	void FixedUpdate()
	{
		float h = Input.GetAxis("Horizontal");

		if(jump)
		{
			anim.SetTrigger("Jump");
            rb2d.AddForce(new Vector2(0, jumpForce));
			jump = false;
		}

		anim.SetFloat("Speed", Mathf.Abs(h));
		if (h * rb2d.velocity.x < maxSpeed)
		{
			rb2d.AddForce(Vector2.right * h * moveForce);
		}

		if(Mathf.Abs(rb2d.velocity.x) > maxSpeed)
		{
			rb2d.velocity = new Vector2(Mathf.Sign (rb2d.velocity.x) * maxSpeed,
			                            rb2d.velocity.y);
		}

		if (h > 0 && !facingRight)
		{
			Flip ();
		} else if (h < 0 && facingRight)
		{
			Flip ();
		}
	}

	void Flip()
	{
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
