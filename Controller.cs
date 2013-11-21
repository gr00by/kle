using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour
{
	[HideInInspector]
	public bool facingRight = true;			// For determining which way the player is currently facing.
	[HideInInspector]
	public float direction = 180;
	public float maxSpeed = 5f;				// The fastest the player can travel in the x axis.

	private Animator anim;					// Reference to the player's animator component.
	
	
	void Awake()
	{
		direction = 180;
		anim = GetComponent<Animator>();
	}
	
	
	void Update()
	{

	}
		
	void FixedUpdate ()
	{
		// Input do poruszania
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");
		
		
		// Speed dla animatora zeby wiedzial kiedy animowac a kiedy nie ha
		anim.SetFloat("Speed", Mathf.Abs(h) + Mathf.Abs(v));

		//Okreslenie kierunku w ktory sie porusza postac (azymut)
		if (v > 0 && h > 0)
						direction = 45;
				else if (v < 0 && h > 0)
						direction = 135;
				else if (v < 0 && h < 0)
						direction = 225;
				else if (v > 0 && h < 0)
						direction = 315;
				else if (h > 0)
						direction = 90;
				else if (h < 0)
						direction = 270;
				else if (v > 0)
						direction = 0;
				else if (v < 0)
						direction = 180;

		//Kolejne zmienne dla animatora, zeby wiedzial w ktora strone idziemy
		if (Mathf.Abs (h) > 0)
			anim.SetBool ("Side", true);
		else if (Mathf.Abs (v) > 0)
			anim.SetBool ("Side", false);
		if (v > 0)
			anim.SetBool ("Up", true);
		else if (v < 0)
		    anim.SetBool ("Up", false);

		//Nadajemy gosciowi predkosc w keirunku h
		if (Mathf.Abs (h) > 0) {
			rigidbody2D.velocity = new Vector2(maxSpeed * Mathf.Sign (h), rigidbody2D.velocity.y);
		}
		else
			rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);

		//Nadajemy gosciowi predkosc w keirunku v
		if (Mathf.Abs (v) > 0) {
			rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, maxSpeed * Mathf.Sign (v));
		}
		else
			rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0);

		//Jezeli idziemy pod skosem, musimy wyrownac predkosc wypadkowa, wiec skladowe dzielimy przez sqrt(2)
		if(Mathf.Abs(rigidbody2D.velocity.x) > 0 && Mathf.Abs(rigidbody2D.velocity.y) > 0)
			rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x / Mathf.Sqrt(2), rigidbody2D.velocity.y / Mathf.Sqrt(2));
		//print (Mathf.Pow (rigidbody2D.velocity.x, 2) + Mathf.Pow (rigidbody2D.velocity.y, 2));

		//Odwracamy goscia
		if(h > 0 && !facingRight)
			Flip();
		else if(h < 0 && facingRight)
			Flip();

	}
	
	//Funkcja do odwracarki
	void Flip ()
	{
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
	
	

}
