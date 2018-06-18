using UnityEngine;
using System.Collections;
using UnityEngine.UI;	//Allows us to use UI.
using UnityEngine.SceneManagement;



//Player inherits from MovingObject, our base class for objects that can move, Enemy also inherits from this.
public class Player : MovingObject
	{
		public Player instance = null;	
		private Animator animator;					//Used to store a reference to the Player's animator component.
		public int hp;                           //Used to store player food points total during level.
		public int maxHP;
        public int thunderDamage = 2;
        public bool spellUsed = false;
        private string direction;
        private UIManager UIManager;
		public bool[] avaibleSpells;
        public int[] numberOfKeys;
        private string spellName;
        private Collider2D spellCollider;
        private SpriteRenderer spellSprite;
        public GameObject tileCollider;
        public int x,y;	
		public int dmg;
        private bool isImbued;
        public Transform[] spellSpawn;
        public Transform[] SpellsIconsLocation;
        public GameObject[] SpellsIcons;
        public GameObject[] Spells;
		//Start overrides the Start function of MovingObject
		protected override void Start (){
		
		           //Check if instance already exists
        //   if (instance == null)

                //if not, set instance to this
           //     instance = this;

            //If instance already exists and it's not this:
         //   else if (instance != this)

                //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            //    Destroy(gameObject);	
			
			//Sets this to not be destroyed when reloading scene
			DontDestroyOnLoad(gameObject);
			//Get a component reference to the Player's animator component
			animator = GetComponent<Animator>();
			UIManager = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIManager>();
			avaibleSpells = new bool[] { false, false, false, false }; 
			//Get the current food point total stored in GameManager.instance between levels.
			hp = GameManager.instance.playerHP;		
			this.x = (int)transform.position.x;
			this.y = (int)transform.position.y;
			//Call the Start function of the MovingObject base class.
			base.Start ();
			maxHP=5;
		}
		
		
		//This function is called when the behaviour becomes disabled or inactive.
		private void OnDisable ()
		{
			//When Player object is disabled, store the current local food total in the GameManager so it can be re-loaded in next level.
			GameManager.instance.playerHP = hp;
		}
		
		
		private void Update ()
		{
        Debug.Log(enabled);
        Debug.Log(spellName);
        Debug.Log(spellUsed);
			//If it's not the player's turn, exit the function.
			if(!GameManager.instance.playersTurn) return;
			int horizontal = 0;  	//Used to store the horizontal move direction.
			int vertical = 0;       //Used to store the vertical move direction.
			
			//Get input from the input manager, round it to an integer and store in horizontal to set x axis move direction
			horizontal = (int) (Input.GetAxisRaw ("Horizontal"));
			
			//Get input from the input manager, round it to an integer and store in vertical to set y axis move direction
			vertical = (int) (Input.GetAxisRaw ("Vertical"));

        /*
        if(Input.GetButtonDown("Spell"))
        {
        spellUsed = true;
        } */
        if (Input.GetKeyDown("w")) direction = "w";
        if (Input.GetKeyDown("s")) direction = "s";
        if (Input.GetKeyDown("a")) direction = "a";
        if (Input.GetKeyDown("d")) direction = "d";
        if (spellName == "FireWeapon") direction = "d"; 
        if (direction=="w" || direction == "s" || direction == "a" || direction == "d")
        {
            Vector3 position = new Vector3(this.gameObject.GetComponent<Transform>().position.x, this.gameObject.GetComponent<Transform>().position.y, this.gameObject.GetComponent<Transform>().position.z);
            Quaternion rotation = new Quaternion(this.gameObject.GetComponent<Transform>().rotation.w, this.gameObject.GetComponent<Transform>().rotation.x, this.gameObject.GetComponent<Transform>().rotation.y, this.gameObject.GetComponent<Transform>().rotation.z);
            switch (direction)
            {
                case "w": rotation.eulerAngles = new Vector3 (0, 0, 0); position = new Vector3(position.x, position.y + 1, position.z); break;
                case "s": rotation.eulerAngles = new Vector3(0, 0, 180); position = new Vector3(position.x, position.y - 1, position.z); break;
                case "a": rotation.eulerAngles = new Vector3(0, 0, 90); position= new Vector3(position.x - 1, position.y, position.z); break;
                case "d": rotation.eulerAngles = new Vector3(0, 0, 270); position = new Vector3(position.x + 1, position.y, position.z); break;
            }
            direction = "no_direction";
           if(spellName!="10" || spellName == "FireWeapon")UseSpell(position, rotation);
        }


			//Check if moving horizontally, if so set vertical to zero.
			if(horizontal != 0)
			{
				vertical = 0;
			}

			//Check if we have a non-zero value for horizontal or vertical
			if((horizontal != 0 || vertical != 0) && spellUsed==false)
			{
				//Call AttemptMove passing in the generic parameter Wall, since that is what Player may interact with if they encounter one (by attacking it)
				//Pass in horizontal and vertical as parameters to specify the direction to move Player in.
				AttemptMove(horizontal, vertical);
			}
		}
		
		//AttemptMove overrides the AttemptMove function in the base class MovingObject
		//AttemptMove takes a generic parameter T which for Player will be of the type Wall, it also takes integers for x and y direction to move in.
		protected override bool AttemptMove(int xDir, int yDir)
		{
			//Call the AttemptMove method of the base class, passing in the component T (in this case Wall) and x and y direction to move.
			if(base.AttemptMove(xDir, yDir)){
				this.x += xDir;
				this.y +=yDir;
			}
						
			//Since the player has moved and lost food points, check if the game has ended.
			CheckIfGameOver ();
			
			//Set the playersTurn boolean of GameManager to false now that players turn is over.
			GameManager.instance.playersTurn = false;
			return true;
		}
		
		
		//OnCantMove overrides the abstract function OnCantMove in MovingObject.
		//It takes a generic parameter T which in the case of Player is a Wall which the player can attack and destroy.
		protected override void OnCantMove <T> (T component)
		{
            if (component.GetComponent<PushableStone>() != null)
            {
                PushableStone stone = component.GetComponent<PushableStone>();
                int xDir, yDir;
                if (stone.Push(out xDir, out yDir))
                {
                    this.x += xDir;
                    this.y += yDir;
                }
            }
            if (component.GetComponent<Door>() != null)
            {
                Door door = component.GetComponent<Door>();
                if (numberOfKeys[door.DoorKey] > 0)
                {
                    door.Open();
                    numberOfKeys[door.DoorKey]--;
                    UIManager.UpdateKeys();                
                }
            }
            if (component.GetComponent<Enemy>() != null)
            {
                //Set hitEnemy to equal the component passed in as a parameter.
                Enemy hitEnemy = component.GetComponent<Enemy>();

            //Call the DamageWall function of the Wall we are hitting.
                if (isImbued)
                {
                    hitEnemy.LoseHP(dmg * 2);
                    isImbued = false;
                    Destroy(this.transform.Find(Spells[3].name).gameObject);
                    UIManager.RemovedImbuement();
                }
                else
                {
                    hitEnemy.LoseHP(dmg);
                }

                //Set the attack trigger of the player's animation controller in order to play the player's attack animation.
                animator.SetTrigger("playerHit");
            }
		}
		
		
		//OnTriggerEnter2D is sent when another object enters a trigger collider attached to this object (2D physics only).
		private void OnTriggerEnter2D (Collider2D other)
		{
			//Check if the tag of the trigger collided with is Exit.
			if(other.tag == "Exit")
			{
				GameManager.instance.playersTurn = false;
				enabled=false;
				//Invoke the Restart function to start the next level with a delay of restartLevelDelay (default 1 second).
				Invoke ("Restart", restartLevelDelay);
			}
        if (other.tag == "Reload")
        {
            GameManager.instance.level--;
            GameManager.instance.playersTurn = false;
            enabled = false;
            //Invoke the Restart function to start the next level with a delay of restartLevelDelay (default 1 second).
            Invoke("Restart", restartLevelDelay);
        }
    }
		
		
		//Restart reloads the scene when called.
		private void Restart ()
		{
			GameManager.instance.NextLevel ();
			enabled=true;
		}
		
		
		//LoseFood is called when an enemy attacks the player.
		//It takes a parameter loss which specifies how many points to lose.
		public void LoseHP (int loss)
		{
			//Set the trigger for the player animator to transition to the playerHit animation.
			animator.SetTrigger ("playerHit");
			
			//Subtract lost food points from the players total.
			hp -= loss;
			
			//Update the food display with the new total.
			Debug.Log(hp);
			
			//Check to see if game has ended.
			CheckIfGameOver ();
		}
		
		
		//CheckIfGameOver checks if the player is out of food points and if so, ends the game.
		private void CheckIfGameOver ()
		{
			//Check if food point total is less than or equal to zero.
			if (hp<= 0) 
			{
				Debug.Log("Game Over");
				GameManager.instance.GameOver ();
			}
		}
    
        public void UseSpell(Vector3 spellTransformPosition, Quaternion spellTransformRotation)
        {
        if(spellName!="10")spellUsed = true;
         switch(spellName)
            {
                case "FireBall": FireBall(spellTransformPosition, spellTransformRotation); break;
                case "Thunder": Thunder(spellTransformPosition); break;
                case "IceBall": IceBall(spellTransformPosition, spellTransformRotation); break;
                case "FireWeapon": FireImbue(); break;
            }
        GameManager.instance.playersTurn = false;
        }


        public void FireBall(Vector3 spellTransformPosition, Quaternion spellTransformRotation)
        {
            if (this.avaibleSpells[0] == true)
            {
                Instantiate(Spells[0], spellTransformPosition, spellTransformRotation);
            }
        }

        public void Thunder(Vector3 spellTransformPosition)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            int count = enemies.Length;
            for (int i = 0; i < count; i++)
            {
                if ((enemies[i].transform.position.x - spellTransformPosition.x) * (enemies[i].transform.position.x - spellTransformPosition.x) <= 1 && (enemies[i].transform.position.y - spellTransformPosition.y) * (enemies[i].transform.position.y - spellTransformPosition.y) <= 1)
                {
                    Enemy enemy = enemies[i].GetComponent<Enemy>();
                    enemy.LoseHP(thunderDamage);
                }
            }

            Quaternion rotation = new Quaternion(0, 0, 0, 0);
            Vector3 position = spellTransformPosition;

            for (int i = -1; i <= 1; i++)
            {
                position.x = spellTransformPosition.x + i;
                for (int j = -1; j <= 1; j++)
                {
                    position.y = spellTransformPosition.y + j;
                    if (position.x != this.transform.position.x || position.y != this.transform.position.y) Instantiate(Spells[1], position, rotation);
                }
            }

        }

        public void IceBall(Vector3 spellTransformPosition, Quaternion spellTransformRotation)
        {
            if (this.avaibleSpells[2])
            {
                Instantiate(Spells[2], spellTransformPosition, spellTransformRotation);
            }
        }

        public void FireImbue()
        {
            if (this.avaibleSpells[3] && !isImbued)
            {
                GameObject we = Instantiate(Spells[3], this.transform);
                we.name = Spells[3].name;
                isImbued = true;
                this.spellUsed = false;
            }
            spellName = UIManager.GetActiveSpell();
        }
    /*
    private void OnMouseDown()
    {
        spellUsed = true;
        CreatIcons(true);
    }

    public void CreatIcons(bool x)
    {
        int spellNumber = SpellsIcons.Length;
        if (x == true)
        {
            for (int i = 0; i < spellNumber; i++)
            {
                Instantiate(SpellsIcons[i], SpellsIconsLocation[i].position, SpellsIconsLocation[i].rotation);
            }
        }
        else
        {            
            GameObject[] CurrentIcons = GameObject.FindGameObjectsWithTag("Icon");
            for (int i=0; i<spellNumber; i++)
            {
                Destroy(CurrentIcons[i]);
            }
            spellUsed = false;
        }
    }
    */
    public void DestroyTiles()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Direction");
        int length = tiles.Length;
        for(int i=0; i<length; i++)
        {
            Destroy(tiles[i]);
        }

       // GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIManager>().SpellUsed();
    }
    
    public void CheckDirection(string x)
    {
        /*
        spellName = x;
        Instantiate(tileCollider, spellSpawn[0].position, spellSpawn[0].rotation);
        Instantiate(tileCollider, spellSpawn[1].position, spellSpawn[1].rotation);
        Instantiate(tileCollider, spellSpawn[2].position, spellSpawn[2].rotation);
        Instantiate(tileCollider, spellSpawn[3].position, spellSpawn[3].rotation);*/
        spellName = x;
    }
    /*


        public int spellRange;
        public int spellDMG;
    private void Bomb()
    {
        //Find all enemies.
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        //check players location.
        int x = (int)this.transform.position.x;
        int y = (int)this.transform.position.y;
        int xEnemy;
        int yEnemy;
        //set length as number of enemies on map.
        int length = enemies.Length;
        for (int i = 0; i < length; i++)
        {
            //get location of ith enemy.
            xEnemy = (int)enemies[i].transform.position.x;
            yEnemy = (int)enemies[i].transform.position.y;
            //check if enemy is in range of spell.
            if ((xEnemy - x) * (xEnemy - x) + (yEnemy - y) * (yEnemy - y) <= spellRange * spellRange)
            {
                Enemy enemy = enemies[i].GetComponent<Enemy>();
                enemy.LoseHP(spellDMG);
            }
        }
        GameManager.instance.playersTurn = false;
        CreatIcons(false);
    }
    */

}


