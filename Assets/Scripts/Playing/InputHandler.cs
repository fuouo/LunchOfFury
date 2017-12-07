using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SwipeDirection{
	None=0,
	Left=1,
	Right=2,
	Up=4,
	Down=8
}

public class InputHandler : MonoBehaviour {
	// Use this for initialization



	public SwipeDirection Direction{ set; get; }

	private Vector3 touchPosition;
	private float swipeResistance = 50.0f;


	void Start () {
	}

	public bool isSwiping(SwipeDirection direction){
		if (this.Direction == direction)
			return true;
		else
			return false;
	}
	// Update is called once per frame
	void Update () {


		Direction = SwipeDirection.None;

		if (Input.GetMouseButtonDown (0)) {
			touchPosition = Input.mousePosition;
		}
		if (Input.GetMouseButtonUp (0)) {
			Vector2 deltaSwipe = touchPosition - Input.mousePosition;

			if (Mathf.Abs (deltaSwipe.x) > swipeResistance) {
				Direction |=(deltaSwipe.x<0) ? SwipeDirection.Right:SwipeDirection.Left;
			}
			if (Mathf.Abs (deltaSwipe.y) > swipeResistance) {
				Direction |=(deltaSwipe.y<0) ? SwipeDirection.Up:SwipeDirection.Down;

			}
		}



		if (Input.GetKeyDown(KeyCode.W)||this.isSwiping(SwipeDirection.Up)) {
			Parameters parameters = new Parameters();
			parameters.PutObjectExtra(GameManager.PUNCH_DIRECTION, SwipeDirection.Up);
			EventBroadcaster.Instance.PostEvent (EventNames.ON_KEY_PRESSED, parameters);
		}
		else if (Input.GetKeyDown(KeyCode.A)||this.isSwiping(SwipeDirection.Left)) {
			Parameters parameters = new Parameters();
			parameters.PutObjectExtra(GameManager.PUNCH_DIRECTION, SwipeDirection.Left);
			EventBroadcaster.Instance.PostEvent (EventNames.ON_KEY_PRESSED, parameters);
		}
		else if (Input.GetKeyDown(KeyCode.D)||this.isSwiping(SwipeDirection.Right)) {
			Parameters parameters = new Parameters();
			parameters.PutObjectExtra(GameManager.PUNCH_DIRECTION, SwipeDirection.Right);
			EventBroadcaster.Instance.PostEvent (EventNames.ON_KEY_PRESSED, parameters);
		}
		else if (Input.GetKeyDown(KeyCode.S)||this.isSwiping(SwipeDirection.Down)) {
			Parameters parameters = new Parameters();
			parameters.PutObjectExtra(GameManager.PUNCH_DIRECTION, SwipeDirection.Down);
			EventBroadcaster.Instance.PostEvent (EventNames.ON_KEY_PRESSED, parameters);
		}
	}
}
