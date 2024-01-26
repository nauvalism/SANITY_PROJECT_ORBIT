using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum InfoID
{
    speed = 0,
    dmgTaken = 1,
    special1 = 2
}


public class InfoTxt : MonoBehaviour
{
    [SerializeField] CanvasGroup cg;
    [SerializeField] TextMeshProUGUI infoTxt;
    [SerializeField] TextMeshProUGUI infoValueTxt;

    public void FillValue()
    {

    }
}
