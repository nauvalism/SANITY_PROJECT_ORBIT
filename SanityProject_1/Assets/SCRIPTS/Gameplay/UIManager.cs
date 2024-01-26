using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI pelletScore;
    [SerializeField] TextMeshProUGUI level;
    [SerializeField] List<HealthUI> healthUI;
    [SerializeField] bool punching = false;
    [SerializeField] float speed = 3.0f;
    [SerializeField] Transform topMover;
    [SerializeField] CanvasGroup rgCanvas;
    [SerializeField] Transform readyO;
    [SerializeField] SoundManager readyAO;
    [SerializeField] Transform goO;
    [SerializeField] SoundManager goAO;
    [SerializeField] Image flash;
    [SerializeField] CanvasGroup startBtnCG;
    [SerializeField] Button startBtn;
    [SerializeField] CanvasGroup openingCG;

    [Header("UI")]
    [SerializeField] CanvasGroup vignetteCG;
    [SerializeField] Image vignette;
    [SerializeField] ColorBlink levelup;
    [SerializeField] SoundManager levelupSound;

    [Header("Game Over Values")]
    [SerializeField] CanvasGroup gameOverRootCG;
    [SerializeField] CanvasGroup gameOverCG;
    [SerializeField] CanvasGroup tapToContinueCG;
    [SerializeField] Transform gameOverContainer;
    [SerializeField] TextMeshProUGUI timeSpentTxt;
    [SerializeField] TextMeshProUGUI pelletTakenTxt;
    [SerializeField] TextMeshProUGUI levelTxt;
    [SerializeField] TextMeshProUGUI hitsTakenTxt;
    [SerializeField] TextMeshProUGUI scoresTxt;


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
            healthUI[i].Dissapear(true);
        }

        for(int i = 0 ; i < maxHP ; i++)
        {
            healthUI[i].Appear(true);
        }
    }

    public void ResetLevel()
    {
        level.text = "Level-1";
    }

    public void UpdateLevel(int to)
    {
        level.text = "Level-"+(to + 1).ToString();
    }

    public void LevellingUp()
    {
        levelup.Levelupping();
        levelupSound.Play(0);
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

    public void HideTop()
    {
        LeanTween.cancel(topMover.gameObject);
        LeanTween.moveLocalY(topMover.gameObject, 250.0f, 1.0f).setEase(LeanTweenType.easeOutQuad);
    }

    public void ShowTop()
    {
        LeanTween.cancel(topMover.gameObject);
        LeanTween.moveLocalY(topMover.gameObject, .0f, 1.0f).setEase(LeanTweenType.easeOutBack);
    }

    public void ShowOpening()
    {
        openingCG.interactable = false;
        openingCG.blocksRaycasts = false; 

        LeanTween.cancel(openingCG.gameObject);
        LeanTween.value(openingCG.gameObject, openingCG.alpha, 1.0f, 2.0f).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float f)=>{
            openingCG.alpha = f;
        }).setOnComplete(()=>{
            openingCG.interactable = true;
            openingCG.blocksRaycasts = true;  
            TapToStart();
        });
        
    }

    public void HideOpening(float duration, System.Action after)
    {
        openingCG.interactable = true;
        openingCG.blocksRaycasts = true; 

        LeanTween.cancel(openingCG.gameObject);
        LeanTween.value(openingCG.gameObject, openingCG.alpha, .0f, duration).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float f)=>{
            openingCG.alpha = f;
        }).setOnComplete(()=>{
            openingCG.interactable = false;
            openingCG.blocksRaycasts = false;
            if(after != null)
            {
                after();
            }
        });
    }

    public void TapToStart()
    {
        
        LeanTween.value(0,1,1).setOnComplete(()=>{
            startBtnCG.interactable = true;
            startBtnCG.blocksRaycasts = true;
        });

        LeanTween.cancel(startBtnCG.gameObject);
        LeanTween.value(startBtnCG.gameObject, startBtnCG.alpha, 1.0f, 1.0f).setLoopType(LeanTweenType.pingPong).setOnUpdate((float f)=>{
            startBtnCG.alpha = f;
        });
    }

    public void HideStartBtn(float duration, System.Action after)
    {
        startBtnCG.interactable = false;
        startBtnCG.blocksRaycasts = false;
        LeanTween.cancel(startBtnCG.gameObject);
        LeanTween.value(startBtnCG.gameObject, startBtnCG.alpha, .0f, duration).setOnUpdate((float f)=>{
            startBtnCG.alpha = f;
        }).setOnComplete(()=>{
            if(after != null)
            {
                after();
            }
        }).setEase(LeanTweenType.easeOutQuad);
    }

    public void ResetUI(int defaultHealth)
    {
        startBtn.gameObject.SetActive(true);
        ResetScore();
        UpdateHP(defaultHealth);
    }

    public void PreStartGame(System.Action after)
    {
        
    }

    public void Opening(System.Action after)
    {
        //startBtn.gameObject.SetActive(false);
        HideStartBtn(.50f,()=>{
            HideOpening(1.0f,()=>{
                after();
            });
            
        });
        
        
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

    public void Flashing(System.Action during, System.Action after)
    {
        flash.color = Color.white;
        during();
        LeanTween.alpha(flash.GetComponent<RectTransform>(), .0f, .50f).setEase(LeanTweenType.easeOutQuad).setIgnoreTimeScale(true).setOnComplete(()=>{
            if(after != null)
            {
                after();
            }
            
        });
    }

    public void UnFlash()
    {
        LeanTween.alpha(flash.GetComponent<RectTransform>(), .0f, .1250f).setEase(LeanTweenType.easeOutQuad).setIgnoreTimeScale(true).setOnComplete(()=>{
            //after();
        });
    }

    public void ShowVignette(Color c)
    {
        vignette.color = new Color(c.r,c.g,c.b,.0f);
        LeanTween.cancel(vignette.gameObject);
        LeanTween.alpha(vignette.GetComponent<RectTransform>(), 0.25f, 0.5f).setLoopType(LeanTweenType.pingPong);

        //LeanTween.cancel(vignetteCG.gameObject);
        LeanTween.value(vignetteCG.gameObject, .0f, 1.0f, 0.25f).setOnUpdate((float f)=>{
            vignetteCG.alpha = f;
        });
    }

    public void HideVignette()
    {
        LeanTween.cancel(vignetteCG.gameObject);
        LeanTween.value(vignetteCG.gameObject, vignetteCG.alpha, .0f, 0.25f).setOnUpdate((float f)=>{
            vignetteCG.alpha = f;
        }).setOnComplete(()=>{
            LeanTween.cancel(vignette.gameObject);
        });
    }








    public void ShowGameOver()
    {

        LeanTween.cancel(gameOverRootCG.gameObject);
        LeanTween.cancel(gameOverCG.gameObject);
        LeanTween.cancel(gameOverContainer.gameObject);


        gameOverContainer.GetComponent<SoundManager>().Play(0);
        LeanTween.value(gameOverRootCG.gameObject, gameOverRootCG.alpha , 1.0f, .50f).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float f)=>{
            gameOverRootCG.interactable = true;
            gameOverRootCG.blocksRaycasts = true;
            gameOverRootCG.alpha = f;
        }).setOnComplete(()=>{
            LeanTween.value(gameOverCG.gameObject, gameOverCG.alpha , 1.0f, 2.0f).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float f)=>{
                gameOverCG.interactable = true;
                gameOverCG.blocksRaycasts = true;
                gameOverCG.alpha = f;
            });

            LeanTween.moveLocalY(gameOverContainer.gameObject, .0f, 2.0f).setEase(LeanTweenType.easeOutQuad).setOnComplete(()=>{
                StartCoroutine(RefreshValuesTxt()); 
            });
        });

        

        IEnumerator RefreshValuesTxt()
        {
            yield return new WaitForSeconds(0.5f);
            CalculatedStats stat = GameplayController.instance.GetStats();
            timeSpentTxt.transform.parent.GetComponent<SoundManager>().Play(0, true);
            LeanTween.value(0, stat.GetTotalTimeSpent(), 1.0f).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float f)=>{
                float second = f;
                System.TimeSpan t = System.TimeSpan.FromSeconds(second);
                timeSpentTxt.text = string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
            });
            yield return new WaitForSeconds(1.0f);
            timeSpentTxt.transform.parent.GetComponent<SoundManager>().Play(1, false);
            LeanTween.scale(timeSpentTxt.gameObject, new Vector3(1.25f,1.25f,1.25f), 1.0f).setEase(LeanTweenType.punch);

            pelletTakenTxt.transform.parent.GetComponent<SoundManager>().Play(0, true);
            LeanTween.value(0.0f, (float)stat.pelletTaken, 1.0f).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float f)=>{
                int pelletTaken = Mathf.CeilToInt(f);
                pelletTakenTxt.text = pelletTaken.ToString();
            });

            yield return new WaitForSeconds(1.0f);
            pelletTakenTxt.transform.parent.GetComponent<SoundManager>().Play(1, false);
            LeanTween.scale(pelletTakenTxt.gameObject, new Vector3(1.25f,1.25f,1.25f), 1.0f).setEase(LeanTweenType.punch);


            levelTxt.transform.parent.GetComponent<SoundManager>().Play(0, true);
            LeanTween.value(0.0f, (float)stat.levelCleared, 1.0f).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float f)=>{
                int levelCleared = Mathf.CeilToInt(f);
                levelTxt.text = levelCleared.ToString();
            });
            yield return new WaitForSeconds(1.0f);
            levelTxt.transform.parent.GetComponent<SoundManager>().Play(1, false);
            LeanTween.scale(levelTxt.gameObject, new Vector3(1.25f,1.25f,1.25f), 1.0f).setEase(LeanTweenType.punch);

            hitsTakenTxt.transform.parent.GetComponent<SoundManager>().Play(0, true);
            LeanTween.value(0.0f, (float)stat.hitsTaken, 1.0f).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float f)=>{
                int hitsTaken = Mathf.CeilToInt(f);
                hitsTakenTxt.text = hitsTaken.ToString();
            });
            yield return new WaitForSeconds(1.0f);
            LeanTween.scale(hitsTakenTxt.gameObject, new Vector3(1.25f,1.25f,1.25f), 1.0f).setEase(LeanTweenType.punch);
            hitsTakenTxt.transform.parent.GetComponent<SoundManager>().Play(1, false);

            scoresTxt.transform.parent.GetComponent<SoundManager>().Play(0, true);
            LeanTween.value(0.0f, (float)stat.scores, 1.0f).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float f)=>{
                int score = Mathf.CeilToInt(f);
                scoresTxt.text = score.ToString();
            });

            yield return new WaitForSeconds(1.0f);
            scoresTxt.transform.parent.GetComponent<SoundManager>().Play(1, false);

            yield return new WaitForSeconds(2.0f);
            LeanTween.cancel(gameOverRootCG.gameObject);
            LeanTween.cancel(gameOverCG.gameObject);
            LeanTween.cancel(gameOverContainer.gameObject);
            LeanTween.cancel(tapToContinueCG.gameObject);

            LeanTween.value(tapToContinueCG.gameObject, tapToContinueCG.alpha, 1.0f, 0.5f).setLoopType(LeanTweenType.pingPong).setOnUpdate((float f)=>{
                tapToContinueCG.alpha = f;
            });

            yield return new WaitForSeconds(0.5f);
            tapToContinueCG.interactable = true;
            tapToContinueCG.blocksRaycasts = true;
        }
    }

    public void Reset()
    {
        LeanTween.cancel(gameOverRootCG.gameObject);
        LeanTween.cancel(gameOverCG.gameObject);
        LeanTween.cancel(gameOverContainer.gameObject);
        LeanTween.cancel(tapToContinueCG.gameObject);

        gameOverCG.interactable = false;
        gameOverCG.blocksRaycasts = false;

        LeanTween.value(tapToContinueCG.gameObject, gameOverCG.alpha , .0f, .750f).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float f)=>{
            tapToContinueCG.alpha = f;
        }).setOnComplete(()=>{
            LeanTween.moveLocalY(gameOverContainer.gameObject, -100.0f, 1.0f).setEase(LeanTweenType.easeOutQuad);
            LeanTween.value(gameOverCG.gameObject, gameOverCG.alpha , .0f, .750f).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float f)=>{
                gameOverCG.alpha = f;
            }).setOnComplete(()=>{
                gameOverCG.interactable = false;
                gameOverCG.blocksRaycasts = false;
                LeanTween.value(gameOverRootCG.gameObject, gameOverRootCG.alpha , .0f, .50f).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float f)=>{
                    gameOverRootCG.alpha = f;
                }).setOnComplete(()=>{
                    gameOverRootCG.interactable = false;
                    gameOverRootCG.blocksRaycasts = false;
                    GameplayController.instance.RestartGame();
                });
            });
        });

        


        
    }


}
