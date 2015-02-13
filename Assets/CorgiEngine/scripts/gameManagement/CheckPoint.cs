using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// Checkpoint class. Will make the player respawn at this point if it dies.
/// </summary>
public class CheckPoint : MonoBehaviour 
{
	private List<IPlayerRespawnListener> _listeners;

	/// <summary>
	/// Initializes the list of listeners
	/// </summary>
	public void Awake () 
	{
		_listeners = new List<IPlayerRespawnListener>();
	}
	
	/// <summary>
	/// Called when the player hits the checkpoint
	/// </summary>
	public void PlayerHitCheckPoint()
	{
		// what happens when the player hits the checkpoint
	}
	
	private IEnumerator PlayerHitCheckPointCo(int bonus)
	{
		yield break;
	}
	
	/// <summary>
	/// Called when the player leaves the checkpoint
	/// </summary>
	public void PlayerLeftCheckPoint()
	{
		// what happens when the player leaves the checkpoint
	}
	
	/// <summary>
	/// Spawns the player at the checkpoint.
	/// </summary>
	/// <param name="player">Player.</param>
	public void SpawnPlayer(CharacterBehavior player)
	{
		player.RespawnAt(transform);
		
		foreach(var listener in _listeners)
		{
			listener.onPlayerRespawnInThisCheckpoint(this,player);
		}
	}
	
	public void AssignObjectToCheckPoint (IPlayerRespawnListener listener) 
	{
		_listeners.Add(listener);
	}
}
