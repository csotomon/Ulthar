using UnityEngine;
using System.Collections;

/// <summary>
/// Adds this class to a body of water. It will handle splash effects on entering/exiting, and allow the player to jump out of it.
/// </summary>
public class Water : MonoBehaviour 
{
	/// the force to add to a character when it exits the water
	public float WaterExitForce=8f;
	/// the effect that will be instantiated everytime the character enters or exits the water
	public GameObject WaterEntryEffect;

	//storage
	private int _numberOfJumpsSaved;

	/// <summary>
	/// Triggered when something collides with the water
	/// </summary>
	/// <param name="collider">Something colliding with the water.</param>
	public void OnTriggerEnter2D(Collider2D collider)
	{
		// we check that the object colliding with the water is actually a corgi controller and a character
		CharacterBehavior character = collider.GetComponent<CharacterBehavior>();
		if (character==null)
			return;		
		CorgiController2D controller = collider.GetComponent<CorgiController2D>();
		if (controller==null)
			return;		
			
		_numberOfJumpsSaved=character.BehaviorState.NumberOfJumpsLeft+1;
		Splash (character.transform.position);
	}
	
	/// <summary>
	/// Triggered when something stays on the water
	/// </summary>
	/// <param name="collider">Something colliding with the water.</param>
	public void OnTriggerStay2D(Collider2D collider)
	{		
		// we check that the object colliding with the water is actually a corgi controller and a character
		CharacterBehavior character = collider.GetComponent<CharacterBehavior>();
		if (character==null)
			return;		
		CorgiController2D controller = collider.GetComponent<CorgiController2D>();
		if (controller==null)
			return;
	}
	
	/// <summary>
	/// Triggered when something exits the water
	/// </summary>
	/// <param name="collider">Something colliding with the water.</param>
	public void OnTriggerExit2D(Collider2D collider)
	{
		// we check that the object colliding with the water is actually a corgi controller and a character
		CharacterBehavior character = collider.GetComponent<CharacterBehavior>();
		if (character==null)
			return;		
		CorgiController2D controller = collider.GetComponent<CorgiController2D>();
		if (controller==null)
			return;
		
		// when the character is not colliding with the water anymore, we reset its various water related states
		character.BehaviorState.NumberOfJumpsLeft=_numberOfJumpsSaved;
		// we also push it up in the air		
		Splash (character.transform.position);
		controller.SetVerticalForce(Mathf.Abs( WaterExitForce ));
	}
	
	/// <summary>
	/// Creates a splash of water at the point of entry
	/// </summary>
	private void Splash(Vector3 splashPosition)
	{
		
		Instantiate(WaterEntryEffect,splashPosition,Quaternion.identity);		
	}
}
