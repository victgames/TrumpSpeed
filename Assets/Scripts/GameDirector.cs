#nullable enable
using System.Collections.Generic;
using System.Linq;
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

    /// <summary>
    /// 使用済みカードリスト（赤）
    /// </summary>
    private List<CardEntry> _entriesNoneRed = new List<CardEntry>();


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

    private void Update()
    {
        int a = 0;
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

    public void TransferCardEntry(GameObject selectedObj, GameObject targetObj)
    {
        CardEntry? selectedEntry = _entriesHandRed.FirstOrDefault(entry => entry.View != null && entry.View.gameObject == selectedObj);
        CardEntry? targetEntry = _entriesFieldRed.FirstOrDefault(entry => entry.View != null && entry.View.gameObject == targetObj);

        _cardManager.UpdateEntry(selectedEntry, targetEntry.View.transform.position, true, CardProperty.Field, targetEntry.View.SpriteRenderer.sortingOrder + 1, targetEntry.Data.SlotIndex);

        _entriesHandRed.Remove(selectedEntry);
        _entriesFieldRed.Add(selectedEntry);
    }

    /// <summary>
    /// 手札を1枚追加する
    /// </summary>
    public void AddHandCard(int slotIndex)
    {
        _cardManager.DrawTopCard(_entriesDeckRed, _entriesHandRed, CardProperty.Hand, Position.Hand[slotIndex], slotIndex);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="slotIndex"></param>
    public void AddUsedCard(GameObject Obj)
    {
        CardEntry? entry = _entriesFieldRed.FirstOrDefault(entry => entry.View != null && entry.View.gameObject == Obj);

        _cardManager.UpdateEntry(entry, Position.None, false, CardProperty.None, 0, 0);
        Obj.SetActive(false);
        
        _entriesFieldRed.Remove(entry);
        _entriesNoneRed.Add(entry);
    }
}
