using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Broken : MonoBehaviour
{ 
    public int ID;

    public int progressRepair = 0;

    bool broken;

    public bool canRepair = false;

    [SerializeField] PlayerMovemment player;

    private void Update()
    {
        Repairing();
        BrokenInteract();

        if (progressRepair >= 200)
        {
            progressRepair = 0;
            canRepair = false;
            print("foi reparado");
        }
    }

    void Repairing()
    {
        if(player != null)
        {
            if (canRepair && player.interact)
            {
                player.teste += 1;
                progressRepair = player.teste;
            }
        }
    }

    void BrokenInteract()
    {
        if (player.itemIDHolding == ID && player.itemHolding != null && player.interact)
        {
            //executa
            Destroy(player.itemHolding);
            player.itemHolding = null;
            player.holding = false;
            canRepair = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //print("aqui");
            player = collision.gameObject.GetComponent<PlayerMovemment>();
            player.teste = progressRepair;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && player.interact)
        {
            

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //print("aqui");
            player = null;
            collision.gameObject.GetComponent<PlayerMovemment>().teste = 0;
        }
    }

}
