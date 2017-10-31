using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*

Idle - 0

Jump - 1

Run - 2

Dead - 3

 */


public class PlayerController : MonoBehaviour {

	public float horizontalSpeed = 10f;

	public float jumpSpeed = 500f;

	Rigidbody2D rb;

	SpriteRenderer sr;

	bool isJumping = false;

	public Transform feet;

	public float feetWidth = 0.5f;

	public float feetHeight = 0.1f;

	public bool isGrounded;

	public LayerMask whatIsGround;

	Animator anim;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		sr = GetComponent<SpriteRenderer>();
		anim = GetComponent<Animator>();
	}

	void OnDrawGizmos() {
		Gizmos.DrawWireCube(feet.position, new Vector3(feetHeight, feetWidth, 0f));
	}

	
	// Update is called once per frame
	void Update () {
		
		isGrounded = Physics2D.OverlapBox(new Vector2(feet.position.x, feet.position.y), new Vector2(feetWidth, feetHeight), 360.0f, whatIsGround);
		
		float horizontalInput = Input.GetAxisRaw("Horizontal"); //-1 Esquerda , 1 Direita, 0 Nada
		float horizontalPlayerSpeed = horizontalSpeed * horizontalInput;
		if (horizontalPlayerSpeed != 0) {
			MoveHorizontal(horizontalPlayerSpeed);
		}
		else {
			StopMovingHorizontal();
		}

		if (Input.GetButtonDown("Jump")){
			Jump();
		}
		
		ShowFalling();
	}

	void MoveHorizontal(float speed){
		rb.velocity = new Vector2(speed, rb.velocity.y);

		if (speed<0f){
			sr.flipX = true;
		}
		else if (speed>0f) {
			sr.flipX = false;
		}

		if (!isJumping){
		anim.SetInteger("State", 2);
		}
	}

	void StopMovingHorizontal(){
		rb.velocity = new Vector2 (0f, rb.velocity.y);
		anim.SetInteger("State", 0);
	}

	void ShowFalling(){
		if (rb.velocity.y < 0f){
			anim.SetInteger("State", 1);
		}
	}

	void Jump(){
		if (isGrounded){
			isJumping = true;
		rb.AddForce(new Vector2(0f, jumpSpeed));
		anim.SetInteger("State", 1);
	}
}
	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.layer == LayerMask.NameToLayer("Ground")){
			isJumping = false;
		}
		else {
			isJumping = true;
		}
	}
}
