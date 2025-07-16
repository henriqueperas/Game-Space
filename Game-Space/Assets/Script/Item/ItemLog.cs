using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLog : MonoBehaviour
{
    Rigidbody2D rb;

    public PlayerMovemment player;

    public bool follow;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (follow)
        {
            FollowPlayer();
        }
    }

    public void FollowPlayer()
    {
        transform.position = player.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            rb.gravityScale = 0;
            rb.velocity = Vector3.zero;
        }
    }
}
