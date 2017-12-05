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
	public const string ON_DEAD = "ON_DEAD"; //this is when player is ddead / has collided with customer
	public const string ON_PLAY = "ON_PLAY"; //this is when intro screen enters game screen. or gameover screen enters game screen

	//For FrontEnd 
	public const string ON_GAME_OVER = "ON_GAME_OVER"; //this is when intro screen enters game screen. or gameover screen enters game screen
	public const string ON_UPDATE_GOLD = "ON_UPDATE_GOLD"; //this is to tell UI to update the displayed gold
	public const string ON_UPDATE_COMBO = "ON_UPDATE_COMBO"; //this is to tell UI to update combo gauge

}







