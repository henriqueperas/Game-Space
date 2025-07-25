using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemCreator : MonoBehaviour
{
    public GameObject creation;

    public int progressCreation = 0;

    [SerializeField] PlayerMovemment player;

    private void Update()
    {
        Creating();

        if (progressCreation >= 200)
        {
            player.teste = 0;
            StartCoroutine(player.TimeOutAction());
            player.InteractionItem(player.creator.gameObject);

            print("foi reparado");
        }
    }

    void Creating()
    {
        if (player != null)
        {
            if (player.interact)
            {
                player.teste += 1;
                progressCreation = player.teste;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player = collision.gameObject.GetComponent<PlayerMovemment>();
            collision.gameObject.GetComponent<PlayerMovemment>().teste = progressCreation;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerMovemment>().creator = gameObject;
            progressCreation = collision.gameObject.GetComponent<PlayerMovemment>().teste;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player = null;
            collision.gameObject.GetComponent<PlayerMovemment>().teste = 0;
            collision.gameObject.GetComponent<PlayerMovemment>().creator = null;
        }
    }

}
