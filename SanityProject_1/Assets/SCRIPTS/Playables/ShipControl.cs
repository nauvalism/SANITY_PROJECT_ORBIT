using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipControl : MonoBehaviour
{
    public BaseShip theShip;
    public Transform rotator;
    public Transform defaultPivot;
    [SerializeField] float rotateSpeed;
    [SerializeField] float defaultSpeed = 1;
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

    public void ResetRotationAndMovement()
    {
        ResetRotatePosition();
        //transform.position = Vector3.zero;
        
    }

    public void ResetSpeed()
    {
        this.rotateSpeed = defaultSpeed;
    }

    public void AddSpeed(int i)
    {
        rotateSpeed += (float)i;
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

    public Transform GetRotator()
    {
        return rotator;
    }

    public void ResetRotatePosition()
    {
        //this.rotator.position = defaultPivot.position;
        this.rotator.localPosition = Vector3.zero;
        this.transform.rotation = Quaternion.identity;
        theShip.SwitchScale(1);
    }

    public void RotateToZero(System.Action next)
    {
        move = false;

        if(transform.rotation.z < 180.0f)
        {
            rotateDirection = 1;
        }
        else
        {
            rotateDirection = -1;
        }

        theShip.SwitchScale(rotateDirection);

        LeanTween.cancel(gameObject);
        LeanTween.rotateZ(gameObject, .0f, 1.0f).setEase(LeanTweenType.easeOutQuad).setOnComplete(()=>{
            if(next != null)
            {
                next();
            }
        });
    }
}
