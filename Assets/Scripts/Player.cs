using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 3f;
    public float jumpForce = 5f;
    public float xInput;
    public bool isGround;
    public SceneLoader sceneLoader;

    private Rigidbody2D playerRb;
    private SpriteRenderer spriteRenderer;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x > 50) // 특정 위치 값
        {
            sceneLoader.LoadNextScene();
        }


    }

    private void FixedUpdate() {
        xInput =Input.GetAxis("Horizontal");
        float jumpInput = Input.GetAxisRaw("Jump");

        if (isGround && jumpInput != 0){
            playerRb.velocity = new Vector2(xInput * speed, playerRb.velocity.y + jumpInput * jumpForce);
            anim.SetBool("Jumping", true);
        } else {
            playerRb.velocity = new Vector2(xInput * speed, playerRb.velocity.y);
        }
        
    }

    private void LateUpdate() {
        anim.SetFloat("Speed", Mathf.Abs(xInput));
        if (xInput != 0){
            spriteRenderer.flipX = xInput < 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Ground")){
            isGround = true;
            anim.SetBool("Jumping", false);
        }
    }

    private void OnCollisionExit2D(Collision2D other) {
        if (other.gameObject.CompareTag("Ground")){
            isGround = false;
        } 
    }
}
