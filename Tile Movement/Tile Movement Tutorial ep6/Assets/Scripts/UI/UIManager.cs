using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public Slider hpBar;
	public Text hpText;
	public Player Player;

    public ToggleGroup spellPanel;
    private Toggle[] toggles;

    public Button enableButton;
    private bool SpellBookIsEnabled;
    public SpellBookController SpellBook;

	// Use this for initialization
	private static bool UIExists;
	
	void Start () {
		if(!UIExists){
			UIExists = true;
			DontDestroyOnLoad(transform.gameObject);
		}
		else
			Destroy(gameObject);

        toggles = spellPanel.GetComponentsInChildren<Toggle>();
        for (int i = 0; i < toggles.Length; i++) {
            int newi = i;
            toggles[i].onValueChanged.AddListener((bool state) => spellToggleChanged(state, newi));
        }

        enableButton.onClick.AddListener(EnableButtonPressed);
        SpellBookIsEnabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		hpBar.maxValue = Player.maxHP;
		hpBar.value = Player.hp;
		hpText.text = "HP: " + Player.hp + "/" + Player.maxHP;
	}

    public void EnableButtonPressed()
    {
        SpellBookIsEnabled = !SpellBookIsEnabled;
        SpellBook.SetActive(SpellBookIsEnabled);
    } 

    void spellToggleChanged(bool state, int toggle) {
        SpellButtonController controller = toggles[toggle].GetComponent<SpellButtonController>();
        string spellText = controller.GetSpellText();
        int spellID = controller.GetSpellID();
        if (state == true)
        {
            Debug.Log("Spell was selected: " + spellText);
        }
        else {
            Debug.Log("Spell was unselected: " + spellText);
        }
    }
}
