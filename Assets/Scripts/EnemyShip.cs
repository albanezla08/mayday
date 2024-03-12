using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyShip : MonoBehaviour, IInteractibleWithShip, IShootable
{
    [SerializeField] protected int collideDamage;
    [SerializeField] protected int bulletDamage;
    [SerializeField] protected int maxHP;
    protected int hp;
    [SerializeField] protected float flySpeed;
    [SerializeField] protected float shootDelay;
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected GameObject explosionPrefab;
    protected Vector3 moveDir = new Vector3(0, 1);
    protected StateMachine stateMachine = new StateMachine();
    protected AudioManager audioManager;
    protected Canvas canvas;
    [SerializeField] protected Image healthBarUI;
    protected Image ownHealthBar;
    protected Camera mainCam;
    // Start is called before the first frame update
    virtual public void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        canvas = FindObjectOfType<Canvas>();
        mainCam = Camera.main;
        hp = maxHP;
        stateMachine.ChangeState(new InactiveState());
    }

    public void OnPlayerInteract(PlayerController playerScript)
    {
        playerScript.HP -= collideDamage;
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        audioManager.PlayAudio("Explosion");
        if (ownHealthBar != null)
        {
            Destroy(ownHealthBar.gameObject);
        }
        Destroy(gameObject);
    }

    public void OnShot(BulletController bulletScript)
    {
        hp -= bulletScript.damage;
        Destroy(bulletScript.bulletGO);
        if (hp <= 0)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            audioManager.PlayAudio("Explosion");
            if (ownHealthBar != null)
            {
                Destroy(ownHealthBar.gameObject);
            }
            Destroy(gameObject);
        }
        else
        {
            audioManager.PlayAudio("Shot");
            if (ownHealthBar == null)
            {
                ownHealthBar = Instantiate(healthBarUI, canvas.transform);
            }
            UpdateHPBar();
        }
    }

    protected void UpdateHPBar()
    {
        if (ownHealthBar != null)
        {
            ownHealthBar.transform.position = mainCam.WorldToScreenPoint(transform.position + new Vector3(0, 0.6f, 0));
            ownHealthBar.transform.GetChild(0).GetComponent<Image>().fillAmount = hp / (float)maxHP;
        }
    }

    protected void ShootBullet(Transform blasterPoint)
    {
        GameObject bullet = Instantiate(bulletPrefab, blasterPoint.position, transform.rotation);
        bullet.GetComponent<BulletController>().ownerName = name;
        bullet.GetComponent<BulletController>().damage = bulletDamage;
    }

    protected void Turn(float rotateAmount)
    {
        transform.eulerAngles += new Vector3(0f, 0f, rotateAmount);
    }
}
