using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeSpellsPanelManager : MonoBehaviour {
    public GameObject SpellPanel;
    public Button enableButton;
    public GameObject changeSpellButtonPrefab;
    private Player player;

    private bool isActive = false;

    private int CurrentI;
    private List<int> spells;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        enableButton.onClick.AddListener(EnableButtonPressed);

        spells = new List<int>();

        gameObject.SetActive(isActive);
	}

    // Update is called once per frame
    private bool added = false;
	void Update () {
        if (!added) {
            AddSpell(0); AddSpell(0);
            added = true;
        }
	}

    public void EnableButtonPressed() {
        isActive = !isActive;
        gameObject.SetActive(isActive);
    }

    public void AddSpell(int spellID) {
        spells.Add(spellID);
        GameObject newbutton = Instantiate(changeSpellButtonPrefab) as GameObject;
        Image image = newbutton.transform.GetChild(0).GetComponent<Image>();
        image.sprite = player.Spells[spellID].GetComponentInChildren<SpriteRenderer>().sprite;
        image.preserveAspect = true;
        newbutton.transform.SetParent(transform);

        int newI = CurrentI;
        CurrentI++;
        newbutton.GetComponent<Button>().onClick.AddListener(() => ButtonPressed(newI));

        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform) transform);
    }

    public void ButtonPressed(int buttonID) {
        Debug.Log("Pressed button of ID: " + buttonID.ToString() + " that has spellID: " + spells[buttonID].ToString());
    }
}
