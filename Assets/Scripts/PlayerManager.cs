using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerManager : NetworkBehaviour
{
    public readonly SyncList<CardId> playerHand = new SyncList<CardId>();
    public readonly SyncList<MinionCardBehaviour> deployedMinions = new SyncList<MinionCardBehaviour>();

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (isLocalPlayer) { return; }

        // if not local player then its the enemy

        // Bind events
    }

    // Might ptimize later so that not the entire handcard class is sent over network
    [Command]
    public void CmdDeployMinion(CardId card, int location)
    {
        var newMinionPrefab = CardScriptable.LoadMinionFromDisk(card);
        if (!newMinionPrefab) { Debug.Log("Card is not minion"); return; }
        var newMinion = Instantiate(newMinionPrefab);
        NetworkServer.Spawn(newMinion.gameObject);
        deployedMinions.Insert(location, newMinion);
        RpcDeployMinion(newMinion.gameObject, location);
    }

    [ClientRpc]
    public void RpcDeployMinion(GameObject newMinion, int location)
    {
        EventDispatcher.Instance.SendEvent(5, new MinionDeployEvent(!isLocalPlayer, newMinion, location));
    }

    // test function
    [Command]
    public void CmdAddCard(CardId card)
    {
        playerHand.Add(card);
        RpcUpdateHand();
    }

    [ClientRpc]
    public void RpcUpdateHand()
    {
        var newHand = new List<CardId>(playerHand);
        EventDispatcher.Instance.SendEvent(4, new HandChangeEvent(!isLocalPlayer, newHand));
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // test button press
        if (!isLocalPlayer) { return; }
        if (Input.GetKeyDown(KeyCode.C))
        {
            CmdAddCard(CardId.BasicMinion);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            CmdDeployMinion(CardId.BasicMinion, 0);
        }
    }
}