﻿using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour, IHitListener
{

	private int spawnedCount;

	[SerializeField]
	private GameObjectPool objectPool;

	[SerializeField]
	private HitHandler hitHandler;

	public const string PARAM_DIRECTION = "PARAM_DIRECTION";
	public const string PARAM_ENEMYCLASS = "PARAM_ENEMYCLASS";

	private System.Random random;

	// Use this for initialization
	void Start()
	{
		random = new System.Random();

		EventBroadcaster.Instance.AddObserver (EventNames.FRENZY_TRIGGERED, this.Frenzy);
		EventBroadcaster.Instance.AddObserver(EventNames.ON_SPAWN_REQUEST, Spawn);
		EventBroadcaster.Instance.AddObserver(EventNames.ON_GAME_RESET, ResetEnemy);
		this.objectPool.Initialize();
		this.hitHandler.SetListener(this);

	}

	private void OnDestroy()
	{
		EventBroadcaster.Instance.RemoveActionAtObserver(EventNames.ON_SPAWN_REQUEST, Spawn);
		EventBroadcaster.Instance.RemoveActionAtObserver(EventNames.ON_GAME_RESET, ResetEnemy);
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
			newEnemy.GetComponent<Renderer>().sortingOrder = this.spawnedCount--;
		}

	}

	public void OnHit(APoolable poolableObject)
	{
		StartCoroutine(FlyAnimation(poolableObject));
	}

	private IEnumerator FlyAnimation(APoolable poolableObject)
	{
		var enemy = (Enemy)poolableObject;

		const float MIN = -15;
		const float MAX = 15;

		var endPosition = GameManager.Instance.GetSpawnPointPosition(enemy.GetDirection()) * 3;
		endPosition.y += GetRandomNumber(MIN, MAX);
		endPosition.x += GetRandomNumber(MIN, MAX);

		// Change angle based on new position
		enemy.transform.DORotate(new Vector3(0f, 0f, AngleBetweenVector2(enemy.transform.localPosition, endPosition)), 1f);

		// Change position on fly
		enemy.transform.DOMove(endPosition, 2f)
			.SetEase(Ease.OutExpo);

		yield return new WaitForSeconds(2f);

        // Destroy object
        enemy.transform.DOKill();
		this.objectPool.ReleasePoolable(poolableObject);
	}

	private float GetRandomNumber(float min, float max)
	{
		return (float) random.NextDouble() * (max - min) + min;
	}

	private static float AngleBetweenVector2(Vector2 vec1, Vector2 vec2)
	{
		var diference = vec2 - vec1;
		var sign = (vec2.y < vec1.y) ? -1.0f : 1.0f;
		return Vector2.Angle(Vector2.right, diference) * sign;
	}
}
