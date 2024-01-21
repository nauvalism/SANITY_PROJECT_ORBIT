using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum bulletType
{
	regular = 0,
	stay = 1
}

public class BaseTurrets : MonoBehaviour
{
	[SerializeField] protected int turretID = 0;
	[SerializeField] protected int turretOrder = 0;
	[SerializeField] protected BaseShip target;
	
	[Header("Main Area")]
	[SerializeField] protected  float healthPoints = 100;
	[SerializeField] protected  float spawnRate;
	[SerializeField] protected  Transform crossHair;
	[SerializeField] protected  Transform mover;
	
	[Header("Bullets")]
	[SerializeField] protected  bulletType typeOfBullet;
	[SerializeField] protected  Transform bulletSpawnFrom;
	[SerializeField] protected  GameObject bulletPrefab;
	[SerializeField] protected  List<BaseBullets> bulletPool;
	[SerializeField] protected  Vector3 mainSpawnPosition;
	[SerializeField] protected  float spacePerPool = 1.0f;
	[SerializeField] protected  int poolLimit = 20;
	[SerializeField] protected  int bulletIndex = 0;

	[Header("Effects")]
	[SerializeField] ParticleSystem spawnParticle;
	
	protected void OnValidate()
	{
		int rawID = (int)typeOfBullet;
		bulletPrefab = Resources.Load<GameObject>("Prefabs/Bullets/BULLET-"+rawID.ToString());
		float _xmsp = 500;
		float _ymsp = 100 + (turretID * 10);
		
		mainSpawnPosition = new Vector2(_xmsp, _ymsp);
	}
	
	// Awake is called when the script instance is being loaded.
	protected void Awake()
	{
		
	}

	protected void Start() {
		InitPool();
	}
	
	public virtual void InitPool()
	{
		bulletPool = new List<BaseBullets>();
		for(int i = 0 ; i < poolLimit ; i++)
		{
			GameObject go;
			Vector3 _pos = new Vector2(mainSpawnPosition.x + (spacePerPool * i), mainSpawnPosition.y);
			go = (GameObject)Instantiate(bulletPrefab, _pos, Quaternion.identity, TBEController.instance.GetBulletParent());
			bulletPool.Add(go.transform.GetChild(1).GetChild(0).GetComponent<BaseBullets>());
			bulletPool[i].Init(this);
			bulletPool[i].Deactivate();
		}
	}
	
	public virtual void ClearPool()
	{
		if(bulletPool != null)
		{
			if(bulletPool.Count > 0)
			{
				for(int i = 0 ; i < bulletPool.Count ; i++)
				{
					Destroy(bulletPool[i]);
				}
			}
		}
		
		bulletPool = new List<BaseBullets>();
	}
	
	public virtual void LaunchBullet()
	{
		spawnParticle.Play();
		int prevIndex = -1;
		if(bulletIndex == 0)
		{
			prevIndex = (bulletPool.Count -1);
		}
		else
		{
			prevIndex = bulletIndex - 1;
		}
		//GetTopObject(bulletPool[prevIndex]).SetActive(false);
		GetTopObject(bulletPool[bulletIndex]).SetActive(true);
		
		Vector2 dir = target.transform.position - bulletSpawnFrom.position;
		dir.Normalize();
		
		
		bulletPool[bulletIndex].Launch(crossHair);
		//bulletPool[bulletIndex].Launch();
		bulletIndex++;
	}

	GameObject GetTopObject(BaseBullets bullet)
	{
		return bullet.transform.parent.parent.gameObject;
	}
	
	public virtual void LaunchBullets(int howMany, float gap)
	{
		StartCoroutine(LaunchBulletConsecutively());
		IEnumerator LaunchBulletConsecutively()
		{
			for(int i = 0 ; i < howMany ; i++)
			{
				LaunchBullet();
				yield return new WaitForSeconds(gap);
			}
		}
	}
	
	
	
	public virtual void ShowTurret()
	{
		
	}
	
	public virtual void ShowTurrte(Vector3 where)
	{
		
	}
	
	public virtual void ActivateTurret()
	{
		
	}
	
	
	
	public virtual void DeactivateTurret()
	{
		
	}
	
	public void RotateTowardsTarget()
	{
		Vector3 dir = (Vector2)target.transform.position - (Vector2)transform.position;
		//get the angle from current direction facing to desired target
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		//set the angle into a quaternion + sprite offset depending on initial sprite facing direction
		Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
		transform.rotation = rotation;
	}

	public Vector3 GetSpawnPlace()
	{
		return bulletSpawnFrom.position;
	}

}
