using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour {

	public GameObject gameManager;
    public AudioSource enemyDeath;
	// Use this for initialization
	void Awake () {
		if(GameManager.instance == null)
			Instantiate(gameManager);
	}
}
