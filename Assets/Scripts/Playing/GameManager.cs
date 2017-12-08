using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
	public const string TAG = "[GameManager]";

	[SerializeField] private float startPlayDelay;
	[SerializeField] private int earnedGold; //this is the gold earned from previous games
	[SerializeField] private int currentGold; //this is the gold earned from current game.
	[SerializeField] private int bestGold; //this is the biggest gold earned from previous games

	private bool isPlaying;
	private bool isDead;

	/* for enemy spawning */

	[SerializeField]
	private Transform spawnPointLeft;

	[SerializeField]
	private Transform spawnPointRight;

	[SerializeField]
	private Transform spawnPointUp;

	[SerializeField]
	private Transform spawnPointDown;

	[SerializeField]
	private EnemyDatabase enemyDatabase;

	// Spawn Rate properties

	[SerializeField]
	private float startSpawnRate = 25;

	[SerializeField]
	private float maxSpawnRate = 95;

	[SerializeField]
	private float spawnRate = 25;

	[SerializeField]
	private float spawnIncreaseRate = 1.02f;

	// Speed properties

	[SerializeField]
	private float startSpeed = 1f;

	[SerializeField]
	private float minSpeedRate = 0.25f;

	[SerializeField]
	private float speed = 1f;

	[SerializeField]
	private float speedIncreaseRate = 1.05f;


	[SerializeField]
	private float nextTime;

	private static Random random;
	/* end of enemy spawning */

	private float time;

	private static GameManager sharedInstance = null;

	public static GameManager Instance
	{
		get
		{
			return sharedInstance;
		}
	}

	private void Awake()
	{
		sharedInstance = this;
		random = new Random();
	}

	public  int getEarnedGold(){
		return this.earnedGold;
	}

	public void setEarnedGold(int EarnedGold){
		this.earnedGold = EarnedGold;
	}
	private void Start()
	{
		EventBroadcaster.Instance.AddObserver (EventNames.ON_DEAD, this.OnDead);
		EventBroadcaster.Instance.AddObserver (EventNames.ON_PLAY, this.OnPlay);
		EventBroadcaster.Instance.AddObserver(EventNames.ON_UPDATE_SCORE, OnUpdateScore);
	}

	private void OnDestroy()
	{
		EventBroadcaster.Instance.RemoveActionAtObserver(EventNames.ON_DEAD, this.OnDead);
		EventBroadcaster.Instance.RemoveActionAtObserver(EventNames.ON_PLAY, this.OnPlay);
		EventBroadcaster.Instance.RemoveActionAtObserver(EventNames.ON_UPDATE_SCORE, OnUpdateScore);
	}

	// Update is called once per frame
	void Update()
	{
		DebugControls ();

		if (!isPlaying)
			return;
		
		time = Time.timeSinceLevelLoad;


		if (!(time >= nextTime)) return;

		//		if (Input.GetKeyDown("space"))

		// Go to next frame
		NextFrame();

		// Spawn opponent based on spawn rate (75% default)
		if (random.Next(100) <= this.spawnRate)
			RequestSpawn();

		// Increase spawn rates and speed
		IncreaseDifficulty();

		nextTime += speed;
	}

	private void IncreaseDifficulty()
	{
		// Shorter == Faster
		if (speed > minSpeedRate)
			speed /= speedIncreaseRate;

		if (spawnRate < maxSpawnRate)
			spawnRate *= spawnIncreaseRate;
	}

	void DebugControls(){
		//TODO: This is for debugging and testing the functionality of the gamemanager

		if (Input.GetKeyDown (KeyCode.Period)) {
			//Testing: Player gets hit by customer
			EventBroadcaster.Instance.PostEvent (EventNames.ON_DEAD);
		}

		if (Input.GetKeyDown (KeyCode.Comma)) {
			//Testing: Player hits customer
			EventBroadcaster.Instance.PostEvent (EventNames.ON_HIT_CUSTOMER);
		}

	}

	private void RequestSpawn()
	{
		// Get Random Direction
		var direction = GetRandomDirection();

		// Get Enemy Class
		// TODO: Support multiple enemy class
		var enemyClass = this.enemyDatabase.GetEnemyClassList()[0];

		// Request spawn from EnemySpawner
		var p = new Parameters();
		p.PutObjectExtra(EnemySpawner.PARAM_DIRECTION, direction);
		p.PutObjectExtra(EnemySpawner.PARAM_ENEMYCLASS, enemyClass);
		EventBroadcaster.Instance.PostEvent(EventNames.ON_SPAWN_REQUEST, p);
	}

	private static Direction GetRandomDirection()
	{
		var values = Enum.GetValues(typeof(Direction));
		Direction dir;

		do {
			dir = (Direction) values.GetValue (random.Next (values.Length));
		} while(dir == Direction.NONE);
			
		return (Direction)dir;
	}

	private void NextFrame()
	{
		Debug.Log("NextFrame()");
		EventBroadcaster.Instance.PostEvent(EventNames.ON_NEXT_FRAME);
	}

	public Vector3 GetSpawnPointPosition(Direction direction)
	{
		var position = this.spawnPointUp.localPosition;

		switch (direction)
		{
			case Direction.DOWN:
				position = this.spawnPointDown.localPosition;
				break;
			case Direction.LEFT:
				position = this.spawnPointLeft.localPosition;
				break;
			case Direction.RIGHT:
				position = this.spawnPointRight.localPosition;
				break;
		}

		return position;
	}

	public EnemyDatabase GetEnemyDatabase()
	{
		return this.enemyDatabase;
	}


	// This is for Playing. From Intro/GameOver state to Play state
	public void OnPlay(){

		ResetState();
		StartCoroutine(DelayPlay(startPlayDelay));
	}

	IEnumerator DelayPlay(float seconds){

		yield return new WaitForSeconds (seconds);

		// Start the game
		isPlaying = true;
	}
	//======================//

	// This is for entering Intro/Gameover
	public void OnDead()
	{
		if (!isPlaying)
			return;
		EventBroadcaster.Instance.PostEvent (EventNames.DEATH);
        isPlaying = false;
		UpdateBestScore();

		StartCoroutine(OnDeadTransition());
    }

    private IEnumerator OnDeadTransition()
    {
		EventBroadcaster.Instance.PostEvent (EventNames.ON_PLAYER_DEATH);
		yield return null;
		EventBroadcaster.Instance.PostEvent(EventNames.ON_GAME_OVER);
    }
    //======================//


    // This is for when player detects correct hit. 
    public void OnUpdateScore(){

		Debug.Log("OnUpdateScore");

		currentGold++;

		// Update current score
		var param = new Parameters();
		param.PutExtra(PlayScreen.PARAM_CURRENT_SCORE, currentGold);
		EventBroadcaster.Instance.PostEvent (EventNames.ON_UPDATE_GOLD_UI, param);
	}
	//======================//

	public void ResetState()
	{
		currentGold = 0;

		// Reset rate
		this.speed = this.startSpeed;
		this.spawnRate = this.startSpawnRate;
		this.nextTime = 0;
		this.time = Time.timeSinceLevelLoad;
		this.nextTime = this.time;

		// Reset frenzy and enemy list
		EventBroadcaster.Instance.PostEvent(EventNames.ON_GAME_RESET);

		//TODO: Reset Player stats here pls (Reset Combo pls)
	}

	private void UpdateBestScore()
	{
		if (currentGold > bestGold) //updates best score
			bestGold = currentGold;
		earnedGold += currentGold;
	}

	public int EarnedGold{//this is the gold earned from previous games
		get{
			return earnedGold;
		}
	}

	public int CurrentGold{//this is the gold earned from current games
		get{
			return currentGold;
		}
	}

	public int BestGold{//this is the BEST gold earned from one run
		get{
			return bestGold;
		}
	}

    public float GetCurrentSpeed()
    {
        return this.speed;
    }

	public bool IsPlaying()
	{
		return this.isPlaying;
	}


}
