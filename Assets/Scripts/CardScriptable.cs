using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu]
public class CardScriptable : ScriptableObject
{
    [Header("Identity")]
    public CardId cardId;
    public CardType cardType;
    public RarityType rarityType;

    [Header("Stats")]
    public int maxHealth;
    public int energy;

    [Header("Metadata")]
    public new string name;
    [TextArea]
    public string description;
    public Sprite avatar;

    // TODO
    public static CardScriptable LoadCardFromDisk(CardId cardId)
    {
        var newCard = Resources.Load<CardScriptable>("Scriptables/" + Enum.GetName(typeof(CardId), cardId));
        if (!newCard) { Debug.Log("Card scriptable does not exist"); return null; }
        return newCard;
    }

    public static MinionCardBehaviour LoadMinionFromDisk(CardId cardId)
    {
        var newMinion = Resources.Load<MinionCardBehaviour>("Prefabs/" + Enum.GetName(typeof(CardId), cardId));
        if (!newMinion) { Debug.Log("Minion prefab does not exist"); return null; }
        return newMinion;
    }
}

public enum CardId
{
    None = 0,
    BasicMinion,
    BasicAction,
    BasicItem
}

public enum CardType
{
    Minion,
    Action,
    Item
}

public enum RarityType
{
    Common,
    Uncommon,
    Rare,
    SuperRare,
    Legendary,
    Transcendant
}