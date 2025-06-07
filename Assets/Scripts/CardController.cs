using UnityEngine;
using static Define;
using static Define.Card;

public class CardController : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    /// <summary>このカードが持つスプライト（表・裏）</summary>
    private Sprite _faceSprite;
    private Sprite _backSprite;

    public Card Card { get; private set; }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// カードとそのスプライト情報をセット
    /// </summary>
    public void SetCard(Card card, Sprite faceSprite, Sprite backSprite)
    {
        Card = card;
        _faceSprite = faceSprite;
        _backSprite = backSprite;

        UpdateSprite();
    }

    /// <summary>
    /// 表裏を切り替えてスプライトを更新
    /// </summary>
    public void SetFaceUp(bool isFaceUp)
    {
        if (Card == null) return;

        Card.IsFaceUp = isFaceUp;
        UpdateSprite();
    }

    /// <summary>
    /// 現在のカード状態に応じて表示スプライトを更新
    /// </summary>
    public void UpdateSprite()
    {
        if (Card == null || _spriteRenderer == null) return;

        _spriteRenderer.sprite = Card.IsFaceUp ? _faceSprite : _backSprite;
    }
}
