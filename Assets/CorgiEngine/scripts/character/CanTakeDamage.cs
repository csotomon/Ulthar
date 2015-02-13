using UnityEngine;
using System.Collections;

/// <summary>
/// Public interface for objects that can take damage
/// </summary>

public interface CanTakeDamage
{

	void TakeDamage(int damage,GameObject instigator);
}
