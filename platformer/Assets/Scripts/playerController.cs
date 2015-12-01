using UnityEngine;
using System.Collections;

public class playerController : MonoBehaviour
{
	private Rigidbody2D rb;

	public int speed;
	public float jumpForce;
	public bool isDead;
	private bool canJump;
	// Use this for initialization
	void Start ()
	{
		rb = GetComponent<Rigidbody2D> ();
		rb.velocity = new Vector2 (speed, 0);
		canJump = false;
		;
		isDead = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void FixedUpdate ()
	{
		rb.velocity = new Vector2 (speed, rb.velocity.y);
		if (Input.GetAxis ("Jump") > 0 && canJump) {
			Debug.Log ("JUMP");
			canJump = false;
			rb.velocity = new Vector2 (rb.velocity.x, jumpForce);
		}
	}
	
	void OnCollisionEnter2D (Collision2D collision)
	{
		Debug.Log ("COLLISION");
		canJump = true;
		//Debug.Log (collision.gameObject.name);
		if (collision.gameObject.name.Contains ("spikes")) {
			isDead = true;
		}
	}
}
