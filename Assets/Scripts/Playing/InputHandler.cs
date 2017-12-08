using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour {
	
	public enum SwipeDirection
	{
		NONE = 0,
		LEFT = 1,
		RIGHT = 2,
		UP = 4,
		DOWN = 8
	}

	// Use this for initialization

	private SwipeDirection direction;

	private Vector3 touchPosition;
	private float swipeResistance = 50.0f;


	void Start ()
	{
	}

	public bool isSwiping(SwipeDirection direction)
	{
		return this.direction == direction;
	}

	// Update is called once per frame
	void Update ()
	{


		if (!GameManager.Instance.IsPlaying())
			return;

		direction = SwipeDirection.NONE;

		if (Input.GetMouseButtonDown (0)) {
			touchPosition = Input.mousePosition;
		}
		if (Input.GetMouseButtonUp (0)) {
			Vector2 deltaSwipe = touchPosition - Input.mousePosition;

			if (Mathf.Abs (deltaSwipe.x) > swipeResistance) {
				direction |= (deltaSwipe.x<0) ? SwipeDirection.RIGHT:SwipeDirection.LEFT;
			}
			if (Mathf.Abs (deltaSwipe.y) > swipeResistance) {
				direction |= (deltaSwipe.y<0) ? SwipeDirection.UP:SwipeDirection.DOWN;

			}
		}
		
		var parameters = new Parameters();
		
		if (Input.GetKeyDown(KeyCode.W) || this.isSwiping(SwipeDirection.UP))
			parameters.PutObjectExtra(EnemyMechanicHandler.PARAM_DIRECTION, Direction.UP);
		else if (Input.GetKeyDown(KeyCode.A) || this.isSwiping(SwipeDirection.LEFT))
			parameters.PutObjectExtra(EnemyMechanicHandler.PARAM_DIRECTION, Direction.LEFT);
		else if (Input.GetKeyDown(KeyCode.D) || this.isSwiping(SwipeDirection.RIGHT))
			parameters.PutObjectExtra(EnemyMechanicHandler.PARAM_DIRECTION, Direction.RIGHT);
		else if (Input.GetKeyDown(KeyCode.S) || this.isSwiping(SwipeDirection.DOWN))
			parameters.PutObjectExtra(EnemyMechanicHandler.PARAM_DIRECTION, Direction.DOWN);
		else
		{
			return;
		}

		// Notify on swipe
		EventBroadcaster.Instance.PostEvent(EventNames.ON_SWIPE, parameters);

	}
}
