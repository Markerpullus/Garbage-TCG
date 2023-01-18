using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu]
public class HandCard : ScriptableObject
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
    public static HandCard CreateCard(CardId cardId)
    {

        var newCard = Resources.Load<HandCard>("Scriptables/" + Enum.GetName(typeof(CardId), cardId));
        if (!newCard) { Debug.Log("name does not exist"); return null; }
        return newCard;
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