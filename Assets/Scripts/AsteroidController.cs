using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour, IInteractibleWithShip, IShootable
{
    [SerializeField] private float bounceForce = 0.01f;
    [SerializeField] private float bounceDuration = 1f;
    [SerializeField] private int damage = 10;
    [SerializeField] private int hp = 15;
    private ParticleSystem ps;
    private int HP
    {
        get
        {
            return hp;
        }
        set
        {
            if (value <= 0)
            {
                hp = 0;
                audioManager.PlayAudio("Explosion");
                Destroy(gameObject);
            }
            else
            {
                hp = value;
                audioManager.PlayAudio("Shot");
            }
        }
    }
    [SerializeField] private float ownBounceForce;
    private Vector3 ownBounceDir;
    private Vector3 playerBounceDir;
    private BoxCollider2D bc;
    private Rigidbody2D rb;

    private AudioManager audioManager;

    private void Start()
    {
        bc = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        ps = GetComponent<ParticleSystem>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void Update()
    {
        if (transform.position.x < CameraController.btmlftBorder.x)
        {
            Destroy(gameObject);
        }
    }

    public void OnPlayerInteract(PlayerController playerScript)
    {
        playerScript.HP -= damage;
        if (playerScript.transform.position.y < bc.bounds.min.y)
        {
            /*playerScript.Bounce(new Vector3(0f, -1f, 0f), bounceForce, bounceDuration);
            rb.AddForce(new Vector2(0f, 1f) * ownBounceForce);*/
            ownBounceDir.y = 1;
            playerBounceDir.y = -1;
        }
        else if (playerScript.transform.position.y > bc.bounds.max.y)
        {
            /*playerScript.Bounce(new Vector3(0f, 1f, 0f), bounceForce, bounceDuration);
            rb.AddForce(new Vector2(0f, -1f) * ownBounceForce);*/
            ownBounceDir.y = -1;
            playerBounceDir.y = 1;
        }
        if (playerScript.transform.position.x < bc.bounds.min.x)
        {
            /*playerScript.Bounce(new Vector3(-2f, 0f, 0f), bounceForce, bounceDuration);
            rb.AddForce(new Vector2(1f, 0f) * ownBounceForce);*/
            ownBounceDir.x = 1;
            playerBounceDir.x = -2;
        }
        else if (playerScript.transform.position.x > bc.bounds.max.x)
        {
            /*playerScript.Bounce(new Vector3(1f, 0f, 0f), bounceForce, bounceDuration);
            rb.AddForce(new Vector2(-1f, 0f) * ownBounceForce);*/
            ownBounceDir.x = -1;
            playerBounceDir.x = 1;
        }
        rb.AddForce(ownBounceDir * ownBounceForce);
        playerScript.Bounce(playerBounceDir, bounceForce, bounceDuration);
        HP -= 7;
        ownBounceDir *= 0;
        playerBounceDir *= 0;
    }

    void IShootable.OnShot(BulletController bulletScript)
    {
        ps.Play();
        HP -= bulletScript.damage;
        Destroy(bulletScript.bulletGO);
    }

    void OnTriggerEnter2D (Collider2D collider)
    {
        if (collider.name.Contains("Asteroid"))
        {
            Vector2 velocity = rb.velocity;
            collider.GetComponent<Rigidbody2D>().AddForce(velocity.normalized * bounceForce);
            rb.AddForce(-velocity.normalized * bounceForce);
        }
    }
}
