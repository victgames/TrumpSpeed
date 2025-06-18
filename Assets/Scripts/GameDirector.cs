using static Define.Card;
using static Define;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class GameDirector : MonoBehaviour
{
    // *******************************************************
    // メンバ変数
    // *******************************************************

    [SerializeField] 
    private CardGenerator _cardGenerator;

    private List<CardEntry> _deckEntries = new List<CardEntry>();

    // *******************************************************
    // プロパティ
    // *******************************************************

    public bool[] HandCardCounts { get; private set; } = new bool[4];

    public int[] FieldCardCounts { get; private set; } = new int[4];


    // *******************************************************
    // 基本メソッド
    // *******************************************************

    /// <summary>
    /// 開始時処理
    /// </summary>
    void Start()
    {
        InitializeDeck();
        //DisplayDeck();

        /*
        for (int i = 0; i < POS_FIELD.Count; i++)
        {
            DrawTopCard(POS_FIELD[i], TAG_FIELD, SORT_LAYER_FIELD);
            FieldCardCounts[i] = 1; // 最初に1枚ずつ置いたので
        }

        for (int i = 0; i < POS_HAND.Count; i++)
        {
            DrawTopCard(POS_HAND[i], TAG_HAND, SORT_LAYER_HAND);
            HandCardCounts[i] = true; // 最初に1枚置いたので
        }
        */
    }

    // *******************************************************
    // メソッド
    // *******************************************************

    /// <summary>
    /// デッキ作成
    /// </summary>
    private void InitializeDeck()
    {
        // デッキエントリーを初期化
        _deckEntries.Clear();

        // デッキリストを作成
        List<Card> cardDataList = _cardGenerator.CreateDeck(SuitColorMode.Both, UseJoker.One, BackSpriteColor.Red);
        _cardGenerator.Shuffle(cardDataList);

        for (int i = 0; i < cardDataList.Count; i++)
        {
            // カードを生成
            Vector3 pos = POS_DECK + POS_DECK_OFFSET * i;
            GameObject cardObj = _cardGenerator.GenerateCard(cardDataList[i], pos, false, TAG_DECK, SORT_LAYER_DECK, i);

            // デッキエントリーに格納
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

    /// <summary>
    /// デッキ表示
    /// </summary>
    private void DisplayDeck()
    {
        for (int i = 0; i < _deckEntries.Count; i++)
        {
            // CardEntryを探索
            CardEntry entry = _deckEntries[i];

            // 位置を変更
            Vector3 pos = POS_DECK + POS_DECK_OFFSET * i;
            entry.View.transform.position = pos;
            entry.Data.Position = pos;

            // SpriteRendererのパラメータ変更
            entry.View.GetComponent<CardController>()?.SetSorting(SORT_LAYER_DECK, i);
        }
    }

    /// <summary>
    /// デッキ1枚目を引く処理
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="tag"></param>
    /// <param name="sortLayer"></param>
    /// <returns></returns>
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
            sr.sortingOrder = 0; // 必要に応じて調整してください
        }

        CardController controller = topCard.View.GetComponent<CardController>();
        if (controller != null)
        {
            //controller.SetFaceUp(true);
            controller.Card.Position = pos;
        }

        return topCard;
    }

}
