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
    [SerializeField] GameObject itemHolding;
    [SerializeField] GameObject creator;

    public bool holding = false;
    public bool interact;
    public bool canInteract;

    [SerializeField] bool colliderCreator;

    int teste = 0;

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

        colliderCreator = creator != null ? true : false;

        if (holding && interact && canInteract)
        {
            StartCoroutine(TimeOutAction());
            itemHolding.GetComponent<ItemLog>().follow = false;
            itemHolding.GetComponent<Rigidbody2D>().gravityScale = 1;
            itemHolding.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            holding = false;
        }

        if (colliderCreator && interact)
        {
            teste+=1;
            print(teste);
        }

        if(teste >= 200)
        {
            teste = 0;
            StartCoroutine(TimeOutAction());
            InteractionItem(creator.gameObject);
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
        if (collision.gameObject.tag == "Creator")
        {
            creator = collision.gameObject;
        }

        if (interact)
        {
            /*

            //&& canInteract
            if (collision.gameObject.tag == "Creator" )
            {
                colliderCreator = true;
                
                //StartCoroutine(TimeOutAction());
                //InteractionItem(collision.gameObject);
            }
            */
            if (collision.gameObject.tag == "Item" && canInteract && !holding)
            {
                StartCoroutine(TimeOutAction());
                itemHolding = collision.gameObject;
                collision.GetComponent<ItemLog>().follow = true;
                holding = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Creator")
        {
            creator = null;
            //colliderCreator = false;
            //StartCoroutine(TimeOutAction());
            //InteractionItem(collision.gameObject);
        }

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

    void InteractionItem(GameObject item)
    {
        holding = true;
        itemHolding = Instantiate(item.gameObject.GetComponent<ItemCreator>().creation);
        itemHolding.GetComponent<ItemLog>().player = gameObject.GetComponent<PlayerMovemment>();
        itemHolding.GetComponent<ItemLog>().follow = true;
        
    }

    void DropItem()
    {
        StartCoroutine(TimeOutAction());
        itemHolding.GetComponent<ItemLog>().follow = false;
        itemHolding.GetComponent<Rigidbody2D>().gravityScale = 1;
        itemHolding.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        holding = false;
    }

    IEnumerator TimeOutAction()
    {
        canInteract = false;
        yield return new WaitForSeconds(0.5f);
        canInteract = true;
    }

}
