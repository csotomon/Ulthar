using UnityEngine;
using System.Collections;

/// <summary>
/// The various states you can use to check if your character is doing something at the current frame
/// </summary>

public class CorgiControllerState2D 
{
	/// is the character colliding right ?
	public bool IsCollidingRight { get; set; }
	/// is the character colliding left ?
	public bool IsCollidingLeft { get; set; }
	/// is the character colliding with something above it ?
	public bool IsCollidingAbove { get; set; }
	/// is the character colliding with something above it ?
	public bool IsCollidingBelow { get; set; }
	/// is the character colliding with anything ?
	public bool HasCollisions { get { return IsCollidingRight || IsCollidingLeft || IsCollidingAbove || IsCollidingBelow; }}
	
	/// is the character moving down a slope ?
	public bool IsMovingDownSlope { get; set; }
	/// is the character moving up a slope ?
	public bool IsMovingUpSlope { get; set; }
	/// returns the slope the character is moving on angle
	public float SlopeAngle { get; set; }
	
	/// Is the character grounded ? 
	public bool IsGrounded { get { return IsCollidingBelow; } }
	/// was the character grounded last frame ?
	public bool WasGroundedLastFrame { get ; set; }
	/// did the character just become grounded ?
	public bool JustGotGrounded { get ; set;  }	
			
	/// <summary>
	/// Reset all collision states to false
	/// </summary>
	public void Reset()
	{
		IsMovingUpSlope =
		IsMovingDownSlope =
		IsCollidingLeft = 
		IsCollidingRight = 
		IsCollidingAbove =
		IsCollidingBelow = 
		JustGotGrounded = false;
		SlopeAngle = 0;
	}
	
	/// <summary>
	/// Serializes the collision states
	/// </summary>
	/// <returns>A <see cref="System.String"/> that represents the current collision states.</returns>
	public override string ToString ()
	{
		return string.Format("(controller: r:{0} l:{1} a:{2} b:{3} down-slope:{4} up-slope:{5} angle: {6}",
		IsCollidingRight,
		IsCollidingLeft,
		IsCollidingAbove,
		IsCollidingBelow,
		IsMovingDownSlope,
		IsMovingUpSlope,
		SlopeAngle);
	}	
}