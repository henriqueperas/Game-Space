using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public bool contact = false;

    float rand;

    private void Start()
    {
        rand = Random.Range(-100, 100);
        print(rand);
    }
    private void Update()
    {
        if (contact)
        {
            GameObject.Find("Space").GetComponent<AsteroidSpawn>().i--;
            Destroy(gameObject);

        }

        gameObject.transform.Rotate(new Vector3(0, 0, (rand / 200)), Space.World);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == gameObject.tag)
        {
            contact = true;
        }
    }
}
