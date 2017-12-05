using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : APoolable, IFaceDirection {

	[SerializeField]
	private float stepLength = 50f;

	private Direction direction;
	private EnemyClass enemyClass;

	// Use this for initialization
	private void Start()
	{
		EventBroadcaster.Instance.AddObserver(EventNames.ON_NEXT_FRAME, Step);
	}

	private void OnDestroy()
	{
		EventBroadcaster.Instance.RemoveActionAtObserver(EventNames.ON_NEXT_FRAME, Step);
	}

	private void Step()
	{
		var newPosition = this.transform.localPosition;

		switch (direction)
		{
			case Direction.UP:
				newPosition.y = newPosition.y - stepLength;
				break;
			case Direction.DOWN:
				newPosition.y = newPosition.y + stepLength;
				break;
			case Direction.LEFT:
				newPosition.x = newPosition.x - stepLength;
				break;
			case Direction.RIGHT:
				newPosition.x = newPosition.x + stepLength;
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}

		this.transform.localPosition = newPosition;
	}

	public void SetEnemyClass(EnemyClass enemyClass)
	{
		this.enemyClass = enemyClass;
		this.ChangeDirection(direction);
	}

	public void ChangeDirection(Direction direction)
	{
		this.direction = direction;

		var spriteRenderer = this.GetComponent<SpriteRenderer>();

		switch (direction)
		{
			case Direction.UP:
				spriteRenderer.sprite = enemyClass.FaceUpSprite;
				break;
			case Direction.DOWN:
				spriteRenderer.sprite = enemyClass.FaceDownSprite;
				break;
			case Direction.LEFT:
				spriteRenderer.sprite = enemyClass.FaceLeftSprite;
				break;
			case Direction.RIGHT:
				spriteRenderer.sprite = enemyClass.FaceRightSprite;
				break;
			default:
				throw new ArgumentOutOfRangeException("direction", direction, null);
		}
	}

	private void ResetPosition()
	{
		this.transform.localPosition = GameManager.Instance.GetSpawnPointPosition(direction);
		Debug.Log(this.transform.localPosition);
	}

	public override void Initialize()
	{
		this.ResetPosition();
	}

	public override void Release()
	{

	}

	public override void OnActivate()
	{
		this.ResetPosition();
	}
}
