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

        spellID = -1;
        spellText = "";
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void UpdateSpellImage() {
        if (spellID == -1) return;
        SpriteRenderer sprite = player.Spells[spellID].GetComponentInChildren<SpriteRenderer>();
        if (sprite == null)
        {
            Debug.Log("No sprite image in spell");
            spellImage.sprite = null;
            Color color = spellImage.color;
            color.a = 0;
            spellImage.color = color;
            return;
        }
        else
        {
			//if(player.avaibleSpells[spellID] == true){
				spellImage.sprite = sprite.sprite;
				spellImage.preserveAspect = true;
				Color color = spellImage.color;
				color.a = 1;
				spellImage.color = color;
			//}
        }
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
