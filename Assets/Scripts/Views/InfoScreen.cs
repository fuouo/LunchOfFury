using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoScreen : View {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnTestPopupClicked() {
		NotificationDialog notifDiaglog = (NotificationDialog) DialogBuilder.Create (DialogBuilder.DialogType.NOTIFICATION);
		notifDiaglog.SetMessage ("Test popup");
	}
}
