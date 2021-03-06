﻿using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Enemy : APoolable, IFaceDirection {

	[SerializeField]
	private float stepLength = 50f;

	// Fly speed in seconds
	[SerializeField]
	private float flyAnimationSpeed = 2f;

	[SerializeField] 
	GameObject food;

	private IBoundaryListener boundaryListener;
	private Random random;

	private Direction direction;
	private EnemyClass enemyClass;

	public bool IsHit { get; set; }

	// Use this for initialization
	private void Start()
	{
		random = new Random();
		EventBroadcaster.Instance.AddObserver(EventNames.ON_NEXT_FRAME, Step);
	}

	private void OnDestroy()
	{
		EventBroadcaster.Instance.RemoveActionAtObserver(EventNames.ON_NEXT_FRAME, Step);
	}

	private void Step()
	{
        if (IsHit)
            return;

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
				newPosition.x = newPosition.x + stepLength;
				break;
			case Direction.RIGHT:
				newPosition.x = newPosition.x - stepLength;
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}

        this.transform.DOMove(newPosition, 0.5f * GameManager.Instance.GetCurrentSpeed()).SetEase(Ease.OutExpo);
		//this.transform.localPosition = newPosition;
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
		}

		ResetPosition();
	}

	private void ResetPosition()
	{
		this.transform.DOKill();
		this.transform.localPosition = GameManager.Instance.GetSpawnPointPosition(direction);
		this.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
		this.transform.localScale = Vector3.one;
		IsHit = false;
		food.GetComponent<SpriteRenderer> ().sprite = null;
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

	public void PlayFlyAnimation()
	{
		EventBroadcaster.Instance.PostEvent (EventNames.HA);
		const float MIN = -10;
		const float MAX = 10;

		Parameters p = new Parameters ();
		p.PutObjectExtra (Player.SERVED_CUSTOMER_KEY, this);
		p.PutObjectExtra (Player.SERVED_DIR_CUSTOMER_KEY, this.GetDirection());
		EventBroadcaster.Instance.PostEvent (EventNames.ON_HIT_CUSTOMER, p);


		// Update sprite
		this.GetComponent<SpriteRenderer>().sprite = enemyClass.HitSprite;

		var endPosition = GameManager.Instance.GetSpawnPointPosition(this.GetDirection());
		endPosition.x *= 1.5f;
		endPosition.y *= 1.5f;
		endPosition.y += GetRandomNumber(MIN, MAX);
		endPosition.x += GetRandomNumber(MIN, MAX);

		// Change angle based on new position
		this.transform.DORotate(new Vector3(0f, 0f, this.transform.localPosition.AngleBetweenVector(endPosition)), this.flyAnimationSpeed / 2);

		this.transform.localScale = new Vector3 (1.5f, 1.5f, 1.5f);

		// Change position on fly
		this.transform.DOMove(endPosition, this.flyAnimationSpeed)
			.SetEase(Ease.OutExpo);
	}

	private float GetRandomNumber(float min, float max)
	{
		return (float)random.NextDouble() * (max - min) + min;
	}

	public float GetFlyAnimationSpeed()
	{
		return this.flyAnimationSpeed;
	}

	public void SetFood(Sprite food){
		this.food.GetComponent<SpriteRenderer> ().sprite = food;
	}
}
