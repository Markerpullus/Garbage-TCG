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
    [SyncVar(hook = nameof(OnEnergyChange))]
    public int energy;

    public override void OnStartClient()
    {
        base.OnStartClient();

        // Bind events
        if (!isLocalPlayer) { return; }
        EventDispatcher.Instance.AddEventHandler<CardClickEvent>(6, OnCardClick);
        EventDispatcher.Instance.AddEventHandler<FieldSelectEvent>(7, OnFieldSelect);
    }

    // temporary: energy will be set by game manager based on turns
    public override void OnStartServer()
    {
        base.OnStartServer();

        energy = 10;
    }

    // TODO: change all cardids to cardbehaviours
    [Command]
    public void CmdRequestDeployMinion(CardBehaviour card, int location)
    {
        // run checks on if the deployment is valid: is card minion, is location valid, does player have card, if energy is enough
        if (card.data.category == CardCategory.Minion
            && location >= 0 && location <= deployedMinions.Count
            && card.owner == this
            && energy >= CardScriptable.LoadCardDataFromDisk(card.data.type).energy)
        {
            ServerDeployMinion(card.data.type, location);
            playerHand.Remove(card);
            NetworkServer.Destroy(card.gameObject);

            return;
        }
        // if deployment not valid, deselect card
        card.RpcOnDeselect();
    }

    // Might ptimize later so that not the entire handcard class is sent over network
    [Server]
    public void ServerDeployMinion(CardType card, int location)
    {
        // Spawns minion
        var newMinionPrefab = CardScriptable.LoadCardFromDisk(card);
        if (!newMinionPrefab) { Debug.Log("Card is not minion"); return; }
        var newMinion = Instantiate(newMinionPrefab);
        newMinion.isDeployed = true;
        newMinion.SetDisplay(false);
        newMinion.owner = this;
        NetworkServer.Spawn(newMinion.gameObject);
        deployedMinions.Insert(location, newMinion.GetComponent<MinionCardBehaviour>());
        RpcDeployMinion(newMinion, location);

        // decrease energy
        energy -= newMinion.data.energy;
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
    void OnEnergyChange(int oldVal, int newVal)
    {
        EventDispatcher.Instance.SendEvent(8, new EnergyChangeEvent(newVal));
    }

    void OnCardClick(short type, CardClickEvent eventData)
    {
        if (eventData.Card.isDeployed)
        {

        }
        else if (!eventData.Card.IsEnemy())
        {
            if (!selectedCard)
            {
                selectedCard = eventData.Card;
                selectedCard.CmdOnSelect();
            }
            else if (selectedCard != eventData.Card)
            {
                selectedCard.CmdOnDeselect();
                selectedCard = eventData.Card;
                selectedCard.CmdOnSelect();
            }
            else if (selectedCard == eventData.Card)
            {
                selectedCard.CmdOnDeselect();
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
                CmdRequestDeployMinion(selectedCard, 0);
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