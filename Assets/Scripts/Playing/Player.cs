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

	//STATES
	private bool isAlive;

	//Animation Parameters
	private const string PUNCH_TRIGGER_PARAM = "punch";
	private const string IS_HIT_ANIM = "isHit";

	//IMPORTANT DELAYS
	public const float FRENZY_DELAY = 1.5f;

	// Use this for initialization
	void Start () {
		EventBroadcaster.Instance.AddObserver (EventNames.ON_SWIPE, this.punch);
		EventBroadcaster.Instance.AddObserver (EventNames.ON_FRENZY_ACTIVATED, this.playFrenzy);
		EventBroadcaster.Instance.AddObserver (EventNames.ON_PLAYER_DEATH, this.playDead);
		EventBroadcaster.Instance.AddObserver (EventNames.ON_GAME_RESET, this.ResetStats);
	
		GetComponent<SpriteRenderer> ().sprite = type.defaultSprite;
		GetComponent<Animator> ().runtimeAnimatorController = type.animator;

		ResetStats ();

	}

	// Update is called once per frame
	void Update () {
	}

	void ResetStats(){
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
		GetComponent<Animator> ().ResetTrigger (IS_HIT_ANIM);
		GetComponent<Animator> ().SetTrigger (IS_HIT_ANIM);
		GetComponent<Animator> ().ResetTrigger (IS_HIT_ANIM);
	}

	public void playFrenzy(Parameters parameter){
		StartCoroutine (animFrenzy ());
	}
	IEnumerator animFrenzy(){
		FrenzyEffect.SetActive (true);
		FrenzyEffect.GetComponent<ParticleSystem> ().Play ();
		yield return new WaitForSeconds (FRENZY_DELAY);
		FrenzyEffect.SetActive (false);
		FrenzyEffect.GetComponent<ParticleSystem> ().Pause ();
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
