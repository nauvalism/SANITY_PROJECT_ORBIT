using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePellet : MonoBehaviour
{
    int pelletID = 0;
    [SerializeField] int pelletValue = 1;
    [SerializeField] Transform mover;
    [SerializeField] Transform graphicRoot;
    [SerializeField] List<SpriteRenderer> graphics;
    [SerializeField] AudioSource pelletSound;
    [SerializeField] Collider2D pelletCol;
    [SerializeField] bool _active = true;

    protected virtual void OnValidate()
    {
        if(graphicRoot != null)
        {
            graphics = new List<SpriteRenderer>();
            TraceGraphics(graphicRoot);
        }
        
    }

    void TraceGraphics(Transform root)
    {
        if (root.GetComponent<SpriteRenderer>())
        {
            graphics.Add(root.GetComponent<SpriteRenderer>());
        }

        foreach (Transform child in root)
        {
            if (child.GetComponent<SpriteRenderer>())
            {
                graphics.Add(child.GetComponent<SpriteRenderer>());
            }
            if (child.transform.childCount > 0)
            {
                TraceGraphics(child);
            }
        }
    }

    public virtual void Init(int id)
    {
        this.pelletID = id;
        _active = true;
    }

    public virtual void Taken()
    {
        _active = false;
        pelletSound.Play();
        pelletCol.enabled = false;
        LeanTween.cancel(mover.gameObject);
        LeanTween.scale(mover.gameObject, new Vector3(1.5f,1.5f,1.5f), 1.0f);
        for(int i = 0 ; i < graphics.Count ; i++)
        {
            LeanTween.cancel(graphics[i].gameObject);
            LeanTween.alpha(graphics[i].gameObject, .0f, .50f);
        }
    }

    public virtual void Spawned()
    {
        LeanTween.cancel(graphicRoot.gameObject);
        LeanTween.scale(graphicRoot.gameObject, Vector3.one, .025f);
    }

    public bool GetActivee()
    {
        return _active;
    }
    public int GetPelletValue()
    {
        return pelletValue;
    }
}
