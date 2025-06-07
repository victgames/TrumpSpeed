using static Define.Card;
using static Define;
using System.Collections.Generic;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    [SerializeField] private CardGenerator _cardGenerator;

    private List<CardEntry> _deckEntries = new List<CardEntry>();

    [SerializeField] private Vector2 _deckBasePosition = new Vector2(4f, -1f);
    [SerializeField] private Vector2 _deckOffset = new Vector2(0.015f, -0.015f);
    [SerializeField] private Vector2 _drawnCardPosition = new Vector2(0f, 0f);  // 表向きカードの表示位置

    void Start()
    {
        InitializeDeck();
        DisplayDeck();
        DrawTopCard();
    }

    private void InitializeDeck()
    {
        _deckEntries.Clear();

        List<Card> cardDataList = _cardGenerator.CreateDeck(SuitColorMode.Both, UseJoker.One);
        _cardGenerator.Shuffle(cardDataList);

        for (int i = 0; i < cardDataList.Count; i++)
        {
            Vector2 pos = _deckBasePosition + _deckOffset * i;

            List<Card> singleCardList = new List<Card>() { cardDataList[i] };
            GameObject cardObj = _cardGenerator.GenerateCard(singleCardList, pos, false, BackSpriteColor.Blue);

            if (cardObj != null)
            {
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
            Vector2 pos = _deckBasePosition + _deckOffset * i;

            entry.View.transform.position = pos;

            SpriteRenderer sr = entry.View.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.sortingOrder = i;
            }
        }
    }

    public CardEntry DrawTopCard()
    {
        if (_deckEntries.Count == 0) return null;

        CardEntry topCard = _deckEntries[0];
        _deckEntries.RemoveAt(0);

        DisplayDeck();

        topCard.View.transform.position = _drawnCardPosition;

        CardController controller = topCard.View.GetComponent<CardController>();
        if (controller != null)
        {
            controller.SetFaceUp(true); // 表向きにしてスプライトを更新
            controller.Card.SpawnPosition = _drawnCardPosition;
        }

        return topCard;
    }
}
