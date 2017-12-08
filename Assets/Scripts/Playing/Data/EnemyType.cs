using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyType", menuName = "LunchRush/Enemy", order = 1)]
public class EnemyType : ScriptableObject {

	//data
	public string id;
	public string typeName;
	public int price;
	public bool locked;

	//art / assets
	public RuntimeAnimatorController animator;
	public Sprite defaultSprite;

	public static EnemyType[] LoadAll() {
		return Resources.LoadAll<EnemyType>("Customers");
	}


}
