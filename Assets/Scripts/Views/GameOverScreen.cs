using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : View {

	[SerializeField] Text EarnedGold;
	[SerializeField] Text CurrentGoldEarned;
	[SerializeField] Text BestGold;
	[SerializeField] Button PlayButton;


	[SerializeField] Text price;


	[SerializeField] Image SelectedSprite;

	[SerializeField] Player CurrentlyBeingUsed;

	PlayerType[] playerTypes;
	int currentCounter =0;
	// Use this for initialization
	void Start () {

		playerTypes = PlayerType.LoadAll ();
		EarnedGold.text = GameManager.Instance.EarnedGold + "";
		CurrentGoldEarned.text = GameManager.Instance.CurrentGold + "";
		BestGold.text = GameManager.Instance.BestGold + "";
		EventBroadcaster.Instance.PostEvent (EventNames.GAME_OVER_SOUND);

	}

	// Update is called once per frame
	void Update () {

	}
	public void OnBuy(){
		if (playerTypes [currentCounter].locked == false) {
			this.OnPlay ();
		}else{
			if (playerTypes [currentCounter].price < GameManager.Instance.getEarnedGold()) {
				GameManager.Instance.setEarnedGold (GameManager.Instance.getEarnedGold() - playerTypes [currentCounter].price);
				//Switch Player



			}
		}
	}


	public void OnPlay() {
		EventBroadcaster.Instance.PostEvent (EventNames.BUTTON_CLICK);
		this.Hide ();
		ViewHandler.Instance.Show (ViewNames.PLAY_SCREEN);

		//Notify GameManager that game starts
		EventBroadcaster.Instance.PostEvent (EventNames.ON_PLAY);

	}


	public void OnClickPlayerSkinRight(){
		//		SelectedSprite.sprite;
		EventBroadcaster.Instance.PostEvent (EventNames.BUTTON_CLICK);
		if (currentCounter + 1 == playerTypes.Length)
			currentCounter = 0;
		else
			currentCounter++;

		SelectedSprite.sprite = playerTypes [currentCounter].defaultSprite;
		if (playerTypes [currentCounter].locked == false) {
			price.text = "";
			//SWITCH PLAYER



		} else {
			price.text = playerTypes [currentCounter].price + " Coins";
		}

	}
	public void OnClickPlayerSkinLeft(){
		EventBroadcaster.Instance.PostEvent (EventNames.BUTTON_CLICK);
		if (currentCounter - 1 == -1)
			currentCounter = playerTypes.Length - 1;
		else
			currentCounter--;

		SelectedSprite.sprite = playerTypes [currentCounter].defaultSprite;

		if (playerTypes [currentCounter].locked == false) {
			price.text = "";
			//SWITCH PLAYER



		} else {
			price.text = playerTypes [currentCounter].price + " Coins";

		}
	}
}
