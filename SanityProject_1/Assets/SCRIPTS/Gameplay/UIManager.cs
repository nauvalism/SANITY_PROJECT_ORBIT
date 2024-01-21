using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI pelletScore;
    [SerializeField] List<HealthUI> healthUI;
    [SerializeField] bool punching = false;
    [SerializeField] float speed = 3.0f;
    [SerializeField] CanvasGroup rgCanvas;
    [SerializeField] Transform readyO;
    AudioSource readyAO;
    [SerializeField] Transform goO;
    AudioSource goAO;
    [SerializeField] Image flash;
    [SerializeField] Button startBtn;


    private void Update() {
        if(punching)
        {
            Vector3 temp = new Vector3(pelletScore.transform.localScale.x, pelletScore.transform.localScale.y, 0);
            if(pelletScore.transform.localScale.x > 1.0f)
            {
                temp.x -= (Time.deltaTime * speed);
                temp.y -= (Time.deltaTime * speed);
                pelletScore.transform.localScale = temp;
            }
            else
            {
                pelletScore.transform.localScale = Vector3.one;
            }
            
        }
        else
        {
            pelletScore.transform.localScale = Vector3.one;
        }
    }

    public void UpdateHP(int howMuchHP)
    {
        for(int i = 0 ; i <healthUI.Count ; i++)
        {
            if(i > (howMuchHP - 1))
            {
                healthUI[i].Dissapear(true);
            }
            else
            {
                healthUI[i].Appear(false);
            }
        }
    }

    public void InitHP(int maxHP)
    {
        for(int i = 0 ; i < maxHP ; i++)
        {
            healthUI[i].Appear(true);
        }
    }

    public void ResetScore()
    {
        pelletScore.text = "0";
        
    }

    public void UpdateScore(int to)
    {
        pelletScore.text = to.ToString();
        Punch();
    }

    void Punch()
    {
        this.punching = true;
        pelletScore.transform.localScale = new Vector3(1.5f,1.5f,1.5f);
    }

    public void ResetUI(int defaultHealth)
    {
        startBtn.gameObject.SetActive(true);
        ResetScore();
        UpdateHP(defaultHealth);
    }

    public void Opening(System.Action after)
    {
        startBtn.gameObject.SetActive(false);
        after();
        
        // LeanTween.value(rgCanvas.gameObject, 0.0f, 1.0f, 0.25f).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float f)=>{
        //     rgCanvas.alpha = f;
        // }).setOnComplete(()=>{
        //     LeanTween.scale(readyO.gameObject, Vector3.one, 1.0f).setEase(LeanTweenType.easeOutQuad).setOnComplete(()=>{
        //         LeanTween.moveLocalX(readyO.gameObject, 1000.0f, 0.5f).setEase(LeanTweenType.easeInBack).setDelay(1.0f).setOnComplete(()=>{
        //             LeanTween.scale(goO.gameObject, Vector3.one, 1.0f).setEase(LeanTweenType.easeOutBack).setDelay(1.0f).setOnComplete(()=>{
        //                 flash.color = Color.white;
        //                 after();
        //                 LeanTween.alpha(flash.GetComponent<RectTransform>(), .0f, 1.0f).setOnComplete(()=>{
        //                     LeanTween.moveLocalX(goO.gameObject, 1000.0f, 0.5f).setEase(LeanTweenType.easeInBack);
        //                 });
        //             });
        //         });
                
        //     });
        // });
    }
}
