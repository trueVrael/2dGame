using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnContact : MonoBehaviour {

    public int spellDMG;

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            enemy.LoseHP(spellDMG);
        }

        if (other.gameObject.tag != "Player" && other.gameObject.tag!= "Direction") { Destroy(gameObject); }
    }
}
