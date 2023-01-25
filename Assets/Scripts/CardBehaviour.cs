using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class CardBehaviour : MonoBehaviour, IPointerClickHandler
{
    public CardScriptable cardData;

    [Header("Display Elements")]
    public TMP_Text energyDisplay;
    public SpriteRenderer avatarDisplay;
    public TMP_Text healthDisplay;
    public SpriteRenderer backgroundDisplay;
    public SpriteRenderer backDisplay;

    // Start is called before the first frame update
    void OnEnable()
    {
        energyDisplay.text = cardData.energy.ToString();
        healthDisplay.text = cardData.maxHealth.ToString();
        avatarDisplay.sprite = cardData.avatar;

        switch (cardData.rarityType)
        {
            case RarityType.Common:
                backgroundDisplay.color = new Color(204, 102, 0); //brown
                break;
            case RarityType.Uncommon:
                backgroundDisplay.color = Color.green; //green
                break;
            case RarityType.Rare:
                backgroundDisplay.color = Color.blue; //blue
                break;
            case RarityType.SuperRare:
                backgroundDisplay.color = new Color(255, 128, 0); //orange
                break;
            case RarityType.Legendary:
                backgroundDisplay.color = Color.red; //red
                break;
            case RarityType.Transcendant:
                backgroundDisplay.color = Color.white; //reflective (post procesing???)
                Debug.Log("I dont fucking know");
                break;
        }
        //Card rarity colors: common brown uncommon green rare blue super rare orange legendary red transcendant glow
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
