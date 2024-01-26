using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BasicPellet : BasePellet
{
    [SerializeField] TextMeshPro valueTxt;

    protected override void OnValidate()
    {
        base.OnValidate();
        try
        {
            valueTxt.text = GetPelletValue().ToString();
        }   
        catch(System.Exception e)
        {
            
        }
    }

    public override void Taken()
    {
        base.Taken();
        LeanTween.cancel(valueTxt.gameObject);
        LeanTween.value(valueTxt.gameObject, 1.0f, .0f, 0.5f).setOnUpdate((float f)=>{
            valueTxt.alpha = f;
        });
    }
}
