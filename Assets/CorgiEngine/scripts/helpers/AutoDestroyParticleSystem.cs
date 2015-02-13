using UnityEngine;
using System.Collections;
/// <summary>
/// Add this class to a ParticleSystem so it auto destroys once it has stopped emitting.
/// Make sure your ParticleSystem isn't looping, otherwise this script will be useless
/// </summary>
public class AutoDestroyParticleSystem : MonoBehaviour 
{
	/// True if the ParticleSystem should also destroy its parent
	public bool DestroyParent=false;
	
	private ParticleSystem _particleSystem;
	
	/// <summary>
	/// Initialization, we get the ParticleSystem component
	/// </summary>
	public void Start()
	{
		_particleSystem = GetComponent<ParticleSystem>();
	}
	
	/// <summary>
	/// When the ParticleSystem stops playing, we destroy it.
	/// </summary>
	public void Update()
	{	
		if (_particleSystem.isPlaying)
			return;
		
		if (transform.parent!=null)
		{
			if(DestroyParent)
			{	
				Destroy(transform.parent.gameObject);	
			}
		}
				
		Destroy (gameObject);
	}

}
