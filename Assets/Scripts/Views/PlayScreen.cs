using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayScreen : View {
	public const string TAG = "[PlayScreen] ";

	public const string PARAM_CURRENT_SCORE = "PARAM_CURRENT_SCORE";
	public const string PARAM_FRENZY_PERCENTAGE = "PARAM_FRENZY_PERCENTAGE";
	public const string PUNCH_DIRECTION = "PUNCH_DIRECTION";

	[SerializeField] Text CurrentGold;
	[SerializeField] Text GoldEarned;
	[SerializeField] Slider FrenzyGauge;

	// Use this for initialization
	void Start () {
		EventBroadcaster.Instance.AddObserver(EventNames.ON_UPDATE_FRENZY_UI, this.UpdateFrenzy);
//		EventBroadcaster.Instance.AddObserver(EventNames.ON_UPDATE_MAX_COMBO_UI, this.UpdateMaxCombo);
		EventBroadcaster.Instance.AddObserver(EventNames.ON_UPDATE_GOLD_UI, this.UpdateCurrentGold); //this is for updating current gold
		EventBroadcaster.Instance.AddObserver(EventNames.ON_GAME_OVER, this.OnGameOver); //this is for when player is hit

		GoldEarned.text = GameManager.Instance.EarnedGold + "";
		CurrentGold.text = GameManager.Instance.CurrentGold + "";
		EventBroadcaster.Instance.PostEvent (EventNames.START_GAME);
		EventBroadcaster.Instance.PostEvent (EventNames.IN_GAME_SOUND);
	}

	// Update is called once per frame
	void Update () {

	}

	void OnDestroy() {
		EventBroadcaster.Instance.RemoveObserver (EventNames.ON_UPDATE_FRENZY_UI);
		EventBroadcaster.Instance.RemoveObserver(EventNames.ON_UPDATE_GOLD_UI); //this is for updating current gold
		EventBroadcaster.Instance.RemoveObserver(EventNames.ON_GAME_OVER); //this is for when player is hit
	}

	void UpdateFrenzy(Parameters parameters){
		var percentage = parameters.GetFloatExtra (PARAM_FRENZY_PERCENTAGE,0);
		FrenzyGauge.value = FrenzyGauge.maxValue * percentage;
	}

	public void InitEarnedGold(){

	}

	public void OnGameOver(){
		this.Hide ();
		ViewHandler.Instance.Show (ViewNames.GAMEOVER_SCREEN);
	}

	public void UpdateCurrentGold(Parameters parameters){
		CurrentGold.text = parameters.GetIntExtra (PARAM_CURRENT_SCORE, Int32.Parse (CurrentGold.text)) + "";
	}
	/*
	public void UpdateFrenzy(Parameters parameters){
		float currentCombo = parameters.GetFloatExtra (GameManager.PARAM_FRENZY_PERCENTAGE, FrenzyGauge.value + 1);
		float maxCombo = parameters.GetFloatExtra (GameManager.PARAM_FRENZY_PERCENTAGE, 100);

		FrenzyGauge.value = currentCombo / maxCombo;
	}*/
}
