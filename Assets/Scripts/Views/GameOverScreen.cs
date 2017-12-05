using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : View {

	[SerializeField] Text EarnedGold;
	[SerializeField] Text CurrentGoldEarned;
	[SerializeField] Button PlayButton;
	[SerializeField] Text BestGold;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}


	public void OnPlay() {

		this.Hide ();
		ViewHandler.Instance.Show (ViewNames.PLAY_SCREEN);

		this.Hide ();
		//TODO: Notify GameManager that game starts
	}
}
