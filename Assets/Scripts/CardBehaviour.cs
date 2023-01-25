using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(CardDisplay))]
public class CardBehaviour : NetworkBehaviour
{
    public CardScriptable cardData;

    [SyncVar]
    public int health;

    // to be used
    [SyncVar]
    public bool isDeployed;
    [SyncVar]
    public PlayerManager owner;

    [Header("Display")]
    CardDisplay display;

    // Start is called before the first frame update
    void OnEnable()
    {
        if (isServer) { health = cardData.maxHealth; }
        display = GetComponent<CardDisplay>();
    }

    public void SetDisplay(bool enabled)
    {
        display.SetDisplay(enabled);
    }

    public void SetCardBack(bool enabled)
    {
        display.SetCardBack(enabled);
    }

    public void OnSelect()
    {
        display.SetYShift(20f);
    }

    public void OnDeselect()
    {
        display.SetYShift(-20f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCardClick()
    {
        EventDispatcher.Instance.SendEvent(6, new CardClickEvent(this));
    }
}
