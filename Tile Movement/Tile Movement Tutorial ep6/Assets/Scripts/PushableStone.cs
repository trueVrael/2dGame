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
	
    public bool Push(out int xDir, out int yDir)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector2 pos = transform.position;
        Vector2 playerPos = player.transform.position;
        Vector2 delta = pos - playerPos;
        xDir = 0;
        yDir = 0;

        if (AttemptMove((int) delta.x, (int)delta.y))
        {
            xDir = (int)delta.x;
            yDir = (int)delta.y;
            Player playercomp = player.GetComponent<Player>();
            float playermt = playercomp.moveTime;
            playercomp.SetMoveTime(moveTime);
            GameManager.instance.stoneMoving = true;
            StartCoroutine(playercomp.SmoothMovement(playerPos + delta, () => PushEnd(playermt)));
            return true;
        }
        return false;
        
    }

    public void PushEnd(float playerMoveTime)
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().SetMoveTime(playerMoveTime);
        GameManager.instance.stoneMoving = false;
    }
}
