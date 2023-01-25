using System;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(CardBehaviour))]
public class MinionCardBehaviour : NetworkBehaviour
{
    [SyncVar]
    public int health;

    CardBehaviour cardObject;

    private void Awake()
    {
        cardObject = GetComponent<CardBehaviour>();
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        health = cardObject.data.maxHealth;
    }
}