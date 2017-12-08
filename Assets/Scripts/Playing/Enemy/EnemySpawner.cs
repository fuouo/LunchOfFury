using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{

	private int spawnedCount;

	[SerializeField]
	private GameObjectPool objectPool;

	[SerializeField]
	private HitHandler hitHandler;

	public const string PARAM_DIRECTION = "PARAM_DIRECTION";
	public const string PARAM_ENEMYCLASS = "PARAM_ENEMYCLASS";
	public const string PARAM_ENEMY_TO_HIT = "PARAM_ENEMY_TO_HIT";

	private static EnemySpawner sharedInstance = null;

	private Queue<Enemy> spawnedEnemyQueue;

	public static EnemySpawner Instance
	{
		get
		{
			return sharedInstance;
		}
	}

	void Awake()
	{
		sharedInstance = this;
		spawnedEnemyQueue = new Queue<Enemy>();
	}


	// Use this for initialization
	void Start()
	{
		this.objectPool.Initialize();

		EventBroadcaster.Instance.AddObserver(EventNames.ON_GAME_RESET, ResetEnemy);
		EventBroadcaster.Instance.AddObserver(EventNames.ON_SPAWN_REQUEST, Spawn);

	}

	private void OnDestroy()
	{
		EventBroadcaster.Instance.RemoveActionAtObserver(EventNames.ON_SPAWN_REQUEST, Spawn);
		EventBroadcaster.Instance.RemoveActionAtObserver(EventNames.ON_GAME_RESET, ResetEnemy);
	}

	public Enemy[] GetEnemyList()
	{
		var usedObjects = objectPool.GetUsedObjects().ToArray();
		var result = new Enemy[usedObjects.Length];

		for (var i = 0; i < usedObjects.Length; i++)
			result[i] = (Enemy) usedObjects[i];

		return result;
	}

	private void ResetEnemy()
	{
		this.spawnedCount = 0;
		var usedObjects = objectPool.GetUsedObjects().ToArray();

		// Reset all objects
		for (var i = 0; i < usedObjects.Length; i++)
			objectPool.ReleasePoolable(usedObjects[i]);

		spawnedEnemyQueue.Clear();
	}

	private void Spawn(Parameters parameters)
	{
		if (!this.objectPool.HasObjectAvailable(1)) return;
		var newEnemy = (Enemy) this.objectPool.RequestPoolable();
		
		var direction = (Direction)parameters.GetObjectExtra(PARAM_DIRECTION);
		var enemyClass = (EnemyClass)parameters.GetObjectExtra(PARAM_ENEMYCLASS);

		// Update enemy direction
		newEnemy.SetEnemyClass(enemyClass);
		newEnemy.ChangeDirection(direction);

		// Update layer
		if (direction == Direction.UP)
		{
			newEnemy.GetComponent<Renderer>().sortingLayerName = SortingLayerNames.CUSTOMER_TOP;
			newEnemy.GetComponent<Renderer>().sortingOrder = Math.Abs(spawnedCount);

		}
		else
		{
			newEnemy.GetComponent<Renderer>().sortingLayerName = SortingLayerNames.CUSTOMER;
			newEnemy.GetComponent<Renderer>().sortingOrder = spawnedCount;
		}

		// Decrease spawn count
		this.spawnedCount--;

		spawnedEnemyQueue.Enqueue(newEnemy);

	}

	public void RemoveEnemy(Enemy enemy)
	{
		spawnedEnemyQueue.Dequeue();
		StartCoroutine(DelayedRemove(enemy));
	}	

	private IEnumerator DelayedRemove(Enemy enemy)
	{
		yield return new WaitForSeconds(enemy.GetFlyAnimationSpeed());

        // Destroy object
        enemy.transform.DOKill();
		this.objectPool.ReleasePoolable(enemy);
	}

	public bool IsEnemyClosest(Enemy enemy)
	{
		return spawnedEnemyQueue.Peek() == enemy;
	}
}
