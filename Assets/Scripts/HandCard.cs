using System.Collections;
using System.Collections.Generic;
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