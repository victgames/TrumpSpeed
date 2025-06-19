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
    private List<CardEntry> _DeckRed = new List<CardEntry>();

    /// <summary>
    /// 場札リスト（赤）
    /// </summary>
    private List<CardEntry> _FieldRed = new List<CardEntry>();

    /// <summary>
    /// 手札リスト（赤）
    /// </summary>
    private List<CardEntry> _HandRed = new List<CardEntry>();



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

        // シャッフル後山札を表示
        _cardManager.Shuffle(_DeckRed);
        _cardManager.DisplayDeck(_DeckRed, Position.Deck, Position.DeckOffset);

        // 場札, 手札を引く
        _cardManager.DrawTopCard(_DeckRed, _FieldRed, CardProperty.Field, Position.Field);
        _cardManager.DrawTopCard(_DeckRed, _HandRed, CardProperty.Hand, Position.Hand);
    }

    // *******************************************************
    // メソッド
    // *******************************************************

    


}
