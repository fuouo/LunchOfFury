using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
	[SerializeField] float minimumComboForFrenzy = 40.0f;
	[SerializeField] float frenzyDecayRate = 0.05f;
	[SerializeField] private GameObject gameOverPanel;
	private float comboPoints;
	private bool alive;

	[SerializeField] Animator animator;


	// Use this for initialization
	void Start () {
		EventBroadcaster.Instance.AddObserver (EventNames.ENEMY_PUNCHED, this.enemyPunched);
		EventBroadcaster.Instance.AddObserver (EventNames.PLAYER_DEATH, this.gameOver);
		comboPoints = 0;
		alive = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (comboPoints > 0) {
			comboPoints -= frenzyDecayRate;
		}

		if (comboPoints >= minimumComboForFrenzy) {
			this.frenzy ();
		}
	}

	public bool isAlive(){
		return this.alive;
	}

	public void enemyPunched(){
		comboPoints++;
	}

	public void gameOver(){
		alive = false;
		gameOverPanel.SetActive (true);
		gameOverPanel.transform.SetSiblingIndex (9999);

	}

	public void onClickPlayAgain(){
		//LoadManager.Instance.LoadScene (SceneNames.GAME_SCENE);	
	}

	private void frenzy(){
		
		comboPoints = 0;
		EventBroadcaster.Instance.PostEvent (EventNames.FRENZY_TRIGGERED);
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var poolableObject = collision.gameObject.GetComponent<APoolable>();

        if (poolableObject == null)
            return;

        EventBroadcaster.Instance.PostEvent(EventNames.ON_DEAD);
    }

}
