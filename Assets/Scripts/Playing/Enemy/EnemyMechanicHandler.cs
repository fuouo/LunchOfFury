using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class EnemyMechanicHandler : MonoBehaviour
{
	public const string PARAM_DIRECTION = "PARAM_DIRECTION";

	// Use this for initialization
	void Start()
	{
		EventBroadcaster.Instance.AddObserver(EventNames.ON_SWIPE, HitEnemy);
	}

	void OnDestroy()
	{
		EventBroadcaster.Instance.RemoveActionAtObserver(EventNames.ON_SWIPE, HitEnemy);
	}

	// Update is called once per frame
	void Update()
	{

	}

	void HitEnemy(Parameters parameters)
	{
		var direction = (Direction) parameters.GetObjectExtra(PARAM_DIRECTION);
		var enemy = HitHandler.Instance.GetEnemyInDirection(direction);

		if (enemy == null)
			return;

		if (enemy.IsHit)
			return;

		enemy.IsHit = true;

		// Play fly animation
		enemy.PlayFlyAnimation();


		// Request release from EnemySpawner
		var p = new Parameters();
		p.PutObjectExtra(EnemySpawner.PARAM_ENEMY_TO_HIT, enemy);
		EventBroadcaster.Instance.PostEvent(EventNames.ON_HIT_CUSTOMER, p);
		EventBroadcaster.Instance.PostEvent(EventNames.ON_UPDATE_SCORE);

	}
}
