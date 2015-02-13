using UnityEngine;
using System.Collections;

/// <summary>
/// Add this class to a camera to have it support parallax layers
/// </summary>
public class ParallaxSetup : MonoBehaviour 
{
	
	public bool moveParallax;

	[SerializeField]
	[HideInInspector]
	private Vector3 storedPosition;

	public void SavePosition() 
	{
		storedPosition = transform.position;
	}

	public void RestorePosition() 
	{
		transform.position = storedPosition;
	}
}