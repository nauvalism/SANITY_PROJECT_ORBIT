using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    opening = 0,
    midGame = 1,
    paused = 2,
    ending = 3
}

public class GameplayController : MonoBehaviour
{
    public static GameplayController instance {get;set;}
    [SerializeField] GameState state;
    [SerializeField] CircleAnimation openingAnimation;
    [SerializeField] Camera OpeningCam;
    [SerializeField] float gameplayCamSize;
    [SerializeField] float idleCamSize;
    [SerializeField] int score = 0;
    [SerializeField] int level = 0;
    [SerializeField] UIManager ui;
    [SerializeField] BaseShip playerShip;
    [SerializeField] PelletSpawner pSpawner;
    [SerializeField] AudioSource music;
    [SerializeField] AudioSource startSfx;
    private void Awake() {
        instance = this;

    }

    private void Start() {
        ResetAll();
    }

    private void OnValidate() {
        this.OpeningCam.orthographicSize = gameplayCamSize;
    }

    public void InitGame()
    {
        SetState(GameState.opening);
        ui.ResetUI(playerShip.hp);
        ui.InitHP(playerShip.GetHP());

        
        


        StartCoroutine(StartSequence());
        IEnumerator StartSequence()
        {
            openingAnimation.RestoreCirclez();
            ZoomCam(true);
            yield return new WaitForSeconds(1.0f);
            playerShip.GoToGamePosition(()=>{
                ui.Opening(()=>{
                    StartGame();
                }); 
            });
            
        }
    }

    public void StartGame()
    {
        SetState(GameState.midGame);
        startSfx.Play();
        music.Play();
        pSpawner.Spawn(20);
        playerShip.Activate();
        TBEController.instance.ActivateActiveTurret();
    }

    public void StopGame()
    {
        ui.ResetUI(playerShip.hp);
    }

    public void ResetAll()
    {
        ZoomCam(false);
        openingAnimation.RandomizeCircles();
        playerShip.ResetPosition();
    }

    public void ZoomCam(bool gameplay)
    {
        LeanTween.cancel(OpeningCam.gameObject);
        if(gameplay)
        {
            LeanTween.value(OpeningCam.gameObject, OpeningCam.orthographicSize, gameplayCamSize, 1.0f).setOnUpdate((float f)=>{
                OpeningCam.orthographicSize = f;
            });
        }
        else
        {
            LeanTween.value(OpeningCam.gameObject, OpeningCam.orthographicSize, idleCamSize, 1.0f).setOnUpdate((float f)=>{
                OpeningCam.orthographicSize = f;
            });
        }
    }

    public void UpdateHP(int amt)
    {
        ui.UpdateHP(amt);
    }

    public void TakePellet(int scr = 1)
    {

        this.AddScore(scr);
        if(EvaluateIfAllPelletTaken())
        {
            RespawnPellet(20);
        }
    }

    public void AddScore(int amt = 1)
    {
        score += amt;
        ui.UpdateScore(score);
    }

    public void RespawnPellet(int param)
    {
        StartCoroutine(Spawnn());
        IEnumerator Spawnn()
        {
            yield return new WaitForSeconds(0.25f);
            StartSpawningPellet(param);
        }
    }

    public void StartSpawningPellet(int param)
    {
        pSpawner.Spawn(param);
    }

    public bool EvaluateIfAllPelletTaken()
    {
        return pSpawner.EvaluateIfAllPelletTaken();
    }

    public void ResetScore()
    {
        score = 0;
    }

    public void SetState(GameState _state)
    {
        this.state = _state;
    }

    public GameState GetState()
    {
        return this.state;
    }
}
