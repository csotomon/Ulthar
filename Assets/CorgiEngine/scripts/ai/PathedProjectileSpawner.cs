using UnityEngine;
using System.Collections;
/// <summary>
/// Spawns pathed projectiles
/// </summary>
public class PathedProjectileSpawner : MonoBehaviour 
{
	/// the pathed projectile's destination
	public Transform Destination;
	/// the projectiles to spawn
	public PathedProjectile Projectile;
	/// the effect to instantiate at each spawn
	public GameObject SpawnEffect;
	/// the speed of the projectiles
	public float Speed;
	/// the frequency of the spawns
	public float FireRate;
	
	private float _nextShotInSeconds;
	
	/// <summary>
	/// Initialization
	/// </summary>
	void Start () 
	{
		_nextShotInSeconds=FireRate;
	}

	/// <summary>
	/// Every frame, we check if we need to instantiate a new projectile
	/// </summary>
	void Update () 
	{
		if((_nextShotInSeconds -= Time.deltaTime)>0)
			return;
			
		_nextShotInSeconds = FireRate;
		var projectile = (PathedProjectile) Instantiate(Projectile, transform.position,transform.rotation);
		projectile.Initialize(Destination,Speed);
		
		if (SpawnEffect!=null)
		{
			Instantiate(SpawnEffect,transform.position,transform.rotation);
		}
	}

	/// <summary>
	/// Debug mode
	/// </summary>
	public void OnDrawGizmos()
	{
		if (Destination==null)
			return;
		
		Gizmos.color=Color.red;
		Gizmos.DrawLine(transform.position,Destination.position);
	}
}
