using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MovingObject {
    public int DoorKey;
    public Sprite OpenSprite;
    public Sprite CloseSprite;

    protected override void OnCantMove <T>(T component)
    {

    } 

    public void Open()
    {
        GetComponent<SpriteRenderer>().sprite = OpenSprite;
        SetBoxColliderEnabled(false);
    }

    // Use this for initialization
    void Start () {
        base.Start();
        GetComponent<SpriteRenderer>().sprite = CloseSprite;
        SetBoxColliderEnabled(true);
	}
}
