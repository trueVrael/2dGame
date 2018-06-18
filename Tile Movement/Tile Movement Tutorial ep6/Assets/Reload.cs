using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reload : MonoBehaviour {

	// Use this for initialization
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(this.gameObject.GetComponent<Collider2D>());
            GameManager.instance.level--;
            GameManager.instance.NextLevel();
        }
    }
}
