using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiveState : IState
{
    private Transform shipTransform;
    private Vector3 moveDir;
    private float flySpeed;
    private Action exitCheck;

    private Vector3 moveVec;
    private Vector3 offsetVec; //used for offsets that dont care about ship's facing
    private float constantMoveRate;

    public DiveState(Transform shipTransform, Vector3 moveDir, float flySpeed, Action exitCheck)
    {
        this.shipTransform = shipTransform;
        this.moveDir = moveDir;
        this.flySpeed = flySpeed;
        this.exitCheck = exitCheck;
    }

    public void Enter()
    {
        constantMoveRate = CameraController.moveSpeed;
    }

    public void Execute()
    {
        
        moveVec = moveDir * flySpeed * Time.deltaTime * 40;
        offsetVec.x = constantMoveRate * Time.deltaTime * 40; // to keep up with camera
        shipTransform.position += shipTransform.TransformVector(moveVec);
        shipTransform.position += offsetVec;
        exitCheck();
    }

    public void Exit()
    {

    }
}
