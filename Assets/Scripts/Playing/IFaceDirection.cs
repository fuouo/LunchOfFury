using UnityEngine;
using System.Collections;

public enum Direction
{
	UP=4, DOWN=8, LEFT=1, RIGHT=2, NONE=0,
}

public interface IFaceDirection
{
	void ChangeDirection(Direction direction);
}