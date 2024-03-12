using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiverController : EnemyShip
{
    private DiverController()
    {
        collideDamage = 20;
        flySpeed = 0.2f;
    }

    private void Update()
    {
        stateMachine.ExecuteStateUpdate();
        if (transform.position.x < CameraController.btmlftBorder.x)
        {
            if (ownHealthBar != null)
            {
                Destroy(ownHealthBar.gameObject);
            }
            Destroy(gameObject);
        }
        UpdateHPBar();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Main Camera")
        {
            stateMachine.ChangeState(new DiveState(transform, moveDir, flySpeed, () => {; }));
        }
        else if (collision.GetComponent<IShootable>() != null && collision.GetComponent<PlayerController>() == null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            collision.GetComponent<IShootable>().OnShot(new BulletController(collideDamage, name, gameObject));
        }
    }
}
