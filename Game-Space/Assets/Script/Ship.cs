using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    [SerializeField] GameObject ship;


    float speed = 10f;
    public void ShipMovemment(float x, float y)
    {
        print(x);
        if(x >= 0)
        {
            //gameObject.transform.localRotation = new Quaternion(0, gameObject.transform.localRotation.y + y, 0, speed);
            ship.transform.Rotate(new Vector3(0,0,y), Space.Self);
            ship.transform.position += ship.transform.right * x;

        }
    }
}
