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
    /// カードリスト作成
    /// </summary>
    /// <param name="colorMode"></param>
    /// <param name="includeJoker"></param>
    /// <returns></returns>
    public List<Card> InitializeCardList(SuitColorMode colorMode, UseJoker includeJoker, BackSpriteColor backColor)
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

            // 1～13までの数字で初期化
            for (int num = 1; num <= 13; num++)
            {
                Sprite? faceSprite = GetFaceSprite(suit, num);
                Sprite? backSprite = GetBackSprite(backColor);
                deck.Add(new Card(suit, num, backColor, faceSprite, backSprite, false, CardProperty.None, null));
            }
        }

        // Jokerの枚数初期化
        for (int i = 0; i < (int)includeJoker; i++)
        {
            // Jokerは_faceSpritesの53番目以降に配置
            Sprite? faceSprite = GetFaceSprite(SuitType.Joker, 53 + i);
            Sprite? backSprite = GetBackSprite(backColor);
            deck.Add(new Card(SuitType.Joker, 0, backColor, faceSprite, backSprite, false, CardProperty.None, null));
        }

        return deck;
    }

    /// <summary>
    /// カード情報リスト作成
    /// </summary>
    public List<CardEntry?> InitializeEntries(List<Card> cardDataList)
    {
        // カード情報リストを初期化
        List<CardEntry?> entries = new List<CardEntry?>();

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
                    CardEntry entry = new CardEntry(cardDataList[i], controller);//, CardProperty.None, 0);
                    entries.Add(entry);
                }
            }
        }
        return entries;
    }

    /// <summary>
    /// カードを生成
    /// </summary>
    /// <param name="cardData"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public GameObject? GenerateCard(Card cardData)
    {
        if (cardData == null)
        {
            return null;
        }

        // カードインスタンス作成
        GameObject cardObj = Instantiate(_cardPrefab, Vector3.zero, Quaternion.identity);

        // インスタンスの情報を更新
        var cardCtrl = cardObj.GetComponent<CardController>();
        if (cardCtrl != null)
        {
            cardCtrl.Card = cardData;
        }
        cardObj.GetComponent<CardController>()?.UpdateSprite();
        cardObj.GetComponent<CardController>()?.SetSorting(SortLayers.Name(CardProperty.None), 0);

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
