using UnityEngine;
using System.Collections;
using UnityEditor;

//This class helps us make scriptable objects
public class MakeScriptableObject
{
	[MenuItem("Assets/Create/Enemy Database")]
	public static void CreateDatabaseAsset()
	{
		var database = ScriptableObject.CreateInstance<EnemyDatabase>();
		AssetDatabase.CreateAsset(database, "Assets/Resources/EnemyDatabase.asset");
		AssetDatabase.SaveAssets();
	}
}
