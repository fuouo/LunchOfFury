using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallObjectCreator : MonoBehaviour {

	public const string NUM_BALLS_KEY = "NUM_BALLS_KEY";

	[SerializeField] private Transform spawnParent;
	[SerializeField] private GameObject ballCopy;

	private List<GameObject> spawnedBalls = new List<GameObject>();

	// Use this for initialization
	void Start () {
		this.ballCopy.SetActive (false);

		//this.SpawnBall (5);

		EventBroadcaster.Instance.AddObserver (BallSpawnEventNames.ON_SPAWN_EVENT, this.OnSpawnEvent);
		EventBroadcaster.Instance.AddObserver (BallSpawnEventNames.ON_CLEAR_EVENT, this.OnClearEvent);
	}

	void OnDestroy() {
		EventBroadcaster.Instance.RemoveObserver (BallSpawnEventNames.ON_SPAWN_EVENT);
		EventBroadcaster.Instance.RemoveObserver (BallSpawnEventNames.ON_CLEAR_EVENT);
	}

	private void SpawnBall(int numBalls) {
		for (int i = 0; i < numBalls; i++) {
			GameObject newBall = GameObject.Instantiate<GameObject> (this.ballCopy, this.spawnParent);
			Vector3 position = Vector3.zero; position.x = Random.Range (-5.0f, 5.0f); position.z = Random.Range (-5.0f, 5.0f);

			newBall.transform.localPosition = position;
			newBall.gameObject.SetActive (true);

			this.spawnedBalls.Add (newBall);
		}
	}

	private void OnSpawnEvent(Parameters parameters) {
		int numBalls = parameters.GetIntExtra (NUM_BALLS_KEY, 0);
		this.SpawnBall (numBalls);
	}

	private void OnClearEvent() {
		for (int i = 0; i < this.spawnedBalls.Count; i++) {
			GameObject.Destroy (this.spawnedBalls [i]);
		}

		this.spawnedBalls.Clear ();
	}
}
