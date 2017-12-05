using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitHandler : MonoBehaviour {

	private IHitListener hitListener;

	// Use this for initialization
	void Start () {
		
	}

	void OnDestroy() {
		this.hitListener = null;
	}

	public void SetListener(IHitListener hitListener) {
		this.hitListener = hitListener;
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		Debug.Log("OnTriggerEnter");
		var poolableObject = collision.gameObject.GetComponent<APoolable> ();

		if (this.hitListener != null) {
			this.hitListener.OnHit(poolableObject);
		}
	}
}
