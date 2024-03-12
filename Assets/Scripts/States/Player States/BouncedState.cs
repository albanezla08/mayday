using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncedState : MonoBehaviour, IState
{
    private float bounceForce;
    private Vector3 bounceDirection;
    private float bounceDuration;
    private float bounceTimer;

    private float constantMoveRate;
    private Vector3 nextPos;
    private Rigidbody2D rb;
    private Action endingAction;

    public BouncedState(float bounceForce, Vector3 bounceDirection, float bounceDuration, Rigidbody2D rb, Action endingAction)
    {
        this.bounceForce = bounceForce;
        this.bounceDirection = bounceDirection;
        this.bounceDuration = bounceDuration;
        this.rb = rb;
        this.endingAction = endingAction;
    }

    public void Enter()
    {
        nextPos = rb.position;
        constantMoveRate = CameraController.moveSpeed;
    }

    public void Execute()
    {
        if (bounceTimer >= bounceDuration)
        {
            endingAction();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (rb.GetComponent<PlayerController>() != null)
            {
                rb.GetComponent<PlayerController>().HP -= 10;
            }
            endingAction();
        }
        nextPos += bounceDirection * (bounceForce + constantMoveRate/bounceDuration) * Time.deltaTime * 40;
        nextPos.x += constantMoveRate * Time.deltaTime * 40;
        rb.MovePosition(nextPos);
        bounceTimer += Time.deltaTime;
    }

    public void Exit()
    {

    }
}
