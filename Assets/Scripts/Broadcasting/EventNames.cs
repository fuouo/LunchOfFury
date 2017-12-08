using UnityEngine;
using System.Collections;

/*
 * Holder for event names
 * Created By: NeilDG
 */ 
public class EventNames {

	//For BackEnd 
	public const string ON_NEXT_FRAME = "ON_NEXT_FRAME";
	public const string ON_SPAWN_REQUEST = "ON_SPAWN_REQUEST";

	public const string ON_HIT_CUSTOMER = "ON_HIT_CUSTOMER"; //this is when player hits the customer. 
	public const string ON_UPDATE_SCORE = "ON_UPDATE_SCORE"; //this is when player hits the customer. 
	public const string ON_DEAD = "ON_DEAD"; //this is when player is ddead / has collided with customer
	public const string ON_PLAY = "ON_PLAY"; //this is when intro screen enters game screen. or gameover screen enters game screen

	public const string ON_GAME_RESET = "ON_GAME_RESET"; //this is when intro screen enters game screen. or gameover screen enters game screen

	// Frenzy
	public const string ON_INCREASE_FRENZY_COMBO = "ON_INCREASE_FRENZY_COMBO";
	public const string ON_UPDATE_FRENZY_UI = "ON_UPDATE_FRENZY_UI";

	//For FrontEnd 
	public const string ON_GAME_OVER = "ON_GAME_OVER"; //this is when intro screen enters game screen. or gameover screen enters game screen
	public const string ON_UPDATE_GOLD_UI = "ON_UPDATE_GOLD_UI"; //this is to tell UI to update the displayed gold
	public const string ON_UPDATE_MAX_COMBO_UI = "ON_UPDATE_MAX_COMBO_UI"; //this is to tell UI to update max combo gauge

	//Call
	public const string START_GAME = "START_GAME"; //this is to tell UI to update max combo gauge

	//Controls
	public const string ON_SWIPE = "ON_SWIPE";

	//Player Events
	public const string ON_FRENZY_ACTIVATED = "ON_FRENZY_ACTIVATED";
	public const string ON_PLAYER_DEATH = "ON_PLAYER_DEATH";

	//BGM
	public const string INTRO_SOUND = "INTRO_SOUND";
	public const string IN_GAME_SOUND = "IN_GAME_SOUND";
	public const string GAME_OVER_SOUND = "GAME_OVER_SOUND";

	//SFX
	public const string BUTTON_CLICK = "BUTTON_CLICK";
	public const string DEATH = "DEATH";
	public const string SERVED = "SERVED";
	public const string HA = "HA";
	public const string FRENZY = "FRENZY";

}







