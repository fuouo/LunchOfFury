using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMScript : MonoBehaviour {


	[SerializeField] AudioClip IntroClip;
	[SerializeField] AudioClip InGameClip;
	[SerializeField] AudioClip GameOverClip;

	[SerializeField] AudioSource AudioSource;
	// Use this for initialization
	void Start () {
		EventBroadcaster.Instance.AddObserver (EventNames.INTRO_SOUND, this.IntroStart);
		EventBroadcaster.Instance.AddObserver (EventNames.IN_GAME_SOUND, this.InGameStart);
		EventBroadcaster.Instance.AddObserver (EventNames.GAME_OVER_SOUND, this.GameOverStart);
	}

	// Update is called once per frame
	void Update () {

	}

	void IntroStart(){
		AudioSource.clip = IntroClip;
		AudioSource.Play ();

	}
	void InGameStart(){
		AudioSource.clip = InGameClip;
		AudioSource.Play ();

	}
	void GameOverStart(){
		AudioSource.clip = GameOverClip;
		AudioSource.Play ();

	}
}
