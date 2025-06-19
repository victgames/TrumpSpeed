using System;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using static Define.Card;

public class CardManager : MonoBehaviour
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
    public List<Card> CreateCardList(SuitColorMode colorMode, UseJoker includeJoker, BackSpriteColor backColor)
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
                Sprite backSprite = _backSprites[(int)backColor];
                deck.Add(new Card(suit, num, backColor, faceSprite, backSprite, false, Vector2.zero));
            }
        }

        // Jokerの枚数初期化
        for (int i = 0; i < (int)includeJoker; i++)
        {
            // Jokerは_faceSpritesの53番目以降に配置
            Sprite faceSprite = GetFaceSprite(SuitType.Joker, 53 + i);
            Sprite backSprite = _backSprites[(int)backColor];
            deck.Add(new Card(SuitType.Joker, 0, backColor, faceSprite, backSprite, false, Vector2.zero));
        }

        return deck;
    }

    /// <summary>
    /// カードを生成
    /// </summary>
    /// <param name="cardData"></param>
    /// <param name="position"></param>
    /// <param name="isFaceUp"></param>
    /// <returns></returns>
    public GameObject GenerateCard(Card cardData, Vector3 position, string tag, string sortingLayer)
    {
        if (cardData == null)
        {
            return null;
        }

        GameObject cardObj = Instantiate(_cardPrefab, position, Quaternion.identity);
        CardController controller = cardObj.GetComponent<CardController>();

        if (controller != null)
        {
            controller.Card = cardData;
            controller.UpdateSprite();
            controller.tag = tag;
            controller.SpriteRenderer.sortingLayerName = sortingLayer;
        }

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
    /// 山札を表示
    /// </summary>
    /// <param name="entries"></param>
    public void DisplayDeck(List<CardEntry> entries, Vector3 pos, Vector3 offset)
    {
        for (int i = 0; i < entries.Count; i++)
        {
            // カードの位置を変更して再表示
            CardEntry entry = entries[i];

            entry.View.transform.position = pos + offset * i;
            entry.Data.Position = pos + offset * i;

            // カードの位置情報を変更
            entry.View.GetComponent<CardController>()?.SetSorting(SORT_LAYER_DECK, i);
        }
    }

    /// <summary>
    /// 山札をシャッフル
    /// </summary>
    /// <param name="deck"></param>
    public void Shuffle(List<CardEntry> deck)
    {
        for (int i = 0; i < deck.Count; i++)
        {
            int j = UnityEngine.Random.Range(i, deck.Count);
            (deck[i], deck[j]) = (deck[j], deck[i]);
        }
    }


    /// <summary>
    /// デッキ1枚目を引く処理
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="tag"></param>
    /// <param name="sortLayer"></param>
    /// <returns></returns>
    public CardEntry DrawTopCard(List<CardEntry> entries, Vector3 pos, string tag, string sortLayer)
    {
        if (entries.Count == 0) return null;

        CardEntry topCard = entries[0];
        entries.RemoveAt(0);

        //DisplayDeck();

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
