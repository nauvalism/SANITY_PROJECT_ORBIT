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

	private void Start() {
		//SpawnTurret(0);
	}
	
	public void ActivateActiveTurret()
	{
		spawnedTurretB.ShowTurret();
		spawnedTurretB.ActivateTurret();
	}

	public void DeactivateActiveturret(bool dimmediate = false)
	{
		if(spawnedTurretB != null)
		{
			spawnedTurretB.DeactivateTurret(dimmediate);
		}
		
	}

	public void DestroyAllTurrets()
	{
		List<BaseTurrets> allTurrets = new List<BaseTurrets>(FindObjectsOfType<BaseTurrets>());
		for(int i = 0 ; i < allTurrets.Count; i++)
		{
			Destroy(allTurrets[i].transform.parent.parent.gameObject);
		}
	}

	public void HideActiveTurret()
	{
		spawnedTurretB.HideTurret();
	}

	public void SetTurretPosition(Transform t)
	{
		this.spawnTurretPosition = t;
	}
    
	public GameObject SpawnTurret(int index)
	{
		if(spawnedTurret != null)
		{
			spawnedTurretB.DeactivateTurret();
			Destroy(spawnedTurret);
		}
		GameObject prefab = turretDB.GetData(index);
		spawnedTurret = (GameObject)Instantiate(prefab, spawnTurretPosition.position, Quaternion.identity, turretParent);
		spawnedTurret.transform.localScale = Vector3.zero;
		//spawnedTurret.transform.localPosition = Vector3.zero;
		spawnedTurretB = spawnedTurret.transform.GetChild(0).GetChild(0).GetComponent<BaseTurrets>();
		
		spawnedTurretB.transform.position = ArenaManager.instance.GetActiveArena().GetTurretnSpawnPlace().position;
		
		//spawnedTurretB.ShowTurret();
		return spawnedTurret;
	}

	public GameObject SpawnTurret(int index, Transform turretPosition)
	{
		if(spawnedTurret != null)
		{
			spawnedTurretB.DeactivateTurret();
			Destroy(spawnedTurret);
		}

		GameObject prefab = turretDB.GetData(index);
		spawnedTurret = (GameObject)Instantiate(prefab, spawnTurretPosition.position, Quaternion.identity, turretParent);
		spawnedTurret.transform.localScale = Vector3.zero;
		spawnedTurret.transform.localPosition = Vector3.zero;
		spawnedTurretB = spawnedTurret.transform.GetChild(0).GetChild(0).GetComponent<BaseTurrets>();
		
		spawnedTurretB.transform.position = turretPosition.position;

		//spawnedTurretB.ShowTurret();
		return spawnedTurret;
	}

	public Transform GetBulletParent()
	{
		return bulletParent;
	}

	public void LevelupTurret()
	{
		spawnedTurretB.AddLevel();
	}

	

}
