using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class HitHandler : MonoBehaviour {
	
	private static HitHandler sharedInstance = null;

	private Dictionary<Direction, Enemy> enemyInDirection;

	public static HitHandler Instance
	{
		get
		{
			return sharedInstance;
		}
	}

	void Awake()
	{
		sharedInstance = this;
		enemyInDirection = new Dictionary<Direction, Enemy>
		{
			{Direction.UP, null},
			{Direction.DOWN, null},
			{Direction.LEFT, null},
			{Direction.RIGHT, null},
		};
	}

	// Use this for initialization
	void Start () {
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		OnTriggerStay2D(collision);
	}
	
	void OnTriggerStay2D(Collider2D collider)
	{
		UpdateEnemyInDirection(collider, false);
	}

	void OnTriggerExit2D(Collider2D collider)
	{
		UpdateEnemyInDirection(collider, true);
	}

	private void UpdateEnemyInDirection(Collider2D collider, bool isExit)
	{
		var poolableObject = collider.gameObject.GetComponent<APoolable>();

		if (poolableObject == null)
			return;

		var enemy = (Enemy)poolableObject;

		// Remove enemy instance in the direction
		if (!isExit)
			enemyInDirection[enemy.GetDirection()] = enemy;
		else
			enemyInDirection[enemy.GetDirection()] = null;
	}

	public Enemy GetEnemyInDirection(Direction direction)
	{
		return enemyInDirection[direction];
	}

	
}
