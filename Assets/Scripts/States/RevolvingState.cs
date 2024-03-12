using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevolvingState : IState
{
    private Transform shipTransform;
    private float moveSpeed;
    private Action ShootMethod;
    private Transform shootingPoint;
    private float shootDelay;
    private Action<float> TurnMethod;
    private float turnAmount;
    private float exitAngle = 270;
    private Action exitCheck;
    
    
    
    private Vector3 moveVec;
    private Vector3 offsetVec;
    private float constantMoveRate;
    private float shootTimer;
    private int loops;
    private float startingRotation;

    public RevolvingState(Transform shipTransform, float moveSpeed, Action shootMethod, float shootDelay, Action<float> turnMethod, float turnAmount, Action exitCheck)
    {
        this.shipTransform = shipTransform;
        this.moveSpeed = moveSpeed;
        ShootMethod = shootMethod;
        this.shootDelay = shootDelay;
        TurnMethod = turnMethod;
        this.turnAmount = turnAmount;
        this.exitCheck = exitCheck;
    }

    void IState.Enter()
    {
        startingRotation = shipTransform.eulerAngles.z;
        constantMoveRate = CameraController.moveSpeed;
    }

    void IState.Execute()
    {
        moveVec.y = moveSpeed;
        offsetVec.x = constantMoveRate * Time.deltaTime * 40;
        shipTransform.position += shipTransform.TransformVector(moveVec * Time.deltaTime * 40);
        shipTransform.position += offsetVec;
        TurnMethod(turnAmount);
        if (shootTimer >= shootDelay)
        {
            ShootMethod();
            shootTimer = 0;
        }
        
        // if (shipTransform.eulerAngles.z == startingRotation)
        // {
        //     Debug.Log("rotated");
        // }
        shootTimer += Time.deltaTime;
        exitCheck();
    }

    void IState.Exit()
    {
        
    }
}
