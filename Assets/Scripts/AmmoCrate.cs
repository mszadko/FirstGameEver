using UnityEngine;
using System.Collections;

public class AmmoCrate : MonoBehaviour {

    public int ammo;
    // Use this for initialization
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player p = other.GetComponent<Player>();
            p.ammo += ammo;
            DestroyObject(gameObject);
        }
    }
}
