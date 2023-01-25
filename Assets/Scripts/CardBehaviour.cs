using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Mirror;
using TMPro;

public class CardBehaviour : NetworkBehaviour, IPointerClickHandler
{
    public CardScriptable cardData;

    [SyncVar]
    public int health;

    // to be used
    [SyncVar]
    public bool isDeployed;
    [SyncVar]
    public PlayerManager owner;

    [Header("Display Elements")]
    public TMP_Text energyDisplay;
    public SpriteRenderer avatarDisplay;
    public TMP_Text healthDisplay;
    public SpriteRenderer backgroundDisplay;
    public SpriteRenderer backDisplay;

    // Start is called before the first frame update
    void OnEnable()
    {
        if (isServer) { health = cardData.maxHealth; }

        energyDisplay.text = cardData.energy.ToString();
        healthDisplay.text = cardData.maxHealth.ToString();
        avatarDisplay.sprite = cardData.avatar;

        switch (cardData.cardRarity)
        {
            case CardRarity.Common:
                backgroundDisplay.color = new Color(204, 102, 0); //brown
                break;
            case CardRarity.Uncommon:
                backgroundDisplay.color = Color.green; //green
                break;
            case CardRarity.Rare:
                backgroundDisplay.color = Color.blue; //blue
                break;
            case CardRarity.SuperRare:
                backgroundDisplay.color = new Color(255, 128, 0); //orange
                break;
            case CardRarity.Legendary:
                backgroundDisplay.color = Color.red; //red
                break;
            case CardRarity.Transcendant:
                backgroundDisplay.color = Color.white; //reflective (post procesing???)
                Debug.Log("I dont fucking know");
                break;
        }
        //Card rarity colors: common brown uncommon green rare blue super rare orange legendary red transcendant glow
    }

    public void SetDisplay(bool enabled)
    {
        if (enabled)
        {
            transform.localScale = new Vector3(15, 15, 15);
        }
        else
        {
            transform.localScale = new Vector3(0, 0, 0);
        }
    }

    public void SetCardBack(bool enabled)
    {
        backDisplay.gameObject.SetActive(enabled);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        EventDispatcher.Instance.SendEvent(6, new CardClickEvent(this));
    }
}
