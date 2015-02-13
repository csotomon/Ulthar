#define DEBUG_RAYS
using UnityEngine;
using System;
using System.Collections;
/// <summary>
/// This class handles all the low level stuff in your character's movements. It handles gravity, slopes, stuff like that.
/// You shouldn't change it too much, but rather use its various methods to control your character.
/// It's independent of your game's rules. If you want your player to triple jump, run super fast or whatever, do that in CharacterBehavior.
/// </summary>
public class CorgiController2D : MonoBehaviour 
{
	/// distance between the edges of the collider and the starting point of the rays
	private const float SkinWidth = .02f;
	/// number of horizontal rays to cast to check for collisions
	private const int HorizontalRaysNumber = 8;
	/// number of vertical rays to cast to check for collisions
	private const int VerticalRaysNumber = 4;
		
	/// The layer mask the platforms are on
	public LayerMask PlatformMask=0;
	/// The layer mask the moving platforms are on
	public LayerMask MovingPlatformMask=0;
	/// The layer mask the one way platforms are on
	public LayerMask OneWayPlatformMask=0;
	
	/// the various states of our character
	public CorgiControllerState2D State { get; private set; }
	/// the initial parameters
	public CorgiControllerParameters2D DefaultParameters;
	/// the current parameters
	public CorgiControllerParameters2D Parameters{get{return _overrideParameters ?? DefaultParameters;}}
	
	/// the current velocity of the character
	public Vector2 Velocity { get{ return _velocity; } }
	/// should the controller handle collisions ?
	public bool HandleCollisions { get; set; }
	/// gives you the object the character is standing on
	public GameObject StandingOn { get; private set; }
	/// gives you the velocity of the platform the character is standing on
	public Vector3 PlatformVelocity {get; private set; }
	/// curve to calculate the character's speed when on a slope. Positive means up slope, negative down slope 
	public AnimationCurve slopeSpeedMultiplier = new AnimationCurve( new Keyframe( -90, 1.5f ), new Keyframe( 0, 1 ), new Keyframe( 90, 0 ) );
				
	// private local references			
	private Vector2 _velocity;
	private Transform _transform;
	private Vector3 _localScale;
	private BoxCollider2D _boxCollider;
	private GameObject _lastStandingOn;
	private Vector3 _activeGlobalPlatformPoint;
	private Vector3 _activeLocalPlatformPoint;
	
	// distance between rays
	private float _verticalDistanceBetweenRays;
	private float _horizontalDistanceBetweenRays;
	
	// parameters override storage
	private CorgiControllerParameters2D _overrideParameters;
	
	// length of the ray that handles slopes detection
	private static readonly float _slopeLimitTangent = Mathf.Tan (75f*Mathf.Deg2Rad);
	
	// raycast position
	private Vector3 _raycastTopLeft,
					_raycastBottomRight,
					_raycastBottomLeft;
	
	/// <summary>
	/// Awake this instance. Initializes private variables, platform mask, and initially sets the distance between rays
	/// </summary>
	public void Awake()
	{
		// initialization
		HandleCollisions=true;
		_transform=transform;
		_localScale=transform.localScale;
		_boxCollider = GetComponent<BoxCollider2D>();
		State = new CorgiControllerState2D();
				
		// we add the 1way platform and moving platform masks to our initial platform mask so they can be walked on		
		PlatformMask |= OneWayPlatformMask;
		PlatformMask |= MovingPlatformMask;
		
		// first calculation of the distance between the rays
		SetDistanceBetweenRays ();
	}

	/// <summary>
	/// Calculates the distance between the rays 
	/// </summary>
	public void SetDistanceBetweenRays()
	{	
		var colliderWidth = _boxCollider.size.x * Mathf.Abs (transform.localScale.x) - (2*SkinWidth);
		_horizontalDistanceBetweenRays = colliderWidth / (VerticalRaysNumber - 1);
		
		var colliderHeight = _boxCollider.size.y * Mathf.Abs (transform.localScale.y) - (2*SkinWidth);
		_verticalDistanceBetweenRays = colliderHeight / (HorizontalRaysNumber - 1);
	}	
	
	[System.Diagnostics.Conditional( "DEBUG_RAYS" )]
	/// Draws the rays in debug mode 
	private void DebugDrawRay( Vector3 start, Vector3 direction, Color rayColor )
	{
		Debug.DrawRay( start, direction, rayColor );
	}
	
	/// <summary>
	/// Use this to add force to the character
	/// </summary>
	/// <param name="force">Force to add to the character.</param>
	public void AddForce(Vector2 force)
	{
		_velocity += force;	
	}
	
	/// <summary>
	/// Use this to set the force applied to the character
	/// </summary>
	/// <param name="force">Force to apply to the character.</param>
	public void SetForce(Vector2 force)
	{
		_velocity = force;
	}
	
	/// <summary>
	///  use this to set the horizontal force applied to the character
	/// </summary>
	/// <param name="x">The x value of the velocity.</param>
	public void SetHorizontalForce (float x)
	{
		_velocity.x = x;
	}
	
	/// <summary>
	///  use this to set the vertical force applied to the character
	/// </summary>
	/// <param name="y">The y value of the velocity.</param>
	public void SetVerticalForce (float y)
	{
		_velocity.y = y;
	}
		
	/// <summary>
	/// During the Late Update, we add the gravity to the character, and make it move
	/// </summary>
	public void LateUpdate()
	{
		_velocity.y += Parameters.Gravity * Time.deltaTime;
		Move (Velocity * Time.deltaTime);
	}
	
	/// <summary>
	/// The Move function is called every Late Update and tries to move the character to the current position + deltaMovement.
	/// </summary>
	/// <param name="deltaMovement">The difference in movement to add to the current position.</param>
	private void Move(Vector2 deltaMovement)
	{
		// We check if the character is grounded, and save that to WasGroundedLastFrame. This can then be used by other classes.
		State.WasGroundedLastFrame = State.IsCollidingBelow;
		// We reset all collision states (apart from that last one).
		State.Reset();
		
		// if the Corgi Controller is supposed to handle collisions
		if (HandleCollisions)
		{
			// This will handle platforms, especially the moving ones
			HandleMovingPlatforms();
			// We reset the ray origins
			CalculateRayOrigins();
			
			// if the character is grounded and moving on a slope
			if (deltaMovement.y<0 && State.WasGroundedLastFrame)
			{
				MoveDownSlope(ref deltaMovement);
			}
			
			// if the character is moving horizontally
			if (deltaMovement.x != 0)
			{
				MoveHorizontally(ref deltaMovement);
			}
			
			// if the character is moving vertically
			if (deltaMovement.y != 0)
			{
				MoveOnSlopes(ref deltaMovement);
			}
			
			// we correct the horizontal placement (this is used to avoid collision issues with moving platforms only
			CorrectHorizontalPlacement(ref deltaMovement, true);
			CorrectHorizontalPlacement(ref deltaMovement, false);
		}
		
		// we move the actual transform
		_transform.Translate(deltaMovement,Space.World);
		
		// we then calculate the velocity
		if (Time.deltaTime > 0)
		{
			_velocity = deltaMovement / Time.deltaTime;
		}
		
		// we make sure the velocity doesn't exceed the MaxVelocity specified in the parameters
		_velocity.x = Mathf.Min (_velocity.x,Parameters.MaxVelocity.x);
		_velocity.y = Mathf.Min (_velocity.y,Parameters.MaxVelocity.y);
		
		// if the character wasn't grounded last frame, and is now colliding, then it just got grounded.
		if( !State.WasGroundedLastFrame && State.IsCollidingBelow )
		{
			State.JustGotGrounded=true;
		}
		
		// velocity reset when moving up a slope
		if (State.IsMovingUpSlope)
		{
			_velocity.y = 0;
		}		
		
		// this part handles moving platforms
		if (StandingOn != null)
		{
			// we store the current position
			_activeGlobalPlatformPoint = transform.position;
			_activeLocalPlatformPoint = StandingOn.transform.InverseTransformPoint(transform.position);		
			
			// if the character standing state has changed
			if (_lastStandingOn != StandingOn)
			{
				// we send the message that this state has changed
				if (_lastStandingOn != null)
				{
					_lastStandingOn.SendMessage("ControllerExit2D",this,SendMessageOptions.DontRequireReceiver);
				}
					
				StandingOn.SendMessage("ControllerEnter2D",this,SendMessageOptions.DontRequireReceiver);				
				_lastStandingOn = StandingOn;
				
			}	
			else if (StandingOn != null)
			{
				StandingOn.SendMessage("ControllerStay2D",this,SendMessageOptions.DontRequireReceiver);
			}
		}
		else if (_lastStandingOn != null)
		{
			_lastStandingOn.SendMessage("ControllerExit2D",this,SendMessageOptions.DontRequireReceiver);
			_lastStandingOn = null;
		}
		
	}
	
	/// <summary>
	/// Makes the character move if what it's standing on is moving.
	/// </summary>
	private void HandleMovingPlatforms()
	{
		// if the character is standing on something
		if (StandingOn!= null)
		{
			// we get the platform's coordinates
			var newGlobalPlatformPoint = StandingOn.transform.TransformPoint(_activeLocalPlatformPoint);
			// we calculate the distance needed to move the accordingly so it appears to be standing still on the platform
			var moveDistance = newGlobalPlatformPoint - _activeGlobalPlatformPoint;
			// if that distance is not null, we move the character accordingly
			if (moveDistance != Vector3.zero)
				transform.Translate(moveDistance,Space.World);
			// we then calculate the platform velocity for further use
			PlatformVelocity = (newGlobalPlatformPoint - _activeGlobalPlatformPoint) / Time.deltaTime;
		}
		else
		{
			// if the character is standing on nothing, we don't move it
			PlatformVelocity = Vector3.zero;
		}		
		// reset for next frame
		StandingOn = null;
	
	}
	
	/// <summary>
	/// Corrects the horizontal placement when the character is colliding with a moving platform.
	/// </summary>
	/// <param name="deltaMovement">Delta movement.</param>
	/// <param name="isRight">Should the method check to the right ?</param>
	private void CorrectHorizontalPlacement(ref Vector2 deltaMovement, bool isRight)
	{
		// we calculate the half width of the character's collider
		float halfWidth = (_boxCollider.size.x * _localScale.x) / 2f;
		// we determine the rays origin point
		Vector3 rayOrigin = isRight ? _raycastBottomRight : _raycastBottomLeft;
		
		// depending on the direction the character is moving, we refine the ray origin's x
		if (isRight)
		{
			rayOrigin.x -= (halfWidth + SkinWidth);
		}
		else
		{
			rayOrigin.x += (halfWidth + SkinWidth);
		}
			
		// we set the ray's direction depending on the direction we're testing for	
		Vector2 rayDirection = isRight ? Vector2.right : -Vector2.right;
		// offset initialization
		float offset = 0f;
		
		for (int i = 1; i < HorizontalRaysNumber - 1 ; i++)
		{
			Vector2 rayVector = new Vector2(deltaMovement.x + rayOrigin.x,deltaMovement.y + rayOrigin.y + (i * _verticalDistanceBetweenRays));
			//DebugDrawRay(rayVector,rayDirection * halfWidth, isRight ? Color.cyan : Color.magenta);
			
			// We cast a number of horizontal rays to check for a moving platform
			RaycastHit2D raycastHit = Physics2D.Raycast(rayVector,rayDirection,halfWidth,MovingPlatformMask);
			if (!raycastHit)
				continue;
			
			// if a ray hits a moving platform in the direction we're checking for, we calculate the corresponding offset in the opposite direction
			offset = isRight ? ((raycastHit.point.x - _transform.position.x) - halfWidth) : (halfWidth - (_transform.position.x - raycastHit.point.x));
			
		}
		// we add the calculated offset to the delta movement for use in late update
		deltaMovement.x += offset;
		
	}
	
	/// <summary>
	/// Calculates the ray origins based on the box collider's bounds
	/// </summary>
	private void CalculateRayOrigins()
	{
		// bounds for the ray origins are based on the boxcollider's size
		Bounds theBounds = _boxCollider.bounds;
		// we expand the bounds to take the skin width into account
		theBounds.Expand( -SkinWidth );
		// we set the raycast origins for each corner
		_raycastTopLeft = new Vector2( theBounds.min.x, theBounds.max.y );
		_raycastBottomRight = new Vector2( theBounds.max.x, theBounds.min.y );
		_raycastBottomLeft = theBounds.min;
	}
	
	/// <summary>
	/// Adjusts the horizontal delta movement
	/// </summary>
	/// <param name="deltaMovement">Delta movement.</param>
	private void MoveHorizontally(ref Vector2 deltaMovement)
	{
		// if delta movement is positive, we're moving right
		bool movingRight = deltaMovement.x > 0;
		// we determine the ray we're gonna cast length, direction and origin depending on the character's direction
		float rayDistance = Mathf.Abs(deltaMovement.x) + SkinWidth;
		Vector2 rayDirection = movingRight ? Vector2.right : -Vector2.right;
		Vector3 rayOrigin = movingRight ?_raycastBottomRight : _raycastBottomLeft;
		
		// for each horizontal rays
		for (int i=0;i<HorizontalRaysNumber;i++)
		{
			// we create a new vector for our ray based on these dimensions
			Vector2 rayVector = new Vector2(rayOrigin.x, rayOrigin.y + (i * _verticalDistanceBetweenRays));
			// if we're in debug mode, the ray is drawn on screen
			DebugDrawRay(rayVector,rayDirection*rayDistance,Color.red);
			
			RaycastHit2D rayCastHit;
			
			// we cast the ray. if the character wasn't grounded last frame (in the air), we'll ignore one way platforms so the character can pass through them
			if( i == 0 && State.WasGroundedLastFrame )
				rayCastHit = Physics2D.Raycast( rayVector, rayDirection, rayDistance, PlatformMask );
			else
				rayCastHit = Physics2D.Raycast( rayVector, rayDirection, rayDistance, PlatformMask & ~OneWayPlatformMask );			
			
			// if the ray doesnt hit anything, we skip to the next ray
			if (!rayCastHit)
				continue;			
			
			// if the bottom ray hits something while the others don't, we break
			if (i == 0 && MoveUpSlope(ref deltaMovement,Vector2.Angle(rayCastHit.normal,Vector2.up),movingRight))
			{
				break;
			}
			
			// if we've hit something with the ray, we change the delta movement
			deltaMovement.x = rayCastHit.point.x - rayVector.x;
			// new ray distance based on that delta movement.
			rayDistance = Mathf.Abs (deltaMovement.x);
			
			// we adjust the deltamovement based on the skin width
			if (movingRight)
			{
				deltaMovement.x -= SkinWidth;
				State.IsCollidingRight = true;
			}
			else
			{
				deltaMovement.x += SkinWidth;
				State.IsCollidingLeft = true;
			}
			
			// if the ray distance is almost the size of the skin width, we're colliding and break 
			if (rayDistance < SkinWidth + 0.0001f)
			{
				break;
			}
		}
		
	}
	
	/// <summary>
	/// Adjusts the deltamovement when moving on a slope
	/// </summary>
	/// <param name="deltaMovement">Delta movement.</param>
	private void MoveOnSlopes(ref Vector2 deltaMovement)
	{
		// if the deltamovement is positive, we're moving up
		bool movingUp = deltaMovement.y > 0;
		// we calculate the initial ray's distance, direction and origin
		float rayDistance = Mathf.Abs (deltaMovement.y) + SkinWidth;
		Vector2 rayDirection = movingUp ? Vector2.up : - Vector2.up;
		Vector3 rayOrigin = movingUp ? _raycastTopLeft : _raycastBottomLeft;		
		LayerMask mask = PlatformMask;
		
		// we start our raycast from the point we'd be at after the deltaMovement
		rayOrigin.x += deltaMovement.x;
		
		// if we're moving up and if we weren't grounded last frame, we won't check for one way platforms anymore
		if( movingUp && !State.WasGroundedLastFrame )
			mask &= ~OneWayPlatformMask;
						
		
		var standingOnDistance = float.MaxValue;		
		for (var i = 0; i < VerticalRaysNumber; i++)
		{
			// we cast a number of vertical rays
			var rayVector = new Vector2(rayOrigin.x + (i * _horizontalDistanceBetweenRays),rayOrigin.y);
			DebugDrawRay(rayVector,rayDirection*rayDistance,Color.red);
			var rayCastHit = Physics2D.Raycast(rayVector,rayDirection,rayDistance,mask);
			
			// if the ray doesn't hit anything, we leave the loop and skip to the next ray
			if (!rayCastHit)
			{
				continue;
			}
			
			// if we're not moving up, we adjust the standing on distance for moving platforms
			if (!movingUp)
			{
				var verticalDistanceToHit = _transform.position.y - rayCastHit.point.y;
				if (verticalDistanceToHit < standingOnDistance)
				{
					standingOnDistance = verticalDistanceToHit;
					StandingOn = rayCastHit.collider.gameObject;
				}
			}
			
			// we set the new deltamovement based on what the ray hit
			deltaMovement.y = rayCastHit.point.y - rayVector.y;
			// we calculate the next ray distance based on the deltamovement
			rayDistance = Mathf.Abs(deltaMovement.y);
			
			// we add or remove the skin width to the deltamovement depending on the character's direction
			if (movingUp)
			{
				deltaMovement.y -= SkinWidth;
				State.IsCollidingAbove = true;
			}
			else
			{
				deltaMovement.y += SkinWidth;
				State.IsCollidingBelow = true;
			}
			
			// if we reach the top of a slope, we force the "moving up slope" state.
			if (!movingUp && deltaMovement.y > 0.0001f)
			{
				State.IsMovingUpSlope = true;
			}
			
			// if the ray distance is almost the size of the skinwidth, we're colliding and break
			if (rayDistance < SkinWidth + 0.0001f)
			{
				break;
			}
		}
		
	}
	
	/// <summary>
	/// Corrects the delta movement when moving down a slope
	/// </summary>
	/// <param name="deltaMovement">Delta movement.</param>
	private void MoveDownSlope(ref Vector2 deltaMovement)
	{
		// we calculate our character's bottom center
		float center = (_raycastBottomLeft.x + _raycastBottomRight.x) / 2;
		// the direction of the ray is downwards
		Vector2 direction = -Vector2.up;
		
		// we calculate the slope distance 				
		float slopeDistance = _slopeLimitTangent * (_raycastBottomRight.x - center);
		Vector2 slopeRayVector = new Vector2(center, _raycastBottomLeft.y);
		
		// if in debug mode, we draw a ray 
		DebugDrawRay(slopeRayVector,direction*slopeDistance, Color.yellow);
		
		// we cast a ray under our character's center.
		RaycastHit2D raycastHit = Physics2D.Raycast(slopeRayVector,direction,slopeDistance,PlatformMask);
		
		// if we don't hit a slope, we do nothing
		if (!raycastHit)
			return;		
		
		// if the character's direction and the raycast's normal are the same, we're moving down a slope.	
		bool movingDownSlope = Mathf.Sign(raycastHit.normal.x) == Mathf.Sign (deltaMovement.x);
		if (!movingDownSlope)
			return;				
		
		// we calculate the slope's angle		
		float angle = Vector2.Angle(raycastHit.normal,Vector2.up);
		
		// if the angle is to low, we do nothing, it's not a slope
		if (Mathf.Abs (angle) < 0.0001f)
			return;
		
		// if we're going down, we change the speed of our character to speed it up according to the parameter's curve
		float slopeModifier = slopeSpeedMultiplier.Evaluate( -angle );
		deltaMovement.x *= slopeModifier;
		deltaMovement.y = raycastHit.point.y - slopeRayVector.y;
		// we change our states to mention that we're on a slope (and thus colliding below)
		State.IsMovingDownSlope=true;
		State.SlopeAngle=angle;
		
	}
	
	/// <summary>
	/// Corrects the delta movement when moving up a slope.
	/// </summary>
	/// <returns><c>true</c>, if the slope angle is within the parameter's limits, <c>false</c> otherwise.</returns>
	/// <param name="deltaMovement">Delta movement.</param>
	/// <param name="angle">Angle.</param>
	/// <param name="isGoingRight">If set to <c>true</c> is going right.</param>
	private bool MoveUpSlope(ref Vector2 deltaMovement, float slopeAngle, bool movingRight)
	{
		// if the slope's angle is 90°, it's a wall, we do nothing
		if (Mathf.RoundToInt(slopeAngle)==90)
			return false;
		
		// if the slope's angle is above our parameter's slope limit, we don't move
		if (slopeAngle>Parameters.SlopeLimit)
		{
			deltaMovement.x=0;
			return true;
		}
		
		// if we're jumping, we do nothing
		if (deltaMovement.y > 0.07f)
			return true;		
		
		// if we're going up a slope, we slow down our character depending on the slope's angle and the parameter's curve			
		float slopeModifier = slopeSpeedMultiplier.Evaluate( slopeAngle );
		deltaMovement.x *= slopeModifier;
		deltaMovement.x += movingRight ? -SkinWidth : SkinWidth;
		deltaMovement.y = Mathf.Abs(Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * deltaMovement.x);
		// we change our states to mention that we're on a slope (and thus colliding below)
		State.IsMovingUpSlope = true;
		State.IsCollidingBelow = true;
		return true;
	}

	/// <summary>
	/// Disables the collisions for the specified duration
	/// </summary>
	/// <param name="duration">the duration for which the collisions must be disabled</param>
	public IEnumerator DisableCollisions(float duration)
	{
		// we turn the collisions off
		HandleCollisions = false;
		// we wait for a few seconds
		yield return new WaitForSeconds (duration);
		// we turn them on again
		HandleCollisions = true;
	}
	
	// Events
	
	/// <summary>
	/// triggered when the character enters a collider
	/// </summary>
	/// <param name="collider">the object we're colliding with.</param>
	public void OnTriggerEnter2D(Collider2D collider)
	{
		
		CorgiControllerPhysicsVolume2D parameters = collider.gameObject.GetComponent<CorgiControllerPhysicsVolume2D>();
		if (parameters == null)
			return;
		// if the object we're colliding with has parameters, we apply them to our character.
		_overrideParameters = parameters.ControllerParameters;
	}	
	
	/// <summary>
	/// triggered while the character stays inside another collider
	/// </summary>
	/// <param name="collider">the object we're colliding with.</param>
	public void OnTriggerStay2D( Collider2D collider )
	{
	}	
	
	/// <summary>
	/// triggered when the character exits a collider
	/// </summary>
	/// <param name="collider">the object we're colliding with.</param>
	public void OnTriggerExit2D(Collider2D collider)
	{		
		CorgiControllerPhysicsVolume2D parameters = collider.gameObject.GetComponent<CorgiControllerPhysicsVolume2D>();
		if (parameters == null)
			return;
		
		// if the object we were colliding with had parameters, we reset our character's parameters
		_overrideParameters = null;
	}
	
}
