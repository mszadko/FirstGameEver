using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    
    public CircleCollider2D head;
    public float fireRate;
    public float reloadingTime;
    public GameObject bullet;
    public GameObject gunfire;
    public Transform barrel;
    public int playerNumber;
    public AudioSource audioSource;
    public AudioClip shooting;
    public AudioClip emptyChamberClick;
    public AudioClip dying;
    public AudioClip reloading;
    public int ammo = 10;
    public int ammoInMag = 30;
    public GameObject HUDComponent;//hud bar to display
    public bool active;

    Rigidbody2D rbPlayer;
    float horizontal;
    float vertical;
    Vector2 movement;
    float xRotation;
    float yRotation;
    Animator anim;
    int health = 100;
    float MovementSpeed = 2;
    float nextFire;
    Collider2D[] colliders;

    // Use this for initialization
    void Start ()
    {
        active = true;
        DontDestroyOnLoad(gameObject);
        rbPlayer = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        head = GetComponent<CircleCollider2D>();
        colliders = GetComponents<Collider2D>();
        HUDComponent.GetComponentInChildren<Slider>().value = health;
        nextFire = Time.time + 3;
    }
    public void Reset()
    {
        anim = GetComponent<Animator>();
        colliders = GetComponents<Collider2D>();
        nextFire = Time.time + 3;
        health = 100;
        ammo = 10;
        ammoInMag = 30;
        HUDComponent.GetComponentInChildren<Slider>().value = health;
        anim.Play("Idle");
        anim.ResetTrigger("Died");
        active = true;
        foreach (var collider in colliders)
        {
            collider.enabled = true;
        }

    }
    // Update is called once per frame
    void Update()
    {
        if (!active)
        {
            return;
        }
        if (Input.GetButton("Fire1"+playerNumber))
        {
            Shoot();
        }
        if (Input.GetButton("Reload"+playerNumber))
        {
            if (nextFire<Time.time)
            {
                Reload();
            }
        }
    }
    void FixedUpdate()
    {
        if (!active)
        {
            return;
        }
        horizontal = Input.GetAxis("Horizontal"+playerNumber);
        vertical = Input.GetAxis("Vertical"+playerNumber);
        xRotation = Input.GetAxis("xRotation"+playerNumber);
        yRotation = Input.GetAxis("yRotation"+playerNumber);
        Move();
        if (xRotation != 0 || yRotation != 0)
        {
            Rotate();
        }
    }
    void Move()
    {
        movement.Set(horizontal, vertical);
        movement = movement.normalized * MovementSpeed * Time.deltaTime;// we don't want to get speed boost when walking diagonally
        rbPlayer.MovePosition(rbPlayer.position + movement);
        
        if (movement.magnitude!=0)
        {
            anim.SetBool("IsWalking", true);
        }
        else
        {
            anim.SetBool("IsWalking", false);
        }
    }
    void Rotate()
    {
        float heading = Mathf.Atan2(xRotation, yRotation);
        transform.rotation = Quaternion.Euler(0f, 0f, heading * Mathf.Rad2Deg);   
    }
    void Shoot()
    {
        float shotTime = Time.time;//firing time
        if (shotTime > nextFire)
        {
            nextFire = Time.time + fireRate;//time when next shoot can be fired
            if (ammoInMag > 0)
            {
                Instantiate(bullet, barrel.position, barrel.rotation);
                Instantiate(gunfire, barrel.position, barrel.rotation);
                ammoInMag--;
                audioSource.clip = shooting;
            }
            else if (ammoInMag == 0)
            {
                audioSource.clip = emptyChamberClick;
            }
            audioSource.Play();
        }
    }
    void Reload()
    {
        if (ammo == 0||ammoInMag==30) 
            return;
        nextFire = Time.time + reloadingTime;
        int x = 30 - ammoInMag;//checking how many bullet we need to add to mag
        x = Mathf.Min(x, ammo);//if we dont have enough we load what we got
        ammo -= x;
        ammoInMag += x;

        audioSource.clip = reloading;
        audioSource.Play();
    }
    public void TakeDamage(int damage, AudioClip headshot = null)
    {
        if (headshot)
        {
            audioSource.clip = headshot;
            audioSource.Play();
        }
        health -= damage;
        HUDComponent.GetComponentInChildren<Slider>().value = health;
        if (health <= 0)
        {
            active = false;
            audioSource.clip = dying;
            audioSource.Play();
            anim.SetTrigger("Died");
            foreach (var collider in colliders)
            {
                collider.enabled = false;
            }
        }
    } 
}
