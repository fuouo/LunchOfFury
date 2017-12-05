using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallObject : APoolable {

	private const float START_Y_POS = -2.51f;
	private const float MIN_X_POS =  -2.46f;
	private const float MAX_X_POS = 2.46f;

	// Use this for initialization
	void Start () {
		
	}

	private void ResetPosition() {
		float randomX = Random.Range (MIN_X_POS, MAX_X_POS);
		Vector3 startPos = new Vector3 (randomX, START_Y_POS, 0.0f);

		this.transform.position = startPos;
	}

	public override void Initialize ()
	{
		this.ResetPosition ();
	}

	public override void Release ()
	{
		
	}

	public override void OnActivate ()
	{
		this.ResetPosition ();
	}
}
