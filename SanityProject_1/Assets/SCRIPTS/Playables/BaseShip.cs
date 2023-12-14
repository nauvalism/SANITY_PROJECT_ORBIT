using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseShip : MonoBehaviour
{
    private Transform orbitter;
    
    [Header("Health and Aliveness")]
    [SerializeField] private List<Sprite> ShipHealthSprites;
    [SerializeField] public int hp = 2;

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
    }

    public virtual void RotateTowards(Transform TargetObjTransform)
    {
        Vector3 dir = TargetObjTransform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle - angleModifier));
        transform.rotation = rotation;
    }

    

    

    protected virtual void OnTriggerEnter2D(Collider2D other) {
        
    }
}
