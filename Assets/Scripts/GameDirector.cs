#nullable enable
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class GameDirector : MonoBehaviour
{
    // *******************************************************
    // メンバ変数
    // *******************************************************

    /// <summary>
    /// カードを生成するコンポーネント
    /// </summary>
    [SerializeField] 
    private CardGenerator _cardGenerator;

    /// <summary>
    /// カード操作を管理するコンポーネント
    /// </summary>
    [SerializeField]
    private CardManager _cardManager;

    /// <summary>
    /// 山札リスト（赤）
    /// </summary>
    private List<CardEntry?> _DeckRed = new List<CardEntry?>();

    /// <summary>
    /// 場札リスト（赤）
    /// </summary>
    private List<CardEntry?> _FieldRed = new List<CardEntry?>();

    /// <summary>
    /// 手札リスト（赤）
    /// </summary>
    private List<CardEntry?> _HandRed = new List<CardEntry?>();



    // *******************************************************
    // プロパティ
    // *******************************************************

    /// <summary>
    /// 手札枚数情報
    /// </summary>
    public bool[] HandCardCounts { get; private set; } = new bool[4];

    /// <summary>
    /// 場札枚数情報
    /// </summary>
    public int[] FieldCardCounts { get; private set; } = new int[4];


    // *******************************************************
    // 基本メソッド
    // *******************************************************

    /// <summary>
    /// 開始時処理
    /// </summary>
    void Start()
    {
        // カードリストを作成
        _DeckRed = _cardGenerator.InitializeEntries(SuitColorMode.SpadeOnly, UseJoker.One, BackSpriteColor.Red, CardProperty.Deck);
        //List<Card> cardList = _cardGenerator.GenerateCardList(SuitColorMode.SpadeOnly, UseJoker.One, BackSpriteColor.Red);
        //_DeckRed

        // シャッフル後山札を表示
        _cardManager.Shuffle(_DeckRed);
        _cardManager.DisplayDeck(_DeckRed, Position.Deck, Position.DeckOffset);

        // 場札, 手札を引く
        _cardManager.DrawTopCard(_DeckRed, _FieldRed, CardProperty.Field, Position.Field);
        _cardManager.DrawTopCard(_DeckRed, _HandRed, CardProperty.Hand, Position.Hand);
    }

    private void OnEnable()
    {
        CardController.OnDroppedToField += HandleCardDropped;
    }

    private void OnDisable()
    {
        CardController.OnDroppedToField -= HandleCardDropped;
    }

    // *******************************************************
    // メソッド
    // *******************************************************



    public void HandleCardDropped(CardController controller)
    {
        if (controller == null || controller.Card == null) return;

        // 所属を更新
        controller.Card.CardProperty = CardProperty.Field;

        // ソート順は現在の場札リストの枚数
        int order = _FieldRed.Count;

        // 表示順・レイヤー設定
        controller.SetSorting(SortLayers.Name(CardProperty.Field), order);

        // 表示位置（必要に応じて）
        if (order < Position.Field.Count)
        {
            controller.transform.position = Position.Field[order];
        }

        // カードエントリとして管理
        var entry = new CardEntry(controller.Card, controller, 0);
        _FieldRed.Add(entry);

        Debug.Log($"カード {controller.Card.ToString()} を場札に追加（順序: {order}）");
    }


}
