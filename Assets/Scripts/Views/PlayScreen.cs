using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayScreen : View {
	public const string TAG = "[PlayScreen] ";

	[SerializeField] Text CurrentGold;
	[SerializeField] Text GoldEarned;
	[SerializeField] Slider ComboGauge;

	// Use this for initialization
	void Start () {
		EventBroadcaster.Instance.AddObserver(EventNames.ON_UPDATE_COMBO_UI, this.UpdateCombo);
//		EventBroadcaster.Instance.AddObserver(EventNames.ON_UPDATE_MAX_COMBO_UI, this.UpdateMaxCombo);
		EventBroadcaster.Instance.AddObserver(EventNames.ON_UPDATE_GOLD_UI, this.UpdateCurrentGold); //this is for updating current gold
		EventBroadcaster.Instance.AddObserver(EventNames.ON_GAME_OVER, this.OnGameOver); //this is for when player is hit

		GoldEarned.text = GameManager.Instance.EarnedGold + "";
		CurrentGold.text = GameManager.Instance.CurrentGold + "";
		EventBroadcaster.Instance.PostEvent (EventNames.START_GAME);
	}

	// Update is called once per frame
	void Update () {

	}

	void OnDestroy() {
		EventBroadcaster.Instance.RemoveObserver (EventNames.ON_UPDATE_COMBO_UI);
		EventBroadcaster.Instance.RemoveObserver(EventNames.ON_UPDATE_GOLD_UI); //this is for updating current gold
		EventBroadcaster.Instance.RemoveObserver(EventNames.ON_GAME_OVER); //this is for when player is hit
	}

	void UpdateCombo(Parameters parameters){
		Debug.Log ("WENT IN MAXCOMBO");
		float minimumComboForFrenzy = parameters.GetFloatExtra (GameManager.MAX_COMBO_KEY,0);
		float currentCombo = parameters.GetFloatExtra (GameManager.CURRENT_COMBO_KEY,0);
		ComboGauge.maxValue = minimumComboForFrenzy;
		ComboGauge.value = currentCombo;
	}

	public void InitEarnedGold(){

	}

	public void OnGameOver(){
		this.Hide ();
		ViewHandler.Instance.Show (ViewNames.GAMEOVER_SCREEN);
	}

	public void UpdateCurrentGold(Parameters parameters){
		Debug.Log (CurrentGold);
		CurrentGold.text = parameters.GetIntExtra (GameManager.CURRENT_GOLD_KEY, int.Parse (CurrentGold.text)) + "";
	}
	/*
	public void UpdateCombo(Parameters parameters){
		float currentCombo = parameters.GetFloatExtra (GameManager.CURRENT_COMBO_KEY, ComboGauge.value + 1);
		float maxCombo = parameters.GetFloatExtra (GameManager.CURRENT_COMBO_KEY, 100);

		ComboGauge.value = currentCombo / maxCombo;
	}*/
}
