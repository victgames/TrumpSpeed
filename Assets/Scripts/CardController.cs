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

    public static event Action<CardController> OnDroppedToField;

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
    /// 常時更新処理
    /// </summary>
    private void Update()
    {
        if (_isDragging)
        {
            // カードの位置をマウス位置にオフセットを加えて更新
            Vector3 mousePos = GetMouseWorldPosition();
            transform.position = mousePos + _offset;

            // ドロップ候補の初期化（毎フレームリセット）
            _currentDropTarget = null;

            // マウス位置にある全ての2DコライダーからFieldカードを探索
            Collider2D[] hits = Physics2D.OverlapPointAll(mousePos);
            foreach (var col in hits)
            {
                // CardPropertyがFieldの場合
                var cardCtrl = col.GetComponent<CardController>();
                if (cardCtrl != null && cardCtrl.Card != null && cardCtrl.Card.CardProperty == CardProperty.Field)
                {
                    // ドロップ対象候補として設定
                    _currentDropTarget = col.gameObject;
                    break;
                }
            }
        }
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
        if (_isDragging)
        {
            _isDragging = false;

            if (IsValidDropArea() && _currentDropTarget != null)
            {
                // _currentDropTarget の位置にスナップ
                transform.position = _currentDropTarget.transform.position;

                // イベント通知
                OnDroppedToField?.Invoke(this);
            }
            else
            {
                // 無効なら元の位置に戻す
                transform.position = _originalPosition;
            }
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
        // _currentDropTargetがnullならドロップ不可
        if (_currentDropTarget == null) return false;

        // _currentDropTargetのCardControllerを取得
        var cardCtrl = _currentDropTarget.GetComponent<CardController>();
        if (cardCtrl != null && cardCtrl.Card != null && cardCtrl.Card.CardProperty == CardProperty.Field)
        {
            return true;
        }

        return false;
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

    /*
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (CompareTag(TAG_HAND) && other.CompareTag(TAG_FIELD))
        {
            _currentDropTarget = other.gameObject;
            Debug.Log("HandCard が FieldCard に重なった");
        }
    }
    */

    /*
    private void OnTriggerExit2D(Collider2D other)
    {
        if (_currentDropTarget == other.gameObject)
        {
            _currentDropTarget = null;
            Debug.Log("HandCard が FieldCard から離れた");
        }
    }
    */

}
