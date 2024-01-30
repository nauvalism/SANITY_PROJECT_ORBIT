using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using SimpleJSON;

public class BaseArena : MonoBehaviour
{
    [SerializeField] AdditionalStats statsAppliedInThis;
    [SerializeField] float cameraZoomValue = 5;
    [SerializeField] Transform center;
    [SerializeField] Transform top;
    [SerializeField] Transform turretPlace;
    [SerializeField] int turretIDInThis;
    [SerializeField] int maxLevel;
    [SerializeField] TextAsset jsonFile;
    [SerializeField] List<AdditionalStats> statsAppliedFromJson;
    [SerializeField] List<AdditionalStats> statsApplieds;
    [SerializeField] int currentlevel;
    [SerializeField] PelletSpawner spawner;

    [Header("Camera and Stuffs")]
    [SerializeField] CinemachineVirtualCamera personalCamera;
    [SerializeField] CinemachineBasicMultiChannelPerlin pCamShake;

    private void OnValidate() {
        if(personalCamera != null)
        {
            this.pCamShake = personalCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        if(jsonFile != null)
        {
            ProcessJSON();
        }
    }

    public float GetCameraZoomValue()
    {
        return cameraZoomValue;
    }

    public Transform GetCenter()
    {
        return center;
    }

    public Transform GetTop()
    {
        return top;
    }

    public void ShakeCamera()
    {
        LeanTween.cancel(personalCamera.gameObject);
        pCamShake.m_AmplitudeGain = 3.0f;
        LeanTween.value(personalCamera.gameObject, pCamShake.m_AmplitudeGain, .0f, 2.0f).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float f)=>{
            pCamShake.m_AmplitudeGain = f;
        });
    }

    public void ShakeCamera(float hold, float magnitude)
    {
        LeanTween.cancel(personalCamera.gameObject);
        pCamShake.m_AmplitudeGain = magnitude;
        LeanTween.value(personalCamera.gameObject, pCamShake.m_AmplitudeGain, .0f, 2.0f).setDelay(hold).setEase(LeanTweenType.easeOutQuad).setOnUpdate((float f)=>{
            pCamShake.m_AmplitudeGain = f;
        });
    }

    public void ActivateArena()
    {
        personalCamera.Priority = 11;
    }

    public void DeactivateArena()
    {
        personalCamera.Priority = 8;
    }

    public Transform GetTurretnSpawnPlace()
    {
        return turretPlace;
    }

    public bool AddLevel()
    {
        currentlevel++;
        return (currentlevel >= maxLevel);
    }

    public int GetArenaLevel()
    {
        return currentlevel;
    }

    public PelletSpawner GetPellet()
    {
        return spawner;
    }

    public AdditionalStats GetCurrentStatLevel(int lvl)
    {
        return statsApplieds[lvl];
    }






    public void ProcessJSON()
    {
        var processed = JSON.Parse(jsonFile.text);
        var data = processed["data"];
        statsAppliedFromJson = new List<AdditionalStats>();
        //Debug.Log("Data Count : "+data.Count);
        for(int i = 0 ; i < data.Count; i++)
        {
            AdditionalStats a = new AdditionalStats();
            //Debug.Log("Data : "+data[i].ToString());
            var dData = data[i];
            double spd = 0; double.TryParse(dData["spd"], out spd);
            int dmgt = 0; int.TryParse(dData["dmt"], out dmgt);
            double vRad; double.TryParse(dData["vis"], out vRad);
            int hrt; int.TryParse(dData["hrt"], out hrt);

            a.Fill((float)spd, dmgt, (float)vRad, hrt);

            statsAppliedFromJson.Add(a);
        }
    }
}
