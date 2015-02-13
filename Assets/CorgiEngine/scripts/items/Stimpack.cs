using UnityEngine;
using System.Collections;
/// <summary>
/// Gives health to the player who collects it
/// </summary>
public class Stimpack : MonoBehaviour,IPlayerRespawnListener
{
	/// the effect to instantiate when the object is collected
	public GameObject Effect;
	/// the amount of health to give the player when collected
	public int HealthToGive;

	/// <summary>
	/// triggered when something collides with the object
	/// </summary>
	/// <param name="collider">Other collider.</param>
	public void OnTriggerEnter2D(Collider2D collider)
	{
		// if what collides with the object ain't the player, we do nothing and exit
		var player = collider.GetComponent<CharacterBehavior>();
		if (player == null)
			return;

		// else, we give health to the player
		player.GiveHealth(HealthToGive,gameObject);
		// we instantiate the hit effect
		Instantiate(Effect,transform.position,transform.rotation);
		// we desactivate the object
		gameObject.SetActive(false);
	}

	/// <summary>
	/// When the player respawns, we reinstate the object
	/// </summary>
	/// <param name="checkpoint">Checkpoint.</param>
	/// <param name="player">Player.</param>
	public void onPlayerRespawnInThisCheckpoint(CheckPoint checkpoint, CharacterBehavior player)
	{
		gameObject.SetActive(true);
	}
}
