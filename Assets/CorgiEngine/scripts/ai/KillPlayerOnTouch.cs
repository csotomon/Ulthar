using UnityEngine;
using System.Collections;
/// <summary>
/// Add this to a GameObject with a Collider2D set to Trigger to have it kill the player on touch.
/// </summary>
public class KillPlayerOnTouch : MonoBehaviour 
{
	/// <summary>
	/// When a collision is triggered, check if the thing colliding is actually the player. If yes, kill it.
	/// </summary>
	/// <param name="collider">The object that collides with the KillPlayerOnTouch object.</param>
	public void OnTriggerEnter2D(Collider2D collider)
	{
		var player = collider.GetComponent<CharacterBehavior>();
		if (player==null)
			return;
		
		LevelManager.Instance.KillPlayer();
	}
}
