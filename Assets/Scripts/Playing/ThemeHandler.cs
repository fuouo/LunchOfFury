using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeHandler : MonoBehaviour {
	[Header("Type")]
	[SerializeField] Themes theme;

	[Header("Elements")]
	[SerializeField] GameObject frontCounter;
	[SerializeField] GameObject backCounter;
	[SerializeField] GameObject leftCounter;
	[SerializeField] GameObject rightCounter;
	[SerializeField] GameObject background;

	// Use this for initialization
	void Start () {

		frontCounter.GetComponent<SpriteRenderer> ().sprite = theme.frontCounter;
		backCounter.GetComponent<SpriteRenderer> ().sprite = theme.frontCounter;
		leftCounter.GetComponent<SpriteRenderer> ().sprite = theme.sideCounter;
		rightCounter.GetComponent<SpriteRenderer> ().sprite = theme.sideCounter;
		background.GetComponent<SpriteRenderer> ().sprite = theme.background;

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
