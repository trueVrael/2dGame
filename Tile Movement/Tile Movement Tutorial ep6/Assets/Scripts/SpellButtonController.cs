using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellButtonController : MonoBehaviour {
    public Image spellImage;
    private int spellID;
    private string spellText;

    private Player player;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update () {
        SetSpell(0);
	}

    private void UpdateSpellImage() {
        SpriteRenderer sprite = player.Spells[spellID].GetComponentInChildren<SpriteRenderer>();
        if (sprite == null) {
            Debug.Log("No sprite image in spell");
            spellImage.sprite = null;
            return;
        }

        spellImage.sprite = sprite.sprite;
    }

    public int GetSpellID() { return spellID; }
    public string GetSpellText() { return spellText; }

    public void SetSpell(int spellID) {
        this.spellID = spellID;
        switch (spellID) {
            case 0:
                spellText = "FireBall";
                break;
            default:
                spellText = "";
                break;
        }

        UpdateSpellImage();
    }

    public void SetSpell(string spellText) {
        this.spellText = spellText;
        switch (spellText) {
            case "FireBall":
                spellID = 0;
                break;
            default:
                spellID = -1;
                break;
        }

        UpdateSpellImage();
    }
}
