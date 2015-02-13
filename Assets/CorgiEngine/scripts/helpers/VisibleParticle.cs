using UnityEngine;
using System.Collections;
/// <summary>
/// Adds this class to particles to force their sorting layer
/// </summary>
public class VisibleParticle : MonoBehaviour {

	/// <summary>
	/// Sets the particle system's renderer to the Visible Particles sorting layer
	/// </summary>
	void Start () 
	{
		particleSystem.renderer.sortingLayerName = "VisibleParticles";
	}
	
}
