using UnityEngine;
using System.Collections;
/// <summary>
/// Add this class to an area (water for example) and it will pass its parameters to any character that gets into it.
/// </summary>
public class CorgiControllerPhysicsVolume2D : MonoBehaviour 
{
	public CorgiControllerParameters2D ControllerParameters;
	public CharacterBehaviorParameters BehaviorParameters;	
}
