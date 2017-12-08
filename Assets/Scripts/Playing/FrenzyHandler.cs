using UnityEngine;
using System.Collections;

public class FrenzyHandler : MonoBehaviour
{
	[SerializeField]
	private float requiredFrenzyCombo = 40.0f;

	[SerializeField]
	private float frenzyDecayRate = 0.05f;

	[SerializeField]
	private float comboIncrementRate = 5f;

	private float comboPoints;

	// Use this for initialization
	void Start()
	{
		EventBroadcaster.Instance.AddObserver(EventNames.ON_INCREASE_FRENZY_COMBO, this.IncreaseFrenzy);
		EventBroadcaster.Instance.AddObserver(EventNames.ON_GAME_RESET, this.ResetFrenzy);
	}

	void OnDestroy()
	{
		EventBroadcaster.Instance.RemoveActionAtObserver(EventNames.ON_INCREASE_FRENZY_COMBO, this.IncreaseFrenzy);
		EventBroadcaster.Instance.RemoveActionAtObserver(EventNames.ON_GAME_RESET, this.ResetFrenzy);
	}

	// Update is called once per frame
	void Update()
	{
		//		scoreText.text = currentScore.ToString ();
		if (comboPoints > 0)
		{
			DecayFrenzy();
		}

		if (comboPoints >= requiredFrenzyCombo)
		{
			this.ActivateFrenzy();
		}
	}

	public void ResetStat()
	{
		this.comboPoints = 0;
	}

	private void IncreaseFrenzy()
	{
		comboPoints = comboPoints + comboIncrementRate;
		UpdateFrenzy();
	}

	private void DecayFrenzy()
	{
		comboPoints -= frenzyDecayRate;
		UpdateFrenzy();
	}

	private void UpdateFrenzy()
	{
		var p = new Parameters();
		p.PutExtra(PlayScreen.PARAM_FRENZY_PERCENTAGE, comboPoints / requiredFrenzyCombo);
		EventBroadcaster.Instance.PostEvent(EventNames.ON_UPDATE_FRENZY_UI, p);
	}

	private void ResetFrenzy()
	{
		comboPoints = 0;
		UpdateFrenzy();
	}

	public void ActivateFrenzy()
	{
		ResetFrenzy();
		var enemyList = EnemySpawner.Instance.GetEnemyList();

		var p = new Parameters();
		p.PutObjectExtra(EnemyMechanicHandler.PARAM_ENEMYLIST, enemyList);
		EventBroadcaster.Instance.PostEvent(EventNames.ON_FRENZY_ACTIVATED, p);
	}
}
