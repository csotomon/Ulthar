using UnityEngine;
using System.Collections;
/// <summary>
/// Handles the behaviors of projectiles
/// </summary>
public abstract class Projectile : MonoBehaviour 
{	
	/// the speed of the projectile
	public float Speed;
	/// the collision mask of the projectile
	public LayerMask CollisionMask;
	/// the projectile's owner
	public GameObject Owner {get; private set; }
	/// the initial direction of the projectile
	public Vector2 Direction  {get; private set; }
	/// the projectile's initial velocity
	public Vector2 InitialVelocity  {get; private set; }

	/// <summary>
	/// Initialize the specified owner, direction and initialVelocity.
	/// </summary>
	/// <param name="owner">Owner.</param>
	/// <param name="direction">Direction.</param>
	/// <param name="initialVelocity">Initial velocity.</param>
	public void Initialize (GameObject owner, Vector2 direction, Vector2 initialVelocity )
	{
		transform.right=direction;
		Owner=owner;
		Direction=direction;
		InitialVelocity=initialVelocity;
		
		OnInitialized();
	}

	/// <summary>
	/// What happens when initialized
	/// </summary>
	protected virtual void OnInitialized()
	{
		// nothing right now
	}

	/// <summary>
	/// triggered when the projectile collides with something
	/// </summary>
	/// <param name="collider">Collider.</param>
	public virtual void OnTriggerEnter2D(Collider2D collider)
	{
		// if the collider we're hitting isn't on the right collision mask, we do nothing and exit
		if ((CollisionMask.value & (1 << collider.gameObject.layer)) == 0)
		{
			OnNotCollideWith(collider);
			return;
		}
		
		var isOwner = collider.gameObject == Owner;
		if (isOwner)
		{
			OnCollideOwner();
			return;
		}
		
		var takeDamage= (CanTakeDamage) collider.GetComponent(typeof(CanTakeDamage));
		if (takeDamage!=null)
		{
			OnCollideTakeDamage(collider,takeDamage);
			return;
		}
		
		OnCollideOther(collider);
	}
	
	protected virtual void OnNotCollideWith(Collider2D collider)
	{
	
	}
	
	protected virtual void OnCollideOwner()
	{
	
	}
	
	protected virtual void OnCollideTakeDamage(Collider2D collider, CanTakeDamage takeDamage)
	{
	
	}
	
	protected virtual void OnCollideOther(Collider2D collider)
	{
	
	}
	
}
