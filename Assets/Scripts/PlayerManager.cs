using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerManager : NetworkBehaviour
{
    public readonly SyncList<CardBehaviour> playerHand = new SyncList<CardBehaviour>();
    public readonly SyncList<MinionCardBehaviour> deployedMinions = new SyncList<MinionCardBehaviour>();

    [SyncVar]
    public CardBehaviour selectedCard = null;

    public override void OnStartClient()
    {
        base.OnStartClient();

        // Bind events
        EventDispatcher.Instance.AddEventHandler<CardClickEvent>(6, OnCardClick);
        EventDispatcher.Instance.AddEventHandler<FieldSelectEvent>(7, OnFieldSelect);
    }

    // TODO: change all cardids to cardbehaviours
    [Command]
    public void CmdRequestDeployMinion(CardBehaviour card, int location)
    {
        // TODO: run checks on if the deployment is valid: is card minion, is location valid, does player have card
        if (card.data.category == CardCategory.Minion
            && location >= 0 && location <= deployedMinions.Count
            && card.owner == this)
        {
            ServerDeployMinion(card.data.type, location);
            playerHand.Remove(card);
            NetworkServer.Destroy(card.gameObject);
        }
    }

    // Might ptimize later so that not the entire handcard class is sent over network
    [Server]
    public void ServerDeployMinion(CardType card, int location)
    {
        Debug.Log(card);
        var newMinionPrefab = CardScriptable.LoadCardFromDisk(card);
        if (!newMinionPrefab) { Debug.Log("Card is not minion"); return; }
        var newMinion = Instantiate(newMinionPrefab);
        newMinion.isDeployed = true;
        newMinion.SetDisplay(false);
        newMinion.owner = this;
        NetworkServer.Spawn(newMinion.gameObject);
        deployedMinions.Insert(location, newMinion.GetComponent<MinionCardBehaviour>());
        RpcDeployMinion(newMinion, location);
    }

    [ClientRpc]
    public void RpcDeployMinion(CardBehaviour newMinion, int location)
    {
        EventDispatcher.Instance.SendEvent(5, new MinionDeployEvent(!isLocalPlayer, newMinion, location));
    }

    // test function
    [Command]
    public void CmdAddCard(CardType card)
    {
        var newCardPrefab = CardScriptable.LoadCardFromDisk(card);
        var newCard = Instantiate(newCardPrefab);
        newCard.isDeployed = false;
        newCard.SetDisplay(false);
        newCard.owner = this;
        NetworkServer.Spawn(newCard.gameObject);
        playerHand.Add(newCard);
        RpcUpdateHand();
    }

    [ClientRpc]
    public void RpcUpdateHand()
    {
        var newHand = new List<CardBehaviour>(playerHand);
        EventDispatcher.Instance.SendEvent(4, new HandChangeEvent(!isLocalPlayer, newHand));
    }

    // Event handlers
    void OnCardClick(short type, CardClickEvent eventData)
    {
        if (eventData.Card.isDeployed)
        {

        }
        else
        {
            if (!selectedCard)
            {
                selectedCard = eventData.Card;
                selectedCard.OnSelect();
            }
            else if (selectedCard != eventData.Card)
            {
                selectedCard.OnDeselect();
                selectedCard = eventData.Card;
                selectedCard.OnSelect();
            }
            else if (selectedCard == eventData.Card)
            {
                selectedCard.OnDeselect();
                selectedCard = null;
            }
        }
    }

    void OnFieldSelect(short type, FieldSelectEvent eventData)
    {
        if (selectedCard)
        {
            // if card is minion
            if (selectedCard.data.category == CardCategory.Minion)
            {
                Debug.Log("Deploy minion");
                CmdRequestDeployMinion(selectedCard, 0);
                selectedCard = null;
            }

            // if card is action

            // if card is item
        }
        selectedCard = null;
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
            CmdAddCard(CardType.BasicMinion);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            //CmdRequestDeployMinion(CardType.BasicMinion, 0);
        }
    }
}