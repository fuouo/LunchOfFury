using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
	[Header("Player Type")]
	[SerializeField] PlayerType type;

	[Header("FOODS SERVED")]
	[SerializeField] GameObject currentFood;
	[SerializeField] GameObject[] foods;

	[Header("Frenzy")]
	[SerializeField]  GameObject FrenzyEffect;

	[Header("Smoke Particles")]
	[SerializeField]  ParticleSystem SmokeN;
	[SerializeField]  ParticleSystem SmokeW;
	[SerializeField]  ParticleSystem SmokeE;
	[SerializeField]  ParticleSystem SmokeS;


	//STATES
	private bool isAlive;

	//Animation Parameters
	private const string PUNCH_TRIGGER_PARAM = "punch";
	private const string IS_HIT_ANIM = "isHit";

	//IMPORTANT DELAYS
	public const float FRENZY_DELAY = 1.5f;

	//KEYS
	public const string PLAYER_TYPE_KEY = "PLAYER_TYPE";
	public const string SERVED_CUSTOMER_KEY = "SERVED_CUSTOMER_KEY";
	public const string SERVED_DIR_CUSTOMER_KEY = "SERVED_CUSTOMER_DIRECTION_KEY";

	// Use this for initialization
	void Start () {
		EventBroadcaster.Instance.AddObserver (EventNames.ON_SWIPE, this.punch);
		EventBroadcaster.Instance.AddObserver (EventNames.ON_FRENZY_ACTIVATED, this.playFrenzy);
		EventBroadcaster.Instance.AddObserver (EventNames.ON_PLAYER_DEATH, this.playDead);
		EventBroadcaster.Instance.AddObserver (EventNames.ON_GAME_RESET, this.ResetStats);
		EventBroadcaster.Instance.AddObserver (EventNames.ON_SET_PLAYERTYPE, this.SetPlayerType);
		EventBroadcaster.Instance.AddObserver (EventNames.ON_HIT_CUSTOMER, this.OnHitCustomer);
	
		GetComponent<SpriteRenderer> ().sprite = type.defaultSprite;
		GetComponent<Animator> ().runtimeAnimatorController = type.animator;

		ResetStats ();

	}

	// Update is called once per frame
	void Update () {
	}

	void OnHitCustomer(Parameters p){
		var enemy = (Enemy)p.GetObjectExtra (SERVED_CUSTOMER_KEY);
		var direction = (Direction)p.GetObjectExtra(SERVED_DIR_CUSTOMER_KEY);
		enemy.SetFood (currentFood.GetComponent<SpriteRenderer>().sprite);

		if (direction == Direction.UP) {
			SmokeN.Play ();
		}
		if (direction == Direction.LEFT) {
			SmokeW.Play ();
		}
		if (direction == Direction.RIGHT) {
			SmokeE.Play ();
		}
		if (direction == Direction.DOWN) {
			SmokeS.Play ();
		}

	}

	void SetPlayerType(Parameters param){
		type  = (PlayerType) param.GetObjectExtra(PLAYER_TYPE_KEY);

		GetComponent<SpriteRenderer> ().sprite = type.defaultSprite;
		GetComponent<Animator> ().runtimeAnimatorController = type.animator;
	}
	void ResetStats(){
		FrenzyEffect.GetComponent<ParticleSystem> ().Pause();
		SmokeN.Pause (); 
		SmokeW.Pause ();
		SmokeE.Pause ();
		SmokeS.Pause ();
		GetComponent<SpriteRenderer>().sortingLayerName = "Player";
		GetComponent<SpriteRenderer> ().sortingOrder = 1;
		GetComponent<Animator> ().SetInteger (PUNCH_TRIGGER_PARAM, (int)Direction.NONE);
		GetComponent<Animator> ().ResetTrigger (IS_HIT_ANIM);
		isAlive = true;


	}

	void punch(Parameters parameters){
		Direction direction = (Direction) parameters.GetObjectExtra (EnemyMechanicHandler.PARAM_DIRECTION);

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

	public void playDead(){	
		if (isAlive)
			isAlive = false;
		else
			return;
		GetComponent<SpriteRenderer>().sortingLayerName = "Food";
		GetComponent<SpriteRenderer> ().sortingOrder = 999;
		StartCoroutine (animDead());

	}

	IEnumerator animDead(){
		GetComponent<Animator> ().ResetTrigger (IS_HIT_ANIM);
		GetComponent<Animator> ().SetTrigger (IS_HIT_ANIM);
		yield return new WaitForSeconds (0.5f);
		GetComponent<Animator> ().ResetTrigger (IS_HIT_ANIM);
	}

	public void playFrenzy(Parameters parameter){
		StartCoroutine (animFrenzy ());
	}
	IEnumerator animFrenzy(){
		FrenzyEffect.GetComponent<ParticleSystem> ().Play ();
		yield return null;
	}


	private void OnTriggerEnter2D(Collider2D collision)
	{
		var poolableObject = collision.gameObject.GetComponent<APoolable>();

		if (poolableObject == null)
			return;

		// To fix player dying even the customer is hit
		var enemy = (Enemy) poolableObject;

		if (enemy.IsHit)
			return;

		EventBroadcaster.Instance.PostEvent(EventNames.ON_DEAD);
	}

}
