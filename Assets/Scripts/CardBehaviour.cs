using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

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

    [Command(requiresAuthority = false)]
    public void CmdOnSelect()
    {
        RpcOnSelect();
    }

    [ClientRpc]
    public void RpcOnSelect()
    {
        display.SetYShift(20f - 40f * Convert.ToInt32(IsEnemy()));
    }

    [Command(requiresAuthority = false)]
    public void CmdOnDeselect()
    {
        RpcOnDeselect();
    }

    [ClientRpc]
    public void RpcOnDeselect()
    {
        display.SetYShift(-20f + 40f * Convert.ToInt32(IsEnemy()));
    }

    public bool IsEnemy()
    {
        return (owner != NetworkClient.localPlayer.GetComponent<PlayerManager>());
    }

    public void OnCardClick()
    {
        EventDispatcher.Instance.SendEvent(6, new CardClickEvent(this));
    }
}
