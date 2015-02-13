using System;
using UnityEngine;
using System.Collections;

/// <summary>
/// A list of permissions for your character. 
/// set them to false if you don't want the player to EVER be able to jump, dash, or any of the actions below
/// (you can set these at the start, or unlock them in your game)	
/// </summary>

[Serializable]
public class CharacterBehaviorPermissions
{
	
	// Set these to false if you don't want the player to EVER be able to jump, dash, or any of the actions below
	// (you can set these at the start, or unlock them in your game)	
	public bool RunEnabled=true;
	public bool DashEnabled=true;
	public bool JetpackEnabled=true;
	public bool JumpEnabled=true;
	public bool CrouchEnabled=true;
	public bool ShootEnabled=true;
	public bool WallJumpEnabled=true;
	public bool WallClingingEnabled=true;
	public bool MeleeAttackEnabled=true;	
}
