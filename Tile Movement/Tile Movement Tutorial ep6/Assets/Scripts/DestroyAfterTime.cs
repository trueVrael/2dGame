using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour {

    public float timeToDestruction;
    private Player player;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        Invoke("DestroyMe", 0.25F);
	}

    private void DestroyMe()
    {
        player.spellUsed = false;       
        Destroy(gameObject);
    }
	
}
