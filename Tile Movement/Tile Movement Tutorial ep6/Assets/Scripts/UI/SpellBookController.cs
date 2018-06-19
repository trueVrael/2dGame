using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellBookController : MonoBehaviour
{
    private Player player;
    public GameObject[] SpellInfoPanels;
    public Button PreviousPageButton;
    public Button NextPageButton;

    private int CurrentPage;
    private List<int> spells = new List<int>();
    [TextArea]
    public string[] DescriptionForSpells;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        CurrentPage = 0;

        PreviousPageButton.onClick.AddListener(PreviousPage);
        NextPageButton.onClick.AddListener(NextPage);

        RefreshPages();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void RefreshPages()
    {
        for (int i = 0; i < 2; i++)
        {
            if (spells.Count > CurrentPage * 2 + i)
            {
                int spellOnPage = spells[CurrentPage * 2 + i];
                Image spellImage = SpellInfoPanels[i].GetComponentInChildren<Image>();
                spellImage.sprite = player.Spells[spellOnPage].GetComponentInChildren<SpriteRenderer>().sprite;
                spellImage.preserveAspect = true;
                Color color = spellImage.color;
                color.a = 1;
                spellImage.color = color;

                Text spellText = SpellInfoPanels[i].GetComponentInChildren<Text>();
                spellText.text = DescriptionForSpells[spellOnPage];
            }
            else
            {
                Image spellImage = SpellInfoPanels[i].GetComponentInChildren<Image>();
                spellImage.sprite = null;
                Color color = spellImage.color;
                color.a = 0;
                spellImage.color = color;
                SpellInfoPanels[i].GetComponentInChildren<Text>().text = "";
            }
        }
    }

    public void NextPage()
    {
        if (CurrentPage < (spells.Count - 1) / 2)
        {
            CurrentPage++;
            RefreshPages();
        }
    }

    public void PreviousPage()
    {
        if (CurrentPage > 0)
        {
            CurrentPage--;
            RefreshPages();
        }
    }

    public void SetActive(bool toState)
    {
        gameObject.SetActive(toState);

        RefreshPages();
    }

    public void AddSpell(int spellID)
    {
        if (spells.Count == 0)
        {
            spells.Add(spellID);
        }
        else
        {
            spells.Insert(0, spellID);
        }
    }
}
