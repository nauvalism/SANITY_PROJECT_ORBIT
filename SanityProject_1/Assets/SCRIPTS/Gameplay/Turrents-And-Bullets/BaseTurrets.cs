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
	[SerializeField] protected int turretLevel = 0;
	[SerializeField] protected float defaultScale = 1;
	[SerializeField] protected float toScale = 1.1f;
	[SerializeField] protected List<TurretAttributes> turretProfiles;
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
	[SerializeField] protected ParticleSystem spawnParticle;
	[SerializeField] protected Transform graphicRoot;
	[SerializeField] protected SoundManager gunSound;
	
	protected void OnValidate()
	{
		int rawID = (int)typeOfBullet;
		bulletPrefab = Resources.Load<GameObject>("Prefabs/Bullets/BULLET-"+rawID.ToString());
		float _xmsp = 500;
		float _ymsp = 100 + (turretID * 10);
		
		mainSpawnPosition = new Vector2(_xmsp, _ymsp);
		turretProfiles = new List<TurretAttributes>(Resources.LoadAll<TurretAttributes>("TurretProfiles/Turret-"+turretID));
	}
	
	// Awake is called when the script instance is being loaded.
	protected virtual void Awake()
	{
		
	}

	protected virtual void Start() {
		InitPool();
		target = FindObjectOfType<BaseShip>();
	}

	protected virtual void Update() {
		if(graphicRoot.transform.localScale.x > defaultScale)
        {
            graphicRoot.transform.localScale = new Vector3(graphicRoot.transform.localScale.x - Time.deltaTime, graphicRoot.transform.localScale.y - Time.deltaTime, 0);
        }
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
		gunSound.Play(0);
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
		if(bulletIndex >= bulletPool.Count)
		{
			bulletIndex = 0;
		}
		graphicRoot.localScale = new Vector3(toScale, toScale, toScale);
	}

	public GameObject GetTopObject(BaseBullets bullet)
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
	
	public virtual void AddLevel(int amount = 1)
	{
		this.turretLevel += amount;
		bool max = false;
		if(turretLevel >= turretProfiles.Count)
		{
			max = true;
			turretLevel = turretProfiles.Count -1;
		}
		ApplyProfile(max);

	}

	public virtual void ApplyProfile(bool max)
	{
		if(max)
		{
			toScale += 0.05f;
			defaultScale += 0.05f;
		}
		else
		{
			this.toScale = turretProfiles[turretLevel].scaleTo;
			this.defaultScale = turretProfiles[turretLevel].scale;
			

		}
		graphicRoot.localScale = new Vector3(toScale, toScale, toScale);
		graphicRoot.GetComponent<SpriteRenderer>().sprite = turretProfiles[turretLevel].spriteChange;
		graphicRoot.GetComponent<SpriteRenderer>().color = turretProfiles[turretLevel].levelColor;
	}
	
	public virtual void ShowTurret()
	{
		Transform tr = transform.parent.parent;
		LeanTween.cancel(tr.gameObject);
		
		tr.localScale = Vector3.one;
		LeanTween.scale(tr.gameObject, new Vector3(1.25f,1.25f,1.25f), 1.0f).setEase(LeanTweenType.punch);
	}

	public void HideTurret()
	{
		Transform tr = transform.parent.parent;
		LeanTween.cancel(tr.gameObject);
		LeanTween.scale(tr.gameObject, Vector3.zero, .25f).setEase(LeanTweenType.easeOutQuad).setDestroyOnComplete(true);
	}
	
	public virtual void ShowTurret(Vector3 where)
	{
		
	}
	
	public virtual void ActivateTurret()
	{
		
	}
	
	
	
	public virtual void DeactivateTurret(bool destroyimmediate = false)
	{
		StopAllCoroutines();
		for(int i = 0 ; i < bulletPool.Count ; i++)
		{
			try
			{
				if(bulletPool[i].isActiveAndEnabled == false)
				{
					Destroy(bulletPool[i]);
				}
				else
				{
					if(destroyimmediate)
					{
						Destroy(bulletPool[i].gameObject);
					}
					else
					{
						bulletPool[i].DestroyAfter();
					}
					
				}
			}
			catch(System.Exception e)
			{

			}
			
		}
	}

	

	public void ChangeAttribute(int level)
	{

	}

	public void ApplyAttribute()
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
