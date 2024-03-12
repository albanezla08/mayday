using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunningState : IState
{
    private Transform shipTransform;
    private Action shootMethod;
    private float shootDelay;
    private Action exitCheck;

    private float shootTimer;
    private Vector3 offsetVec;
    private float constantMoveRate;

    public GunningState(Transform shipTransform, Action shootMethod, float shootDelay, Action exitCheck)
    {
        this.shipTransform = shipTransform;
        this.shootMethod = shootMethod;
        this.shootDelay = shootDelay;
        this.exitCheck = exitCheck;
    }

    void IState.Enter()
    {
        constantMoveRate = CameraController.moveSpeed;
    }

    void IState.Execute()
    {
        offsetVec.x = constantMoveRate * Time.deltaTime * 40;
        shipTransform.position += offsetVec;
        if (shootTimer >= shootDelay)
        {
            shootTimer = 0;
            shootMethod();
        }
        shootTimer += Time.deltaTime;
        exitCheck();
    }

     void IState.Exit()
    {

    }
}
