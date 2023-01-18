using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MyNetworkManager : NetworkManager
{
    public GameManager gameManagerPrefab;

    public override void OnStartServer()
    {
        base.OnStartServer();

        var gameManager = Instantiate(gameManagerPrefab);
        NetworkServer.Spawn(gameManager.gameObject);
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);

    }
}
