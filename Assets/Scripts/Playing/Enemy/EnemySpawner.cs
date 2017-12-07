using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour, IHitListener {
	
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
		this.objectPool.Initialize();
		this.hitHandler.SetListener(this);

	}

	private void Frenzy(){
		List <APoolable> objects =  objectPool.GetUsedObjects();

		objects.ForEach (delegate(APoolable poolableObject) {
			this.objectPool.ReleasePoolable(poolableObject);

			EventBroadcaster.Instance.PostEvent(EventNames.ON_HIT_CUSTOMER);
		});

	}

	private void OnDestroy()
	{
		EventBroadcaster.Instance.RemoveActionAtObserver(EventNames.ON_SPAWN_REQUEST, Spawn);
	}

	private void Spawn(Parameters parameters)
	{
		const int SPAWN_COUNT = 1;

		if (!this.objectPool.HasObjectAvailable(SPAWN_COUNT)) return;
		var newEnemy = (Enemy) this.objectPool.RequestPoolableBatch(SPAWN_COUNT)[0];

		var direction = (Direction) parameters.GetObjectExtra(PARAM_DIRECTION);
		Debug.Log(direction);
		var enemyClass = (EnemyClass) parameters.GetObjectExtra(PARAM_ENEMYCLASS);

		// Update enemy direction
		newEnemy.SetEnemyClass(enemyClass);
		newEnemy.ChangeDirection(direction);
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
