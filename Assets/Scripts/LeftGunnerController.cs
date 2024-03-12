using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftGunnerController : EnemyShip
{
    private bool started;
    [SerializeField] private Vector3 stoppingPoint1 = new Vector3(0.5f, 0); //relative to left edge of screen
    [SerializeField] private float shootAngle1 = 270; //angle to shoot at when on left
    [SerializeField] private Transform firePoint;
    [SerializeField] private float turnSpeed = 10;
    [SerializeField] private float switchSidesDelay;
    [SerializeField] private Vector3 stoppingPoint2 = new Vector3(0.5f, 0); //relative to right edge of screen
    [SerializeField] private float shootAngle2 = 90; //angle to shoot at when on right
    [SerializeField] private float exitOffScreenDelay;
    private float switchSidesTimer;
    private float exitTimer;


    private LeftGunnerController()
    {
        collideDamage = 10;
        bulletDamage = 6;
        flySpeed = 0.1f;
        shootDelay = 0.6f;

    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.ExecuteStateUpdate();
        if (transform.position.x < CameraController.btmlftBorder.x - 15)
        {
            if (ownHealthBar != null)
            {
                Destroy(ownHealthBar.gameObject);
            }
            Destroy(gameObject);
        }
    }

    void SwitchToRevolving()
    {
        if (transform.position.x <= CameraController.btmlftBorder.x + stoppingPoint1.x)
        {
            transform.position = new Vector3(CameraController.btmlftBorder.x + stoppingPoint1.x, transform.position.y);
            stateMachine.ChangeState(new RevolvingState(transform, flySpeed, () => {; }, 999, Turn, turnSpeed, SwitchToGunningFromLeft));
        }
        UpdateHPBar();
    }

    void SwitchToGunningFromLeft()
    {
        if (transform.eulerAngles.z < 280 && transform.eulerAngles.z > 260)
        {
            transform.eulerAngles = new Vector3(0, 0, 270);
            stateMachine.ChangeState(new GunningState(transform, () => ShootBullet(firePoint), shootDelay, SwitchToDivingToRight));
        }
    }

    void SwitchToDivingToRight()
    {
        if (switchSidesTimer >= switchSidesDelay)
        {
            if (transform.eulerAngles.z < shootAngle1 + 10 && transform.eulerAngles.z > shootAngle1 - 10)
            {
                switchSidesTimer = 0;
                transform.eulerAngles = new Vector3(0, 0, 270);
                stateMachine.ChangeState(new DiveState(transform, moveDir, flySpeed, SwitchToRevolvingRight));
            }
        }
        switchSidesTimer += Time.deltaTime;
    }

    void SwitchToRevolvingRight()
    {
        if (transform.position.x >= CameraController.topritBorder.x - stoppingPoint2.x)
        {
            transform.position = new Vector3(CameraController.topritBorder.x - stoppingPoint2.x, transform.position.y);
            stateMachine.ChangeState(new RevolvingState(transform, 0, () => {; }, 999, Turn, -turnSpeed, SwitchToGunningFromRight));
        }
        UpdateHPBar();
    }

    void SwitchToGunningFromRight()
    {
        if (transform.eulerAngles.z < shootAngle2 + 10 && transform.eulerAngles.z > shootAngle2 - 10)
        {
            transform.eulerAngles = new Vector3(0, 0, shootAngle2);
            stateMachine.ChangeState(new GunningState(transform, () => ShootBullet(firePoint), shootDelay, SwitchToExit));
        }
    }

    void SwitchToExit()
    {
        if (exitTimer >= exitOffScreenDelay)
        {
            exitTimer = 0;
            stateMachine.ChangeState(new DiveState(transform, moveDir, flySpeed, () => { UpdateHPBar(); }));
        }
        exitTimer += Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Main Camera" && !started)
        {
            started = true;
            stateMachine.ChangeState(new DiveState(transform, moveDir, flySpeed, SwitchToRevolving));
        }
        else if (collision.name.Contains("Asteroid"))
        {
            if (ownHealthBar != null)
            {
                Destroy(ownHealthBar.gameObject);
            }
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            audioManager.PlayAudio("Explosion");
            Destroy(gameObject);
        }
    }

    public void VictoryCutscene()
    {
        stateMachine.ChangeState(new DiveState(transform, moveDir, flySpeed, SwitchToRevolvingRight));
    }
}
