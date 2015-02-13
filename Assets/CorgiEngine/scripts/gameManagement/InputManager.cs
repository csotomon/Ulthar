using UnityEngine;
using System.Collections;
using UnitySampleAssets.CrossPlatformInput;
/// <summary>
/// This persistent singleton handles the inputs and sends commands to the player
/// </summary>
public class InputManager : PersistentSingleton<InputManager>
{
		
	private static CharacterBehavior _player;
	
	/// <summary>
	/// We get the player from its tag.
	/// </summary>
	void Start()
	{
		if (GameObject.FindGameObjectWithTag("Player")!=null)
			_player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBehavior>();		
	}
	
	/// <summary>
	/// At update, we check the various commands and send them to the player.
	/// </summary>
	void Update()
	{		
		// if we can't get the player, we do nothing
		if (_player == null) 
		{
			if (GameObject.FindGameObjectWithTag("Player")!=null)
			{
				if (GameObject.FindGameObjectWithTag ("Player").GetComponent<CharacterBehavior> () != null)
					_player = GameObject.FindGameObjectWithTag ("Player").GetComponent<CharacterBehavior> ();
			}
			else
				return;
		}
		
		if ( CrossPlatformInputManager.GetButtonDown("Pause") )
			GameManager.Instance.Pause();
			
		if (GameManager.Instance.Paused)
			return;
			
		_player.SetHorizontalMove(CrossPlatformInputManager.GetAxis ("Horizontal"));
		_player.SetVerticalMove(CrossPlatformInputManager.GetAxis ("Vertical"));	
		
		if ((CrossPlatformInputManager.GetButtonDown("Run")||CrossPlatformInputManager.GetButton("Run")) )
			_player.RunStart();		
		
		if (CrossPlatformInputManager.GetButtonUp("Run"))
			_player.RunStop();		
				
		if (CrossPlatformInputManager.GetButtonDown("Jump"))
			_player.JumpStart();
						
		if (CrossPlatformInputManager.GetButtonUp("Jump"))
			_player.JumpStop();
				
		if ((CrossPlatformInputManager.GetButtonDown("Jetpack")||CrossPlatformInputManager.GetButton("Jetpack")) )
			_player.JetpackStart();
				
		if (CrossPlatformInputManager.GetButtonUp("Jetpack"))
			_player.JetpackStop();
				
		if ( CrossPlatformInputManager.GetButtonDown("Dash") )
			_player.Dash();
				
		if ( CrossPlatformInputManager.GetButtonDown("Melee")  )
			_player.Melee();
				
		if (CrossPlatformInputManager.GetButtonDown("Fire"))
			_player.ShootOnce();			
				
		if (CrossPlatformInputManager.GetButton("Fire")) 
			_player.ShootStart();
				
		if (CrossPlatformInputManager.GetButtonUp("Fire"))
			_player.ShootStop();
	}	
}
