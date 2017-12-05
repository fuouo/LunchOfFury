﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
	[SerializeField] Text scoreText;
	[SerializeField] float minimumComboForFrenzy = 40.0;
	[SerializeField] float frenzyDecayRate = 0.05;
	private int currentScore;
	private float comboPoints;

	// Use this for initialization
	void Start () {
		EventBroadcaster.Instance.AddObserver (EventNames.ENEMY_HIT, this.enemyHit);
		currentScore = 0;
		comboPoints = 0;
	}
	
	// Update is called once per frame
	void Update () {
		scoreText.text = currentScore.ToString ();
		if (comboPoints > 0) {
			comboPoints -= frenzyDecayRate;
		}

		if (comboPoints >= minimumComboForFrenzy) {
			this.frenzy ();
		}
	}

	public void enemyHit(){
		currentScore++;
		comboPoints++;
	}

	private void frenzy(){
		
		comboPoints = 0;
		EventBroadcaster.Instance.PostEvent (EventNames.FRENZY_TRIGGERED);
	}

}
