using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableStone : MovingObject {
    protected override void OnCantMove<T>(T component)
    {
        // Do nothing
    }

    // Use this for initialization
    void Start () {
        base.Start();
	}
	
    public void Push()
    {
        Vector2 pos = transform.position;
        Vector2 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        Vector2 delta = pos - playerPos;

        AttemptMove((int) delta.x, (int) delta.y);
    }
}
