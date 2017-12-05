using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolUIScreen : View {

	// Use this for initialization
	void Start () {
		
	}

	public void OnMainMenuClicked() {
		LoadManager.Instance.LoadScene (SceneNames.MAIN_SCENE);
	}

}
