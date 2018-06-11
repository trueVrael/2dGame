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
    private readonly string[] ToggleKeys = { "SpellLeft", "SpellUp", "SpellDown", "SpellRight"};
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
    private bool FirstFrame = true;
	void Update () {
        if (FirstFrame)
        {
            AddSpell(0);
            SpellBook.SetActive(SpellBookIsEnabled);
            FirstFrame = false;
        }
		hpBar.maxValue = Player.maxHP;
		hpBar.value = Player.hp;
		hpText.text = "HP: " + Player.hp + "/" + Player.maxHP;

        for (int i = 0; i < 4; i++)
        {
            if (Input.GetButtonUp(ToggleKeys[i]))
            {
                toggles[i].isOn = !toggles[i].isOn;
            }
        }
	}

    public void EnableButtonPressed()
    {
        SpellBookIsEnabled = !SpellBookIsEnabled;
        SpellBook.SetActive(SpellBookIsEnabled);
    } 

    public void AddSpell(int spellID)
    {
        SpellBook.AddSpell(spellID);
        toggles[spellID].GetComponent<SpellButtonController>().SetSpell(spellID);
    }

    private bool IgnoreEvent = false;
    public void SpellUsed()
    {
        if (!spellPanel.AnyTogglesOn()) return;
        IgnoreEvent = true;
        spellPanel.ActiveToggles().FirstOrDefault<Toggle>().isOn = false;
    }

    public void spellToggleChanged(bool state, int toggle) {
        if (IgnoreEvent)
        {
            IgnoreEvent = false;
            return;
        }
        SpellButtonController controller = toggles[toggle].GetComponent<SpellButtonController>();
        string spellText = controller.GetSpellText();
        int spellID = controller.GetSpellID();
        if (spellID == -1) return;
        if (state == true)
        {
            Debug.Log("Spell was selected: " + spellText);
            Player.CheckDirection(spellText);
        }
        else
        {
            Debug.Log("Spell was unselected: " + spellText);
            Player.DestroyTiles();
        }
    }
}
