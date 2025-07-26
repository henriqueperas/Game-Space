using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class PlayerMovemment : MonoBehaviour
{
    public int moveSpeed;
    bool ladder;
    Vector2 moveInput;

    Rigidbody2D rb;
    BoxCollider2D bc;
    Animator anim;

    bool ladderAnim = false;

    public GameObject itemHolding;
    public GameObject creator;
    public GameObject broken;

    public Ship ship; 

    public bool holding = false;
    public bool interact;
    public bool canInteract;

    [SerializeField]  bool shipControlling = false;
    [SerializeField] bool colliderCreator;

    public int teste = 0;
    public int itemIDHolding;

    int fallSpeed = 4;
    bool fall = true;

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
        Gravity();

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

        if (shipControlling){
            if (interact)
            {
                shipControlling = false;
            }
            else
            {
                ship.ShipMovemment(moveInput.x, moveInput.y);
            }
        }

    }

    private void FixedUpdate()
    {
        if (!shipControlling)
        {
            rb.velocity = new Vector2(moveInput.x * moveSpeed, rb.velocity.y);
        }

        if (ladder && moveInput.y != 0)
        {
            ladderAnim = true;
        }
        else if (!ladder)
        {
            ladderAnim = false;
        }

        print(ladderAnim);

        Animatons();
    }

    #region Inputs
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
        }
    }

    public void OnAction(InputAction.CallbackContext context)
    {
        interact = context.performed ? true : false; 
    }

    #endregion

    #region Triggers
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
            //rb.gravityScale = 1;
            InLadder(1f);
        }
    }

    #endregion

    #region Collisions
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            fall = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            fall = true;
        }
    }

    #endregion

    #region Ladder
    public void InLadder(float size)
    {
        bc.size = new Vector2(size, bc.size.y);
    }

    public void OnMoveLadder()
    {
        InLadder(0.75f);
        rb.velocity = new Vector2(rb.velocity.x, moveInput.y * moveSpeed);
    }

    #endregion

    #region Interaction
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

    #endregion

    #region Animations

    void Animatons()
    {
        anim.SetBool("walking", moveInput.x != 0 ? true : false);

        anim.SetBool("ladder", ladderAnim);
    }

    #endregion
    void Gravity()
    {
        if (fall && !ladder)
        {
            rb.velocity = -transform.up * fallSpeed;
        }
    }

}
