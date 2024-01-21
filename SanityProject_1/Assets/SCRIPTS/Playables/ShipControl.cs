using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipControl : MonoBehaviour
{
    public BaseShip theShip;
    public Transform rotator;
    [SerializeField] float rotateSpeed;
    [SerializeField] int rotateDirection;
    [SerializeField] bool move = true;

    public void EnableMovement()
    {
        move = true;
    }

    public void DisableMovement()
    {
        move = false;
    }



    private void FixedUpdate()
    {
        if(theShip.Alive() && move)
            transform.Rotate(0, 0, Time.fixedDeltaTime * rotateSpeed * rotateDirection);
    }

    public void SwitchDirection()
    {
        if(rotateDirection == 1)
        {
            rotateDirection = -1;
        }
        else
        {
            rotateDirection = 1;
        }
    }

    public int GetDirection()
    {
        return rotateDirection;
    }
}
