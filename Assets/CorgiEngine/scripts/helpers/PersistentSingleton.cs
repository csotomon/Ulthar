﻿using UnityEngine;

/// <summary>
/// Persistent singleton.
/// </summary>
public class PersistentSingleton<T> : MonoBehaviour	where T : Component
{
	private static T _instance;
	
	/// <summary>
	/// Singleton design pattern
	/// </summary>
	/// <value>The instance.</value>
	public static T Instance 
	{
		get 
		{
			if (_instance == null) 
			{
				_instance = FindObjectOfType<T> ();
				if (_instance == null) 
				{
					GameObject obj = new GameObject ();
					obj.hideFlags = HideFlags.HideAndDontSave;
					_instance = obj.AddComponent<T> ();
				}
			}
			return _instance;
		}
	}
	
	/// <summary>
	/// On awake, we check if there's already a copy of the object in the scene. If there's one, we destroy it.
	/// </summary>
	public virtual void Awake ()
	{
		//Debug.Log ("dont destroy : "+this);
		DontDestroyOnLoad (this.gameObject);
		if (_instance == null) 
		{
			_instance = this as T;
		} 
		else 
		{
			Destroy (gameObject);
		}
	}
}