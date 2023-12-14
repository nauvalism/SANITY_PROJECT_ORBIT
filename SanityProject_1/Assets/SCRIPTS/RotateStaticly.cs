using UnityEngine;
using System.Collections;

public class RotateStaticly : MonoBehaviour {
	public float Speed = 1;
	// Use this for initialization
	void Start () {
//		int Name = 0;
//		if (gameObject.name.IndexOf ("Static") != -1) {
//			Name = Random.Range (1, 6);
//			gameObject.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Obstacles/" + Name.ToString ());
//			//////Debug.Log (gameObject.name + " Is Color changeable ");
//		}
		//LeanTween.rotate(gameObject, new Vector3(.0f,.0f,719.0f), Speed).setLoopType(LeanTweenType.clamp).setIgnoreTimeScale(true);
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
