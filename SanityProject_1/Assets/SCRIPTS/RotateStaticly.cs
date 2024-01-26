using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RotateStaticly : MonoBehaviour {
	public float Speed = 1;
	public bool randomDirection = false;
	public List<float> speedRandom;
	// Use this for initialization
	void Start () {
		if(speedRandom != null)
		{
			if(speedRandom.Count == 2)
			{
				Speed = Random.Range(speedRandom[0], speedRandom[1]);
				if(randomDirection)
				{
					int modifier = Random.Range(0,2);
					if(modifier == 0)
					{
						Speed *= -1;
					}
				}
				
				
			}
		}
		
		
	}
	
	// Update is called once per frame
	void Update () {
		//gameObject.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, gameObject.transform.rotation.z + Time.deltaTime));
		
	}

    private void FixedUpdate()
    {
        transform.Rotate(0, 0, Time.fixedDeltaTime * Speed);
    }
}
