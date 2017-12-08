using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXScript : MonoBehaviour {

	[SerializeField] AudioClip PunchHitClip;
	[SerializeField] AudioClip ServedClip;
	[SerializeField] AudioClip PunchMissClip;
	[SerializeField] AudioClip ButtonClickClip;
	[SerializeField] AudioClip DeathClip;
	[SerializeField] AudioClip HaClip;

	[SerializeField] AudioSource AudioSource;
	// Use this for initialization
	void Start () {
		EventBroadcaster.Instance.AddObserver (EventNames.BUTTON_CLICK, this.ButtonClick);
		EventBroadcaster.Instance.AddObserver (EventNames.DEATH, this.Death);
		EventBroadcaster.Instance.AddObserver (EventNames.SERVED, this.Served);
		EventBroadcaster.Instance.AddObserver (EventNames.HA, this.CustomerServed);

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void Served(){
		AudioSource.clip = ServedClip;
		AudioSource.Play ();

	}

	void ButtonClick(){
		AudioSource.clip = ButtonClickClip;
		AudioSource.Play ();

	}

	void CustomerServed(){

		AudioSource.clip = HaClip;
		AudioSource.Play ();
	}


	void Death(){
		AudioSource.clip = DeathClip;
		AudioSource.Play ();

	}
}
