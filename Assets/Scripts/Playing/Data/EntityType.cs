using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityType : ScriptableObject {

	//data
	public string id;
	public string typeName;
	public int price;
	public bool locked;

	//art / assets
	public RuntimeAnimatorController animator;
	public Sprite defaultSprite;
}
