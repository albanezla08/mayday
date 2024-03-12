using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevolvingController : EnemyShip
{
    [SerializeField] private Vector3 stoppingPoint = new Vector3(1f, 0); //relative to right edge of screen
    [SerializeField] private Transform firePoint;
    [SerializeField] private float turnAmount = 10f;
    private bool positionReached;

    public int loops;

    private RevolvingController()
    {
        collideDamage = 10;
        bulletDamage = 6;
        flySpeed = 0.1f;
        shootDelay = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.ExecuteStateUpdate();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Main Camera")
        {
            stateMachine.ChangeState(new RevolvingState(transform, flySpeed, () => ShootBullet(firePoint), shootDelay, Turn, turnAmount, () => {; }));
        }
    }

    
}
