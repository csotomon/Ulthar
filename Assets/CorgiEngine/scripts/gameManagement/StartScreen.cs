using UnityEngine;
using System.Collections;
using UnitySampleAssets.CrossPlatformInput;
/// <summary>
/// Simple start screen class.
/// </summary>
public class StartScreen : MonoBehaviour 
{
	public string FirstLevel;
	
	private float _delayAfterClick=1f;
	
	/// <summary>
	/// Initialization
	/// </summary>
	void Start()
	{	
		GUIManager.Instance.SetHUDActive(false);
		GUIManager.Instance.FaderOn(false,1f);
	}
	
	/// <summary>
	/// During update we simply wait for the user to press the "jump" button.
	/// </summary>
	void Update () 
	{
		if (!CrossPlatformInputManager.GetButtonDown("Jump"))
			return;
		
		GUIManager.Instance.FaderOn(true,_delayAfterClick);
		// if the user presses the "Jump" button, we start the first level.
		StartCoroutine(LoadFirstLevel());
	}
	
	IEnumerator LoadFirstLevel()
	{
		yield return new WaitForSeconds(_delayAfterClick);
		Application.LoadLevel(FirstLevel);	
	}
	
	
}
