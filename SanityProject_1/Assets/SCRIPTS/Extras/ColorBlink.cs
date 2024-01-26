using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ColorBlink : MonoBehaviour
{
    [SerializeField] CanvasGroup textCG;
    [SerializeField] TextMeshProUGUI theText;
    [SerializeField] Transform toPosition;
    [SerializeField] Transform textPosition;
    [SerializeField] float upSpeed = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        theText.color = new Color(Random.Range(0.0f, 1.0f),Random.Range(0.0f, 1.0f),Random.Range(0.0f, 1.0f),1);
    }

    public void Levelupping()
    {

        float timeTook = 1.0f / upSpeed;
        float alphaTook = timeTook / 2;
        timeTook += .5f;
        Time.timeScale = .0f;
        GameplayController.instance.Flashing(()=>{
            LeanTween.cancel(textPosition.gameObject);
            textPosition.transform.localPosition = Vector3.zero;
            LeanTween.moveLocalY(textPosition.gameObject, toPosition.localPosition.y, timeTook).setIgnoreTimeScale(true);


            LeanTween.cancel(textCG.gameObject);
            LeanTween.value(textCG.gameObject, .0f, 1.0f, alphaTook).setIgnoreTimeScale(true).setOnUpdate((float f)=>{
                textCG.alpha = f;
            }).setOnComplete(()=>{
                LeanTween.cancel(textCG.gameObject);
                LeanTween.value(textCG.gameObject, 1.0f, .0f, alphaTook).setIgnoreTimeScale(true).setDelay(1.0f).setOnUpdate((float f)=>{
                    textCG.alpha = f;
                });
            });
        }, ()=>{
            
        });


        
    }
}
