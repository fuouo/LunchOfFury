using UnityEngine;
using System.Collections;

public enum Direction
{
	UP, DOWN, LEFT, RIGHT
}

public interface IFaceDirection
{
	void ChangeDirection(Direction direction);
}