using System;
using UnityEngine;
using System.Collections;

/// <summary>
/// Parameters for the Corgi Controller class.
/// This is where you define your slope limit, gravity, and speed dampening factors
/// </summary>

[Serializable]
public class CorgiControllerParameters2D 
{
	/// Maximum velocity for your character, to prevent it from moving too fast on a slope for example
	public Vector2 MaxVelocity = new Vector2(float.MaxValue, float.MaxValue);	
	/// Maximum angle (in degrees) the character can walk on
	[Range(0,90)]
	public float SlopeLimit = 45;		
	/// Gravity
	public float Gravity = -15;	
	// Speed dampening factor on the ground
	public float SpeedAccelerationOnGround = 20f;
	// Speed dampening factor in the air
	public float SpeedAccelerationInAir = 5f;	
}
