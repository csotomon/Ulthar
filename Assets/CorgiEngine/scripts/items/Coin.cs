using UnityEngine;
using System.Collections;
/// <summary>
/// Coin manager
/// </summary>
public class Coin : MonoBehaviour, IPlayerRespawnListener
{
	/// The effect to instantiate when the coin is hit
	public GameObject Effect;
	/// The amount of points to add when collected
	public int PointsToAdd = 10;

	/// <summary>
	/// Triggered when something collides with the coin
	/// </summary>
	/// <param name="collider">Other.</param>
	public void OnTriggerEnter2D (Collider2D collider) 
	{
		// if what's colliding with the coin ain't a characterBehavior, we do nothing and exit
		if (collider.GetComponent<CharacterBehavior>() == null)
			return;

		// We pass the specified amount of points to the game manager
		GameManager.Instance.AddPoints(PointsToAdd);
		// adds an instance of the effect at the coin's position
		Instantiate(Effect,transform.position,transform.rotation);
		// we desactivate the gameobject
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
