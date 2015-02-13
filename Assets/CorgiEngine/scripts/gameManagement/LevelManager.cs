using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
/// <summary>
/// Spawns the player, and 
/// </summary>
public class LevelManager : MonoBehaviour
{
	/// Singleton
	public static LevelManager Instance { get; private set; }		
	/// the prefab you want for your player
	public CharacterBehavior PlayerPrefab ;
	/// the camera controller
	public CameraController Camera { get; private set; }
	/// the elapsed time since the start of the level
	public TimeSpan RunningTime { get { return DateTime.UtcNow - _started ;}}
	/// Debug spawn	
	public CheckPoint DebugSpawn;
	/// duration of the initial fade in
	public float IntroFadeDuration=1f;
	/// duration of the fade to black at the end of the level
	public float OutroFadeDuration=1f;
	
	// private stuff
	private CharacterBehavior _player;
	private List<CheckPoint> _checkpoints;
	private int _currentCheckPointIndex;
	private DateTime _started;
	private int _savedPoints;
	
	/// <summary>
	/// On awake, instantiates the player
	/// </summary>
	public void Awake()
	{
		Instance=this;
		_player = (CharacterBehavior)Instantiate(PlayerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
	}
	
	/// <summary>
	/// Initialization
	/// </summary>
	public void Start()
	{
		// storage
		_savedPoints=GameManager.Instance.Points;
		_checkpoints = FindObjectsOfType<CheckPoint>().OrderBy(t => t.transform.position.x).ToList();
		_currentCheckPointIndex = _checkpoints.Count > 0 ? 0 : -1;
		_started = DateTime.UtcNow;

		// we get the camera				
		Camera = FindObjectOfType<CameraController>();

		// we get the list of spawn points
		var listeners = FindObjectsOfType<MonoBehaviour>().OfType<IPlayerRespawnListener>();
		foreach(var listener in listeners)
		{
			for (var i = _checkpoints.Count - 1; i>=0; i--)
			{
				var distance = ((MonoBehaviour) listener).transform.position.x - _checkpoints[i].transform.position.x;
				if (distance<0)
					continue;
				
				_checkpoints[i].AssignObjectToCheckPoint(listener);
				break;
			}
		}
		
		// fade in
		GUIManager.Instance.FaderOn(false,IntroFadeDuration);

		// in debug mode we spawn the player on the debug spawn point
		#if UNITY_EDITOR
		if (DebugSpawn!= null)
		{
			DebugSpawn.SpawnPlayer(_player);
		}
		else if (_currentCheckPointIndex != -1)
		{
			_checkpoints[_currentCheckPointIndex].SpawnPlayer(_player);
		}
		#else
		if (_currentCheckPointIndex != -1)
		{			
			_checkpoints[_currentCheckPointIndex].SpawnPlayer(_player);
		}
		#endif		
	}

	/// <summary>
	/// Every frame we check for checkpoint reach
	/// </summary>
	public void Update()
	{
		var isAtLastCheckPoint = _currentCheckPointIndex + 1 >= _checkpoints.Count;
		if (isAtLastCheckPoint)
			return;
		
		var distanceToNextCheckPoint = _checkpoints[_currentCheckPointIndex+1].transform.position.x - _player.transform.position.x;
		if (distanceToNextCheckPoint>=0)
			return;
		
		_checkpoints[_currentCheckPointIndex].PlayerLeftCheckPoint();
		
		_currentCheckPointIndex++;
		_checkpoints[_currentCheckPointIndex].PlayerHitCheckPoint();
		
		_savedPoints = GameManager.Instance.Points;
		_started = DateTime.UtcNow;
	}

	/// <summary>
	/// Gets the player to the specified level
	/// </summary>
	/// <param name="levelName">Level name.</param>
	public void GotoLevel(string levelName)
	{		
		GUIManager.Instance.FaderOn(true,OutroFadeDuration);
		StartCoroutine(GotoLevelCo(levelName));
	}

	/// <summary>
	/// Waits for a short time and then loads the specified level
	/// </summary>
	/// <returns>The level co.</returns>
	/// <param name="levelName">Level name.</param>
	private IEnumerator GotoLevelCo(string levelName)
	{
		_player.Disable();
		yield return new WaitForSeconds(OutroFadeDuration);
		
		if (string.IsNullOrEmpty(levelName))
			Application.LoadLevel("StartScreen");
		else
			Application.LoadLevel(levelName);
		
	}

	/// <summary>
	/// Kills the player.
	/// </summary>
	public void KillPlayer()
	{
		StartCoroutine(KillPlayerCo());
	}

	/// <summary>
	/// Coroutine that kills the player, stops the camera, resets the points.
	/// </summary>
	/// <returns>The player co.</returns>
	private IEnumerator KillPlayerCo()
	{
		_player.Kill();
		Camera.FollowsPlayer=false;
		yield return new WaitForSeconds(2f);
		
		Camera.FollowsPlayer=true;
		if (_currentCheckPointIndex!=-1)
			_checkpoints[_currentCheckPointIndex].SpawnPlayer(_player);
		
		_started = DateTime.UtcNow;
		GameManager.Instance.SetPoints(_savedPoints);
	}
}

