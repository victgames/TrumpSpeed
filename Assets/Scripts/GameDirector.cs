#nullable enable
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲームシーンのメイン処理
/// </summary>
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
    /// 山札リスト
    /// </summary>
    private List<CardEntry> _entriesDeck = new List<CardEntry>();

    /// <summary>
    /// 場札リスト
    /// </summary>
    private List<CardEntry> _entriesField = new List<CardEntry>();

    /// <summary>
    /// 手札リスト
    /// </summary>
    private List<CardEntry> _entriesHand = new List<CardEntry>();

    /// <summary>
    /// 使用済みカードリスト
    /// </summary>
    private List<CardEntry> _entriesNone = new List<CardEntry>();

    /// <summary>
    /// ゲームBGM音オーディオソース
    /// </summary>
    public AudioSource _gameBGMAudioSource;

    /// <summary>
    /// 正解音オーディオソース
    /// </summary>
    public AudioSource _correctAudioSource;

    /// <summary>
    /// クリア音オーディオソース
    /// </summary>
    public AudioSource _clearAudioSource;

    /// <summary>
    /// ゲームオーバー音オーディオソース
    /// </summary>
    public AudioSource _gameOverAudioSource;


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
            // 複数生成されていたら削除
            Destroy(gameObject);
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
        _entriesDeck = _cardGenerator.InitializeEntries(GameSettings.SuitColorMode, GameSettings.UseJoker, GameSettings.BackSpriteColor);

        // 山札, 手札, 場札を表示
        _cardManager.ArrangeFieldCards(_entriesDeck, _entriesField, _entriesHand, true);
    }

    /// <summary>
    /// 手札入れ替えボタンクリック時操作
    /// </summary>
    private void OnRedistributionButtonClick()
    {
        _uiManager.DisplayRedistributionButton(false);

        _cardManager.ArrangeFieldCards(_entriesDeck, _entriesField, _entriesHand, false);
    }

    /// <summary>
    /// スタートシーン以降ボタンクリック時操作
    /// </summary>
    private void OnNextButtonClick()
    {
        SceneManager.LoadScene("StartScene"); // シーン名で遷移
    }

    // *******************************************************
    // メソッド
    // *******************************************************

    /// <summary>
    /// ゲーム終了を判定
    /// 手札を場札に重ねた直後に呼び出し
    /// </summary>
    /// <param name="selectedObj">選択されたカード</param>
    /// <param name="targetObj">重ねる対象のカード</param>
    public void JudgeGameFinish(GameObject selectedObj, GameObject targetObj)
    {
        // 場のカードの位置と役割を更新
        _cardManager.TransferCardEntry(_entriesDeck, _entriesField, _entriesHand, _entriesNone, selectedObj, targetObj);

        // 山札と手札をすべて使い切っているか
        if (_entriesDeck.Count == 0 && _entriesHand.Count == 0)
        {
            // 「ゲームクリア」を表示
            _uiManager.DisplayPanel(true);

            // ゲームBGMを止める
            _gameBGMAudioSource.Stop();

            // クリアBGMを再生
            _clearAudioSource.Play();
        }
        else
        {
            // 重ねられるカードがあるか判定
            bool judgeHand = SpeedRule.JudgeSequential(_entriesField, _entriesHand);
            bool judgeDeck = SpeedRule.JudgeSequential(_entriesField, _entriesDeck);

            // 山札と手札に重ねられるカードがない
            if (!judgeHand && !judgeDeck)
            {
                // これ以上は重ねられないためGameOver
                _uiManager.DisplayPanel(false);

                // ゲームBGMを止める
                _gameBGMAudioSource.Stop();

                // ゲームオーバーBGMを再生
                _gameOverAudioSource.Play();
            }
            else
            {
                if (!judgeHand)
                {
                    // 「山札切り直し」ボタン表示
                    _uiManager.DisplayRedistributionButton(true);
                }
                // 正解音再生
                _correctAudioSource.Play();
            }
        }
    }
}
