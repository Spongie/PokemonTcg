using NetworkingCore;

public interface IPlayerService
{
	int Login(string username, NetworkId playerId);
}