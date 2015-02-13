using UnityEngine;
using UnityEngine.UI;
using System.Collections;
/// <summary>
/// Handles all GUI effects and changes
/// </summary>
public class GUIManager : MonoBehaviour 
{
	/// the game object that contains the heads up display (avatar, health, points...)
	public GameObject HUD;
	/// the pause screen game object
	public GameObject PauseScreen;	
	/// the time splash gameobject
	public GameObject TimeSplash;
	/// the points counter
	public Text PointsText;
	/// the screen used for all fades
	public Image Fader;
	
	private static GUIManager _instance;
	
	// Singleton pattern
	public static GUIManager Instance
	{
		get
		{
			if(_instance == null)
				_instance = GameObject.FindObjectOfType<GUIManager>();
			return _instance;
		}
	}

	/// <summary>
	/// Initialization
	/// </summary>
	public void Start()
	{
		PointsText.text="$"+GameManager.Instance.Points;	
	}
	
	/// <summary>
	/// Sets the HUD active or inactive
	/// </summary>
	/// <param name="state">If set to <c>true</c> turns the HUD active, turns it off otherwise.</param>
	public void SetHUDActive(bool state)
	{
		HUD.SetActive(state);
	}

	/// <summary>
	/// Sets the pause.
	/// </summary>
	/// <param name="state">If set to <c>true</c>, sets the pause.</param>
	public void SetPause(bool state)
	{
		PauseScreen.SetActive(state);
	}

	/// <summary>
	/// Sets the time splash.
	/// </summary>
	/// <param name="state">If set to <c>true</c>, turns the timesplash on.</param>
	public void SetTimeSplash(bool state)
	{
		TimeSplash.SetActive(state);
	}

	/// <summary>
	/// Sets the text to the game manager's points.
	/// </summary>
	public void RefreshPoints()
	{
		PointsText.text="$"+GameManager.Instance.Points;		
	}
	
	/// <summary>
	/// Fades the fader in or out depending on the state
	/// </summary>
	/// <param name="state">If set to <c>true</c> fades the fader in, otherwise out if <c>false</c>.</param>
	public void FaderOn(bool state,float duration)
	{
		Fader.gameObject.SetActive(true);
		if (state)
			StartCoroutine(FadeTo(Fader,1f, duration));
		else
			StartCoroutine(FadeTo(Fader,0f, duration));
	}
	
	IEnumerator FadeTo(Image target, float opacity, float duration)
	{
		//float alpha = target.renderer.material.color.a;
		float alpha = target.color.a;
		
		for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / duration)
		{
			Color newColor = new Color(0, 0, 0, Mathf.SmoothStep(alpha,opacity,t));
			target.color=newColor;
			//target.renderer.material.color = newColor;
			yield return null;
		}
	}
}
