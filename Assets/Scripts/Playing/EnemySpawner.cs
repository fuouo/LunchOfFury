using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
	
	[SerializeField]
	private GameObjectPool objectPool;

	public const string PARAM_DIRECTION = "PARAM_DIRECTION";
	public const string PARAM_ENEMYCLASS = "PARAM_ENEMYCLASS";

	

	// Use this for initialization
	void Start()
	{
		EventBroadcaster.Instance.AddObserver(EventNames.ON_SPAWN_REQUEST, Spawn);
		this.objectPool.Initialize();
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
}
