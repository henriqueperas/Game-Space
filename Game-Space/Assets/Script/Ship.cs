using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    float speed = 10f;
    public void ShipMovemment(float x, float y)
    {

        print(x + y);
        if(x >= 0)
        {
            //gameObject.transform.localRotation = new Quaternion(0, gameObject.transform.localRotation.y + y, 0, speed);
            gameObject.transform.Rotate(new Vector3(0,y,0), Space.World);

            gameObject.transform.position = new Vector3(gameObject.transform.position.x + x, 0,0);
        }
    }
}
