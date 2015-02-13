using UnityEngine;
using System.Collections;
/// <summary>
/// Add this class to a trigger and it will send your player to the next level
/// </summary>
public class FinishLevel : MonoBehaviour 
{
	public string LevelName;

	/// <summary>
	/// When triggered, goes to next level
	/// </summary>
	/// <param name="collider">a collider colliding with our trigger.</param>
	public void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.GetComponent<CharacterBehavior>() == null)
			return;
			
		LevelManager.Instance.GotoLevel(LevelName);
	}
}
