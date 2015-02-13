using UnityEngine;
using System.Collections;
/// <summary>
/// Add this component to a CorgiController2D and it will try to kill your player on sight.
/// </summary>
public class SimpleEnemyAi : MonoBehaviour,IPlayerRespawnListener
{
	/// The speed of the agent
	public float Speed;
	/// The fire rate (in seconds)
	public float FireRate = 1;
	/// The kind of projectile shot by the agent
	public Projectile Projectile;

	// private stuff
	private CorgiController2D _controller;
	private Vector2 _direction;
	private Vector2 _startPosition;
	private float _canFireIn;
	
	/// <summary>
	/// Initialization
	/// </summary>
	public void Start () 
	{
		// we get the CorgiController2D component
		_controller = GetComponent<CorgiController2D>();
		// initialize the direction and start position
		_direction = new Vector2(-1,0);
		_startPosition = transform.position;
	}
	
	/// <summary>
	/// Every frame, moves the agent and checks if it can shoot at the player.
	/// </summary>
	public void Update () 
	{
		// moves the agent in its current direction
		_controller.SetHorizontalForce(_direction.x * Speed);
				
		// if the agent is colliding with something, make it turn around
		if ((_direction.x < 0 && _controller.State.IsCollidingLeft) || (_direction.x > 0 && _controller.State.IsCollidingRight))
		{
			_direction = -_direction;
			transform.localScale = new Vector3(-transform.localScale.x,transform.localScale.y,transform.localScale.z);
		}
		
		// fire cooldown
		if ((_canFireIn-=Time.deltaTime) > 0)
		{
			return;
		}
		
		// we cast a ray in front of the agent to check for a Player
		var raycastOrigin = new Vector2(transform.position.x,transform.position.y-(transform.localScale.y/2));
		var raycast = Physics2D.Raycast(raycastOrigin,_direction,10,1<<LayerMask.NameToLayer("Player"));
		if (!raycast)
			return;
		
		// if the ray has hit the player, we fire a projectile
		var projectile = (Projectile)Instantiate(Projectile, transform.position,transform.rotation);
		projectile.Initialize(gameObject,_direction,_controller.Velocity);
		_canFireIn=FireRate;
	}

	/// <summary>
	/// When the player respawns, we reinstate this agent.
	/// </summary>
	/// <param name="checkpoint">Checkpoint.</param>
	/// <param name="player">Player.</param>
	public void onPlayerRespawnInThisCheckpoint (CheckPoint checkpoint, CharacterBehavior player)
	{
		_direction = new Vector2(-1,0);
		transform.localScale=new Vector3(1,1,1);
		transform.position=_startPosition;
		gameObject.SetActive(true);
	}

}
