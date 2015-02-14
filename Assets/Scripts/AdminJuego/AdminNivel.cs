using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class AdminNivel : LevelManager {



	private IEnumerator KillPlayerCo()
	{
		Debug.Log("entro aqui");
		_player.Kill();
		//Camera.FollowsPlayer=false;
		yield return new WaitForSeconds(2f);
		
		Camera.FollowsPlayer=true;
		if (_currentCheckPointIndex!=-1)
			_checkpoints[_currentCheckPointIndex].SpawnPlayer(_player);
		
		_started = DateTime.UtcNow;
		GameManager.Instance.SetPoints(_savedPoints);
	}
}
