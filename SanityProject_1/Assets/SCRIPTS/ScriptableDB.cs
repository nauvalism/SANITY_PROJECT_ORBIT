using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public enum SCDatabaseType
{
	Prefabs = 0,
	TextAssets = 1,
	Ship = 2
}


[CreateAssetMenu(fileName = "New Database", menuName = "Scriptables/Database", order = 1)]
public class ScriptableDB : ScriptableObject
{
	[SerializeField] SCDatabaseType dbType;
	[SerializeField] string databasePrefix;
	[SerializeField] List<GameObject> data;
	[SerializeField] string totalPath;
	protected void OnValidate()
	{
		string p = System.Enum.GetName(typeof(SCDatabaseType), dbType);
		totalPath = p+"/"+databasePrefix;
		data = new List<GameObject>(Resources.LoadAll<GameObject>(totalPath));	
	}
	
	public GameObject GetData(int index)
	{
		return data[index];
	}
}
