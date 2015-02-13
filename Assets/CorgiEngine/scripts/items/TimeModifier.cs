using UnityEngine;
using System.Collections;
/// <summary>
/// Add this to an item to make it modify time when it gets picked up by a Character
/// </summary>
public class TimeModifier : MonoBehaviour, IPlayerRespawnListener
{
	/// the effect to instantiate when picked up
	public GameObject Effect;
	/// the time speed to apply while the effect lasts
	public float TimeSpeed = 0.5f;
	/// how long the duration will last , in seconds
	public float Duration = 1.0f;

	/// <summary>
	/// Triggered when something collides with the TimeModifier
	/// </summary>
	/// <param name="collider">The object that collide with the TimeModifier</param>
	public void OnTriggerEnter2D (Collider2D collider) 
	{
		// if the other collider isn't a CharacterBehavior, we exit and do nothing
		if (collider.GetComponent<CharacterBehavior>() == null)
			return;
		// we start the ChangeTime coroutine
		StartCoroutine (ChangeTime ());

		// adds an instance of the effect at the TimeModifier's position
		Instantiate(Effect,transform.position,transform.rotation);
		// we disable the sprite and the collider
		gameObject.GetComponent<SpriteRenderer> ().enabled = false;
		gameObject.GetComponent<CircleCollider2D> ().enabled = false;
	}

	/// <summary>
	/// Asks the Game Manager to change the time scale for a specified duration.
	/// </summary>
	/// <returns>The time.</returns>
	private IEnumerator ChangeTime()
	{
		GameManager.Instance.SetTimeScale (TimeSpeed);
		GUIManager.Instance.SetTimeSplash (true);
		// we multiply the duration by the timespeed to get the real duration in seconds
		yield return new WaitForSeconds (Duration*TimeSpeed);
		GameManager.Instance.ResetTimeScale ();
		GUIManager.Instance.SetTimeSplash (false);
		// we re enable the sprite and collider, and desactivate the object
		gameObject.GetComponent<SpriteRenderer> ().enabled = true;
		gameObject.GetComponent<CircleCollider2D> ().enabled = true;
		gameObject.SetActive(false);
	}

	/// <summary>
	/// Sets the TimeModifier on again when the player respawns.
	/// </summary>
	/// <param name="checkpoint">Checkpoint.</param>
	/// <param name="character">Character.</param>
	public void onPlayerRespawnInThisCheckpoint(CheckPoint checkpoint, CharacterBehavior character)
	{
		gameObject.SetActive(true);
	}
}
