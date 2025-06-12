using UnityEngine;
using static Define;
using static Define.Card;

public class CardController : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    /// <summary>このカードが持つスプライト（表・裏）</summary>
    private Sprite _faceSprite;
    private Sprite _backSprite;

    private bool _isDragging = false;
    private Vector3 _offset;

    private Vector3 _originalPosition;

    [SerializeField]
    private GameObject _currentDropTarget = null;



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

    void OnMouseDown()
    {
        Debug.Log("Clicked!");
        //_spriteRenderer.color = Color.red;
        _isDragging = true;
        //_offset = transform.position - GetMouseWorldPosition();
        _originalPosition = transform.position;
    }

    void OnMouseUp()
    {
        _isDragging = false;
        //_spriteRenderer.color = Color.white;

        if (IsValidDropArea())
        {
            // そのまま配置（例としてなにもしない）
            Debug.Log("有効なドロップエリア！");
        }
        else
        {
            // 元の位置に戻す
            transform.position = _originalPosition;
        }
    }

    void Update()
    {
        if (_isDragging)
        {
            transform.position = GetMouseWorldPosition() + _offset;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                Debug.Log("クリックされたオブジェクト: " + hit.collider.name);
            }
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 screenPos = Input.mousePosition;
        screenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(screenPos);
    }

    private bool IsValidDropArea()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider != null && hit.collider.CompareTag("DropArea"))
        {
            _currentDropTarget = hit.collider.gameObject;
            return true;
        }

        _currentDropTarget = null;
        return false;
    }

}
