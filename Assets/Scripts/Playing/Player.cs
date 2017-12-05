using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
	[SerializeField] Text scoreText;
	[SerializeField] float minimumComboForFrenzy = 40.0f;
	[SerializeField] float frenzyDecayRate = 0.05f;
	[SerializeField] private GameObject gameOverPanel;
	private int currentScore;
	private float comboPoints;
	private bool alive;

	[SerializeField] Animator animator;


	// Use this for initialization
	void Start () {
		EventBroadcaster.Instance.AddObserver (EventNames.ENEMY_PUNCHED, this.enemyPunched);
		EventBroadcaster.Instance.AddObserver (EventNames.PLAYER_DEATH, this.gameOver);
		currentScore = 0;
		comboPoints = 0;
		alive = true;
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

	public bool isAlive(){
		return this.alive;
	}

	public void enemyPunched(){
		currentScore++;
		comboPoints++;
	}

	public void gameOver(){
		alive = false;
		gameOverPanel.SetActive (true);
		gameOverPanel.transform.SetSiblingIndex (9999);

	}

	public void onClickPlayAgain(){
		//LoadManager.Instance.LoadScene (SceneNames.GAME_SCENE);	
	}

	private void frenzy(){
		
		comboPoints = 0;
		EventBroadcaster.Instance.PostEvent (EventNames.FRENZY_TRIGGERED);
	}

}
