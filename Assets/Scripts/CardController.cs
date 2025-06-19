using System;
using UnityEngine;
using static Define;

public class CardController : MonoBehaviour
{
    // *******************************************************
    // メンバ変数
    // *******************************************************

    /// <summary>
    /// ドラッグ状態を判定するフラグ
    /// </summary>
    private bool _isDragging = false;

    /// <summary>
    /// ドラッグ開始時位置
    /// </summary>
    private Vector3 _originalPosition;

    /// <summary>
    /// マウスをオブジェクトの中心のずれ
    /// </summary>
    private Vector3 _offset;

    private GameObject _currentDropTarget = null;

    // *******************************************************
    // プロパティ
    // *******************************************************

    /// <summary>
    /// カード情報
    /// </summary>
    public Card Card { get; private set; }

    /// <summary>
    /// スプライト情報
    /// </summary>
    public SpriteRenderer SpriteRenderer { get; private set; }

    // *******************************************************
    // 基本メソッド
    // *******************************************************

    /// <summary>
    /// コンポーネント生成時処理
    /// </summary>
    private void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// マウスクリック時処理
    /// </summary>
    private void OnMouseDown()
    {
        if (Card.CardProperty != CardProperty.Hand) return;

        // ドラッグ状態にフラグ変更
        _isDragging = true;

        // ドラッグ開始時の位置を記録
        _originalPosition = transform.position;

        // マウスとオブジェクトの位置の差分を算出
        _offset = transform.position - GetMouseWorldPosition();
    }

    /// <summary>
    /// マウスクリック解除時処理
    /// </summary>
    private void OnMouseUp()
    {
        if (!_isDragging) return;

        // ドラッグ解除状態にフラグ変更
        _isDragging = false;

        // 位置を変更できるエリアにいる場合に以下を処理
        if (IsValidDropArea())
        {
            /*
            // ドロップ先にスナップ
            transform.position = _currentDropTarget.transform.position;

            // ソート順：既存FieldCardの最大orderを1つ上回る
            int maxOrder = 0;
            foreach (var col in Physics2D.OverlapPointAll(_currentDropTarget.transform.position))
            {
                if (col.CompareTag(TAG_FIELD))
                {
                    var sr = col.GetComponent<SpriteRenderer>();
                    if (sr != null && sr.sortingOrder > maxOrder)
                    {
                        maxOrder = sr.sortingOrder;
                    }
                }
            }

            // レイヤー・タグ変更
            SpriteRenderer.sortingLayerName = "Field";
            SpriteRenderer.sortingOrder = maxOrder + 1;
            tag = TAG_FIELD;

            Debug.Log("HandCard を FieldCard にドロップしました（Layer変更済）");
            */
        }
        else
        {
            /*
            transform.position = _originalPosition;
            SpriteRenderer.sortingLayerName = SORT_LAYER_FIELD;
            */
        }

        _currentDropTarget = null;
    }

    private void Update()
    {
        if (_isDragging)
        {
            // マウスとの差分だけ常に移動させる
            transform.position = GetMouseWorldPosition() + _offset;
        }
    }

    // *******************************************************
    // メソッド
    // *******************************************************

    /// <summary>
    /// マウスのスクリーン座標をワールド座標に変換
    /// </summary>
    /// <returns></returns>
    private Vector3 GetMouseWorldPosition()
    {
        // 画面上のマウスの位置で初期化
        Vector3 screenPos = Input.mousePosition;

        // Z座標をカメラからの距離とする
        screenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;

        // スクリーン座標（x, y, z）をワールド座標に変換
        return Camera.main.ScreenToWorldPoint(screenPos);
    }

    private bool IsValidDropArea()
    {
        return _currentDropTarget != null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        /*
        if (CompareTag(TAG_HAND) && other.CompareTag(TAG_FIELD))
        {
            _currentDropTarget = other.gameObject;
            Debug.Log("HandCard が FieldCard に重なった");
        }
        */
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (_currentDropTarget == other.gameObject)
        {
            _currentDropTarget = null;
            Debug.Log("HandCard が FieldCard から離れた");
        }
    }

    /// <summary>
    /// カード情報を更新
    /// </summary>
    /// <param name="card"></param>
    public void SetCard(Card card)
    {
        Card = card;
    }

    /// <summary>
    /// ソート情報を更新
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
    /// 表裏に応じて表示スプライトを更新
    /// </summary>
    public void SetSprite(bool isFaceUp)
    {
        if (Card == null || SpriteRenderer == null) return;

        if (isFaceUp)
        {
            SpriteRenderer.sprite = Card.FaceSprite;
        }
        else
        {
            SpriteRenderer.sprite = Card.BackSprite;
        }
    }

}
