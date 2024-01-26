using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Turret Attribute", menuName = "Scriptables/Turret Attribute", order = 2)]
public class TurretAttributes : ScriptableObject
{
	public int turretIDAttribute = 0;
	public int level = 0;
	public float scale = 1;
	public float scaleTo = 1.1f;
	public float fireRate = 0.75f;
	public Color levelColor;
	public Sprite spriteChange;
}
