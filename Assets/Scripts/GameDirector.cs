using static Define.Card;
using static Define;
using System.Collections.Generic;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    [SerializeField] private CardGenerator _cardGenerator;

    private List<CardEntry> _deckEntries = new List<CardEntry>();

    private Vector2 _deckBasePosition = new Vector2(7.0f, -2.0f);

    private Vector2 _deckOffset = new Vector2(0.010f, -0.010f);

    private List<Vector2> _fieldCardPosition = new List<Vector2>
    {
        new Vector2(-3.6f, 1.0f),
        new Vector2(-1.2f, 1.0f),
        new Vector2( 1.2f, 1.0f),
        new Vector2( 3.6f, 1.0f)
    };


    private List<Vector2> _handCardPosition = new List<Vector2>
    {
        new Vector2(-4.5f, -2.0f),
        new Vector2(-1.5f, -2.0f),
        new Vector2( 1.5f, -2.0f),
        new Vector2( 4.5f, -2.0f)
    };

    

    void Start()
    {
        InitializeDeck();


        // ê∂ê¨å„Ç…à íuÇ‚ï`âÊèáÇÃí≤êÆÇDisplayDeckÇ…îCÇπÇÈ
        DisplayDeck();

        foreach (Vector2 fieldPos in _fieldCardPosition)
        {
            string tag = "FieldCard";
            string sortingLayer = "Field";
            DrawTopCard(fieldPos, tag, sortingLayer);
        }

        foreach (Vector2 drawnPos in _handCardPosition)
        {
            string tag = "HandCard";
            string sortingLayer = "Hand";
            DrawTopCard(drawnPos, tag, sortingLayer);
        }
    }

    private void InitializeDeck()
    {
        _deckEntries.Clear();

        List<Card> cardDataList = _cardGenerator.CreateDeck(SuitColorMode.Both, UseJoker.One);
        _cardGenerator.Shuffle(cardDataList);

        for (int i = 0; i < cardDataList.Count; i++)
        {
            Vector2 pos = _deckBasePosition + _deckOffset * i;

            List<Card> singleCardList = new List<Card> { cardDataList[i] };
            GameObject cardObj = _cardGenerator.GenerateCard(singleCardList, pos, false, BackSpriteColor.Blue);

            if (cardObj != null)
            {
                cardObj.tag = "DeckCard";

                CardController controller = cardObj.GetComponent<CardController>();
                if (controller != null)
                {
                    CardEntry entry = new CardEntry(cardDataList[i], controller);
                    _deckEntries.Add(entry);
                }
            }
        }

    }

    private void DisplayDeck()
    {
        for (int i = 0; i < _deckEntries.Count; i++)
        {
            CardEntry entry = _deckEntries[i];
            Vector3 pos = (Vector3)(_deckBasePosition + _deckOffset * i);
            pos.z = -i * 0.01f; // Zé≤ÇÇ∏ÇÁÇ∑

            entry.View.transform.position = pos;

            SpriteRenderer sr = entry.View.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.sortingLayerName = "Deck";
                sr.sortingOrder = i;
            }
        }
    }

    public CardEntry DrawTopCard(Vector2 pos, string tag, string sortLayer)
    {
        if (_deckEntries.Count == 0) return null;

        CardEntry topCard = _deckEntries[0];
        _deckEntries.RemoveAt(0);

        DisplayDeck();

        topCard.View.transform.position = pos;
        topCard.View.tag = tag;

        SpriteRenderer sr = topCard.View.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sortingLayerName = sortLayer;
            sr.sortingOrder = 0; // ïKóvÇ…âûÇ∂Çƒí≤êÆÇµÇƒÇ≠ÇæÇ≥Ç¢
        }

        CardController controller = topCard.View.GetComponent<CardController>();
        if (controller != null)
        {
            controller.SetFaceUp(true);
            controller.Card.SpawnPosition = pos;
        }

        return topCard;
    }
}
