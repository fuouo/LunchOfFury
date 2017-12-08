using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyType", menuName = "LunchRush/EntityType/Enemy", order = 1)]
public class EnemyType : EntityType {

	public static EnemyType[] LoadAll() {
		return Resources.LoadAll<EnemyType>("Customers");
	}


}
