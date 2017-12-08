using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerType", menuName = "LunchRush/Player", order = 1)]
public class PlayerType : ScriptableObject {

	//data
	public string id;
	public string typeName;
	public int price;
	public bool locked;

	//art / assets
	public RuntimeAnimatorController animator;
	public Sprite defaultSprite;

	public static PlayerType[] LoadAll() {
		return Resources.LoadAll<PlayerType>("Players");
	}


}
