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
    public GameObject itemHolding;
    public GameObject creator;
    public GameObject broken;


    public Ship ship; 

    public bool holding = false;
    public bool interact;
    public bool canInteract;

    [SerializeField]  bool shipControlling = false;

    [SerializeField] bool colliderCreator;

    Animator anim;

    public int teste = 0;

    public int itemIDHolding;

    Vector2 moveInput;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
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

        if(moveInput.x == 1)
        {
            gameObject.transform.localScale = new Vector2(1, 1);
        }
        else if(moveInput.x == -1)
        {
            gameObject.transform.localScale = new Vector2(-1, 1);
        }

        if (moveInput.x != 0)
        {
            anim.SetBool("walking", true);
        }
        else
        {
            anim.SetBool("walking", false);
        }


    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput.x * moveSpeed, rb.velocity.y);
        // animação de movimento
    }

    public void OnMove(InputAction.CallbackContext context)
    {

        if (!shipControlling)
        {
            moveInput = context.ReadValue<Vector2>();

            if (ladder)
            {
                OnMoveLadder();
            }
        }
        
    }

    public void OnMoveShip(InputAction.CallbackContext context)
    {
        if (shipControlling)
        {
            moveInput = context.ReadValue<Vector2>();

            ship.ShipMovemment(moveInput.x, moveInput.y);
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

            if (collision.gameObject.tag == "Item" && canInteract && !holding)
            {
                StartCoroutine(TimeOutAction());
                itemHolding = collision.gameObject;
                collision.GetComponent<ItemLog>().follow = true;
                itemIDHolding = collision.GetComponent<ItemLog>().itemID;
                holding = true;
            }

            if(collision.gameObject.tag == "Controller" && canInteract)
            {
                shipControlling = true;
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

    public void InteractionItem(GameObject item)
    {
        print("aqui");
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

    public IEnumerator TimeOutAction()
    {
        canInteract = false;
        yield return new WaitForSeconds(0.5f);
        canInteract = true;
    }

}
