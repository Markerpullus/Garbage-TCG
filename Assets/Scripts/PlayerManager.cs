using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerManager : NetworkBehaviour
{
    public readonly SyncList<CardId> playerHand = new SyncList<CardId>();
    public readonly SyncList<MinionCardBehaviour> deployedMinions = new SyncList<MinionCardBehaviour>();

    [SerializeField]
    CardBehaviour selectedCard = null;

    public override void OnStartClient()
    {
        base.OnStartClient();

        // Bind events
        EventDispatcher.Instance.AddEventHandler<CardClickEvent>(6, OnCardClick);
        EventDispatcher.Instance.AddEventHandler<FieldSelectEvent>(7, OnFieldSelect);
    }

    [Command]
    public void CmdRequestDeployMinion(CardId card, int location)
    {
        // TODO: run checks on if the deployment is valid
        ServerDeployMinion(card, location);
    }

    // Might ptimize later so that not the entire handcard class is sent over network
    [Server]
    public void ServerDeployMinion(CardId card, int location)
    {
        var newMinionPrefab = CardScriptable.LoadMinionFromDisk(card);
        if (!newMinionPrefab) { Debug.Log("Card is not minion"); return; }
        var newMinion = Instantiate(newMinionPrefab);
        newMinion.transform.localScale = new Vector3(0, 0, 0);
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

    // Event handlers
    void OnCardClick(short type, CardClickEvent eventData)
    {
        selectedCard = eventData.Card;
    }

    void OnFieldSelect(short type, FieldSelectEvent eventData)
    {
        if (selectedCard)
        {
            // if card is minion
            if (selectedCard.cardData.cardType == CardType.Minion)
            {
                Debug.Log("Deploy minion");
                
            }

            // if card is action

            // if card is item
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
        if (!isLocalPlayer) { return; }
        if (Input.GetKeyDown(KeyCode.C))
        {
            CmdAddCard(CardId.BasicMinion);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            CmdRequestDeployMinion(CardId.BasicMinion, 0);
        }
    }
}