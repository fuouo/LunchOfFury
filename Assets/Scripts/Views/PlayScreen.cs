using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayScreen : View {
	public const string TAG = "[PlayScreen] ";

	public const string CURRENT_GOLD_KEY = "CURRENT_GOLD_KEY";
	public const string MAX_COMBO_KEY = "MAX_COMBO_KEY";
	public const string CURRENT_COMBO_KEY = "CURRENT_COMBO_KEY";


	[SerializeField] Text CurrentGold;
	[SerializeField] Text GoldEarned;
	[SerializeField] Slider ComboGauge;

	// Use this for initialization
	void Start () {
		EventBroadcaster.Instance.AddObserver(EventNames.ON_UPDATE_COMBO, this.UpdateCombo);
		EventBroadcaster.Instance.AddObserver(EventNames.ON_UPDATE_GOLD, this.UpdateCurrentGold); //this is for updating current gold
		EventBroadcaster.Instance.AddObserver(EventNames.ON_GAME_OVER, this.OnGameOver); //this is for when player is hit

		GoldEarned.text = GameManager.Instance.EarnedGold + "";

	}

	// Update is called once per frame
	void Update () {

	}

	public void InitEarnedGold(){

	}

	public void OnGameOver(){
		this.Hide ();
		ViewHandler.Instance.Show (ViewNames.GAMEOVER_SCREEN);
	}

	public void UpdateCurrentGold(Parameters parameters){
		CurrentGold.text = parameters.GetIntExtra (CURRENT_GOLD_KEY, int.Parse (CurrentGold.text)) + "";
	}

	public void UpdateCombo(Parameters parameters){
		Debug.Log (TAG + "Updating Current Combo" +  ComboGauge.value + 1);

		float currentCombo = parameters.GetFloatExtra (CURRENT_COMBO_KEY, ComboGauge.value + 1);
		float maxCombo = parameters.GetFloatExtra (CURRENT_COMBO_KEY, 100);

		ComboGauge.value = currentCombo / maxCombo;
	}
}
