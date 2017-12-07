using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
	[Header("Player Type")]
	[SerializeField] PlayerType type;


	[Header("COMBO")]
	[SerializeField] float minimumComboForFrenzy = 40.0f;
	[SerializeField] float frenzyDecayRate = 0.05f;
	private float comboPoints;

	[Header("STATES")]
	[SerializeField] private GameObject gameOverPanel;
	private bool alive;


	// Use this for initialization
	void Start () {
		EventBroadcaster.Instance.AddObserver (EventNames.ENEMY_PUNCHED, this.enemyPunched);
		EventBroadcaster.Instance.AddObserver (EventNames.ON_KEY_PRESSED, this.punch);
		EventBroadcaster.Instance.AddObserver (EventNames.PLAYER_DEATH, this.gameOver);
		comboPoints = 0;
		alive = true;

		GetComponent<SpriteRenderer> ().sprite = type.defaultSprite;
		GetComponent<Animator> ().runtimeAnimatorController = type.animator;

	}

	void updateCombo(){

		Debug.Log ("WENT IN START");

		Parameters parameters = new Parameters();
		parameters.PutExtra(GameManager.MAX_COMBO_KEY, minimumComboForFrenzy);
		parameters.PutExtra (GameManager.CURRENT_COMBO_KEY, comboPoints);
		EventBroadcaster.Instance.PostEvent (EventNames.ON_UPDATE_COMBO,parameters);
	}
	
	// Update is called once per frame
	void Update () {
//		scoreText.text = currentScore.ToString ();
		if (comboPoints > 0) {
			comboPoints -= frenzyDecayRate;
//			comboGauge.value = comboPoints;
			updateCombo();
		}

		if (comboPoints >= minimumComboForFrenzy) {
			this.frenzy ();
		}
	}

	void punch(Parameters parameters){
		//		currentScore++;

		SwipeDirection direction = (SwipeDirection) parameters.GetObjectExtra (GameManager.PUNCH_DIRECTION);
		comboPoints++;
		//		comboGauge.value = comboPoints;
		updateCombo();

		Debug.Log (direction);
	}	



	public bool isAlive(){
		return this.alive;
	}

	public void enemyPunched(){
		comboPoints++;
//		comboGauge.value = comboPoints;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var poolableObject = collision.gameObject.GetComponent<APoolable>();

        if (poolableObject == null)
            return;

        EventBroadcaster.Instance.PostEvent(EventNames.ON_DEAD);
    }

}
