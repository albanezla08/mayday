using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunnerController : EnemyShip
{
    [SerializeField] private Vector3 stoppingPoint = new Vector3(0.5f, 0); //relative to right edge of screen
    [SerializeField] private float shootAngle = 90; //angle to shoot at
    [SerializeField] private float turnAmount = 10; //angle to shoot at
    [SerializeField] private float shootTime = 5f; //time to spend shooting
    [SerializeField] private Transform firePoint;
    private float shootTimer;


    private GunnerController()
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

    void SwitchToPivoting()
    {
        if (transform.position.x <= CameraController.topritBorder.x - stoppingPoint.x)
        {
            transform.position = new Vector3(CameraController.topritBorder.x - stoppingPoint.x, transform.position.y);
            stateMachine.ChangeState(new RevolvingState(transform, 0, null, 999, Turn, turnAmount, SwitchToGunning));
        }
        UpdateHPBar();
    }

    void SwitchToGunning()
    {
        if (transform.eulerAngles.z > shootAngle - 10 && transform.eulerAngles.z < shootAngle + 10)
        {
            transform.rotation = Quaternion.Euler(0, 0, shootAngle);
            stateMachine.ChangeState(new GunningState(transform, () => ShootBullet(firePoint), shootDelay, SwitchToExit));
        }
    }

    void SwitchToExit()
    {
        if (shootTimer >= shootTime)
        {
            stateMachine.ChangeState(new DiveState(transform, moveDir, flySpeed, () => { UpdateHPBar(); }));
        }
        shootTimer += Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Main Camera")
        {
            stateMachine.ChangeState(new DiveState(transform, moveDir, flySpeed, SwitchToPivoting));
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
        stateMachine.ChangeState(new DiveState(transform, moveDir, flySpeed, SwitchToPivoting));
    }

    /*private void SwitchToShootingShip(Vector3 stoppingPoint)
    {
        if (transform.position.x >= CameraController.btmlftBorder.x + stoppingPoint.x)
        {
            transform.position = new Vector3(CameraController.btmlftBorder.x + stoppingPoint.x, transform.position.y);
            stateMachine.ChangeState(new GunningState(transform,()=> {; },999,);
        }
    }*/
}
