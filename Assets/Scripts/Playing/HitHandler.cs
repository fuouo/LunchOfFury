using System;
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
		OnTriggerStay2D(collision);
	}


	void OnTriggerStay2D(Collider2D collision)
	{
		var poolableObject = collision.gameObject.GetComponent<APoolable>();

		if (poolableObject == null)
			return;

		var enemy = (Enemy) poolableObject;

		// TODO: Remove later; for testing of scoring lang
		switch (enemy.GetDirection())
		{
			case Direction.UP:
				if (!Input.GetKey(KeyCode.W))
					return;
				break;
			case Direction.DOWN:
				if (!Input.GetKey(KeyCode.S))
					return;
				break;
			case Direction.LEFT:
				if (!Input.GetKey(KeyCode.D))
					return;
				break;
			case Direction.RIGHT:
				if (!Input.GetKey(KeyCode.A))
					return;
				break;
		}

		// Check first if already hit to avoid double points
		if (enemy.IsHit)
			return;

		enemy.IsHit = true;

		EventBroadcaster.Instance.PostEvent (EventNames.ENEMY_PUNCHED);
		if (this.hitListener != null) {
			this.hitListener.OnHit(poolableObject);
		}

		// Update score
		EventBroadcaster.Instance.PostEvent(EventNames.ON_HIT_CUSTOMER);
	}
}
