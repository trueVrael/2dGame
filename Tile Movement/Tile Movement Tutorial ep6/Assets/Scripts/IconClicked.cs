using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconClicked : MonoBehaviour {

    private Player player;
    public string spellName;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void OnMouseDown()
    {
        player.CheckDirection(spellName);
        player.CreatIcons(false);
    }
}
