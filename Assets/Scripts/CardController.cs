using UnityEngine;
using static Define;

/// <summary>
/// カードオブジェクトで処理するクラス
/// </summary>
public class CardController : MonoBehaviour
{
    // *******************************************************
    // メンバ変数
    // *******************************************************

    /// <summary>
    /// カード定義
    /// </summary>
    public Card Card { get; private set; }

    /// <summary>
    /// スプライトレンダリング用コンポーネント
    /// </summary>
    public SpriteRenderer SpriteRenderer { get; private set; }

    // *******************************************************
    // 基本メソッド
    // *******************************************************

    /// <summary>
    /// ゲーム開始前の初期化処理
    /// </summary>
    private void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // *******************************************************
    // メソッド
    // *******************************************************

    /// <summary>
    /// カード定義
    /// </summary>
    /// <param name="card"></param>
    public void SetCard(Card card)
    {
        Card = card;
    }

    /// <summary>
    /// カード裏表（isFaceUp）に合わせてスプライト（sprite）を更新
    /// </summary>
    public void SetSprite(bool isFaceUp)
    {
        if (Card == null || SpriteRenderer == null) return;

        Card.IsFaceUp = isFaceUp;
        SpriteRenderer.sprite = Card.IsFaceUp ? Card.FaceSprite : Card.BackSprite;
    }

    /// <summary>
    /// カードの所属（cardProperty）, レンダラー順序（sortingLayer）を更新
    /// </summary>
    /// <param name="cardProperty"></param>
    public void SetCardProperty(CardProperty cardProperty)
    {
        if (Card == null || SpriteRenderer == null) return;

        Card.CardProperty = cardProperty;
        SpriteRenderer.sortingLayerName = SortLayers.Name(cardProperty);
    }

    /// <summary>
    /// レンダラーオーダー順（sortingOrder）を更新
    /// </summary>
    /// <param name="order"></param>
    public void SetSortingOrder(int order)
    {
        if (SpriteRenderer == null) return;

        SpriteRenderer.sortingOrder = order;
    }

    /// <summary>
    /// slotIndexを更新
    /// </summary>
    /// <param name="slotIndex"></param>
    public void SetSlotIndex(int slotIndex)
    {
        if (Card == null) return;

        Card.SlotIndex = slotIndex;
    }
}