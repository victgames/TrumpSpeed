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
    private List<CardEntry> _entriesDeckRed = new List<CardEntry>();

    /// <summary>
    /// 場札リスト（赤）
    /// </summary>
    private List<CardEntry> _entriesFieldRed = new List<CardEntry>();

    /// <summary>
    /// 手札リスト（赤）
    /// </summary>
    private List<CardEntry> _entriesHandRed = new List<CardEntry>();


    // *******************************************************
    // プロパティ
    // *******************************************************

    /// <summary>
    /// 
    /// </summary>
    public static GameDirector Instance { get; private set; }


    // *******************************************************
    // Unityメソッド
    // *******************************************************

    /// <summary>
    /// ゲーム開始前の初期化処理
    /// </summary>
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 複数生成されていたら削除
            return;
        }

        Instance = this;
    }

    /// <summary>
    /// ゲーム開始時の初期化処理
    /// </summary>
    private void Start()
    {
        // カードリストを作成
        List<Card> cardList = _cardGenerator.InitializeCardList(SuitColorMode.SpadeOnly, UseJoker.One, BackSpriteColor.Red);
        _entriesDeckRed = _cardGenerator.InitializeEntries(cardList);

        // シャッフル後山札を表示
        _cardManager.Shuffle(_entriesDeckRed);
        _cardManager.DisplayDeck(_entriesDeckRed, Position.Deck, Position.DeckOffset);

        // 場札, 手札を引く
        for (int slotIndex = 0; slotIndex < Position.Field.Count; slotIndex++)
        {
            _cardManager.DrawTopCard(_entriesDeckRed, _entriesFieldRed, CardProperty.Field, Position.Field[slotIndex], slotIndex);
        }
        for (int slotIndex = 0; slotIndex < Position.Hand.Count; slotIndex++)
        {
            _cardManager.DrawTopCard(_entriesDeckRed, _entriesHandRed, CardProperty.Hand, Position.Hand[slotIndex], slotIndex);
        }
    }

    /// <summary>
    /// 手札入れ替えボタンクリック時操作
    /// </summary>
    public void OnButtonClick()
    {
        // カードリストを作成
        _cardManager.MergeCardEntries(_entriesHandRed, _entriesDeckRed);

        // シャッフル後山札を表示
        _cardManager.Shuffle(_entriesDeckRed);
        _cardManager.DisplayDeck(_entriesDeckRed, Position.Deck, Position.DeckOffset);

        // 手札を引く
        for (int slotIndex = 0; slotIndex < Position.Hand.Count; slotIndex++)
        {
            _cardManager.DrawTopCard(_entriesDeckRed, _entriesHandRed, CardProperty.Hand, Position.Hand[slotIndex], slotIndex);
        }
    }

    // *******************************************************
    // メソッド
    // *******************************************************

    /// <summary>
    /// 手札を1枚追加する
    /// </summary>
    public void AddHandCard(int slotIndex)
    {
        _cardManager.DrawTopCard(_entriesDeckRed, _entriesHandRed, CardProperty.Hand, Position.Hand[slotIndex], slotIndex);
    }
}
