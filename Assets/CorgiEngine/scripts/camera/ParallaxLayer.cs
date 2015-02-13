using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
/// <summary>
/// Add this to a GameObject to have it move in parallax 
/// </summary>
public class ParallaxLayer : MonoBehaviour 
{
	/// horizontal speed of the layer
	public float speedX;
	/// vertical speed of the layer
	public float speedY;
	/// defines if the layer moves in the same direction as the camera or not
	public bool moveInOppositeDirection;

	// private stuff
	private Transform cameraTransform;
	private Vector3 previousCameraPosition;
	private bool previousMoveParallax;
	private ParallaxSetup options;

	/// <summary>
	/// Initialization
	/// </summary>
	void OnEnable() 
	{
		GameObject gameCamera = GameObject.Find("Main Camera");
		options = gameCamera.GetComponent<ParallaxSetup>();
		cameraTransform = gameCamera.transform;
		previousCameraPosition = cameraTransform.position;
	}

	/// <summary>
	/// Every frame, we move the parallax layer according to the camera's position
	/// </summary>
	void Update () 
	{
		if(options.moveParallax && !previousMoveParallax)
			previousCameraPosition = cameraTransform.position;

		previousMoveParallax = options.moveParallax;

		if(!Application.isPlaying && !options.moveParallax)
			return;

		Vector3 distance = cameraTransform.position - previousCameraPosition;
		float direction = (moveInOppositeDirection) ? -1f : 1f;
		transform.position += Vector3.Scale(distance, new Vector3(speedX, speedY)) * direction;

		previousCameraPosition = cameraTransform.position;
	}
}
