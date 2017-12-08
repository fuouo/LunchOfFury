using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
	[Header("Player Type")]
	[SerializeField] PlayerType type;


	// Use this for initialization
	void Start () {
		EventBroadcaster.Instance.AddObserver (EventNames.ON_SWIPE, this.punch);

		GetComponent<SpriteRenderer> ().sprite = type.defaultSprite;
		GetComponent<Animator> ().runtimeAnimatorController = type.animator;

	}

	// Update is called once per frame
	void Update () {
	}

	void punch(Parameters parameters){
		//		currentScore++;
		//EventBroadcaster.Instance.PostEvent (EventNames.SERVED);
//		SwipeDirection direction = (SwipeDirection) parameters.GetObjectExtra (GameManager.PUNCH_DIRECTION);
//
//		//		comboGauge.value = comboPoints;
//		updateCombo();
//
//		Debug.Log (direction);
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
