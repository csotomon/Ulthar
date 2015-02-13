using UnityEngine;
using System.Collections;
/// <summary>
/// The Corgi Engine's Camera Controller. Handles camera movement, shakes, player follow.
/// </summary>
public class CameraController : MonoBehaviour 
{
	/// How far ahead from the Player the camera is supposed to be		
	public float AheadFactor = 3;
	/// How fast the camera goes back to the Player
	public float ReturnSpeed = 0.5f;
	/// Camera threshold
	public float Threshold = 0.1f;
	/// How fast the camera slows down
	public float SlowFactor = 0.3f;
	/// True if the camera should follow the player
	public bool FollowsPlayer{get;set;}
	
	// Private variables
	
	private Transform _target;
	private LevelLimits _levelBounds;
	
	private float xMin;
	private float xMax;
	private float yMin;
	private float yMax;	
	
	private float offsetZ;
	private Vector3 lastTargetPosition;
	private Vector3 currentVelocity;
	private Vector3 lookAheadPos;
	
	private float shakeIntensity;
	private float shakeDecay;
	private float shakeDuration;
	
	/// <summary>
	/// Initialization
	/// </summary>
	void Start ()
	{		
		// We make the camera follow the player
		FollowsPlayer=true;
		
		// player and level bounds initialization
		_target = GameObject.FindGameObjectWithTag("Player").transform;
		_levelBounds = GameObject.FindGameObjectWithTag("LevelBounds").GetComponent<LevelLimits>();		
		
		// we store the target's last position
		lastTargetPosition = _target.position;
		offsetZ = (transform.position - _target.position).z;
		transform.parent = null;
		
		// camera size calculation (orthographicSize is half the height of what the camera sees.
		float cameraHeight = Camera.main.orthographicSize * 2f;		
		float cameraWidth = cameraHeight * Camera.main.aspect;
			
		// we get the levelbounds coordinates to lock the camera into the level
		xMin = _levelBounds.LeftLimit+(cameraWidth/2);
		xMax = _levelBounds.RightLimit-(cameraWidth/2); 
		yMin = _levelBounds.BottomLimit+(cameraHeight/2); 
		yMax = _levelBounds.TopLimit-(cameraHeight/2);		
	}
	
	/// <summary>
	/// Every frame, we move the camera if needed
	/// </summary>
	void LateUpdate () 
	{
		// if the camera is not supposed to follow the player, we do nothing
		if (!FollowsPlayer)
			return;
			
		// if the player has moved since last update
		float xMoveDelta = (_target.position - lastTargetPosition).x;
		
		bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > Threshold;
		
		if (updateLookAheadTarget) 
		{
			lookAheadPos = AheadFactor * Vector3.right * Mathf.Sign(xMoveDelta);
		} 
		else 
		{
			lookAheadPos = Vector3.MoveTowards(lookAheadPos, Vector3.zero, Time.deltaTime * ReturnSpeed);	
		}
		
		Vector3 aheadTargetPos = _target.position + lookAheadPos + Vector3.forward * offsetZ;
		Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref currentVelocity, SlowFactor);
		
		
		Vector3 shakeFactorPosition = new Vector3(0,0,0);
		
		// If shakeDuration is still running.
		if (shakeDuration>0)
		{
			shakeFactorPosition= Random.insideUnitSphere * shakeIntensity * shakeDuration;
			shakeDuration-=shakeDecay*Time.deltaTime ;
		}
		
		newPos = newPos+shakeFactorPosition;		
		
		// Level boundaries
		float posX = Mathf.Clamp(newPos.x, xMin, xMax);
		float posY = Mathf.Clamp(newPos.y, yMin, yMax);
		float posZ = newPos.z;
		
		//create new position - within set boundaries
		newPos = new Vector3(posX, posY, posZ);
		
		transform.position=newPos;
		
		lastTargetPosition = _target.position;		
	}
	
	/// <summary>
	/// Use this method to shake the camera, passing in a Vector3 for intensity, duration and decay
	/// </summary>
	/// <param name="shakeParameters">Shake parameters : intensity, duration and decay.</param>
	public void Shake(Vector3 shakeParameters)
	{
		shakeIntensity = shakeParameters.x;
		shakeDuration=shakeParameters.y;
		shakeDecay=shakeParameters.z;
	}
}