using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CardBehaviour : NetworkBehaviour
{
    public CardScriptable data;

    // to be used
    [SyncVar]
    public bool isDeployed = false;
    [SyncVar]
    public PlayerManager owner;

    [Header("Display")]
    CardDisplay display;

    void OnEnable()
    {
        if (!TryGetComponent(out display))
        {
            Debug.Log("No display attached to this card");
        }
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
