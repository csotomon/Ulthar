using UnityEngine;
using System.Collections;
/// <summary>
/// Add this class to a trigger to cause the level to restart when the player hits the trigger
/// </summary>
public class LevelRestarter : MonoBehaviour 
{	
	void OnTriggerEnter2D (Collider2D collider)
	{
		if(collider.tag == "Player")
			Application.LoadLevel(Application.loadedLevelName);
	}
}