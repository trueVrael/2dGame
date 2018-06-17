using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBlockExpire : MonoBehaviour {

    // Use this for initialization
    private Enemy frozenEnemy = null;
    public int IceDMG;

	void Start () {
        Invoke("DestroyMe", 0.5F);
	}
    private void Update()
    {
        if (frozenEnemy.frozen == false || frozenEnemy == null) Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            CancelInvoke("DestroyMe");
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            enemy.frozen = true;
            enemy.LoseHP(IceDMG);
            enemy.turnFrozen = 0;
            frozenEnemy = enemy;
            enemy.iceWall = gameObject;
            Destroy(gameObject.GetComponent<Collider2D>());
        }
    }

    private void DestroyMe()
    {
        Destroy(gameObject);
    }
}
