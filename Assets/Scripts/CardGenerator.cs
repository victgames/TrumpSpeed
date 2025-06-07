using System;
using System.Collections.Generic;
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
    /// デッキからカードを1枚引いて生成（表/裏共通）
    /// </summary>
    public void GenerateCard(List<Card> deck, Vector2 spawnPosition, bool isFaceUp, BackSpriteColor backColor)
    {
        if (deck.Count == 0)
        {
            Debug.LogWarning("デッキが空です");
            return;
        }

        Card drawCard = deck[0];
        deck.RemoveAt(0);

        // 新しいカード情報として状態を更新したCardを再生成
        Card updatedCard = new Card(
            drawCard.Suit,
            drawCard.Number,
            isFaceUp,
            spawnPosition,
            backColor
        );

        GameObject cardObj = Instantiate(_cardPrefab, spawnPosition, Quaternion.identity);
        CardController controller = cardObj.GetComponent<CardController>();
        controller.SetCard(updatedCard, _faceSprites, _backSprites);
    }

    /// <summary>
    /// デッキ生成
    /// </summary>
    public List<Card> CreateDeck(SuitColorMode colorMode, UseJoker includeJoker)
    {
        List<Card> deck = new List<Card>();

        foreach (SuitType suit in Enum.GetValues(typeof(SuitType)))
        {
            if (suit == SuitType.Joker) continue;

            if (colorMode == SuitColorMode.BlackOnly && (suit != SuitType.Spade && suit != SuitType.Club)) continue;
            if (colorMode == SuitColorMode.RedOnly && (suit != SuitType.Heart && suit != SuitType.Diamond)) continue;

            for (int num = 1; num <= 13; num++)
            {
                deck.Add(new Card(suit, num, false, Vector2.zero, BackSpriteColor.Red));
            }
        }

        // Joker の追加（0とする）
        for (int i = 0; i < (int)includeJoker; i++)
        {
            deck.Add(new Card(SuitType.Joker, 0, false, Vector2.zero, BackSpriteColor.Red));
        }

        return deck;
    }

    /// <summary>
    /// デッキをシャッフルする（Fisher-Yatesアルゴリズム）
    /// </summary>
    public void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int j = UnityEngine.Random.Range(i, list.Count);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}
