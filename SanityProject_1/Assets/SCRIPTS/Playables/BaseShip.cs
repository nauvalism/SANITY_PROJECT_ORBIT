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

    [Header("Positions")]
    [SerializeField] Transform scaleRoot;
    [SerializeField] Transform moveRoot;
    [SerializeField] List<Transform> leftRightMost;
    

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
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //RotateTowards(orbitter);
        if(Input.GetMouseButtonDown(0))
        {
            if(GameplayController.instance.GetState() == GameState.midGame)
            {
                shipMovement.SwitchDirection();
                moveRoot.transform.localScale = new Vector3(shipMovement.GetDirection(), 1,1);
            }
        }
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
            
            other.transform.parent.parent.GetComponent<BasePellet>().Taken();
            GameplayController.instance.TakePellet();
        }
    }

    public void Activate()
    {
        shipMovement.EnableMovement();
    }

    public void ResetPosition()
    {
        LeanTween.cancel(moveRoot.gameObject);
        moveRoot.transform.position = leftRightMost[0].position;
        
    }

    public void GoToGamePosition(System.Action next)
    {
        LeanTween.cancel(moveRoot.gameObject);
        LeanTween.moveLocalX(moveRoot.gameObject, .0f, 1.5f).setEase(LeanTweenType.easeOutQuad).setOnComplete(()=>{
            next();
        });
    }

    public virtual void ShowShip()
    {
        LeanTween.cancel(scaleRoot.gameObject);
        LeanTween.scale(scaleRoot.gameObject, Vector3.one, 1.0f).setEase(LeanTweenType.easeOutBack);
    }
}
