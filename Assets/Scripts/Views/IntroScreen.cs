﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroScreen : View {

	[SerializeField] Button PlayButton;


	[SerializeField] Image SelectedSprite;

	[SerializeField] Player CurrentlyBeingUsed;

	PlayerType[] playerTypes;
	int currentCounter =0;

	// Use this for initialization
	void Start () {
		playerTypes = PlayerType.LoadAll ();
		EventBroadcaster.Instance.PostEvent (EventNames.INTRO_SOUND);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnPlay() {

		EventBroadcaster.Instance.PostEvent (EventNames.BUTTON_CLICK);
		this.Hide ();
		ViewHandler.Instance.Show (ViewNames.PLAY_SCREEN);

		//TODO: Notify GameManager that game starts
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

	}
	public void OnClickPlayerSkinLeft(){
		EventBroadcaster.Instance.PostEvent (EventNames.BUTTON_CLICK);
		if (currentCounter - 1 == -1)
			currentCounter = playerTypes.Length - 1;
		else
			currentCounter--;


		SelectedSprite.sprite = playerTypes [currentCounter].defaultSprite;
	}
}
