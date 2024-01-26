using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTurret : BaseTurrets
{
	[SerializeField] float rotationaddition;
	[SerializeField] float stepLimit = 1.0f;
	[SerializeField] List<Transform> spawnPlaces;
	[SerializeField] List<Transform> crossHairs;
	[SerializeField] List<ParticleSystem> spawnParticles;
	public override void LaunchBullet()
	{
		
		StartCoroutine(LaunchingBullet());
		IEnumerator LaunchingBullet()
		{
			float _step = 0;
			while(true)
			{
				while(_step < stepLimit)
				{
					_step += Time.deltaTime;
					yield return null;
					
				}
				_step = 0;
				DoLaunchBullet();
			}
			

		}

		void DoLaunchBullet()
		{
			if(target)
			{
				//crossHair.position = target.transform.position;
				RotateTowardsTarget();
				float _add = Random.Range(0.0f, rotationaddition);
				int multiplicator = target.GetDirection();
				transform.Rotate(new Vector3(0,0,_add *  multiplicator));
			}
			else
			{
				RandomizeRotation();
			}
			LaunchBulletOnly();
			//base.LaunchBullet();	
		}

		
	}

	public void LaunchBulletOnly()
	{
		gunSound.Play(0);
		int prevIndex = -1;

		for(int i = 0 ; i < spawnPlaces.Count ; i++)
		{
			if(bulletIndex == 0)
			{
				prevIndex = (bulletPool.Count -1);
			}
			else
			{
				prevIndex = bulletIndex - 1;
			}

			GetTopObject(bulletPool[bulletIndex]).SetActive(true);
		
			Vector2 dir = target.transform.position - bulletSpawnFrom.position;
			dir.Normalize();
			
			spawnParticles[i].Play();
			bulletPool[bulletIndex].Launch(spawnPlaces[i], crossHairs[i]);
			
			//bulletPool[bulletIndex].Launch();
			
			bulletIndex++;
			if(bulletIndex >= bulletPool.Count)
			{
				bulletIndex = 0;
			}
			
		}
		graphicRoot.localScale = new Vector3(toScale, toScale, toScale);
	}

    public override void LaunchBullets(int howMany, float gap)
    {
        gunSound.Play(0);
		int prevIndex = -1;

		for(int i = 0 ; i < spawnPlaces.Count ; i++)
		{
			if(bulletIndex == 0)
			{
				prevIndex = (bulletPool.Count -1);
			}
			else
			{
				prevIndex = bulletIndex - 1;
			}

			GetTopObject(bulletPool[bulletIndex]).SetActive(true);
		
			Vector2 dir = target.transform.position - bulletSpawnFrom.position;
			dir.Normalize();
			
			spawnParticles[i].Play();
			bulletPool[bulletIndex].Launch(spawnPlaces[i], crossHairs[i]);
			
			//bulletPool[bulletIndex].Launch();
			
			bulletIndex++;
			if(bulletIndex >= bulletPool.Count)
			{
				bulletIndex = 0;
			}
			
		}
		graphicRoot.localScale = new Vector3(toScale, toScale, toScale);
		
		//GetTopObject(bulletPool[prevIndex]).SetActive(false);
		
		
		
		
    }

    
	
	public override void ActivateTurret()
	{
		base.ActivateTurret();
		//InvokeRepeating("LaunchBullet", 0, 1.0f);
		LaunchBullet();
	}

    public override void DeactivateTurret(bool destroyimmediate = false)
    {
		
		StopAllCoroutines();
        base.DeactivateTurret();
    }


    
	
	
	void RandomizeRotation()
	{
		float randomRotation = Random.Range(0,360.0f);
		mover.Rotate(new Vector3(0,0,randomRotation));
	}
}
