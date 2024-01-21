using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TBEController : MonoBehaviour
{
	public static TBEController instance {get;set;}
	
	[SerializeField] ScriptableDB turretDB;
	[SerializeField] ScriptableDB bulletDB;
	[SerializeField] GameObject spawnedTurret;
	[SerializeField] BaseTurrets spawnedTurretB;
	[SerializeField] Transform spawnTurretPosition;
	[SerializeField] Transform turretParent;
	[SerializeField] Transform bulletParent;

	void Awake()
	{
		instance = this;
	}
	
	public void ActivateActiveTurret()
	{
		spawnedTurretB.ActivateTurret();
	}
    
	public GameObject SpawnTurret(int index)
	{
		GameObject prefab = turretDB.GetData(index);
		spawnedTurret = (GameObject)Instantiate(prefab, spawnTurretPosition.position, Quaternion.identity, turretParent);
		spawnedTurret.transform.localScale = Vector3.zero;
		spawnedTurret.transform.localPosition = Vector3.zero;
		spawnedTurretB = spawnedTurret.transform.GetChild(0).GetChild(0).GetComponent<BaseTurrets>();
		spawnedTurretB.ShowTurret();
		return spawnedTurret;
	}

	public Transform GetBulletParent()
	{
		return bulletParent;
	}
}
