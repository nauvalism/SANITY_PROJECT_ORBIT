using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PelletSpawner : MonoBehaviour
{
    public static PelletSpawner instance {get;set;}
    public GameObject pelletPrefab;
    public GameObject radiusHand;
    public Transform pelletParent;
    public List<BasePellet> spawnedPellets;
    public UnityEvent spawnSequences;


    private void Awake() {
        instance = this;
    }

    private void Start() {
        //Spawn(20);
    }

    public GameObject DoSpawn(Vector2 t)
    {
        GameObject spawned;
        spawned = Instantiate(pelletPrefab, Vector3.zero, Quaternion.identity);
        spawned.transform.position = t;
        spawned.transform.parent = pelletParent;
        return spawned;
    }


    public void Spawn(float gap)
    {
        spawnedPellets = new List<BasePellet>();
        radiusHand.transform.localRotation = Quaternion.identity;
        int counter = 0;
        for(float i = 0 ; i < 360.0f ; i += gap)
        {
            if(i == 0)
            {
                continue;
            }
            radiusHand.transform.localRotation = Quaternion.Euler(0,0,i);
            GameObject go;
            go = DoSpawn(transform.position);
            BasePellet bp = go.GetComponent<BasePellet>();
            bp.Init(counter);
            spawnedPellets.Add(bp);
            counter++;
        }

    
    }

    public void SpawnNotFromBeginning(float gap)
    {

    }

    public bool EvaluateIfAllPelletTaken()
    {
        for(int i = 0 ; i < spawnedPellets.Count ; i++)
        {
            if(spawnedPellets[i].GetActive())
            {
                return false;
            }
        }

        return true;
    }


}
