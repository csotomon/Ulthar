using UnityEngine;
using System.Collections;
/// <summary>
/// Add this class to a collectible to have the player change weapon when collecting it
/// </summary>
public class WeaponUpgrade : MonoBehaviour, IPlayerRespawnListener
{
	/// the effect to instantiate when collected
	public GameObject Effect;
	/// the new weapon the player gets when collecting this object
	public Weapon WeaponToGive;

	/// <summary>
	/// Triggered when something collides with the object
	/// </summary>
	/// <param name="collider">Other collider.</param>
	public void OnTriggerEnter2D (Collider2D collider) 
	{
		// 
		if (collider.GetComponent<CharacterBehavior>() == null)
		{
			return;
		}		
		// adds an instance of the effect at the coin's position
		Instantiate(Effect,transform.position,transform.rotation);
		
		collider.GetComponent<CharacterBehavior>().ChangeWeapon(WeaponToGive);
		
		gameObject.SetActive(false);
	}
	public void onPlayerRespawnInThisCheckpoint(CheckPoint checkpoint, CharacterBehavior player)
	{
		gameObject.SetActive(true);
	}
}
