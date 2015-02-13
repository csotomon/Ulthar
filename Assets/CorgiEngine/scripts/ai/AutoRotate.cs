using UnityEngine;
using System.Collections;
/// <summary>
/// Add this class to a GameObject to make it rotate on itself
/// </summary>
public class AutoRotate : MonoBehaviour 
{
	/// The rotation speed. Positive means clockwise, negative means counter clockwise.
	public float rotationSpeed = 100f;

	/// <summary>
	/// Makes the object rotate on its center every frame.
	/// </summary>
	void Update () 
	{
		transform.Rotate(rotationSpeed*Vector3.forward*Time.deltaTime);
	}
}
