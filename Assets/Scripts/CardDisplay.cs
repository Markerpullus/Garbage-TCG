using System;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

[RequireComponent(typeof(CardBehaviour))]
public class CardDisplay : MonoBehaviour, IPointerClickHandler
{
    CardBehaviour cardObject;

    [Header("Display Elements")]
    public TMP_Text energyDisplay;
    public SpriteRenderer avatarDisplay;
    public TMP_Text healthDisplay;
    public SpriteRenderer backgroundDisplay;
    public SpriteRenderer backDisplay;

    private void OnEnable()
    {
        cardObject = GetComponent<CardBehaviour>();

        var cardData = cardObject.data;
        energyDisplay.text = cardData.energy.ToString();
        healthDisplay.text = cardData.maxHealth.ToString();
        avatarDisplay.sprite = cardData.avatar;

        switch (cardData.rarity)
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
            transform.localScale = new Vector3(15, 15, 1);
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

    public void SetYShift(float y)
    {
        transform.localPosition += new Vector3(0f, y, 0f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        cardObject.OnCardClick();
    }
}
