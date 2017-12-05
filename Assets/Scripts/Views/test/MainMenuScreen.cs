using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScreen : View {

	// Use this for initialization
	void Start () {
		
	}

	public void OnUIDemoClicked() {
		LoadManager.Instance.LoadScene (SceneNames.UI_DEMO_SCENE);
	}

	public void OnObjectPoolClicked() {
		LoadManager.Instance.LoadScene (SceneNames.OBJECT_POOL_SCENE);
	}

	public void OnBroadcastingDemoClicked() {
		LoadManager.Instance.LoadScene (SceneNames.BROADCASTING_SCENE);
	}

	public void OnSceneLightingClicked() {
		LoadManager.Instance.LoadScene (SceneNames.SCENE_LIGHTING);
	}
}
