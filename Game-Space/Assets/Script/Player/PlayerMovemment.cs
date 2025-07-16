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
    GameObject item;

    public bool holding = false;
    public bool interact;
    public bool canInteract;

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
        if (holding && interact)
        {
            item.GetComponent<ItemLog>().follow = false;
            //item = null;
            holding = false;
        }
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
        interact = context.performed ? true : false; 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ladder")
        {
            ladder = true;
            rb.gravityScale = 0;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (interact)
        {
            if (collision.gameObject.tag == "Creator")
            {
                StartCoroutine(TimeOutAction());
                GameObject c = Instantiate(collision.gameObject.GetComponent<ItemCreator>().creation);
                c.GetComponent<ItemLog>().player = gameObject.GetComponent<PlayerMovemment>();
                c.GetComponent<ItemLog>().follow = true;
                holding = true;
                item = c;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ladder")
        {
            ladder = false;
            rb.gravityScale = 1;
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

    IEnumerator TimeOutAction()
    {
        canInteract = false;
        yield return new WaitForSeconds(1f);
        canInteract = true;
    }

}
