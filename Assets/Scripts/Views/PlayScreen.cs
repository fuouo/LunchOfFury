using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayScreen : View {

	[SerializeField] Text CurrentGold;
	[SerializeField] Text GoldEarned;
	[SerializeField] Slider ComboGauge;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {

	}

	public void InitEarnedGold(){

	}

	public void AddToCurrentGold(){
		CurrentGold.text = int.Parse(CurrentGold.text) + 1 + "";
	}

	public void AddCombo(){
		//ADD COMBO TO GAUGE 
		//ComboGauge.value;

		
	}
}
