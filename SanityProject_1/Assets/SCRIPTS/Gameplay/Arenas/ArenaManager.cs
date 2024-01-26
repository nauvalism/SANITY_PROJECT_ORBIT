using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaManager : MonoBehaviour
{
    public static ArenaManager instance {get;set;}
    [SerializeField] ScriptableDB arenaDB;
    [SerializeField] BaseArena currentArena;
    [SerializeField] BaseArena nextArena;
    [SerializeField] Transform arenaParent;
    // Start is called before the first frame update
    
    private void Awake() {
        instance = this;
    }
    
    public BaseArena GetActiveArena()
    {
        if(currentArena == null)
        {
            BaseArena ba = FindObjectOfType<BaseArena>();
            if(ba == null)
            {
                GameObject go;
                go = (GameObject)Instantiate(arenaDB.GetData(0), new Vector3(2000,2000,0) , Quaternion.identity, arenaParent);
                go.transform.localPosition = new Vector3(0,2,0);
                currentArena = go.GetComponent<BaseArena>();
            }
        }
        
        return currentArena;
    }

    public BaseArena SpawnNextArena(int id)
    {
        BaseArena result = null;

        GameObject go;
        go = (GameObject)Instantiate(arenaDB.GetData(id), new Vector3(2000,2000,0) , Quaternion.identity, arenaParent);
        go.transform.localPosition = new Vector3(10 * id,2,0);
        nextArena = go.GetComponent<BaseArena>();
        result = nextArena;
        return result;
    }

    public void ActivateNextArena()
    {
        currentArena = nextArena;
        nextArena = null;
    }
}
