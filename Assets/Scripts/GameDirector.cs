using System.Collections.Generic;
using UnityEngine;
using static Define;
using static Define.Card;

public class GameDirector : MonoBehaviour
{
    // *******************************************************
    // メンバ変数
    // *******************************************************

    /// <summary>
    /// カードを生成するコンポーネント
    /// </summary>
    [SerializeField] 
    private CardManager _cardManager;

    /// <summary>
    /// カード情報リスト
    /// </summary>
    private List<CardEntry> _CardEntriesRed = new List<CardEntry>();

    // *******************************************************
    // プロパティ
    // *******************************************************

    /// <summary>
    /// 手札枚数情報
    /// </summary>
    public bool[] HandCardCounts { get; private set; } = new bool[4];

    /// <summary>
    /// 場札枚数情報
    /// </summary>
    public int[] FieldCardCounts { get; private set; } = new int[4];


    // *******************************************************
    // 基本メソッド
    // *******************************************************

    /// <summary>
    /// 開始時処理
    /// </summary>
    void Start()
    {
        // カードリストを作成
        InitializeDeckEntries(_CardEntriesRed, SuitColorMode.Both, UseJoker.One, BackSpriteColor.Red);
        
        // シャッフル後山札を表示
        _cardManager.Shuffle(_CardEntriesRed);
        _cardManager.DisplayDeck(_CardEntriesRed, POS_DECK, POS_DECK_OFFSET);

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
    /// カード情報リスト作成
    /// </summary>
    private void InitializeDeckEntries(List<CardEntry> entries, SuitColorMode mode, UseJoker joker, BackSpriteColor color)
    {
        // カード情報リストを初期化
        entries.Clear();

        // カード情報リストを作成
        List<Card> cardDataList = _cardManager.CreateCardList(mode, joker, color);

        for (int i = 0; i < cardDataList.Count; i++)
        {
            // カードを生成
            GameObject cardObj = _cardManager.GenerateCard(cardDataList[i], Vector3.zero, TAG_DECK, SORT_LAYER_DECK);

            // カード情報リストに格納
            if (cardObj != null)
            {
                CardController controller = cardObj.GetComponent<CardController>();
                if (controller != null)
                {
                    CardEntry entry = new CardEntry(cardDataList[i], controller);
                    entries.Add(entry);
                }
            }
        }
    }


}
