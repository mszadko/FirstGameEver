using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    public int damage;
    public float speed;
    public AudioClip headshotsound;
    Rigidbody2D rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        //rb.AddForce(transform.up.normalized * speed);
        rb.velocity = transform.up * speed;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Pickable")) return;
        Destroy(gameObject);
        if (other.tag=="Player")
        {
            Player playerScript = other.GetComponent<Player>();
            if (other == playerScript.head)
            {
                playerScript.TakeDamage(damage * 3,headshotsound);
            }
            else
            {
                playerScript.TakeDamage(damage);
            }
        }
       
    }
}
