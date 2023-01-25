using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu]
public class CardScriptable : ScriptableObject
{
    [Header("Identity")]
    public CardType cardType;
    public CardCategory cardCategory;
    public CardRarity cardRarity;

    [Header("Stats")]
    public int maxHealth;
    public int energy;

    [Header("Metadata")]
    public new string name;
    [TextArea]
    public string description;
    public Sprite avatar;

    // TODO
    public static CardScriptable LoadCardDataFromDisk(CardType cardId)
    {
        var newCard = Resources.Load<CardScriptable>("Scriptables/" + Enum.GetName(typeof(CardType), cardId));
        if (!newCard) { Debug.Log("Card scriptable does not exist"); return null; }
        return newCard;
    }

    public static CardBehaviour LoadCardFromDisk(CardType cardId)
    {
        var newCard = Resources.Load<CardBehaviour>("Prefabs/" + Enum.GetName(typeof(CardType), cardId));
        if (!newCard) { Debug.Log("Card prefab does not exist"); return null; }
        return newCard;
    }
}

public enum CardType
{
    None = 0,
    BasicMinion,
    BasicAction,
    BasicItem
}

public enum CardCategory
{
    Minion,
    Action,
    Item
}

public enum CardRarity
{
    Common,
    Uncommon,
    Rare,
    SuperRare,
    Legendary,
    Transcendant
}