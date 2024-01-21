using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBullets : MonoBehaviour
{
	[SerializeField] Rigidbody2D moverRb;
	[SerializeField] BaseTurrets owner;
	[SerializeField] float speed = 100;
	[SerializeField] Vector2 launchedDirection;
	[SerializeField] Transform directionFrom;
	[SerializeField] float angleModifier = 0;
	[SerializeField] float rotateSpeed = 100;
	[SerializeField] Transform target;

	private void FixedUpdate() {
		if(target != null)
		{
			Vector2 direction = (Vector2)target.position - 	moverRb.position;
            direction.Normalize();

            float rotateAmount = Vector3.Cross(direction, transform.right * -1).z;
            moverRb.angularVelocity = rotateAmount * rotateSpeed;
		}
		else
		{
			Vector3 dir = moverRb.velocity.normalized;
			float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
			Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle - angleModifier));
			transform.rotation = rotation;
		}
		
	}
	public void Init(BaseTurrets owner)
	{
		this.owner = owner;
	}

	public virtual void SetTarget(Transform target)
	{
		this.target = target;
	}
	
	public virtual void Launch()
	{
		moverRb.velocity = Vector2.zero;
		moverRb.position = owner.GetSpawnPlace();
		//Vector2 dir = directionFrom.position - target.position;
		Vector2 dir = new Vector2(0,1);
		dir.Normalize();
		moverRb.AddForce(speed * dir);
	
	}
	
	public virtual void Launch(Transform direction)
	{
		moverRb.velocity = Vector2.zero;
		moverRb.position = owner.GetSpawnPlace();
		Vector2 dir = direction.position - owner.GetSpawnPlace();
		dir.Normalize();
		moverRb.AddForce(speed * dir);
	
	}
	
	public virtual void Launch(Vector2 direction)
	{
		moverRb.velocity = Vector2.zero;
		moverRb.position = owner.GetSpawnPlace();
		Vector2 dir = direction;
		dir.Normalize();
		moverRb.AddForce(speed * dir);
	}

	public virtual void Deactivate()
	{
		transform.parent.parent.gameObject.SetActive(false);
	}

	private void OnCollisionEnter2D(Collision2D other) {
		if(other.gameObject.CompareTag("BulletLimit"))
		{
			Deactivate();
		}
	}
	
}
