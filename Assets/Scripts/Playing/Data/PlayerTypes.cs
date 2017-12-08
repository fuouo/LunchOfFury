using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerType", menuName = "LunchRush/EntityType/Player", order = 1)]
public class PlayerType : EntityType {


	public static PlayerType[] LoadAll() {
		return Resources.LoadAll<PlayerType>("Players");
	}


}
