using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UIに関する処理を定義するクラス
/// </summary>
public class UIManager : MonoBehaviour
{
    // *******************************************************
    // メンバ変数
    // *******************************************************

    /// <summary>
    /// 山札の引き直しをするボタン用コンポーネント
    /// </summary>
    [SerializeField]
    private GameObject _redistributionButton;

    /// <summary>
    /// スタートシーンに進むボタン用コンポーネント
    /// </summary>
    [SerializeField]
    private GameObject _nextButton;

    /// <summary>
    /// ゲーム終了時に表示するパネル
    /// </summary>
    [SerializeField]
    private GameObject _gameFinishPanel;

    /// <summary>
    /// ゲームクリア時に時に表示するテキスト
    /// </summary>
    [SerializeField]
    private GameObject _clearText;

    /// <summary>
    /// ゲームオーバー時に時に表示するテキスト
    /// </summary>
    [SerializeField]
    private GameObject _gameOverText;

    // *******************************************************
    // Unityメソッド
    // *******************************************************

    /// <summary>
    /// 起動時処理
    /// </summary>
    private void Awake()
    {
        _redistributionButton.GetComponent<Button>().interactable = false;

        _gameFinishPanel.SetActive(false);
        _nextButton.SetActive(false);
        _clearText.SetActive(false);
        _gameOverText.SetActive(false);
    }

    // *******************************************************
    // メソッド
    // *******************************************************

    /// <summary>
    /// ゲーム終了パネル表示
    /// </summary>
    /// <param name="gameResult">ゲームクリア or ゲームオーバー</param>
    public void DisplayPanel(bool gameResult)
    {
        _gameFinishPanel.SetActive(true);
        _nextButton.SetActive(true);
        if (gameResult)
        {
            _clearText.SetActive(true);
        }
        else
        {
            _gameOverText.SetActive(true);
        }
    }

    /// <summary>
    /// 山札引き直しボタン有効化
    /// </summary>
    /// <param name="enable">ボタンを有効化するか</param>
    public void DisplayRedistributionButton(bool enable)
    {
        _redistributionButton.GetComponent<Button>().interactable = enable;
    }
}
