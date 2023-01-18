using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerManager : NetworkBehaviour
{
    public readonly SyncList<CardId> playerHand = new SyncList<CardId>();

    public CardDisplay cardDisplay;

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (isLocalPlayer) { return; }

        // if not local player then its the enemy

        // Bind events
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
        if (isLocalPlayer)
        {
            EventDispatcher.Instance.SendEvent(4, new HandChangeEvent(false, newHand));
        }
        else
        {
            // draw only the card backs at enemycardspawn
            EventDispatcher.Instance.SendEvent(4, new HandChangeEvent(true, newHand));
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // test button press
        if (Input.GetKeyDown(KeyCode.C))
        {
            CmdAddCard(CardId.BasicMinion);
        }
    }
}