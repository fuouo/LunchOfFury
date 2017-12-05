using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallSpawnControlUI : MonoBehaviour {

	[SerializeField] private InputField inputField;
	// Use this for initialization
	void Start () {
		
	}

	public void OnSpawnClicked() {
		if (this.inputField.text!= "") {
			int numBalls = int.Parse (this.inputField.text);
			Debug.Log ("Num balls to spawn is: " + numBalls);

			Parameters parameters = new Parameters ();
			parameters.PutExtra (BallObjectCreator.NUM_BALLS_KEY, numBalls);

			EventBroadcaster.Instance.PostEvent (BallSpawnEventNames.ON_SPAWN_EVENT, parameters);
		}
	}

	public void OnClearClicked() {
		EventBroadcaster.Instance.PostEvent (BallSpawnEventNames.ON_CLEAR_EVENT);
	}

	public void OnMainMenuClicked() {
		LoadManager.Instance.LoadScene (SceneNames.MAIN_SCENE);
	}
}
