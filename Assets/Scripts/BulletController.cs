using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] Vector3 moveDir = new Vector3(0, 1);
    public float moveSpeed = 0.6f;
    private Vector3 moveVec;
    public int damage = 3;
    public string ownerName = "";
    public GameObject bulletGO;

    public BulletController(int damage, string ownerName, GameObject bulletGO)
    {
        this.damage = damage;
        this.ownerName = ownerName;
        this.bulletGO = bulletGO;
    }

    void Start()
    {
        bulletGO = gameObject;
        moveVec = moveDir * moveSpeed;
    }

    void Update()
    {

        transform.position += transform.TransformVector(moveVec * Time.deltaTime * 40);
        if (transform.position.x < CameraController.btmlftBorder.x || transform.position.x > CameraController.topritBorder.x
            || transform.position.y < CameraController.btmlftBorder.y || transform.position.y > CameraController.topritBorder.y)
        {
            Destroy(gameObject);
        }
    }

    public virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.name != ownerName)
        {
            IShootable shootable = collider.gameObject.GetComponent<IShootable>();
            if (shootable != null) {
                shootable.OnShot(this);
            }
        }
    }
}
