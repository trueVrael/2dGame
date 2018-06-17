using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject {

	public int dmg;
	public int hp;
	public int x,y;
    public bool frozen = false;
    public GameObject iceWall;
    public int turnFrozen = 0;
    private Animator animator;
	private GameObject target;
	private Player player;
	private bool skipMove;
	private int seeRange;
	private Transform targetPlayer;

	// Use this for initialization
	void Start () {
		GameManager.instance.AddEnemyToList(this);
		animator = GetComponent<Animator>();
		target = GameObject.FindWithTag("Player");
		player = target.GetComponent<Player>();
		targetPlayer = GameObject.FindGameObjectWithTag ("Player").transform;
		this.x = (int)transform.position.x;
		this.y = (int)transform.position.y;
		this.hp = 4;
		seeRange = 3;
		base.Start();
	}
	
	//Override the AttemptMove function of MovingObject to include functionality needed for Enemy to skip turns.
		//See comments in MovingObject for more on how base AttemptMove function works.
	protected override bool AttemptMove(int xDir, int yDir)
	{
			//Check if skipMove is true, if so set it to false and skip this turn.
		if(Mathf.Abs (player.x - this.x) + Mathf.Abs (player.y - this.y) <= seeRange)
		{
			//Call the AttemptMove function from MovingObject.
			if(base.AttemptMove(xDir, yDir)){
				this.x += xDir;
				this.y +=yDir;
				return true;
			}
				
		}
		return false;
	}
		
		
		//MoveEnemy is called by the GameManger each turn to tell each Enemy to try to move towards the player.
	public void MoveEnemy ()
	{
		//Declare variables for X and Y axis move directions, these range from -1 to 1.
		//These values allow us to choose between the cardinal directions: up, down, left and right.
		int xDir = 0;
		int yDir = 0;
		//If the difference in positions is approximately zero (Epsilon) do the following:
		if(Mathf.Abs (targetPlayer.transform.position.x - transform.position.x) < float.Epsilon)
			
			//If the y coordinate of the target's (player) position is greater than the y coordinate of this enemy's position set y direction 1 (to move up). If not, set it to -1 (to move down).
			yDir = targetPlayer.position.y > transform.position.y ? 1 : -1;
		
		//If the difference in positions is not approximately zero (Epsilon) do the following:
		else
			//Check if target x position is greater than enemy's x position, if so set x direction to 1 (move right), if not set to -1 (move left).
			xDir = targetPlayer.position.x > transform.position.x ? 1 : -1;
		
		//Call the AttemptMove function and pass in the generic parameter Player, because Enemy is moving and expecting to potentially encounter a Player
		AttemptMove(xDir, yDir);
	}
		
		
	//OnCantMove is called if Enemy attempts to move into a space occupied by a Player, it overrides the OnCantMove function of MovingObject 
	//and takes a generic parameter T which we use to pass in the component we expect to encounter, in this case Player
	protected override void OnCantMove <T> (T component)
	{
        //Declare hitPlayer and set it to equal the encountered component.
        if (component.GetComponent<Player>() != null)
        {
            Player hitPlayer = component.GetComponent<Player>();

            //Call the LoseFood function of hitPlayer passing it playerDamage, the amount of hp to be subtracted.
            if (hitPlayer != null)
            {
                hitPlayer.LoseHP(dmg);

                //Set the attack trigger of animator to trigger Enemy attack animation.
                animator.SetTrigger("enemyAttack");
            }
            //Call the RandomizeSfx function of SoundManager passing in the two audio clips to choose randomly between.
            //SoundManager.instance.RandomizeSfx (attackSound1, attackSound2);
        }
	}
	
	public void LoseHP (int loss)
	{
		//Set the trigger for the player animator to transition to the playerHit animation.
		animator.SetTrigger ("playerHit");
		
		//Subtract lost food points from the players total.
		hp -= loss;
		
		//Update the food display with the new total.
		Debug.Log(hp);
		
		//Check to see if game has ended.
		CheckIfDead ();
	}
	public void CheckIfDead(){
		if (hp<= 0){ 
			Debug.Log("Enemy killed");
					//Call this to remove Enemies from the list.
			GameManager.instance.RemoveEnemyFromList(this);
			Destroy(this.gameObject);
		}
	}
}
