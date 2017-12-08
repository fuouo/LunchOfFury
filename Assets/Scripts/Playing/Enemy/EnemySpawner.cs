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

	private System.Random random;

	// Use this for initialization
	void Start()
	{
		random = new System.Random();
		this.objectPool.Initialize();

		EventBroadcaster.Instance.AddObserver (EventNames.FRENZY_TRIGGERED, this.Frenzy);
		EventBroadcaster.Instance.AddObserver(EventNames.ON_GAME_RESET, ResetEnemy);
		EventBroadcaster.Instance.AddObserver(EventNames.ON_SPAWN_REQUEST, Spawn);
		EventBroadcaster.Instance.AddObserver(EventNames.ON_HIT_CUSTOMER, OnHit);

	}

	private void OnDestroy()
	{
		EventBroadcaster.Instance.RemoveActionAtObserver(EventNames.ON_SPAWN_REQUEST, Spawn);
		EventBroadcaster.Instance.RemoveActionAtObserver(EventNames.ON_GAME_RESET, ResetEnemy);
		EventBroadcaster.Instance.RemoveActionAtObserver(EventNames.ON_HIT_CUSTOMER, OnHit);
	}

	private void Frenzy(){
		var usedObjects =  objectPool.GetUsedObjects().ToArray();

		for (var i = 0; i < usedObjects.Length; i++)
		{
			StartCoroutine(FlyAnimation(usedObjects[i]));
			EventBroadcaster.Instance.PostEvent(EventNames.ON_HIT_CUSTOMER);
		}

	}
	
	IEnumerator pauser()
	{
		yield return new WaitForSeconds(3);
	}

	private void ResetEnemy()
	{
		this.spawnedCount = 0;
		var usedObjects = objectPool.GetUsedObjects().ToArray();

		// Reset all objects
		for (var i = 0; i < usedObjects.Length; i++)
			objectPool.ReleasePoolable(usedObjects[i]);
	}

	private void Spawn(Parameters parameters)
	{
		const int SPAWN_COUNT = 1;

		if (!this.objectPool.HasObjectAvailable(SPAWN_COUNT)) return;
		var newEnemyList = this.objectPool.RequestPoolableBatch(SPAWN_COUNT);

		if (newEnemyList == null)
			return;

		for (var i = 0; i < newEnemyList.Length; i++)
		{
			var newEnemy = (Enemy) newEnemyList[i];

			var direction = (Direction)parameters.GetObjectExtra(PARAM_DIRECTION);
			var enemyClass = (EnemyClass)parameters.GetObjectExtra(PARAM_ENEMYCLASS);

			// Update enemy direction
			newEnemy.SetEnemyClass(enemyClass);
			newEnemy.ChangeDirection(direction);

			// Update layer
			if (direction == Direction.UP)
			{
				newEnemy.GetComponent<Renderer>().sortingLayerName = SortingLayerNames.CUSTOMER_TOP;
				newEnemy.GetComponent<Renderer>().sortingOrder = Math.Abs(this.spawnedCount--);

			}
			else
			{
				newEnemy.GetComponent<Renderer>().sortingLayerName = SortingLayerNames.CUSTOMER;
				newEnemy.GetComponent<Renderer>().sortingOrder = this.spawnedCount--;
			}
		}

	}

	public void OnHit(Parameters parameters)
	{
		// The enemy that has been hit
		var poolableObject = (APoolable) parameters.GetObjectExtra(PARAM_ENEMY_TO_HIT);
		StartCoroutine(FlyAnimation(poolableObject));
	}	

	private IEnumerator FlyAnimation(APoolable poolableObject)
	{
		var enemy = (Enemy)poolableObject;

		yield return new WaitForSeconds(enemy.GetFlyAnimationSpeed());

        // Destroy object
        enemy.transform.DOKill();
		this.objectPool.ReleasePoolable(poolableObject);
	}
}
