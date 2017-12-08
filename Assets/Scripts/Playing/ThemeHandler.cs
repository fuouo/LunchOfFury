using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeHandler : MonoBehaviour {
	[Header("Type")]
	[SerializeField] Themes theme;

	[Header("Elements")]
	[SerializeField] GameObject background;

	// Use this for initialization
	void Start () {

		background.GetComponent<SpriteRenderer> ().sprite = theme.background;

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
