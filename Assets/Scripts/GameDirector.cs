#nullable enable
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine;
using UnityEngine.UI; // ← これを追加！
using static Define;
using static UnityEngine.EventSystems.EventTrigger;


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
    /// UI操作を管理するコンポーネント
    /// </summary>
    [SerializeField]
    private UIManager _uiManager;

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
    /// インスタンス
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
        List<Card> cardList = _cardGenerator.InitializeCardList(SuitColorMode.SpadeOnly, UseJoker.None, BackSpriteColor.Red);
        _entriesDeckRed = _cardGenerator.InitializeEntries(cardList);

        // シャッフル後山札を表示
        _cardManager.Shuffle(_entriesDeckRed);

        for (int i = 0; i < _entriesDeckRed.Count; i++)
        {
            Vector3 position = Position.Deck + Position.DeckOffset * i;
            _cardManager.UpdateEntry(_entriesDeckRed[i], position, false, CardProperty.Deck, i, 0);
        }

        // 場札, 手札を引く
        for (int slotIndex = 0; slotIndex < Position.Field.Count; slotIndex++)
        {
            _cardManager.DrawTopCard(_entriesDeckRed, _entriesFieldRed, CardProperty.Field, Position.Field[slotIndex], slotIndex);
        }
        for (int slotIndex = 0; slotIndex < Position.Hand.Count; slotIndex++)
        {
            _cardManager.DrawTopCard(_entriesDeckRed, _entriesHandRed, CardProperty.Hand, Position.Hand[slotIndex], slotIndex);
        }

        MargeHandCardAndDraw();
    }

    /// <summary>
    /// 手札入れ替えボタンクリック時操作
    /// </summary>
    public void OnRedistributionButtonClick()
    {
        _uiManager.DisplayRedistributionButton(false);

        MargeHandCardAndDraw();
    }

    /// <summary>
    /// スタートシーン以降ボタンクリック時操作
    /// </summary>
    public void OnNextButtonClick()
    {
        SceneManager.LoadScene("StartScene"); // シーン名で遷移
    }

    // *******************************************************
    // メソッド
    // *******************************************************

    public void TransferCardEntry(GameObject selectedObj, GameObject targetObj)
    {
        int slotIndex = selectedObj.GetComponent<CardController>().Card.SlotIndex;

        CardEntry? selectedEntry = _entriesHandRed.FirstOrDefault(entry => entry.View != null && entry.View.gameObject == selectedObj);
        CardEntry? targetEntry = _entriesFieldRed.FirstOrDefault(entry => entry.View != null && entry.View.gameObject == targetObj);

        _cardManager.UpdateEntry(selectedEntry, targetEntry.View.transform.position, true, CardProperty.Field, targetEntry.View.SpriteRenderer.sortingOrder + 1, targetEntry.Data.SlotIndex);

        _entriesHandRed.Remove(selectedEntry);
        _entriesFieldRed.Add(selectedEntry);

        

        //CardEntry? entry = _entriesFieldRed.FirstOrDefault(entry => entry.View != null && entry.View.gameObject == gameObject);

        //_cardManager.UpdateEntry(entry, Position.None, false, CardProperty.None, 0, 0);

        targetObj.SetActive(false);

        _entriesFieldRed.Remove(targetEntry);
        _entriesNoneRed.Add(targetEntry);

        _cardManager.DrawTopCard(_entriesDeckRed, _entriesHandRed, CardProperty.Hand, Position.Hand[slotIndex], slotIndex);

        JudgeGameFinish();

    }

    /*
    /// <summary>
    /// 手札を1枚追加する
    /// </summary>
    public void AddHandCard(int slotIndex)
    {
        _cardManager.DrawTopCard(_entriesDeckRed, _entriesHandRed, CardProperty.Hand, Position.Hand[slotIndex], slotIndex);
    }
    */

    /*
    /// <summary>
    /// Noneを1枚追加する
    /// </summary>
    /// <param name="slotIndex"></param>
    public void AddNoneCard(GameObject gameObject)
    {
        CardEntry? entry = _entriesFieldRed.FirstOrDefault(entry => entry.View != null && entry.View.gameObject == gameObject);

        _cardManager.UpdateEntry(entry, Position.None, false, CardProperty.None, 0, 0);
        gameObject.SetActive(false);
        
        _entriesFieldRed.Remove(entry);
        _entriesNoneRed.Add(entry);

        JudgeGameFinish();
    }
    */

    /// <summary>
    /// 
    /// </summary>
    private void JudgeGameFinish()
    {
        if (_entriesDeckRed.Count == 0 && _entriesHandRed.Count == 0)
        {
            _uiManager.DisplayPanel(true);
            //SceneManager.LoadScene("ClearScene"); // シーン名で遷移
        }
        else
        {
            bool judgeDeck = SpeedRule.JudgeSequential(_entriesFieldRed, _entriesDeckRed);
            bool judgeHand = SpeedRule.JudgeSequential(_entriesFieldRed, _entriesHandRed);

            if (!judgeDeck && !judgeHand)
            {
                // これ以上は重ねられないためGameOver
                _uiManager.DisplayPanel(false);

                //SceneManager.LoadScene("ClearScene"); // シーン名で遷移
            }
            else if (!judgeHand)
            {
                _uiManager.DisplayRedistributionButton(true);
            }
        }
    }

    /// <summary>
    /// 手札入れ替え管理
    /// </summary>
    private void MargeHandCardAndDraw()
    {
        while (!SpeedRule.JudgeSequential(_entriesFieldRed, _entriesHandRed))
        {
            // 山札と手札を混ぜる
            _cardManager.MergeCardEntries(_entriesHandRed, _entriesDeckRed);

            // シャッフル後山札を表示
            _cardManager.Shuffle(_entriesDeckRed);
            for (int i = 0; i < _entriesDeckRed.Count; i++)
            {
                Vector3 position = Position.Deck + Position.DeckOffset * i;
                _cardManager.UpdateEntry(_entriesDeckRed[i], position, false, CardProperty.Deck, i, 0);
            }

            // 手札を引く
            for (int slotIndex = 0; slotIndex < Position.Hand.Count; slotIndex++)
            {
                _cardManager.DrawTopCard(_entriesDeckRed, _entriesHandRed, CardProperty.Hand, Position.Hand[slotIndex], slotIndex);
            }
        }

        /*
        do
        {
            // 山札と手札を混ぜる
            _cardManager.MergeCardEntries(_entriesHandRed, _entriesDeckRed);

            // シャッフル後山札を表示
            _cardManager.Shuffle(_entriesDeckRed);
            for (int i = 0; i < _entriesDeckRed.Count; i++)
            {
                Vector3 position = Position.Deck + Position.DeckOffset * i;
                _cardManager.UpdateEntry(_entriesDeckRed[i], position, false, CardProperty.Deck, i, 0);
            }

            // 手札を引く
            for (int slotIndex = 0; slotIndex < Position.Hand.Count; slotIndex++)
            {
                _cardManager.DrawTopCard(_entriesDeckRed, _entriesHandRed, CardProperty.Hand, Position.Hand[slotIndex], slotIndex);
            }
        }
        while (!SpeedRule.JudgeSequential(_entriesFieldRed, _entriesHandRed));
        */
    }
    
}
