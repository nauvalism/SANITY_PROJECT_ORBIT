using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleAnimation : MonoBehaviour
{
	public List<Transform> circles;
	
	public void RandomizeCircles()
	{
		for(int i = 0 ; i < circles.Count ; i++)
		{
			LeanTween.cancel(circles[i].gameObject);
			float addition = Random.Range(0.05f, 0.025f);
			int plusMinus = Random.Range(0,2);
			if(plusMinus == 0)
			{
				float _to = 1 + addition;
				LeanTween.scale(circles[i].gameObject, new Vector3(_to,_to,_to), Random.Range(2.5f, 4.0f)).setLoopType(LeanTweenType.pingPong).setEase(LeanTweenType.easeInOutCirc);
			}
			else
			{
				float _to = 1 - addition;
				LeanTween.scale(circles[i].gameObject, new Vector3(_to,_to,_to), Random.Range(2.5f, 4.0f)).setLoopType(LeanTweenType.pingPong).setEase(LeanTweenType.easeInOutCirc);
			}
		}
	}
	
	public void RestoreCirclez()
	{
		for(int i = 0 ; i < circles.Count ; i++)
		{
			LeanTween.cancel(circles[i].gameObject);
			LeanTween.scale(circles[i].gameObject, Vector3.one, 1.5f).setEase(LeanTweenType.easeOutQuad);
		}
	}
}
