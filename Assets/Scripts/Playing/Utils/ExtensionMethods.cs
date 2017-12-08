using UnityEngine;
using System.Collections;

public static class ExtensionMethods
{
	public static float AngleBetweenVector(this Vector3 vec1, Vector3 vec2)
	{
		var diference = vec2 - vec1;
		var sign = (vec2.y < vec1.y) ? -1.0f : 1.0f;
		return Vector3.Angle(Vector2.right, diference) * sign;
	}
}
