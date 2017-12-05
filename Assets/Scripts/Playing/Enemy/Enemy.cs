using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : APoolable, IFaceDirection {

	private IBoundaryListener boundaryListener;

	[SerializeField]
	private float stepLength = 50f;

	private Direction direction;
	private EnemyClass enemyClass;

	public bool IsHit { get; set; }

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

		ResetPosition();
	}

	private void ResetPosition()
	{
		this.transform.localPosition = GameManager.Instance.GetSpawnPointPosition(direction);
		this.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
		IsHit = false;
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

	public Direction GetDirection()
	{
		return this.direction;
	}
}
