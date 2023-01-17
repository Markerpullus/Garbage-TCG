using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MyNetworkManager : NetworkManager
{
    public GameManager gameManagerReference;

    public override void OnStartServer()
    {
        base.OnStartServer();

        var gameManager = Instantiate(gameManagerReference);
        NetworkServer.Spawn(gameManager.gameObject);
    }
}
