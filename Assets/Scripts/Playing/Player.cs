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
	[SerializeField] float comboIncrementRate =1;
	private float comboPoints;

	[Header("FOODS SERVED")]
	[SerializeField] GameObject currentFood;
	[SerializeField] GameObject[] foods;

	[Header("STATES")]
	[SerializeField] private GameObject gameOverPanel;
	private bool alive;

	//Animation Parameters
	private const string PUNCH_TRIGGER_PARAM = "punch";
	private const string IS_HIT_ANIM = "isHit";

	// Use this for initialization
	void Start () {
		EventBroadcaster.Instance.AddObserver (EventNames.ENEMY_PUNCHED, this.enemyPunched);
		EventBroadcaster.Instance.AddObserver (EventNames.ON_SWIPE, this.punch);
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
		EventBroadcaster.Instance.PostEvent (EventNames.ON_UPDATE_COMBO_UI,parameters);
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
			EventBroadcaster.Instance.PostEvent (EventNames.FRENZY);
			this.frenzy ();
		}
	}

	void punch(Parameters parameters){
		Direction direction = (Direction) parameters.GetObjectExtra (EnemyMechanicHandler.PARAM_DIRECTION);
		updateCombo();

		int randomFoodIndex = (int) Random.Range (0, foods.Length);

		currentFood.SetActive (false);
		currentFood = foods [randomFoodIndex];
		currentFood.SetActive (true);

		StartCoroutine (playPunchAnimation (direction));
	}	

	IEnumerator playPunchAnimation(Direction direction){
		GetComponent<Animator> ().SetInteger (PUNCH_TRIGGER_PARAM, (int)direction);
		Debug.Log (direction + " = " + (int)direction);
		yield return null;
		GetComponent<Animator> ().SetInteger (PUNCH_TRIGGER_PARAM, (int)Direction.NONE);


	}

	public bool isAlive(){
		return this.alive;
	}

	public void enemyPunched(){
		comboPoints=comboPoints+comboIncrementRate;
		//		comboGauge.value = comboPoints;
	}

	public void gameOver(){
		GetComponent<SpriteRenderer>().sortingLayerName = "Food";
		GetComponent<SpriteRenderer> ().sortingOrder = 999;
		GetComponent<Animator> ().SetInteger (IS_HIT_ANIM, 1); 

		//StartCoroutine(playDead ());
	}

	IEnumerator playDead(){
		yield return null;

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
