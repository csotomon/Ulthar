using UnityEngine;
using System.Collections;

/// <summary>
/// Adds this class to your ladders so a Character can climb them.
/// </summary>
public class Ladder : MonoBehaviour 
{
	/// the platform at the top of the ladder - this can be a ground platform 
	public GameObject ladderPlatform;

	/// <summary>
	/// Triggered when something collides with the ladder
	/// </summary>
	/// <param name="collider">Something colliding with the ladder.</param>
	public void OnTriggerEnter2D(Collider2D collider)
	{
		// we check that the object colliding with the ladder is actually a corgi controller and a character
		CharacterBehavior character = collider.GetComponent<CharacterBehavior>();
		if (character==null)
			return;		
		CorgiController2D controller = collider.GetComponent<CorgiController2D>();
		if (controller==null)
			return;		
	}
	
	/// <summary>
	/// Triggered when something stays on the ladder
	/// </summary>
	/// <param name="collider">Something colliding with the ladder.</param>
	public void OnTriggerStay2D(Collider2D collider)
	{		
		// we check that the object colliding with the ladder is actually a corgi controller and a character
		CharacterBehavior character = collider.GetComponent<CharacterBehavior>();
		if (character==null)
			return;		
		CorgiController2D controller = collider.GetComponent<CorgiController2D>();
		if (controller==null)
			return;
		
		// if we're still here, it's a character, we set its colliding state accordingly
		character.BehaviorState.LadderColliding=true;
		// if the character is climbing a ladder, we center it on the ladder
		if (character.BehaviorState.LadderClimbing)
		{			
			controller.transform.position=new Vector2(transform.position.x,controller.transform.position.y);
		}
				
		// if the feet of the character are above the ladder platform, we release it from the ladder.
		if (ladderPlatform.transform.position.y < controller.transform.position.y-(controller.transform.localScale.y/2)-0.1f)
		{
			character.BehaviorState.LadderClimbing=false;
			character.BehaviorState.CanMoveFreely=true;
			character.BehaviorState.LadderClimbingSpeed=0;			
		}
		
	}
	
	/// <summary>
	/// Triggered when something exits the ladder
	/// </summary>
	/// <param name="collider">Something colliding with the ladder.</param>
	public void OnTriggerExit2D(Collider2D collider)
	{
		// we check that the object colliding with the ladder is actually a corgi controller and a character
		CharacterBehavior character = collider.GetComponent<CharacterBehavior>();
		if (character==null)
			return;		
		CorgiController2D controller = collider.GetComponent<CorgiController2D>();
		if (controller==null)
			return;
		
		// when the character is not colliding with the ladder anymore, we reset its various ladder related states
		character.BehaviorState.LadderColliding=false;
		character.BehaviorState.LadderClimbing=false;
		character.BehaviorState.CanMoveFreely=true;	
	}
}
