using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDemoScreen : View {

	void Start() {

	}

	public void MainMenuClicked() {
		LoadManager.Instance.LoadScene (SceneNames.MAIN_SCENE);
	}

	public void OnUIDemoClicked() {
		ViewHandler.Instance.Show (ViewNames.BASIC_UI_SCREEN_NAME);
	}

	public void OnUIAnimationsClicked() {
		ViewHandler.Instance.Show (ViewNames.UI_ANIM_SCREEN_NAME);
	}

	public void OnInfiniteScrollClicked() {
		ViewHandler.Instance.Show (ViewNames.SCROLL_DEMO_SCREEN);
	}
}
