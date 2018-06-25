using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;		//Allows us to use Lists. 
using UnityEngine.UI;					//Allows us to use UI.
	
public class GameManager : MonoBehaviour
{
	public float levelStartDelay = 2f;						//Time to wait before starting level, in seconds.
	public float turnDelay = 0.1f;							//Delay between each Player turn.
	public static GameManager instance = null;			//Static instance of GameManager which allows it to be accessed by any other script.
	[HideInInspector] public bool playersTurn = true;		//Boolean to check if it's players turn, hidden in inspector but public.
		
	private Text levelText;									//Text to display current level number.	
	private Player player_sc;
	private GameObject levelImage;	//Image to block out level as levels are being set up, background for levelText.
	private GameObject player;
	public int level = 1;									//Current level number.
	public int playerHP = 5;
	private List<Enemy> enemies;							//List of all Enemy units, used to issue them move commands.
	private bool enemiesMoving;								//Boolean to check if enemies are moving.
    public bool stoneMoving = false;
	private bool doingSetup = true;							//Boolean to check if we're setting up board, prevent Player from moving during setup.
	//Awake is always called before any Start functions
	void Awake()
	{
           //Check if instance already exists
           if (instance == null)

                //if not, set instance to this
                instance = this;

            //If instance already exists and it's not this:
            else if (instance != this)

                //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
                Destroy(gameObject);	
			
			//Sets this to not be destroyed when reloading scene
			DontDestroyOnLoad(gameObject);
			
			//Assign enemies to a new List of Enemy objects.
			enemies = new List<Enemy>();
			InitGame();
		}

        //this is called only once, and the paramter tell it to be called only after the scene was loaded
        //(otherwise, our Scene Load callback would be called the very first load, and we don't want that)
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static public void CallbackInitialization()
        {
            //register the callback to be called everytime the scene is loaded
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        //This is called each time a scene is loaded.
        static private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            instance.level++;
            instance.InitGame();
        }

		
		//Initializes the game for each level.
		void InitGame()
		{
			Debug.Log("trasfafsafsa" + playersTurn);
			//While doingSetup is true the player can't move, prevent player from moving while title card is up.
			doingSetup = true;
			
			//Get a reference to our image LevelImage by finding it by name.
			levelImage = GameObject.Find("LevelImage");
			
			//Get a reference to our text LevelText's text component by finding it by name and calling GetComponent.
			levelText = GameObject.Find("LevelText").GetComponent<Text>();
			player_sc = GameObject.Find("Player").GetComponent<Player>();
			//Set the text of levelText to the string "Day" and append the current level number.
			//levelText.text = "Day " + level;
			
			//Set levelImage to active blocking player's view of the game board during setup.
			levelImage.SetActive(true);
			
			//Call the HideLevelImage function with a delay in seconds of levelStartDelay.
			Invoke("HideLevelImage", levelStartDelay);
			
			//Clear any Enemy objects in our List to prepare for next level.
			enemies.Clear();
			
			//Call the SetupScene function of the BoardManager script, pass it current level number.
			//boardScript.SetupScene(level);
			
		}
		
		
		//Hides black image used between levels
		void HideLevelImage()
		{
			//Disable the levelImage gameObject.
			levelImage.SetActive(false);
			
			//Set doingSetup to false allowing player to move again.
			doingSetup = false;
			playersTurn = true;
		}
		
		//Update is called every frame.
		void Update()
		{
			//Check that playersTurn or enemiesMoving or doingSetup are not currently true.
			if(playersTurn || enemiesMoving || doingSetup)
				
				//If any of these are true, return and do not start MoveEnemies.
				return;

        //Start moving enemies.
        if (player_sc.spellUsed==false)
            StartCoroutine (MoveEnemies ());
		}
		
		//Call this to add the passed in Enemy to the List of Enemy objects.
		public void AddEnemyToList(Enemy script)
		{
			//Add Enemy to List enemies.
			enemies.Add(script);
		}
		
		//Call this to remove Enemies from the list.
		public void RemoveEnemyFromList(Enemy script)
		{
			//Add Enemy to List enemies.
			enemies.Remove(script);
		}
		
		//GameOver is called when the player reaches 0 food points
		public void GameOver()
		{
			//Set levelText to display number of levels passed and game over message
			levelText.text = "GAME OVER";
			
			//Enable black background image gameObject.
			levelImage.SetActive(true);
			
			//Disable this GameManager.
			enabled = false;
		}
		
		public void NextLevel()
		{
			doingSetup = true;
			playersTurn = false;
			int l = instance.level+1;
			string scene = "Level"+l;
			//Set levelText to display number of levels passed and game over message
			if(l!=5)levelText.text = "Level "+l;
			//Enable black background image gameObject.
			levelImage.SetActive(true);
			//disable player
			player = GameObject.Find("Player");
            //gameObject.SetActive(false);
            if (l == 2)
            {
                player.transform.position = new Vector2(1.5f, 2.5f);
                player_sc.x = 1;
                player_sc.y = 3;
            }
            else if (l == 3)
            {
                player.transform.position = new Vector2(1.5f, 0.5f);
                player_sc.x = (int)player.transform.position.x;
                player_sc.y = 1;
            }
            else if (l == 4)
            {
                player.transform.position = new Vector2(-2.5f, 2.5f);
                player_sc.x = -2;
                player_sc.y = 3;
            }
            else if (l == 5)
            {
                player.transform.position = new Vector2(0.5f, 1.5f);
                player_sc.x = 0;
                player_sc.y = 1;
            }
            else if(l==6)
            {
                player.transform.position = new Vector2(-6.5f, -1.5f);
                player_sc.x = -6;
                player_sc.y = -1;
            }
            else if (l == 7)
            {
                Canvas canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
                canvas.enabled = false;
                Invoke("Quit", 3.0F);
            }
			//Disable this GameManager.
			enabled = false;
			Application.LoadLevel(scene);
			enabled = true;
		}
		
		//Coroutine to move enemies in sequence.
		IEnumerator MoveEnemies()
		{
			//While enemiesMoving is true player is unable to move.
			enemiesMoving = true;
			
			//Wait for turnDelay seconds, defaults to .1 (100 ms).
			yield return new WaitForSeconds(turnDelay);
			
			//If there are no enemies spawned (IE in first level):
			if (enemies.Count == 0) 
			{
				//Wait for turnDelay seconds between moves, replaces delay caused by enemies moving when there are none.
				yield return new WaitForSeconds(turnDelay);
			}
			
			//Loop through List of Enemy objects.
			for (int i = 0; i < enemies.Count; i++)
			{
			yield return new WaitForSeconds(0.1F);
            //Call the MoveEnemy function of Enemy at index i in the enemies List.
            if (enemies[i].turnFrozen > 4)
            {
                enemies[i].turnFrozen = 0;
                enemies[i].frozen = false;
            }
            if (enemies[i].frozen == false)
            {
                enemies[i].MoveEnemy();
            }
            else
            {
                enemies[i].turnFrozen++;
            }
			}
        //Once Enemies are done moving, set playersTurn to true so player can move.
            yield return new WaitForSeconds(0.01F);
            playersTurn = true;
			
			//Enemies are done moving, set enemiesMoving to false.
			enemiesMoving = false;
		}
    private void Quit()
    {
        Application.Quit();
    }
}

