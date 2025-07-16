using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class PlayerMovemment : MonoBehaviour
{
    public int moveSpeed;

    [SerializeField] bool ladder;

    Rigidbody2D rb;
    BoxCollider2D bc;

    Vector2 moveInput;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput.x * moveSpeed, rb.velocity.y);
        // animação de movimento
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if (ladder)
        {
            OnMoveLadder();
        }
    }

    public void OnAction(InputAction.CallbackContext context)
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ladder")
        {
            ladder = true;
            rb.gravityScale = 0;
            //collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ladder")
        {
            ladder = false;
            rb.gravityScale = 1;
            //collision.gameObject.GetComponent<BoxCollider2D>().enabled = true;
            InLadder(1f);
        }
    }

    public void InLadder(float size)
    {
        bc.size = new Vector2(size, bc.size.y);
    }

    public void OnMoveLadder()
    {
        InLadder(0.75f);
        rb.velocity = new Vector2(rb.velocity.x, moveInput.y * moveSpeed);
    }

}
