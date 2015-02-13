/// <summary>
/// Interface for player respawn
/// </summary>
public interface IPlayerRespawnListener
{
	void onPlayerRespawnInThisCheckpoint(CheckPoint checkpoint, CharacterBehavior player);
}