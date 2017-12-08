using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour {
	
	void Start ()
	{
		// Support for gestures
		SimpleGesture.On4AxisFlickSwipeUp(() => SwipeCallback(Direction.UP));
		SimpleGesture.On4AxisFlickSwipeDown(() => SwipeCallback(Direction.DOWN));
		SimpleGesture.On4AxisFlickSwipeLeft(() => SwipeCallback(Direction.LEFT));
		SimpleGesture.On4AxisFlickSwipeRight(() => SwipeCallback(Direction.RIGHT));
	}

	// Update is called once per frame
	void Update ()
	{
		if (!GameManager.Instance.IsPlaying())
			return;
		
		Direction direction;

		if (Input.GetKeyDown(KeyCode.W))
			direction = Direction.UP;
		else if (Input.GetKeyDown(KeyCode.A))
			direction = Direction.LEFT;
		else if (Input.GetKeyDown(KeyCode.D))
			direction = Direction.RIGHT;
		else if (Input.GetKeyDown(KeyCode.S))
			direction = Direction.DOWN;
		else
		{
			return;
		}

		// Notify on swipe
		var parameters = new Parameters();
		parameters.PutObjectExtra(EnemyMechanicHandler.PARAM_DIRECTION, direction);
		EventBroadcaster.Instance.PostEvent(EventNames.ON_SWIPE, parameters);
	}

	private static void SwipeCallback(Direction direction)
	{
		if (!GameManager.Instance.IsPlaying())
			return;
		
		// Notify on swipe
		var parameters = new Parameters();
		parameters.PutObjectExtra(EnemyMechanicHandler.PARAM_DIRECTION, direction);
		EventBroadcaster.Instance.PostEvent(EventNames.ON_SWIPE, parameters);
	}
}
