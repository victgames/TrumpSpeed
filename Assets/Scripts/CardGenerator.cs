using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Define;
using static Define.Card;

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
    /// カードを生成
    /// </summary>
    /// <param name="cardData"></param>
    /// <param name="spawnPosition"></param>
    /// <param name="isFaceUp"></param>
    /// <returns></returns>
    public GameObject GenerateCard(Card cardData, Vector3 spawnPosition, bool isFaceUp, string tag, string sortingLayer, int sortingOrder)
    {
        if (cardData == null)
        {
            Debug.LogWarning("カードデータが null です");
            return null;
        }

        GameObject cardObj = Instantiate(_cardPrefab, spawnPosition, Quaternion.identity);
        CardController controller = cardObj.GetComponent<CardController>();

        if (controller != null)
        {
            controller.Card = cardData;
            controller.UpdateSprite();
            controller.tag = tag;
            controller.SpriteRenderer.sortingLayerName = sortingLayer;
            controller.SpriteRenderer.sortingOrder = sortingOrder;
        }

        return cardObj;
    }

    private Card CreateCardWithSprites(Card baseCard, BackSpriteColor backColor, bool isFaceUp, Vector2 position)
    {
        Sprite faceSprite = GetFaceSprite(baseCard.Suit, baseCard.Number);
        Sprite backSprite = _backSprites[(int)backColor];

        return new Card(
            baseCard.Suit,
            baseCard.Number,
            backColor,
            faceSprite,
            backSprite,
            isFaceUp,
            position
        );
    }

    // 表スプライトを1枚だけ取得
    private Sprite GetFaceSprite(SuitType suit, int number)
    {
        if (suit == SuitType.Joker)
        {
            return _faceSprites[_faceSprites.Length - 1]; // ジョーカーは最後
        }

        int index = ((int)suit) * 13 + (number - 1);
        if (index >= 0 && index < _faceSprites.Length)
        {
            return _faceSprites[index];
        }

        Debug.LogWarning($"GetFaceSprite: 無効なインデックス {index}");
        return null;
    }


    /// <summary>
    /// デッキ作成
    /// </summary>
    /// <param name="colorMode"></param>
    /// <param name="includeJoker"></param>
    /// <returns></returns>
    public List<Card> CreateDeck(SuitColorMode colorMode, UseJoker includeJoker, BackSpriteColor backColor)
    {
        List<Card> deck = new List<Card>();

        // SuitTypeすべての値を順に列挙してsuitに代入
        foreach (SuitType suit in Enum.GetValues(typeof(SuitType)))
        {
            // Jokerを除外
            if (suit == SuitType.Joker) continue;

            // SuitColorModeに併せて山札に含めないカードを除外
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
    /// シャッフル
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    public void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int j = UnityEngine.Random.Range(i, list.Count);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}
