#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using static UnityEngine.EventSystems.EventTrigger;



public class CardGenerator : MonoBehaviour
{
    // *******************************************************
    // メンバ変数
    // *******************************************************

    /// <summary>
    /// カードのプレハブ
    /// </summary>
    [SerializeField]
    private GameObject _cardPrefab;

    /// <summary>
    /// 52枚＋ジョーカー分の表面スプライト（インスペクタでセット）
    /// </summary>
    [SerializeField]
    private Sprite[] _faceSprites;

    /// <summary>
    /// カード裏面スプライト（0:赤, 1:青）（インスペクタでセット）
    /// </summary>
    [SerializeField]
    private Sprite[] _backSprites;


    // *******************************************************
    // メソッド
    // *******************************************************

    /// <summary>
    /// カードエントリーを作成
    /// </summary>
    /// <param name="colorMode">トランプの使用色</param>
    /// <param name="useJoker">ジョーカーの使用</param>
    /// <param name="backColor">カード裏面の絵柄</param>
    /// <returns>カードエントリー</returns>
    public List<CardEntry> InitializeEntries(SuitColorMode colorMode, UseJoker useJoker, BackSpriteColor backColor)
    {
        // カードエントリーを初期化
        List<CardEntry> entries = new List<CardEntry>();

        // カード情報リストを初期化
        List<Card> cardDataList = InitializeCardList(colorMode, useJoker, backColor);

        for (int i = 0; i < cardDataList.Count; i++)
        {
            // カードを生成
            GameObject? cardObj = GenerateCard(cardDataList[i]);

            // カード情報リストに格納
            if (cardObj != null)
            {
                CardController controller = cardObj.GetComponent<CardController>();
                if (controller != null)
                {
                    // 所属:None, インデックス:0として初期化
                    CardEntry entry = new CardEntry(cardDataList[i], controller);
                    entries.Add(entry);
                }
            }
        }
        return entries;
    }

    /// <summary>
    /// カードリスト作成
    /// </summary>
    /// <param name="colorMode">トランプの使用色</param>
    /// <param name="useJoker">ジョーカーの使用</param>
    /// <returns></returns>
    private List<Card> InitializeCardList(SuitColorMode colorMode, UseJoker useJoker, BackSpriteColor backColor)
    {
        List<Card> deck = new List<Card>();

        // SuitTypeすべての値を順に列挙してsuitに代入
        foreach (SuitType suit in Enum.GetValues(typeof(SuitType)))
        {
            // Jokerを除外
            if (suit == SuitType.Joker) continue;

            // SuitColorModeに併せてカードリストに含めないカードを除外
            if (colorMode == SuitColorMode.BlackOnly && (suit != SuitType.Spade && suit != SuitType.Club)) continue;
            if (colorMode == SuitColorMode.RedOnly && (suit != SuitType.Heart && suit != SuitType.Diamond)) continue;
            if (colorMode == SuitColorMode.SpadeOnly && (suit != SuitType.Spade)) continue;
            if (colorMode == SuitColorMode.ClubOnly && (suit != SuitType.Club)) continue;
            if (colorMode == SuitColorMode.DiamondOnly && (suit != SuitType.Diamond)) continue;
            if (colorMode == SuitColorMode.HeartOnly && (suit != SuitType.Heart)) continue;

            // 1〜13までの数字で初期化
            for (int num = 1; num <= 13; num++)
            {
                Sprite? faceSprite = GetFaceSprite(suit, num);
                Sprite? backSprite = GetBackSprite(backColor);
                deck.Add(new Card(suit, num, backColor, faceSprite, backSprite, false, CardProperty.None, 0));
            }
        }

        // Jokerの枚数初期化
        for (int i = 0; i < (int)useJoker; i++)
        {
            // Jokerは_faceSpritesの52番目以降に配置
            Sprite? faceSprite = GetFaceSprite(SuitType.Joker, 52 + i);
            Sprite? backSprite = GetBackSprite(backColor);
            deck.Add(new Card(SuitType.Joker, 0, backColor, faceSprite, backSprite, false, CardProperty.None, -1));
        }

        return deck;
    }

    /// <summary>
    /// カードを生成
    /// </summary>
    /// <param name="card">カード情報</param>
    /// <returns></returns>
    private GameObject? GenerateCard(Card card)
    {
        if (card == null)
        {
            return null;
        }

        // カードインスタンス作成
        GameObject cardObj = Instantiate(_cardPrefab, Position.None, Quaternion.identity);

        cardObj.GetComponent<CardController>()?.SetCard(card);
        cardObj.GetComponent<CardController>()?.SetSprite(false);
        cardObj.GetComponent<CardController>()?.SetSortingOrder(-1);

        return cardObj;
    }

    /// <summary>
    /// 表面スプライトを探索
    /// </summary>
    /// <param name="suit"></param>
    /// <param name="number"></param>
    /// <returns></returns>
    private Sprite? GetFaceSprite(SuitType suit, int number)
    {
        // ジョーカーの場合は入力値のスプライトを取得
        if (suit == SuitType.Joker)
        {
            return _faceSprites[number];
        }

        // 数字の場合はsuitと数字で合致するスプライトを探索
        int index = ((int)suit) * 13 + (number - 1);
        if (index >= 0 && index < _faceSprites.Length)
        {
            return _faceSprites[index];
        }

        return null;
    }

    /// <summary>
    /// 裏面スプライトを探索
    /// </summary>
    /// <param name="backColor"></param>
    /// <returns></returns>
    private Sprite? GetBackSprite(BackSpriteColor backColor)
    {
        return _backSprites[(int)backColor];
    }
}
