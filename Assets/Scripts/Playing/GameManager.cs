using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
	public const string TAG = "[GameManager]";

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

	[SerializeField]
	private float speed = 1f;

	[SerializeField]
	private float nextTime;

	private static Random random;
	/* end of enemy spawning */

	private static GameManager sharedInstance = null;

	public static GameManager Instance
	{
		get
		{
			if (sharedInstance == null)
			{
				sharedInstance = new GameManager();
				random = new Random();
			}

			return sharedInstance;
		}
	}

	private void Awake()
	{
		sharedInstance = this;
		random = new Random();
	}

	private void Start()
	{
		EventBroadcaster.Instance.AddObserver (EventNames.ON_HIT_CUSTOMER, this.OnHitCustomer);
		EventBroadcaster.Instance.AddObserver (EventNames.ON_DEAD, this.OnDead);
		EventBroadcaster.Instance.AddObserver (EventNames.ON_PLAY, this.OnPlay);
	}

	private void OnDestroy()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		if (!isPlaying)
			return;

		DebugControls ();
	 
		Debug.Log (TAG + " Is Playing"); //TODO: Testing if isPlaying var works

		if (Input.GetKeyDown("space"))
			RequestSpawn();


		if (!(Time.time >= nextTime)) return;

		// Go to next frame
		NextFrame();
		nextTime += speed;
	}

	void DebugControls(){
		//TODO: This is for debugging and testing the functionality of the gamemanager

		if (Input.GetKeyDown (KeyCode.Period)) {
			//Testing: Player gets hit by customer
			EventBroadcaster.Instance.PostEvent (EventNames.ON_DEAD);
		}

		if (Input.GetKeyDown (KeyCode.Comma)) {
			Debug.Log ("Hitting");
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
		return (Direction) values.GetValue(random.Next(values.Length));
	}

	private void NextFrame()
	{
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

		Debug.Log("direction " + direction);
		Debug.Log("spawnPointUp.localPosition " + spawnPointUp.localPosition);
		Debug.Log("spawnPointDown.localPosition " + spawnPointDown.localPosition);
		Debug.Log("spawnPointLeft.localPosition " + spawnPointLeft.localPosition);
		Debug.Log("spawnPointRight.localPosition " + spawnPointRight.localPosition);
		Debug.Log("position " + position);

		return position;
	}

	public EnemyDatabase GetEnemyDatabase()
	{
		return this.enemyDatabase;
	}


	// This is for Playing. From Intro/GameOver state to Play state
	public void OnPlay(){
		StartCoroutine(DelayPlay(1.0f));
	}

	IEnumerator DelayPlay(float seconds){
		yield return new WaitForSeconds (0.05f);
		isPlaying = true;
	}
	//======================//

	// This is for entering Intro/Gameover
	//TODO: @Dyan. Please reprocess this method :) thanks
	public void OnDead(){
		isPlaying = false;

		if (earnedGold > bestGold) //updates best score
			bestGold = earnedGold;

		EventBroadcaster.Instance.PostEvent (EventNames.ON_GAME_OVER);
	}
	//======================//


	// This is for when player detects correct hit. 
	public void OnHitCustomer(){
		currentGold++;
		Parameters param = new Parameters ();
		//TODO: Integrate Player's combo and gold stats here
		//param.PutExtra(PlayScreen.MAX_COMBO_KEY, <max combo of player> );
		//param.PutExtra(PlayScreen.CURRENT_COMBO_KEY, <current combo of player> );
		//param.PutExtra(PlayScreen.CURRENT_GOLD_KEY, <current gold of player> );

		EventBroadcaster.Instance.PostEvent (EventNames.ON_UPDATE_COMBO, param); //TODO: Since combo is part of Player, pass the Player's Parameters next time
		EventBroadcaster.Instance.PostEvent (EventNames.ON_UPDATE_GOLD, param);
	}
	//======================//

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


}
