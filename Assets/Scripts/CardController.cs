using UnityEngine;
using static Define;
using static Define.Card;

public class CardController : MonoBehaviour
{
    // *******************************************************
    // メンバ変数
    // *******************************************************

    //public SpriteRenderer _spriteRenderer;

    private Sprite _faceSprite;
    private Sprite _backSprite;

    private bool _isDragging = false;
    private Vector3 _offset;
    private Vector3 _originalPosition;

    private GameObject _currentDropTarget = null;

    // *******************************************************
    // プロパティ
    // *******************************************************

    public Card Card { get; set; }

    public SpriteRenderer SpriteRenderer { get; set; }

    // *******************************************************
    // 基本メソッド
    // *******************************************************

    private void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // *******************************************************
    // メソッド
    // *******************************************************

    /*
    public void SetCard(Card card, Sprite faceSprite, Sprite backSprite)
    {
        Card = card;
        _faceSprite = faceSprite;
        _backSprite = backSprite;
        UpdateSprite();
    }
    */

    /*
    public void SetFaceUp(bool isFaceUp)
    {
        if (Card == null) return;

        Card.IsFaceUp = isFaceUp;
        UpdateSprite();
    }
    */

    public void UpdateSprite()
    {
        if (Card == null || SpriteRenderer == null) return;

        SpriteRenderer.sprite = Card.IsFaceUp ? Card.FaceSprite : Card.BackSprite;
    }

    private void OnMouseDown()
    {
        if (!CompareTag(TAG_HAND)) return;

        _isDragging = true;
        _originalPosition = transform.position;
        _offset = transform.position - GetMouseWorldPosition();
    }

    private void OnMouseUp()
    {
        if (!_isDragging) return;

        _isDragging = false;

        if (IsValidDropArea())
    {
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
    }
        else
        {
            transform.position = _originalPosition;
            SpriteRenderer.sortingLayerName = SORT_LAYER_FIELD;
        }

        _currentDropTarget = null;
    }

    private void Update()
    {
        if (_isDragging)
        {
            transform.position = GetMouseWorldPosition() + _offset;
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 screenPos = Input.mousePosition;
        screenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(screenPos);
    }

    private bool IsValidDropArea()
    {
        return _currentDropTarget != null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (CompareTag(TAG_HAND) && other.CompareTag(TAG_FIELD))
        {
            _currentDropTarget = other.gameObject;
            Debug.Log("HandCard が FieldCard に重なった");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (_currentDropTarget == other.gameObject)
        {
            _currentDropTarget = null;
            Debug.Log("HandCard が FieldCard から離れた");
        }
    }

    public void SetSorting(string layerName, int order)
    {
        if (SpriteRenderer == null) return;

        SpriteRenderer.sortingLayerName = layerName;
        SpriteRenderer.sortingOrder = order;
    }
}
