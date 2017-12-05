using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicUIScreen : View {

	[SerializeField] private Text displayText;

	// Use this for initialization
	void Start () {
		
	}

	public void SetText(string text) {
		this.displayText.text = text;
	}

	public void OnShowNextClicked() {
		BasicUIScreenModel.Instance.IncrementDuplicate ();
		BasicUIScreen basicUIScreen = (BasicUIScreen) ViewHandler.Instance.Show(ViewNames.BASIC_UI_SCREEN_NAME, true);
		basicUIScreen.SetText ("This is a new basic UI instance #: " + BasicUIScreenModel.Instance.GetDuplicateCount().ToString());
	}

	public void OnExitClicked() {
		this.Hide ();
	}

	public override void OnHideCompleted ()
	{
		BasicUIScreenModel.Instance.DecrementDuplicate ();
		base.OnHideCompleted ();
	}
}
