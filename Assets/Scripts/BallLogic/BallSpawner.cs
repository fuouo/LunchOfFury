using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour, IBoundaryListener {

	[SerializeField] private GameObjectPool objectPool;
	[SerializeField] private BoundaryHandler boundaryHandler;

	private const float SPAWN_DELAY = 0.25f;
	private float ticks = 0.0f;

	// Use this for initialization
	void Start () {
		this.objectPool.Initialize ();
		this.boundaryHandler.SetListener (this);
	}
	
	// Update is called once per frame
	void Update () {
		this.ticks += Time.deltaTime;

		if (this.ticks >= SPAWN_DELAY) {
			this.ticks = 0.0f;

			int numObjects = Random.Range (1, 25);

			if (this.objectPool.HasObjectAvailable (numObjects)) {
				APoolable[] poolableObjects = this.objectPool.RequestPoolableBatch (numObjects);

				for (int i = 0; i < poolableObjects.Length; i++) {
					//apply random explosion force
					float x = Random.Range(20.0f, 50.0f);
					float y = Random.Range (20.0f, 50.0f);
					Vector3 force = new Vector3 (x, y, 0.0f);
					Rigidbody rigidBody = poolableObjects[i].GetComponent<Rigidbody>();
					rigidBody.AddForce (force);
				}
			}

		}
	}

	public void OnExitBoundary(APoolable poolableObject) {
		this.objectPool.ReleasePoolable (poolableObject);
	}
}
