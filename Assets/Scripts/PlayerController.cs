using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using static Define;

public class PlayerController : MonoBehaviour
{
    // *******************************************************
    // メンバ変数
    // *******************************************************

    /// <summary>
    /// 選択したカード
    /// </summary>
    private GameObject _selectedCard;

    /// <summary>
    /// ターゲットのカード
    /// </summary>
    private GameObject _targetCard = null;

    /// <summary>
    /// 元の位置
    /// </summary>
    private Vector3 _originalPosition;

    /// <summary>
    /// オブジェクトとマウス位置の差分
    /// </summary>
    private Vector3 _offset;

    /// <summary>
    /// 移動（ドラッグ）フラグ
    /// </summary>
    private bool _dragFlag = false;
    
    /// <summary>
    /// スナップ（位置合わせ）フラグ
    /// </summary>
    private bool _snapFlag = false;


    // *******************************************************
    // Unityメソッド
    // *******************************************************

    /// <summary>
    /// 毎フレーム呼ばれる処理
    /// </summary>
    private void Update()
    {
        // マウスボタンクリック時
        if (Input.GetMouseButtonDown(0))
        {
            TryStartDrag();
        }

        // マウスボタンクリック中
        if (Input.GetMouseButton(0))
        {
            HandleDragging();
        }

        // マウスボタンクリック後
        if (Input.GetMouseButtonUp(0))
        {
            HandleDrop();
        }
    }

    // *******************************************************
    // メソッド
    // *******************************************************

    /// <summary>
    /// マウス位置のカードを選択し、ドラッグ開始の準備を行う
    /// </summary>
    private void TryStartDrag()
    {
        GameObject selected = TrySelectCard();
        if (selected == null) return;

        var cardCtrl = selected.GetComponent<CardController>();

        // 手札のカードのみドラッグ可能
        if (cardCtrl == null || cardCtrl.Card.CardProperty != CardProperty.Hand) return;

        // オブジェクトとフラグの初期化
        _selectedCard = selected;
        _dragFlag = true;
        _snapFlag = false;

        // 位置を保存
        _originalPosition = _selectedCard.transform.position;
        _offset = _selectedCard.transform.position - GetMouseWorldPosition();

        // 選択中は色を緑に変更（視覚フィードバック）
        _selectedCard.GetComponent<SpriteRenderer>().color = Color.green;

        // sortingOrderを一時的に 固定値1 にする
        _selectedCard.GetComponent<CardController>()?.SetSortingOrder(1);
    }

    /// <summary>
    /// ドラッグ中のカードをマウス位置に追従させ、
    /// Field上のカードと重なった場合は見た目だけスナップする
    /// </summary>
    private void HandleDragging()
    {
        if (!_dragFlag || _selectedCard == null)
            return;

        // マウスの位置に合わせてカードを動かす（掴んだ位置のズレを考慮）
        _selectedCard.transform.position = GetMouseWorldPosition() + _offset;

        // カードにCardControllerが付いているか確認（安全対策）
        if (_selectedCard.GetComponent<CardController>() == null) return;

        // オブジェクトとフラグの初期化
        _snapFlag = false;
        _targetCard = null;

        // ドラッグ中のカードの位置を中心に、半径0.1の円で周囲のColliderを全て取得
        Collider2D[] hits = Physics2D.OverlapCircleAll(_selectedCard.transform.position, 0.1f);

        foreach (var hit in hits)
        {
            // 自分自身（ドラッグ中のカード）との接触は無視
            if (hit.gameObject == _selectedCard) continue;

            // 相手がCardControllerを持っているか確認
            var targetCard = hit.GetComponent<CardController>();

            // Fieldカードに当たっていたらスナップ対象として記録
            if (targetCard != null && targetCard.Card.CardProperty == CardProperty.Field)
            {
                // 数字の条件を SpeedRule で判定
                if (SpeedRule.IsSequential(_selectedCard.GetComponent<CardController>()?.Card, targetCard.Card))
                {
                    _snapFlag = true; // スナップ対象あり
                    _targetCard = targetCard.gameObject; // スナップ先を記憶
                    break; // 最初に見つかった1枚でOK
                }
            }
        }

        // 重なっていたら見た目だけスナップ（位置を合わせる）
        if (_snapFlag && _targetCard != null)
        {
            _selectedCard.transform.position = _targetCard.transform.position;
        }
    }

    /// <summary>
    /// ドラッグ終了時の処理。カードをスナップ先に正式配置するか、
    /// ドロップ失敗なら元の位置に戻す
    /// </summary>
    private void HandleDrop()
    {
        if (!_dragFlag)
            return;

        // フラグの初期化
        _dragFlag = false;

        if (_selectedCard != null)
        {
            var draggedCard = _selectedCard.GetComponent<CardController>();
            var sr = _selectedCard.GetComponent<SpriteRenderer>();

            if (_snapFlag && _targetCard != null)
            {
                // ドロップ成功

                // カードの状態を targetCard と同じにする
                draggedCard.Card.CardProperty = CardProperty.Field;

                var targetCard = _targetCard.GetComponent<CardController>();
                int newSlotIndex = 0;
                int newOrder = 0;
                if (targetCard != null)
                {
                    newSlotIndex = (int)targetCard.Card.SlotIndex;
                    newOrder = targetCard.SpriteRenderer.sortingOrder;
                }

                int oldSlotIndex = (int)draggedCard.Card.SlotIndex;

                // 場のスロット位置を targetCard と同じにする
                draggedCard.Card.SlotIndex = newSlotIndex;

                // sortingLayerを targetCard と同じ, sortingOrderを targetCard + 1 に変更
                draggedCard.SetCardProperty(CardProperty.Field);
                draggedCard.SetSortingOrder(newOrder + 1);

                GameDirector.Instance.AddHandCard(oldSlotIndex);
            }
            else
            {
                // ドロップ失敗：元の位置に戻す
                _selectedCard.transform.position = _originalPosition;

                // sortingOrderを 0 に変更
                draggedCard.SetSortingOrder(0);
            }

            // 選択色を元に戻す
            _selectedCard.GetComponent<SpriteRenderer>().color = Color.white;
        }

        // 状態をリセット
        _selectedCard = null;
        _snapFlag = false;
        _targetCard = null;
    }

    /// <summary>
    /// マウス位置にあるカードのGameObjectを取得する
    /// </summary>
    /// <returns>カードのGameObject</returns>
    private GameObject TrySelectCard()
    {
        // マウス座標をワールド座標に変換
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // マウス位置を起点にレイキャスト（点検出）
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);

        return hit.collider?.gameObject; // ヒットしたらゲームオブジェクトを返す、なければnull
    }

    /// <summary>
    /// マウスのスクリーン座標をワールド座標に変換して取得する
    /// </summary>
    /// <returns>マウスのワールド座標</returns>
    private Vector3 GetMouseWorldPosition()
    {
        Vector3 screenPos = Input.mousePosition;
        screenPos.z = 0f; // 2Dの場合はZ軸は0固定（3Dならカメラとの距離を設定すること）

        return Camera.main.ScreenToWorldPoint(screenPos);
    }
}
