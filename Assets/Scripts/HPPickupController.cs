using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPPickupController : MonoBehaviour, IInteractibleWithShip
{
    [SerializeField] private int healAmount;
    [SerializeField] private Vector2 driftForce;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void IInteractibleWithShip.OnPlayerInteract(PlayerController playerScript)
    {
        playerScript.HP += healAmount;
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Main Camera")
        {
            rb.AddForce(driftForce);
        }
    }
}
