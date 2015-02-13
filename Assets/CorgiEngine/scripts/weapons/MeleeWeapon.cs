using UnityEngine;
using System.Collections;
/// <summary>
/// Handles melee attacks
/// </summary>
public class MeleeWeapon : MonoBehaviour 
{
	/// the collision mask that will get hit by the melee attack
	public LayerMask CollisionMask;
	/// the amount of damage to inflict
	public int Damage;
	/// the effect to instantiate on hit
	public GameObject HitEffect;
	/// the owner of the attack
	public GameObject Owner;

	/// <summary>
	/// Triggered when something collides with the melee attack
	/// </summary>
	/// <param name="collider">Collider.</param>
	public virtual void OnTriggerEnter2D(Collider2D collider)
	{
		// if the collider the melee weapon is colliding with is not on the targeted layer mask, we do nothing
		if ((CollisionMask.value & (1 << collider.gameObject.layer)) == 0)
		{
			return;
		}
				
		// if the collider the melee weapon is colliding with is its owner (the player), we do nothing	
		var isOwner = collider.gameObject == Owner;
		if (isOwner)
		{
			return;
		}

		// if the collider the melee weapon is colliding with can take damage, we apply the melee weapon's damage to it, and instantiate a hit effect
		var takeDamage= (CanTakeDamage) collider.GetComponent(typeof(CanTakeDamage));
		if (takeDamage!=null)
		{
			OnCollideTakeDamage(collider,takeDamage);
			return;
		}
		
		OnCollideOther(collider);
	}


	void OnCollideTakeDamage(Collider2D collider, CanTakeDamage takeDamage)
	{
		Instantiate(HitEffect,collider.transform.position,collider.transform.rotation);
		takeDamage.TakeDamage(Damage,gameObject);
		DisableMeleeWeapon();		
	}
	
	void OnCollideOther(Collider2D collider)
	{
		DisableMeleeWeapon();
	}
		
	void DisableMeleeWeapon()
	{
		// if you have longer lasting melee animations, you might want to disable the melee weapon's collider after it hits something, until the end of the animation.
	}
}
