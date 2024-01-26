using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BaseArena : MonoBehaviour
{
    [SerializeField] float cameraZoomValue = 5;
    [SerializeField] Transform center;
    [SerializeField] Transform top;
    [SerializeField] Transform turretPlace;
    [SerializeField] int turretIDInThis;
    [SerializeField] int maxLevel;
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
}
