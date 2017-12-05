using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : View {

	[SerializeField] Text EarnedGold;
	[SerializeField] Text CurrentGoldEarned;
	[SerializeField] Text BestGold;
	[SerializeField] Button PlayButton;

	// Use this for initialization
	void Start () {

		EarnedGold.text = GameManager.Instance.EarnedGold + "";
		CurrentGoldEarned.text = GameManager.Instance.CurrentGold + "";
		BestGold.text = GameManager.Instance.BestGold + "";

	}

	// Update is called once per frame
	void Update () {

	}


	public void OnPlay() {
		this.Hide ();
		ViewHandler.Instance.Show (ViewNames.PLAY_SCREEN);

		//Notify GameManager that game starts
		EventBroadcaster.Instance.PostEvent (EventNames.ON_PLAY);

	}
}
