using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseShip : MonoBehaviour
{
    private Transform orbitter;
    
    [Header("Health and Aliveness")]
    [SerializeField] private List<Sprite> ShipHealthSprites;
    [SerializeField] public int hp = 2;
    [SerializeField] ShipControl shipMovement;
    [SerializeField] Collider2D mainCollider;

    [Header("Positions")]
    [SerializeField] Transform scaleRoot;
    [SerializeField] Transform moveRoot;
    [SerializeField] List<Transform> leftRightMost;
    [SerializeField] Transform followCam;

    [Header("Graphics")]
    [SerializeField] List<SpriteRenderer> sprites;
    [SerializeField] SpriteRenderer mainShipSprite;
    [SerializeField] Explosion explosion;
    [SerializeField] List<Explosion> deathExplosions;
    [SerializeField] ParticleSystem deathParticle;

    [Header("Extra")]
    [SerializeField] SoundManager engineSound;
    [SerializeField] Animator anim;

    public void Damage(int amt)
    {

    }

    public void Heal(int amt)
    {

    }

    public bool Alive()
    {
        if(hp <= 0)
        {
            return false;
        }

        return true;
    }

    public int GetHP()
    {
        return hp;
    }

    [Header("Movements and Rotations")]
    [SerializeField] public float angleModifier = 0;
    
    // Start is called before the first frame update

    protected virtual void OnValidate() {
        hp = ShipHealthSprites.Count;
    }

    protected virtual void Start()
    {
        PlayIdle();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //RotateTowards(orbitter);
        if(GameplayController.instance.GetState() == GameState.midGame)
        {
            if(Input.GetMouseButtonDown(0))
            {
                if(GameplayController.instance.GetState() == GameState.midGame)
                {
                    shipMovement.SwitchDirection();
                    moveRoot.transform.localScale = new Vector3(shipMovement.GetDirection(), 1,1);
                }
            }
        }
        
    }

    public virtual void BackToOrigin(Transform origin)
    {
        transform.position = origin.position;
    }
    
    public virtual void SwitchScale(int to)
    {
moveRoot.transform.localScale = new Vector3(shipMovement.GetDirection(), 1,1);
    }

    public virtual void RotateTowards(Transform TargetObjTransform)
    {
        Vector3 dir = TargetObjTransform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle - angleModifier));
        transform.rotation = rotation;
    }

    

    

    protected virtual void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Pellet"))
        {
            //Debug.Log("Get Pellet");
            BasePellet pellet = other.transform.parent.parent.GetComponent<BasePellet>();
            int _score = pellet.GetPelletValue();
            pellet.Taken();
            GameplayController.instance.TakePellet(_score);
        }

        if(other.CompareTag("Bullet"))
        {
            BaseBullets bl = other.transform.parent.GetComponent<BaseBullets>();
            GetHit(bl);
        }
    }

    public void GetHit(BaseBullets bullet)
    {
        int dmg = bullet.GetDamage();
        if(this.hp == 0)
        {
            //dead
            TBEController.instance.DeactivateActiveturret();
            Dead();
        }
        else
        {
            this.hp -= dmg;
            GameplayController.instance.GetHit();
            if(this.hp > 0)
            {
                GameplayController.instance.UpdateHP(hp);
                GameplayController.instance.ShakeCamera();
                GameplayController.instance.ShowVignette(Color.red);
                HitRecovery();
                explosion.Exploding();
            }
            else
            {
                GameplayController.instance.GameOver();
                TBEController.instance.DeactivateActiveturret();
                Dead();
            }

            
        }
        RefreshHealthSprite();
        
    }


    public void HitRecovery()
    {
        mainCollider.enabled = false; 
        for(int i = 0 ; i < sprites.Count ; i++)
        {
            LeanTween.cancel(sprites[i].gameObject);
            LeanTween.alpha(sprites[i].gameObject, .0f, .5f).setLoopPingPong(3);
        }

        StartCoroutine(AfterRecovery());

        IEnumerator AfterRecovery()
        {
            yield return new WaitForSeconds(3);
            mainCollider.enabled = true;
            for(int i = 0 ; i < sprites.Count ; i++)
            {
                LeanTween.cancel(sprites[i].gameObject);
                LeanTween.alpha(sprites[i].gameObject, 1.0f, 0.25f).setEase(LeanTweenType.easeOutQuad);
            }
            GameplayController.instance.HideVignette();
        }
    }

    public void Dead()
    {
        StartCoroutine(DeathSequence());

        IEnumerator DeathSequence()
        {
            Deactivate();
            
            GameplayController.instance.ShakeCamera(2.0f, 4.0f);
            GameplayController.instance.UnFlash();
            for(int i = 0 ; i < deathExplosions.Count ;i++)
            {
                deathExplosions[i].Exploding();
                yield return new WaitForSeconds(0.25f);
            }

            yield return new WaitForSeconds(0.75f);
            GameplayController.instance.Flashing(()=>{
                HideShipGraphic();
                deathParticle.Play();
            }, null);
            deathExplosions[deathExplosions.Count - 1].ExplodingBig();
            anim.Play("Stop");
            yield return new WaitForSeconds(3.0f);
            GameplayController.instance.ShowGameOver();
        }
    }

    public void Activate()
    {
        shipMovement.EnableMovement();
    }

    public void Deactivate()
    {
        shipMovement.DisableMovement();
        
    }

    public void RefreshHealthSprite()
    {
        mainShipSprite.sprite = ShipHealthSprites[(hp-1)];
    }

    public void Reset()
    {
        hp = ShipHealthSprites.Count;
        RefreshHealthSprite();
        ResetPosition();
    }

    public void ResetPosition()
    {
        LeanTween.cancel(moveRoot.gameObject);
        //moveRoot.transform.position = leftRightMost[0].position;
        shipMovement.ResetRotationAndMovement();
        moveRoot.transform.position = GameplayController.instance.GetLeftMost().position;
        
    }

    public void GoToGamePosition(System.Action next, float duration = 5.0f)
    {
        engineSound.Play(0);
        LeanTween.cancel(moveRoot.gameObject);
        PlayFast();
        LeanTween.value(0,1,duration/2).setOnComplete(()=>{
            PlayIdle();
        });
        GameplayController.instance.ShakeMainCamera(duration / 2, 0.25f);
        LeanTween.moveLocalX(moveRoot.gameObject, .0f, duration).setEase(LeanTweenType.easeInOutQuad).setOnComplete(()=>{
            LeanTween.value(0,1,2.0f).setOnComplete(()=>{
                PlayIdle();
                next();
            });
            
        });
    }

    public virtual void ShowShip()
    {
        ShowShipGraphic();
        LeanTween.cancel(scaleRoot.gameObject);
        LeanTween.scale(scaleRoot.gameObject, Vector3.one, 1.0f).setEase(LeanTweenType.easeOutBack);
    }

    public void ShowShipGraphic()
    {
        for(int i = 0 ; i < sprites.Count ; i++)
        {
            sprites[i].enabled = true;
        }
    }

    public void HideShipGraphic()
    {
        for(int i = 0 ; i < sprites.Count ; i++)
        {
            sprites[i].enabled = false;
        }
    }

    public void PrepareGoNextArena(BaseArena to)
    {
        mainCollider.enabled = false;
        Deactivate();
        shipMovement.RotateToZero(()=>{
            SwitchScale(1);
            StartCoroutine(GoNextArena(to));
        });


        IEnumerator GoNextArena(BaseArena too)
        {
            yield return new WaitForSeconds(1.0f);
            LeanTween.cancel(shipMovement.GetRotator().gameObject);
            LeanTween.moveX(shipMovement.GetRotator().gameObject, too.transform.position.x, 2.0f).setOnComplete(()=>{
                Transform movementParent = shipMovement.rotator;
                transform.parent = null;
                movementParent.position = too.GetCenter().position;
                transform.SetParent(movementParent, true);
                GameplayController.instance.AfterNextArena();
                mainCollider.enabled = true;
            });
        }
    }

    public virtual void PlayFast()
    {
        anim.Play("ShipFast");
    }

    public virtual void PlayIdle()
    {
        anim.Play("ShipIdle");
    }

    public virtual int GetDirection()
    {
        return shipMovement.GetDirection();
    }

    public virtual Transform GetCamFollow()
    {
        return followCam;
    }
    
}
