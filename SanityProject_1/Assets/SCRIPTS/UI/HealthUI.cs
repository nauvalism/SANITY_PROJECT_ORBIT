using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] Transform rootManipulation;
    [SerializeField] Image healthUIGraphic;
    [SerializeField] Animator healthAnimator;

    public void Appear(bool fromZero = false)
    {
        LeanTween.cancel(rootManipulation.gameObject);
        if(fromZero)
            rootManipulation.transform.localScale = Vector3.zero;
        LeanTween.scale(rootManipulation.gameObject, Vector3.one, 0.25f).setEase(LeanTweenType.easeOutBack);
    }

    public void Dissapear(bool fromOne)
    {
        LeanTween.cancel(rootManipulation.gameObject);
        if(healthUIGraphic.color.a < 1)
            return;
        if(fromOne)
        rootManipulation.localScale = Vector3.one;
        healthUIGraphic.color = Color.white;
        LeanTween.scale(rootManipulation.gameObject, new Vector3(1.5f,1.5f,1.5f), 1.0f);
        LeanTween.alpha(healthUIGraphic.GetComponent<RectTransform>(), .0f, .5f);
    }
}
