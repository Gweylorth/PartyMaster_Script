using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager
{
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        base.OnServerAddPlayer(conn, playerControllerId);
        if (numPlayers == maxConnections)
        {
            foreach (var playerController in conn.playerControllers)
            {
                playerController.gameObject.GetComponent<PlayerController>().RpcStartGame();
            }

            gameObject.GetComponent<NetworkDiscovery>().StopBroadcast();
        }
    }
}
