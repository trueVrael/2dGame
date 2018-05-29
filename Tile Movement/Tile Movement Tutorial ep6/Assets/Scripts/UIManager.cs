using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public Slider hpBar;
	public Text hpText;
	public Player Player;
	// Use this for initialization
	private static bool UIExists;
	
	void Start () {
		if(!UIExists){
			UIExists = true;
			DontDestroyOnLoad(transform.gameObject);
		}
		else
			Destroy(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		hpBar.maxValue = Player.maxHP;
		hpBar.value = Player.hp;
		hpText.text = "HP: " + Player.hp + "/" + Player.maxHP;
	}
}
