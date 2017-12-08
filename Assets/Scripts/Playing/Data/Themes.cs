using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Theme", menuName = "LunchRush/Themes", order = 1)]
public class Themes : ScriptableObject {

	//data
	public string id;
	public string typeName;
	public string price;
	public bool locked;

	//art / assets
	//background
	public Sprite background;


	public static Themes[] LoadAll() {
		return Resources.LoadAll<Themes>("Themes");
	}


}
