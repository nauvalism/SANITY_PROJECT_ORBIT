using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] SoundManager explodeSound;
    
    public void Exploding()
    {
        anim.Play("Explode", -1, 0);
        explodeSound.Play(0);
    }

    public void ExplodingBig()
    {
        anim.Play("Explode", -1, 0);
        explodeSound.Play(2);
    }
}
