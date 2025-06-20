using System;
using UnityEngine;

public class CardController : MonoBehaviour
{
    // *******************************************************
    // メンバ変数
    // *******************************************************

    /// <summary>
    /// カード定義
    /// </summary>
    public Card Card { get; set; }

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
    /// sortingを変更
    /// </summary>
    /// <param name="layerName"></param>
    /// <param name="order"></param>
    public void SetSorting(string layerName, int order)
    {
        if (SpriteRenderer == null) return;
        SpriteRenderer.sortingLayerName = layerName;
        SpriteRenderer.sortingOrder = order;
    }

    /// <summary>
    /// カード裏表に合わせてスプライト変更
    /// </summary>
    public void UpdateSprite()
    {
        if (Card == null || SpriteRenderer == null) return;
        SpriteRenderer.sprite = Card.IsFaceUp ? Card.FaceSprite : Card.BackSprite;
    }
}