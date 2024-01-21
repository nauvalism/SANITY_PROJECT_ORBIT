using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTurret : BaseTurrets
{
	[SerializeField] float rotationaddition;
	[SerializeField] float stepLimit = 1.0f;
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
				transform.Rotate(new Vector3(0,0,_add));
			}
			else
			{
				RandomizeRotation();
			}
			base.LaunchBullet();	
		}

		
	}
	
	public override void ActivateTurret()
	{
		base.ActivateTurret();
		//InvokeRepeating("LaunchBullet", 0, 1.0f);
		LaunchBullet();
	}

    public override void DeactivateTurret()
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
