using UnityEngine;
using System.Collections;
/// <summary>
/// Weapon parameters
/// </summary>
public class Weapon : MonoBehaviour 
{
	/// the projectile type the weapon shoots
	public Projectile Projectile;
	/// the firing frequency
	public float FireRate;
	/// the particle system to instantiate every time the weapon shoots
	public ParticleSystem gunFlames;
	/// the shells the weapon emits
	public ParticleSystem gunShells;
}
