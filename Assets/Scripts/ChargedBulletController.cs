using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargedBulletController : BulletController
{
    public ChargedBulletController(int damage, string ownerName, GameObject bulletGO):base(damage, ownerName, bulletGO)
    {
    }
    public override void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.name != ownerName)
        {
            if (collider.GetComponent<IShootable>() != null)
            {
                foreach (Collider2D body in Physics2D.OverlapCircleAll(transform.position, 3f))
                {
                    IShootable bodyScript = body.GetComponent<IShootable>();
                    if (bodyScript != null && body.name != ownerName)
                    {
                        bodyScript.OnShot(this);
                    }
                }
            }
        }
    }
}
