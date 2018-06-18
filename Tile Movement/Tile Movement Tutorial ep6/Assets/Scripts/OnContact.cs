using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnContact : MonoBehaviour {

    public int spellDMG;
    private Player player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();  
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            enemy.LoseHP(spellDMG);
        }

        if (other.gameObject.tag != "Player"/* && other.gameObject.tag!= "Direction"*/) {player.spellUsed = false; Destroy(gameObject); }
    }
}
