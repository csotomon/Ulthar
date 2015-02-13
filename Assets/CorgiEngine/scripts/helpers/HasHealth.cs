using UnityEngine;
using System.Collections;
/// <summary>
/// Adds this class to an object so it can have health (and lose it)
/// </summary>
public class HasHealth : MonoBehaviour, CanTakeDamage
{
	/// the current amount of health of the object
	public int Health;
	/// the points the player gets when the object's health reaches zero
	public int PointsWhenDestroyed;
	/// the effect to instantiate when the object takes damage
	public GameObject HurtEffect;
	/// the effect to instantiate when the object gets destroyed
	public GameObject DestroyEffect;

	/// <summary>
	/// What happens when the object takes damage
	/// </summary>
	/// <param name="damage">Damage.</param>
	/// <param name="instigator">Instigator.</param>
	public void TakeDamage(int damage,GameObject instigator)
	{	
		// when the object takes damage, we instantiate its hurt effect
		Instantiate(HurtEffect,instigator.transform.position,transform.rotation);
		// and remove the specified amount of health	
		Health -= damage;
		// if the object doesn't have health anymore, we destroy it
		if (Health<=0)
		{
			DestroyObject();
		}
	}

	/// <summary>
	/// Destroys the object
	/// </summary>
	private void DestroyObject()
	{
		// instantiates the destroy effect
		if (DestroyEffect!=null)
		{
			Instantiate(DestroyEffect,transform.position,transform.rotation);
		}
		// Adds points if needed.
		if(PointsWhenDestroyed != 0)
		{
			GameManager.Instance.AddPoints(PointsWhenDestroyed);
		}
		// destroys the object
		Destroy(gameObject);
	}
}
