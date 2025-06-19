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
    /// カード情報リスト作成
    /// </summary>
    public List<CardEntry> InitializeEntries(SuitColorMode mode, UseJoker joker, BackSpriteColor color, CardProperty cardProperty)
    {
        // カード情報リストを初期化
        List<CardEntry> entries = new List<CardEntry>();

        // カード情報リストを作成
        List<Card> cardDataList = GenerateCardList(mode, joker, color, cardProperty);

        for (int i = 0; i < cardDataList.Count; i++)
        {
            // カードを生成
            GameObject cardObj = GenerateCard(cardDataList[i], Vector3.zero);

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

        return entries;
    }

    /// <summary>
    /// カードリスト作成
    /// </summary>
    /// <param name="colorMode"></param>
    /// <param name="includeJoker"></param>
    /// <returns></returns>
    public List<Card> GenerateCardList(SuitColorMode colorMode, UseJoker includeJoker, BackSpriteColor backColor, CardProperty cardProperty)
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
                Sprite faceSprite = GetFaceSprite(suit, num);
                Sprite backSprite = GetBackSprite(backColor);
                deck.Add(new Card(suit, num, backColor, faceSprite, backSprite, false, cardProperty, Vector3.zero));
            }
        }

        // Jokerの枚数初期化
        for (int i = 0; i < (int)includeJoker; i++)
        {
            // Jokerは_faceSpritesの53番目以降に配置
            Sprite faceSprite = GetFaceSprite(SuitType.Joker, 53 + i);
            Sprite backSprite = GetBackSprite(backColor);
            deck.Add(new Card(SuitType.Joker, 0, backColor, faceSprite, backSprite, false, cardProperty, Vector3.zero));
        }

        return deck;
    }

    /// <summary>
    /// カードを生成
    /// </summary>
    /// <param name="cardData"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public GameObject GenerateCard(Card cardData, Vector3 position)
    {
        if (cardData == null)
        {
            return null;
        }

        // カードインスタンス作成
        GameObject cardObj = Instantiate(_cardPrefab, position, Quaternion.identity);

        // インスタンスの情報を更新
        cardObj.GetComponent<CardController>()?.SetCard(cardData);
        cardObj.GetComponent<CardController>()?.SetSprite(cardData.IsFaceUp);
        cardObj.GetComponent<CardController>()?.SetSorting(SortLayers.Name(cardData.CardProperty), 0);

        return cardObj;
    }

    /// <summary>
    /// 表面スプライトを探索
    /// </summary>
    /// <param name="suit"></param>
    /// <param name="number"></param>
    /// <returns></returns>
    private Sprite GetFaceSprite(SuitType suit, int number)
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
    private Sprite GetBackSprite(BackSpriteColor backColor)
    {
        return _backSprites[(int)backColor];
    }


}
