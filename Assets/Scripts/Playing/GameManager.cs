using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
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

	}

	private void OnDestroy()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown("space"))
			RequestSpawn();


		if (!(Time.time >= nextTime)) return;

		// Go to next frame
		NextFrame();
		nextTime += speed;
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
}
