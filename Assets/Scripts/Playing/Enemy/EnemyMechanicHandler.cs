using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class EnemyMechanicHandler : MonoBehaviour
{
	public const string PARAM_DIRECTION = "PARAM_DIRECTION";
	public const string PARAM_ENEMYLIST = "PARAM_ENEMYLIST";

	// Use this for initialization
	void Start()
	{
		EventBroadcaster.Instance.AddObserver(EventNames.ON_SWIPE, HitEnemyByDirection);
		EventBroadcaster.Instance.AddObserver(EventNames.ON_FRENZY_ACTIVATED, HitAllEnemy);
	}

	void OnDestroy()
	{
		EventBroadcaster.Instance.RemoveActionAtObserver(EventNames.ON_SWIPE, HitEnemyByDirection);
		EventBroadcaster.Instance.AddObserver(EventNames.ON_FRENZY_ACTIVATED, HitAllEnemy);
	}

	// Update is called once per frame
	void Update()
	{

	}

	void HitEnemyByDirection(Parameters parameters)
	{
		var direction = (Direction) parameters.GetObjectExtra(PARAM_DIRECTION);
		var enemy = HitHandler.Instance.GetEnemyInDirection(direction);

		if (enemy == null)
			return;

		HitEnemy(enemy);

	}

	void HitEnemy(Enemy enemy, bool willIncreaseCombo = true)
	{
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

		// Shake camera
		CameraShake.Shake(0.15f, 0.25f);

		if (!willIncreaseCombo)
			return;

		EventBroadcaster.Instance.PostEvent(EventNames.ON_INCREASE_FRENZY_COMBO);
	}

	void HitAllEnemy(Parameters parameters)
	{
		var enemyListParam = parameters.GetObjectExtra(PARAM_ENEMYLIST);

		if (enemyListParam == null)
			return;

		var enemyList = (Enemy[]) enemyListParam;

		for (var i = 0; i < enemyList.Length; i++)
		{
			HitEnemy(enemyList[i]);
		}
	}
}
