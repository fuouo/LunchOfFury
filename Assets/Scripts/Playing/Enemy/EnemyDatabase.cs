using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDatabase : ScriptableObject
{
	[SerializeField]
	private EnemyClass[] enemyClassList;

	public EnemyClass[] GetEnemyClassList()
	{
		return this.enemyClassList;
	}
}
