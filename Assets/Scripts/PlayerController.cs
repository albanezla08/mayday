using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour, IShootable
{
    //Movement Vars
    [SerializeField] private float moveSpeed = 0.09f;

    //Health
    [SerializeField] private int startingHP = 100;
    public int hpDepletionRate;
    private float hpDepletionTimer;
    public float hpDepletionTime;
    public int hpReplenishRate;
    public float hpReplenishTime;
    [SerializeField] private int hp;

    //Shooting
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject chargedBulletPrefab;
    private string ownName;
    [SerializeField] int baseDamage = 5;
    [SerializeField] int baseFireCost = 2;
    [SerializeField] int chargedDamage = 20;
    [SerializeField] int chargedFireCost = 5;

    //UI
    [SerializeField] private Image healthBar;

    //Death Stuff
    [SerializeField] private GameObject explosionPrefab;
    private AudioManager audioManager;
    private bool isDead;
    public int HP
    {
        get{return hp;}
        set
        {
            if (value <= 0)
            {
                hp = 0;
                healthBar.fillAmount = hp / (float)startingHP;
                if (!isDead)
                {
                    StartCoroutine(DeathEffect());
                }
            }
            else if (value >= startingHP)
            {
                hp = startingHP;
                healthBar.fillAmount = hp / (float)startingHP;
            }
            else
            {
                hp = value;
                healthBar.fillAmount = hp / (float)startingHP;
            }
        }
    }

    //Children and components
    private Rigidbody2D rb;
    private StateMachine stateMachine;

    private IEnumerator DeathEffect()
    {
        isDead = true;
        hpReplenishRate = 0;
        hpDepletionRate = 0;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        audioManager.PlayAudio("Explosion");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("GameScene");
    }
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        stateMachine = new StateMachine();
        HP = startingHP;
        ownName = name;
        audioManager = FindObjectOfType<AudioManager>();
        stateMachine.ChangeState(new ControlledState(moveSpeed, rb, this, Shoot, ChargedShoot));
    }

    void Update()
    {
        if (hpDepletionTimer >= hpDepletionTime)
        {
            hpDepletionTimer = 0;
            HP -= hpDepletionRate;
        }
        stateMachine.ExecuteStateUpdate();
        
        hpDepletionTimer += Time.deltaTime;
    }

    public void Bounce(Vector3 bounceDirection, float bounceForce, float bounceDuration)
    {
        stateMachine.ChangeState(new BouncedState(bounceForce, bounceDirection, bounceDuration, rb, ReturnFromBounce));
    }

    private void ReturnFromBounce()
    {
        stateMachine.ChangeState(new ControlledState(moveSpeed, rb, this, Shoot, ChargedShoot));
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0, 0, 270));
        bullet.GetComponent<BulletController>().damage = baseDamage;
        bullet.GetComponent<BulletController>().moveSpeed = 0.4f;
        bullet.GetComponent<BulletController>().ownerName = ownName;
        HP -= baseFireCost;
    }

    private void ChargedShoot()
    {
        GameObject bullet = Instantiate(chargedBulletPrefab, transform.position, Quaternion.Euler(0, 0, 270));
        bullet.GetComponent<BulletController>().damage = chargedDamage;
        bullet.GetComponent<BulletController>().moveSpeed = 0.5f;
        bullet.GetComponent<BulletController>().ownerName = ownName;
        HP -= chargedFireCost;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        IInteractibleWithShip interactible = collider.gameObject.GetComponent<IInteractibleWithShip>();
        if (interactible != null) {
            interactible.OnPlayerInteract(this);
        }
    }

    void IShootable.OnShot(BulletController bulletScript)
    {
        HP -= bulletScript.damage;
        Destroy(bulletScript.bulletGO);
        audioManager.PlayAudio("Shot");
    }

    public void OnPlayerWon()
    {
        stateMachine.ChangeState(new InactiveState());
        hpDepletionRate = 0;
        hpReplenishRate = 0;
    }
}
