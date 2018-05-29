using UnityEngine;
using System.Collections.Generic;

public class Unit : MonoBehaviour {

	public int tileX;
	public int tileY;
	public TileMap map;
	private GameObject player;
	private Player p1;
	public List<Node> currentPath = null;
	int moveSpeed = 2;
	
	void Awake () {
		player = GameObject.Find("Player");
		p1 = player.GetComponent<Player>();
	}
	
	void Update() {
		
		if(currentPath != null) {

			int currNode = 0;

			while( currNode < currentPath.Count-1 ) {

				Vector3 start = map.TileCoordToWorldCoord( currentPath[currNode].x+0.5f, currentPath[currNode].y+0.5f ) + 
					new Vector3(0, 0, -1f) ;
				Vector3 end   = map.TileCoordToWorldCoord( currentPath[currNode+1].x+0.5f, currentPath[currNode+1].y+0.5f )  + 
					new Vector3(0, 0, -1f) ;

				Debug.DrawLine(start, end, Color.red);

				currNode++;
			}

		}
	}

	public void MoveNextTile() {
		float remainingMovement = moveSpeed;

		while(remainingMovement > 0) {
			if(currentPath==null)
				return;

			// Get cost from current tile to next tile
			remainingMovement -= map.CostToEnterTile(currentPath[0].x, currentPath[0].y, currentPath[1].x, currentPath[1].y );

			// Move us to the next tile in the sequence
			tileX = currentPath[1].x;
			tileY = currentPath[1].y;

			transform.position = map.TileCoordToWorldCoord( tileX, tileY );	// Update our unity world position
			if(GameManager.instance.playersTurn){
				p1.x = tileX;
				p1.y = tileY;
			}
			// Remove the old "current" tile
			currentPath.RemoveAt(0);

			if(currentPath.Count == 1) {
				// We only have one tile left in the path, and that tile MUST be our ultimate
				// destination -- and we are standing on it!
				// So let's just clear our pathfinding info.
				currentPath = null;
			}
		}
		if(GameManager.instance.playersTurn == true)
			GameManager.instance.playersTurn = false;
	}
}
