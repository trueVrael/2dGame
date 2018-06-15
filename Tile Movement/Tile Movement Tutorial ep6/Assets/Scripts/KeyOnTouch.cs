using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyOnTouch : MonoBehaviour {

    public int KeyType;
    private Player player;
    private UIManager UIManager;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        UIManager = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIManager>();
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.numberOfKeys[KeyType]++;

            UIManager.UpdateKeys();
            Destroy(this.gameObject);
        }
    }
}
