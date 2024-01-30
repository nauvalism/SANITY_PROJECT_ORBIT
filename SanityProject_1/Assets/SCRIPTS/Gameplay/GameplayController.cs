 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public enum GameState
{
    opening = 0,
    midGame = 1,
    paused = 2,
    processing = 3,
    ending = 4
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
    [SerializeField] int localLevel = 0;
    [SerializeField] int arenaTier = 0;
    [SerializeField] UIManager ui;
    [SerializeField] BaseShip playerShip;
    [SerializeField] PelletSpawner pSpawner;
    [SerializeField] AudioSource music;
    [SerializeField] MusicManager musicManager;
    [SerializeField] AudioSource startSfx;
    [SerializeField] SoundManager startSoundManager;
    [SerializeField] Transform absoluteOrigin;
    
    [Header("Arena")]
    [SerializeField] Transform leftMost;
    [SerializeField] BaseArena currentArena;
    [SerializeField] BaseArena nextArena;

    [Header("Camera")]
    [SerializeField] BaseArena activeArena;
    [SerializeField] CinemachineVirtualCamera vCam;
    [SerializeField] CinemachineBasicMultiChannelPerlin vCamShake;
    [SerializeField] List<CinemachineVirtualCamera> vCams;
    [SerializeField] CinemachineVirtualCamera activeCam;

    [Header("EFFECTS")]
    [SerializeField] ParticleSystem nearStar;
    [SerializeField] ParticleSystem farStar;
    [SerializeField] List<float> minMaxIdleN;
    [SerializeField] List<float> minMaxIdleF;
    [SerializeField] List<float> minMaxMoveN;
    [SerializeField] List<float> minMaxMoveF;
    [SerializeField] GameObject idleStars;
    [SerializeField] GameObject movingStars;

    [Header("Stats")]
    [SerializeField] float sessionSeconds;
    [SerializeField] CalculatedStats stats;
    [SerializeField] AdditionalStats extraStats;
    
    private void Awake() {
        instance = this;
        vCamShake = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void InitStats()
    {
        stats = new CalculatedStats();
        extraStats = new AdditionalStats();
    }

    public void AddExtraStats(AdditionalStats addition)
    {
        extraStats.AddStats(addition);
    }

    public void ApplyStats(AdditionalStats stat)
    {
        extraStats.AddStats(stat);
        playerShip.AddSpeed(stat.speedAddition);
        playerShip.AddDmgTaken(stat.dmgTaken);
        playerShip.AddHealth(stat.shot);
    }

    public CalculatedStats GetStats()
    {
        return stats;
    }

    public AdditionalStats GetExtraStats()
    {
        return extraStats;
    }

    private void Start() {
        ResetAll();
        activeArena = FindObjectOfType<BaseArena>();
    }

    private void OnValidate() {
        this.OpeningCam.orthographicSize = gameplayCamSize;
    }

    private void Update() {
        if(state == GameState.midGame)
        {
            sessionSeconds += Time.deltaTime;
        }
        
    }

    public void InitGame()
    {
        SetState(GameState.opening);
        ui.ResetUI(playerShip.hp);
        ui.InitHP(playerShip.GetHP());
        activeArena = ArenaManager.instance.GetActiveArena();
        
        IdleParticle();


        StartCoroutine(StartSequence());
        IEnumerator StartSequence()
        {
            //openingAnimation.RestoreCirclez();
            ZoomCam(true);
            yield return new WaitForSeconds(1.0f);
            
            ui.Opening(()=>{
                MoveParticle();
                playerShip.GoToGamePosition(()=>{
                    StartGame();
                    IdleParticle();
                }); 
            });
            
        }
    }

    public void StartGame()
    {
        Flashing(()=>{
            ui.ShowTop();
            SetState(GameState.midGame);
            


        
            
            ActivateArena();
            TBEController.instance.SetTurretPosition(activeArena.GetTurretnSpawnPlace());
            TBEController.instance.SpawnTurret(activeArena.GetArenaLevel());
            TBEController.instance.ActivateActiveTurret();


            startSoundManager.Play(0);
            musicManager.Play(0);
            
            pSpawner = activeArena.GetPellet();
            
            pSpawner.Spawn(20);
            playerShip.Activate();



        }, null);
        
    }

    public void StopGame()
    {
        ui.ResetUI(playerShip.hp);
    }

    public void GameOver()
    {
        stats.UpdateTimeSpent(arenaTier, sessionSeconds);
        SetState(GameState.ending);
        musicManager.Stop();
        SetState(GameState.ending);
        
    }

    public void ShowGameOver()
    {
        ui.ShowGameOver();
    }

    public void ResetAll()
    {
        level = 0;
        localLevel = 0;
        InitStats();
        ClearArena();
        ClearPellet();
        ZoomCam(false);
        stats = new CalculatedStats();
        ui.UpdateLevel(0);
        ui.HideTop();
        ui.HideVignette();
        //openingAnimation.RandomizeCircles();
        playerShip.BackToOrigin(absoluteOrigin);
        playerShip.ResetPosition();
        
        playerShip.ShowShip();
        vCam.Priority = 12;
        sessionSeconds = 0;
        
    }

    public void RestartGame()
    {
        ResetAll();
        ui.ShowOpening();
        //InitGame();

    }

    public void ClearArena()
    {
        TBEController.instance.DeactivateActiveturret();
        TBEController.instance.DestroyAllTurrets();
        List<BaseArena> allArena = new List<BaseArena>(FindObjectsOfType<BaseArena>());
        for(int i = 0 ; i < allArena.Count ; i++)
        {
            
            Destroy(allArena[i].gameObject);
        }
        arenaTier = 0;
    }

    public void ClearPellet()
    {

        List<BasePellet> pellets = new List<BasePellet>(FindObjectsOfType<BasePellet>());
        for(int i = 0 ; i < pellets.Count ; i++)
        {
            Destroy(pellets[i].gameObject);
        }
    }

    public void GetHit()
    {
        stats.AddHitsTaken();
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

    public void DecreaseHP()
    {

    }

    public void UpdateHP(int amt)
    {
        Debug.Log("Updating HP");
        ui.UpdateHP(amt);
    }

    public void TakePellet(int scr = 1)
    {

        this.AddScore(scr);
        stats.AddPelletTaken(1);
        if(EvaluateIfAllPelletTaken())
        {
            SetState(GameState.processing);
            AddLevel();
            //RespawnPellet(20);
        }
    }

    public void AddScore(int amt = 1)
    {
        stats.AddScore(amt);
        score += amt;
        ui.UpdateScore(score);
    }

    public void AddLevel()
    {
        this.level++;
        this.localLevel++;
        stats.AddLevelClear();
        bool next = ArenaManager.instance.GetActiveArena().AddLevel();
        
        if(next)
        {
            TBEController.instance.DeactivateActiveturret(true);
            Flashing(null,null);
            playerShip.Deactivate();
            AddArenaTier();
            
        }
        else
        {
            ui.UpdateLevel(this.level);
            
            AdditionalStats stat = ArenaManager.instance.GetActiveArena().GetCurrentStatLevel(level);
            bool viewUpdate = false;
            if(stat.visionRadius != extraStats.visionRadius)
            {
                viewUpdate = true;
            }
            AdditionalStats beforeStat = new AdditionalStats(extraStats);
            ApplyStats(stat);
            TBEController.instance.LevelupTurret();
            Flashing(()=>{
                
                ui.LevellingUp();
                LeanTween.value(Time.timeScale, 1.0f, 1.0f).setIgnoreTimeScale(true).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float f)=>{
                    Time.timeScale = f;
                    Time.fixedDeltaTime = Time.timeScale * .02f;
                });
                RespawnPellet(20);
            },()=>{
                if(viewUpdate)
                {
                    playerShip.ModifyViews(extraStats.visionRadius);
                }
                ui.ShowAdditionalStats(beforeStat, stat);
                //Time.timeScale = 1.0f;
            });
            SetState(GameState.midGame);
        }
        
    }

    public void AddArenaTier()
    {
        arenaTier++;
        stats.AddTimeSpent(sessionSeconds);
        BaseArena a = SpawnNextArena();
        activeArena = a;
        playerShip.PrepareGoNextArena(a);
        sessionSeconds = 0;
        localLevel = 0;
    }

    public BaseArena SpawnNextArena()
    {
        return ArenaManager.instance.SpawnNextArena(arenaTier);
    }

    public void AfterNextArena()
    {
        RefreshActiveCamera();
        ArenaManager.instance.ActivateNextArena();
        activeArena = ArenaManager.instance.GetActiveArena();
        TBEController.instance.SetTurretPosition(activeArena.GetTurretnSpawnPlace());
        TBEController.instance.SpawnTurret(arenaTier);
        TBEController.instance.ActivateActiveTurret();
        pSpawner = activeArena.GetPellet();
        pSpawner.Spawn(20);
        playerShip.Activate();
        SetState(GameState.midGame);
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

    public void ActivateArena()
    {
        activeArena.ActivateArena();
        vCam.Priority = 7;
        RefreshActiveCamera();
    }

    public bool EvaluateIfAllPelletTaken()
    {
        return pSpawner.EvaluateIfAllPelletTaken();
    }
    
    public void ShakeCamera()
    {
        activeArena.ShakeCamera();
    }

    public void ShakeCamera(float hold, float magnitude)
    {
        activeArena.ShakeCamera(hold, magnitude);
    }

    public void ShakeMainCamera(float hold, float magnitude)
    {
        LeanTween.cancel(vCam.gameObject);
        vCamShake.m_AmplitudeGain = magnitude;
        LeanTween.value(vCam.gameObject, vCamShake.m_AmplitudeGain, .0f, 2.0f).setDelay(hold).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float f)=>{
            vCamShake.m_AmplitudeGain = f;
        });
    }

    public void SwitchCameraToArena()
    {

    }

    public void RefreshActiveCamera()
    {
        vCams = new List<CinemachineVirtualCamera>(FindObjectsOfType<CinemachineVirtualCamera>());
        activeCam = SearchActiveCam();


        CinemachineVirtualCamera SearchActiveCam()
        {
            CinemachineVirtualCamera result = vCam;
            for(int i = 0 ; i < vCams.Count ; i++)
            {
                if(vCams[i].Priority > 10)
                {
                    result = vCams[i];
                    break;
                }
            }

            return result;
        }
    }

    public void IdleParticle()
    {
        Debug.Log("Move Particle ====");
        
        idleStars.SetActive(true);
        movingStars.SetActive(false);

        // var trailPar = nearStar.trails;
        // trailPar.enabled = false;

        // var trailParF = farStar.trails;
        // trailParF.enabled = false;

        // var parLTN = nearStar.main;
        // var lifetimeN = new ParticleSystem.MinMaxCurve(2,6);
        // parLTN.startLifetime = lifetimeN;

        // var parLTF = farStar.main;
        // var lifetimeF = new ParticleSystem.MinMaxCurve(5,10);
        // parLTF.startLifetime = lifetimeF;


        
        // var velN = nearStar.velocityOverLifetime;
        // var xCurveN = new ParticleSystem.MinMaxCurve(minMaxIdleN[0], minMaxIdleN[1]);
        // var yCurveN = new ParticleSystem.MinMaxCurve(0,0);


        // var velF = nearStar.velocityOverLifetime;
        // var xCurveF = new ParticleSystem.MinMaxCurve(minMaxIdleF[0], minMaxIdleF[1]);
        // var yCurveF = new ParticleSystem.MinMaxCurve(0,0);
    
    
        // velN.x = xCurveN;
        // velN.y = yCurveN;

        // velF.x = xCurveF;
        // velF.y = yCurveF;
    }

    public void MoveParticle()
    {
        Debug.Log("Move Particle ====");

        idleStars.SetActive(false);
        movingStars.SetActive(true);

        float xMinF = minMaxMoveF[0];
        float xMaxF = minMaxMoveF[1];

        float xMinN = minMaxMoveN[0];
        float xMaxN = minMaxMoveN[1];

        LeanTween.value(xMinF, 0, 4.0f).setOnUpdate((float f)=>{
            xMinF = f;
        });

        LeanTween.value(xMaxF, 0, 4.0f).setOnUpdate((float f)=>{
            xMaxF = f;
        });

        LeanTween.value(xMinN, 0, 4.0f).setOnUpdate((float f)=>{
            xMinN = f;
        });

        LeanTween.value(xMaxN, 0, 4.0f).setOnUpdate((float f)=>{
            xMaxN = f;
        });

        LeanTween.value(0.0f,1.0f,4.0f).setOnUpdate((float f)=>{
            var trailPar = nearStar.trails;
            trailPar.enabled = true;

            var trailParF = farStar.trails;
            trailParF.enabled = true;

            var parLTN = nearStar.main;
            var lifetimeN = new ParticleSystem.MinMaxCurve(1,2);
            parLTN.startLifetime = lifetimeN;

            var parLTF = farStar.main;
            var lifetimeF = new ParticleSystem.MinMaxCurve(2,3);
            parLTF.startLifetime = lifetimeF;
            
            var velN = nearStar.velocityOverLifetime;
            var xCurveN = new ParticleSystem.MinMaxCurve(xMinN, xMaxN);
            var yCurveN = new ParticleSystem.MinMaxCurve(0,0);


            var velF = farStar.velocityOverLifetime;
            var xCurveF = new ParticleSystem.MinMaxCurve(xMinF, xMaxF);
            var yCurveF = new ParticleSystem.MinMaxCurve(0,0);
        


            // xMaxF -= Time.deltaTime;
            // xMinF -= Time.deltaTime;

            // xMaxN -= Time.deltaTime;
            // xMinN -= Time.deltaTime;

            

        
            velN.x = xCurveN;
            velN.y = yCurveN;

            velF.x = xCurveF;
            velF.y = yCurveF;

            



        }).setOnComplete(()=>{
            idleStars.SetActive(true);
            movingStars.SetActive(false);
        });

        
    }

    public void ResetScore()
    {
        score = 0;
    }

    public void SetState(GameState _state)
    {
        this.state = _state;
    }

    public void ShowVignette(Color c)
    {
        ui.ShowVignette(c);
    }

    public void HideVignette()
    {
        ui.HideVignette();
    }

    public void Flashing(System.Action during, System.Action after)
    {
        ui.Flashing(during,after);
    }

    public void UnFlash()
    {
        ui.UnFlash();
    }

    public GameState GetState()
    {
        return this.state;
    }

    public Transform GetLeftMost()
    {
        return leftMost;
    }
}



[System.Serializable]
public class CalculatedStats
{
    public List<float> timeSpents;
    public int pelletTaken;
    public int levelCleared;
    public int hitsTaken;
    public int scores;
    public List<int> nearMisses;

    public CalculatedStats()
    {
        timeSpents = new List<float>();
        nearMisses = new List<int>();
    }

    public void UpdateTimeSpent(int arenaIndex, float v)
    {
        try
        {
            timeSpents[arenaIndex] = v;
        }catch(System.Exception e)
        {

        }
    }

    public void AddTimeSpent(float v)
    {
        timeSpents.Add(v);
    }

    public void AddNearMiss(int t)
    {
        nearMisses.Add(t);
    }

    public void AddPelletTaken(int val)
    {
        pelletTaken += val;
    }

    public void AddScore(int val)
    {
        scores += val;
    }

    public void AddLevelClear()
    {
        ++levelCleared;
    }

    public void AddHitsTaken()
    {
        ++hitsTaken;
    }

    public float GetTotalTimeSpent()
    {
        float result = 0;
        for(int i = 0 ; i < timeSpents.Count; i++)
        {
            result += timeSpents[i];
        }

        return result;
    }

    public int GetTotalNearMisses()
    {
        int result = 0;
        for(int i = 0 ; i < nearMisses.Count ; i++)
        {
            result += nearMisses[i];
        }
        return result;
    }

}


[System.Serializable]
public class AdditionalStats
{
    public float speedAddition = 0;
    public int dmgTaken = 0;
    public float visionRadius = 45.0f;
    public int shot = 0;

    public AdditionalStats()
    {

    }

    public void Fill(float speed, int dmgtkn, float vision, int heart)
    {
        this.speedAddition = speed;
        this.dmgTaken = dmgtkn;
        this.visionRadius = vision;
        this.shot = heart;
    }

    public AdditionalStats(AdditionalStats cloned)
    {
        this.speedAddition = cloned.speedAddition;
        this.dmgTaken = cloned.dmgTaken;
        this.visionRadius = cloned.visionRadius;
        this.shot = cloned.shot;
    }

    public void AddStats(AdditionalStats otherStats)
    {
        this.speedAddition += otherStats.speedAddition;
        this.dmgTaken += otherStats.dmgTaken;
        this.visionRadius = otherStats.visionRadius;
        this.shot += otherStats.shot;
    }
}