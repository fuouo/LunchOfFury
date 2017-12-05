using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour {
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown(KeyCode.W)) {
			EventBroadcaster.Instance.PostEvent (EventNames.ON_KEY_PRESSED_W);
		}
		else if (Input.GetKeyDown(KeyCode.A)) {
			EventBroadcaster.Instance.PostEvent (EventNames.ON_KEY_PRESSED_A);
		}
		else if (Input.GetKeyDown(KeyCode.D)) {
			EventBroadcaster.Instance.PostEvent (EventNames.ON_KEY_PRESSED_D);
		}
		else if (Input.GetKeyDown(KeyCode.S)) {
			EventBroadcaster.Instance.PostEvent (EventNames.ON_KEY_PRESSED_S);
		}
	}
}
