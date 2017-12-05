using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimScreen : View {

	// Use this for initialization
	void Start () {
		this.SetCancelable (false); //make this UI not cancelable through outside clicks or ESC button.	
	}

	public void OnFromMidClicked() {
		this.ShowModifiedEntranceType (DOViewAnimation.EntranceType.FROM_MID);
	}

	public void OnFromTopClicked() {
		this.ShowModifiedEntranceType (DOViewAnimation.EntranceType.FROM_TOP);
	}

	public void OnFromBtmClicked() {
		this.ShowModifiedEntranceType (DOViewAnimation.EntranceType.FROM_BOT);
	}

	public void OnToMidClicked() {
		this.SetHOAnimation (this.GetEntranceType (), DOViewAnimation.ExitType.TO_MID);
		this.Hide ();
	}

	public void OnToTopClicked() {
		this.SetHOAnimation (this.GetEntranceType (), DOViewAnimation.ExitType.TO_TOP);
		this.Hide ();
	}

	public void OnToBtmClicked() {
		this.SetHOAnimation (this.GetEntranceType (), DOViewAnimation.ExitType.TO_BOT);
		this.Hide ();
	}

	private void ShowModifiedEntranceType(DOViewAnimation.EntranceType entranceType) {
		UIAnimScreen uiAnimScreen = (UIAnimScreen) ViewHandler.Instance.Show(ViewNames.UI_ANIM_SCREEN_NAME, true);
		uiAnimScreen.DoNotDestroy (); //do not destroy the view but put it into the view pool
		ViewHandler.Instance.HideCurrentView (); //immediately hide the view first then change the animation
		uiAnimScreen.SetHOAnimation(entranceType, uiAnimScreen.GetExitType());
		uiAnimScreen.Show ();
		uiAnimScreen.MakeDestroyable (); //make the copy destroyable so it does not 'leak'.
	}



}
