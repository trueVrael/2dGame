using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollOnTouch : MonoBehaviour {

    public int spellNumber;
    private Player player;
	//private SpellButtonController spell;	
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
	//	spell = GetComponent<SpellButtonController>();
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.avaibleSpells[spellNumber] = true;
		//	spell.UpdateSpellImage();
            Destroy(this.gameObject);
        }
    }
}
