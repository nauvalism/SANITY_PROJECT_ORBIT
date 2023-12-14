using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipControl : MonoBehaviour
{
    public BaseShip theShip;
    public Transform rotator;
    public float rotateSpeed;
    public int rotateDirection;
    public bool move = true;

    public void EnableMovement()
    {

    }

    public void DisableMovement()
    {
        
    }

    private void FixedUpdate()
    {
        if(theShip.Alive() && move)
            transform.Rotate(0, 0, Time.fixedDeltaTime * rotateSpeed);
    }
}
