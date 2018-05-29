using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellDirection : MonoBehaviour {

    private Player player;

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
	
    private void OnMouseDown()
    {
        player.UseSpell(gameObject.transform);
        player.DestroyTiles();
    }
}
