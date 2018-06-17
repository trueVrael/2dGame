using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBallOnContact : MonoBehaviour {

    public int spellDMG;
    public int rotation;
    private Vector3 position;
    public GameObject IceBlock;
    private Player player;

    private void Start()
    {
        rotation = (int)this.gameObject.transform.rotation.eulerAngles.z;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            enemy.LoseHP(spellDMG);
            enemy.frozen = true;
            enemy.turnFrozen = 0;
            Instantiate(IceBlock, enemy.transform.position, enemy.transform.rotation);
        }

        if (other.gameObject.tag != "Player" && other.gameObject.tag!="Enemy" && other.gameObject.tag!="Spell" /*&& other.gameObject.tag != "Direction"*/)
        {
            position = new Vector3((float)(((int)this.gameObject.transform.position.x)+0.5),(float)(((int)this.gameObject.transform.position.y)+0.5), 0);
            if (this.gameObject.transform.position.x < 0) position.x--;
            if (this.gameObject.transform.position.y < 0) position.y--;

                switch (rotation)
            {
                case 0:
                    position.x -= 1;
                    if (player.transform.position.y + 1.1 > gameObject.transform.position.y) position.y += 1;
                    else position.y += 2;
                    Instantiate(IceBlock, position, other.transform.rotation);
                    position.x += 1;
                    Instantiate(IceBlock, position, other.transform.rotation);
                    position.x += 1;
                    Instantiate(IceBlock, position, other.transform.rotation);
                    break;
                case 180:
                    position.x -= 1;
                    if (player.transform.position.y - 1.1 < gameObject.transform.position.y) position.y -= 1;
                    else position.y -= 2;
                    Instantiate(IceBlock, position, other.transform.rotation);
                    position.x += 1;
                    Instantiate(IceBlock, position, other.transform.rotation);
                    position.x += 1;
                    Instantiate(IceBlock, position, other.transform.rotation);
                    break;
                case 90:
                    if (player.transform.position.x - 1.1 < gameObject.transform.position.x) position.x -= 1;
                    else position.x -= 2;
                    position.y -= 1;
                    Instantiate(IceBlock, position, other.transform.rotation);
                    position.y += 1;
                    Instantiate(IceBlock, position, other.transform.rotation);
                    position.y += 1;
                    Instantiate(IceBlock, position, other.transform.rotation);
                    break;
                case 270:
                    if (player.transform.position.x + 1.1 > gameObject.transform.position.x) position.x += 1;
                    else position.x += 2;
                    position.y -= 1;
                    Instantiate(IceBlock, position, other.transform.rotation);
                    position.y += 1;
                    Instantiate(IceBlock, position, other.transform.rotation);
                    position.y += 1;
                    Instantiate(IceBlock, position, other.transform.rotation);
                    break;
            }
            player.spellUsed = false;
            Destroy(gameObject);
        }
    }
}
