using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroScreen : View {

	[SerializeField] Button PlayButton;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnPlay() {

		this.Hide ();
		ViewHandler.Instance.Show (ViewNames.PLAY_SCREEN);

		//TODO: Notify GameManager that game starts
		EventBroadcaster.Instance.PostEvent (EventNames.ON_PLAY);

	}
}
