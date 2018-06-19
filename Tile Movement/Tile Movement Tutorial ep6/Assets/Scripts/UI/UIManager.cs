using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public Slider hpBar;
	public Text hpText;
	public Player Player;

    public Sprite[] KeyTypeSprites;
    public GameObject KeyPanel;
    public GameObject KeyShowerPrefab;
    private List<GameObject> KeyBars = new List<GameObject>();

    public ToggleGroup spellPanel;
    private readonly string[] ToggleKeys = { "Spell1", "Spell2", "Spell3", "Spell4"};
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
		if(!Player){
			Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		}
        toggles = spellPanel.GetComponentsInChildren<Toggle>();
        for (int i = 0; i < toggles.Length; i++) {
            int newi = i;
            toggles[i].onValueChanged.AddListener((bool state) => spellToggleChanged(state, newi));
        }

        enableButton.onClick.AddListener(EnableButtonPressed);
        SpellBookIsEnabled = false;

        for (int i = 0; i < Player.numberOfKeys.Length; i++)
        {
            GameObject newPanel = new GameObject("", typeof(RectTransform));
            HorizontalLayoutGroup hlg = newPanel.AddComponent<HorizontalLayoutGroup>();
            hlg.childAlignment = TextAnchor.UpperRight;
            hlg.spacing = -2 * KeyShowerPrefab.GetComponent<RectTransform>().rect.width;
            hlg.childControlHeight = false;
            hlg.childControlWidth = false;
            hlg.childForceExpandHeight = false;
            hlg.childForceExpandWidth = false;
            newPanel.transform.SetParent(KeyPanel.transform);
            KeyBars.Add(newPanel);
        }
	}

    // Update is called once per frame
    private bool FirstFrame = true;
	void Update () {
		if(!Player){
			Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		}
        if (FirstFrame)
        {
            SpellBook.SetActive(SpellBookIsEnabled);
            UpdateKeys();

            FirstFrame = false;
        }
		hpBar.maxValue = Player.maxHP;
		hpBar.value = Player.hp;
		hpText.text = "HP: " + Player.hp + "/" + Player.maxHP;

        for (int i = 0; i < 4; i++)
        {
            if (Input.GetButtonUp(ToggleKeys[i]))
            {
                if (Player.avaibleSpells[i])
                {
                    if (i == 3 && toggles[i].isOn) continue;
                    toggles[i].isOn = !toggles[i].isOn;
                }
            }
        }
	}

    public void UpdateKeys()
    {
        for (int i = 0; i < Player.numberOfKeys.Length; i++)
        {
            int numKeys = Player.numberOfKeys[i];
            int children = KeyBars[i].transform.childCount;
            while (children > numKeys)
            {
                Destroy(KeyBars[i].transform.GetChild(children - 1).gameObject);
                children--;
            }
            while (children < numKeys)
            {
                GameObject go = Instantiate(KeyShowerPrefab);
                go.GetComponentInChildren<Image>().sprite = KeyTypeSprites[i];
                go.transform.SetParent(KeyBars[i].transform);
                children++;
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

    public void RemovedImbuement()
    {
        toggles[3].isOn = false;
    }

    public string GetActiveSpell()
    {
        for (int i = 0; i < 2; i++)
        {
            if (toggles[i].isOn)
            {
                return toggles[i].GetComponent<SpellButtonController>().GetSpellText();
            }
        }
        return "10";
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
            Player.CheckDirection("10");
        }
    }
}
