using UnityEngine;
using System.Collections;
/// <summary>
/// A simple projectile behavior
/// </summary>
public class SimpleProjectile : Projectile, CanTakeDamage
{
	/// the amount of damage the projectile inflicts
	public int Damage;
	/// the effect to instantiate when the projectile gets destroyed
	public GameObject DestroyedEffect;
	/// the amount of points to give the player when destroyed
	public int PointsToGiveToPlayer;	
	/// the lifetime of the projectile
	public float TimeToLive;

	/// <summary>
	/// Every frame, we check if the projectile has outlived its lifespan
	/// </summary>
	public void Update () 
	{
		// if true, we destroy it
		if ((TimeToLive -= Time.deltaTime) <= 0)
		{
			DestroyProjectile();
			return;
		}
		// we move the projectile
		transform.Translate(Direction * ((Mathf.Abs (InitialVelocity.x)+Speed) * Time.deltaTime),Space.World);
	}	

	/// <summary>
	/// Called when the projectile takes damage
	/// </summary>
	/// <param name="damage">Damage.</param>
	/// <param name="instigator">Instigator.</param>
	public void TakeDamage(int damage, GameObject instigator)
	{
		if (PointsToGiveToPlayer!=0)
		{
			var projectile = instigator.GetComponent<Projectile>();
			if (projectile != null && projectile.Owner.GetComponent<CharacterBehavior>() != null)
			{
				GameManager.Instance.AddPoints(PointsToGiveToPlayer);
			}
		}
		
		DestroyProjectile();
	}

	/// <summary>
	/// Triggered when the projectile collides with something
	/// </summary>
	/// <param name="collider">Collider.</param>
	protected override void OnCollideOther(Collider2D collider)
	{
		DestroyProjectile();
	}

	/// <summary>
	/// Raises the collide take damage event.
	/// </summary>
	/// <param name="collider">Collider.</param>
	/// <param name="takeDamage">Take damage.</param>
	protected override void OnCollideTakeDamage(Collider2D collider, CanTakeDamage takeDamage)
	{
		takeDamage.TakeDamage(Damage,gameObject);
		DestroyProjectile();
	}

	/// <summary>
	/// Destroys the projectile.
	/// </summary>
	private void DestroyProjectile()
	{
		if (DestroyedEffect!=null)
		{
			Instantiate(DestroyedEffect,transform.position,transform.rotation);
		}
		
		Destroy(gameObject);
	}
}
