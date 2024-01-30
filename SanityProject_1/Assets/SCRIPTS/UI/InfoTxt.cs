using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum InfoID
{
    speed = 0,
    dmgTaken = 1,
    shot = 2,
    special1 = 3
}


public class InfoTxt : MonoBehaviour
{
    [SerializeField] Transform mover;
    [SerializeField] CanvasGroup cg;
    [SerializeField] SoundManager sound;
    [SerializeField] TextMeshProUGUI infoTxt;
    [SerializeField] TextMeshProUGUI infoValueTxt;
    

    public void FillValue(string what, string value, bool plus)
    {
        infoTxt.text = what;
        if(plus)
        {
            infoValueTxt.text = "+"+value; 
        }
        else
        {
            infoValueTxt.text = "-"+value; 
        }
        
    }

    public void Show()
    {
        sound.Play(0);
        LeanTween.cancel(cg.gameObject);
        LeanTween.cancel(mover.gameObject);
        cg.alpha = 0;
        mover.transform.localPosition = new Vector3(50.0f, .0f, .0f);
        LeanTween.value(cg.gameObject, .0f, 1.0f, 0.25f).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float f)=>{
            cg.alpha = f;
        });
        LeanTween.moveLocalX(mover.gameObject, .0f, 0.35f).setEase(LeanTweenType.easeOutQuad).setOnComplete(()=>{
            Hide(2);
        });
    }

    public void Hide(float delay = 0)
    {
        LeanTween.moveLocalY(mover.gameObject, 50.0f, 0.35f).setEase(LeanTweenType.easeOutQuad).setDelay(delay);
        LeanTween.value(cg.gameObject, 1.0f, .0f, 0.25f).setEase(LeanTweenType.easeOutQuad).setDelay(delay).setOnUpdate((float f)=>{
            cg.alpha = f;
        });
    }
}
