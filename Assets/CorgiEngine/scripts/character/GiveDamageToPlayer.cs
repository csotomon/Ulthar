using UnityEngine;
using System.Collections;
/// <summary>
/// Add this class to an object that should give damage to the player when colliding with it
/// </summary>
public class GiveDamageToPlayer : MonoBehaviour 
{
	/// The amount of health to remove from the player's health
	public int DamageToGive = 10;
	
	// storage		
	private Vector2
		_lastPosition,
		_velocity;
	
	/// <summary>
	/// During last update, we store the position and velocity of the object
	/// </summary>
	public void LateUpdate () 
	{
		_velocity = (_lastPosition - (Vector2)transform.position) /Time.deltaTime;
		_lastPosition = transform.position;
	}
	
	/// <summary>
	/// When a collision with the player is triggered, we give damage to the player and make it knock back
	/// </summary>
	/// <param name="collider">what's colliding with the object.</param>
	public void OnTriggerEnter2D(Collider2D collider)
	{
		var player=collider.GetComponent<CharacterBehavior>();
		if (player==null)
			return;
		
		player.TakeDamage(DamageToGive,gameObject);
		var controller=player.GetComponent<CorgiController2D>();
		var totalVelocity=controller.Velocity + _velocity;
		
		controller.SetForce(new Vector2(
			-1*Mathf.Sign(totalVelocity.x) * Mathf.Clamp(Mathf.Abs(totalVelocity.x) * 5,10,20),
			-1*Mathf.Sign(totalVelocity.y) * Mathf.Clamp(Mathf.Abs(totalVelocity.y) * 2,0,15)			
			));
	}
}
