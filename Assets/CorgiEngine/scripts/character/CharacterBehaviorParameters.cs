using System;
using UnityEngine;
using System.Collections;

/// <summary>
/// The various parameters related to the CharacterBehavior class.
/// </summary>

[Serializable]
public class CharacterBehaviorParameters 
{
	/// defines how high the character can jump
	public float JumpHeight = 3.025f;
	/// the minimum time in the air allowed when jumping - this is used for pressure controlled jumps
	public float JumpMinimumAirTime = 0.1f;
	/// basic movement speed
	public float MovementSpeed = 8f;
	/// the speed of the character when it's crouching
	public float CrouchSpeed = 4f;
	/// the speed of the character when it's walking
	public float WalkSpeed = 8f;
	/// the speed of the character when it's running
	public float RunSpeed = 16f;
	/// the speed of the character when climbing a ladder
	public float LadderSpeed = 2f;
	/// the duration of dash (in seconds)
	public float DashDuration = 0.15f;
	/// the force of the dash
	public float DashForce = 5f;	
	/// the duration of the cooldown between 2 dashes (in seconds)
	public float DashCooldown = 2f;	
	/// the duration of a walljump (in seconds)
	public float WallJumpDuration = 0.15f;
	/// the force of a walljump
	public float WallJumpForce = 3f;
	/// the force applied by the jetpack
	public float JetpackForce = 2.5f;
	/// the maximum number of jumps allowed (0 : no jump, 1 : normal jump, 2 : double jump, etc...)
	public int NumberOfJumps=3;
	/// the maximum health of the character
	public int MaxHealth = 100;
	/// true if the character is allowed to shoot in more than 1 direction
	public bool EightDirectionShooting=true;
	/// true if the character can only shoot in the "strict" 8 directions (top, top right, right, bottom right, etc...)
	public bool StrictEightDirectionShooting=true;	
	public enum JumpBehavior
	{
		CanJumpOnGround,
		CanJumpAnywhere,
		CantJump,
		CanJumpAnywhereAnyNumberOfTimes
	}
	/// basic rules for jumps : where can the player jump ?
	public JumpBehavior JumpRestrictions;	
}
