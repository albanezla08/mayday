using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlledState : IState
{
    private float moveSpeed = 0.09f;
    private float constantMoveRate;
    private Vector3 moveVec; //vec added to current position (currentPos + moveVec = nextPos)
    private Vector3 nextPos; //the direct vector that the rb moves to
    private Action Shoot; //shoots a bullet
    private Action ChargedShoot; //shoots a secondary bullet

    //hp
    private float hpReplenishTimer;

    //shooting
    private bool isKeyDown;
    private float bulletKeyHeldTimer;
    private float chargeTime = 0.6f;

    Rigidbody2D rb;
    PlayerController playerScript;

    public ControlledState(float moveSpeed, Rigidbody2D rb, PlayerController playerScript, Action Shoot, Action ChargedShoot)
    {
        this.moveSpeed = moveSpeed;
        this.rb = rb;
        this.playerScript = playerScript;
        this.Shoot = Shoot;
        this.ChargedShoot = ChargedShoot;
    }

    public void Enter()
    {
        nextPos = rb.position;
        constantMoveRate = CameraController.moveSpeed;
    }

    public void Execute()
    {
        //Movement
        moveVec.x = Input.GetAxisRaw("Horizontal") * moveSpeed;
        moveVec.y = Input.GetAxisRaw("Vertical") * moveSpeed;
        //prevents faster movement diagonally; removed cause it doesnt feel good
        /*if (moveVec.magnitude > moveSpeed)
        {
            moveVec /= 2;
        }*/
        nextPos.x += constantMoveRate * Time.deltaTime * 40;
        if (moveVec.x < 0 && nextPos.x < CameraController.btmlftBorder.x)
        {
            moveVec.x = 0;
        }
        else if (moveVec.x > constantMoveRate && nextPos.x > CameraController.topritBorder.x)
        {
            moveVec.x = 0;
        }
        if (moveVec.y < 0 && nextPos.y < CameraController.btmlftBorder.y)
        {
            moveVec.y = 0;
        }
        else if (moveVec.y > 0 && nextPos.y > CameraController.topritBorder.y)
        {
            moveVec.y = 0;
        }
        nextPos += moveVec * Time.deltaTime * 40;

        if (rb.position.x > CameraController.topritBorder.x || rb.position.x < CameraController.btmlftBorder.x //if the player is off screen
            || rb.position.y > CameraController.topritBorder.y || rb.position.y < CameraController.btmlftBorder.y) //take extra damage
        {
            hpReplenishTimer += Time.deltaTime; //currently making the replenish timer multipurpose cause lazy
            if (hpReplenishTimer >= 1)
            {
                hpReplenishTimer = 0;
                playerScript.HP -= 1;
            }
        }
        /*else if (moveVec == new Vector3()) //if the player isn't moving, they'll heal
        {
            hpReplenishTimer += Time.deltaTime;
            if (hpReplenishTimer >= playerScript.hpReplenishTime)
            {
                hpReplenishTimer = 0;
                playerScript.HP += playerScript.hpReplenishRate;
            }
        }*/
        else
        {
            hpReplenishTimer = 0;
        }
        rb.MovePosition(nextPos);

        //checks if bullet was shot
        if (Input.GetKeyDown(KeyCode.C))
        {
            isKeyDown = true;
        }
        if (isKeyDown)
        {
            if (bulletKeyHeldTimer >= chargeTime)
            {
                ChargedShoot();
                isKeyDown = false;
                bulletKeyHeldTimer = 0;
            }
            else if (Input.GetKeyUp(KeyCode.C))
            {
                Shoot();
                isKeyDown = false;
                bulletKeyHeldTimer = 0;
            }
            bulletKeyHeldTimer += Time.deltaTime;
        }
    }

    public void Exit()
    {

    }
}
