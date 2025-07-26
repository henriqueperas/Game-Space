using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawn : MonoBehaviour
{
    [SerializeField] float xAxis;
    [SerializeField] float yAxis;

    [SerializeField] int amountAsteroid;

    [SerializeField] GameObject[] asteroids;

    [SerializeField] GameObject currentAsteroid;

    public int i;

    // Start is called before the first frame update
    void Start()
    {
        Spawn(amountAsteroid);
    }

    void Spawn(int amount)
    {

        for (i = 0; i < amount; i++)
        {
            currentAsteroid = Instantiate(asteroids[Random.Range(0, asteroids.Length)]);

            currentAsteroid.transform.SetParent(gameObject.transform);

            currentAsteroid.transform.position = new Vector2 (Random.Range((-xAxis + (xAxis / 2)), (xAxis + (xAxis / 2))), Random.Range(-yAxis, yAxis));
            
        }
    }

}
