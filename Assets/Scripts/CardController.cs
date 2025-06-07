using UnityEngine;
using static Define;
using static Define.Card;

public class CardController : MonoBehaviour
{
    // *******************************************************
    // メンバ変数
    // *******************************************************

    /// <summary>
    /// スプライト描画用コンポーネント
    /// </summary>
    private SpriteRenderer _spriteRenderer;


    // *******************************************************
    // プロパティ
    // *******************************************************

    /// <summary>
    /// カード情報を丸ごと保持
    /// </summary>
    public Card Card { get; private set; }


    // *******************************************************
    // メソッド
    // *******************************************************

    /// <summary>
    /// 初期化
    /// </summary>
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="card"></param>
    /// <param name="faceSprites"></param>
    /// <param name="backSprites"></param>
    public void SetCard(Card card, Sprite[] faceSprites, Sprite[] backSprites)
    {
        Card = card;

        if (Card.IsFaceUp)
        {
            int index = GetSpriteIndex(Card.Suit, Card.Number, faceSprites.Length);
            if (index >= 0 && index < faceSprites.Length)
            {
                _spriteRenderer.sprite = faceSprites[index];
            }
            else
            {
                Debug.LogWarning($"無効なスプライトインデックス: {index}");
            }
        }
        else
        {
            int backIndex = (int)Card.BackColor;
            if (backIndex >= 0 && backIndex < backSprites.Length)
            {
                _spriteRenderer.sprite = backSprites[backIndex];
            }
            else
            {
                Debug.LogWarning($"無効な裏面スプライトインデックス: {backIndex}");
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="suit"></param>
    /// <param name="number"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    private int GetSpriteIndex(SuitType suit, int number, int length)
    {
        if (suit == SuitType.Joker)
        {
            return length - 1;
        }
        return ((int)suit) * 13 + (number - 1);
    }
}
