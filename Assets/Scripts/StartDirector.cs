using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Define;

/// <summary>
/// スタートシーンのメイン処理を定義するクラス
/// </summary>
public class StartDirector : MonoBehaviour
{
    // *******************************************************
    // メンバ変数
    // *******************************************************

    /// <summary>
    /// 難易度
    /// </summary>
    [SerializeField]
    private TMP_Dropdown _difficulty;

    // *******************************************************
    // Unityメソッド
    // *******************************************************

    /// <summary>
    /// ゲーム開始前の初期化処理
    /// </summary>
    private void Awake()
    {
        // 難易度「ふつう」を初期に設定
        _difficulty.value = 2;
    }

    /// <summary>
    /// スタートボタンクリック時操作
    /// </summary>
    private void OnStartButton()
    {
        int difficulty = _difficulty.value;

        switch (difficulty)
        {
            case 0:     // おに
                GameSettings.SetGameSettings(SuitColorMode.All, UseJoker.None, BackSpriteColor.Red);
                break;

            case 1:     // むずかしい
                GameSettings.SetGameSettings(SuitColorMode.All, UseJoker.Two, BackSpriteColor.Red);
                break;

            case 2:     // ふつう
                GameSettings.SetGameSettings(SuitColorMode.BlackOnly, UseJoker.Two, BackSpriteColor.Red);
                break;

            case 3:     // やさしい
                GameSettings.SetGameSettings(SuitColorMode.SpadeOnly, UseJoker.One, BackSpriteColor.Red);
                break;

            default:    // エラーを防ぐため代入
                GameSettings.SetGameSettings(SuitColorMode.BlackOnly, UseJoker.Two, BackSpriteColor.Red);
                break;
        }

        // ゲームシーンに遷移
        SceneManager.LoadScene("GameScene");
    }

    /// <summary>
    /// クローズボタンクリック時操作
    /// </summary>
    private void OnCloseButton()
    {
        // アプリ終了
        Application.Quit();
#if UNITY_EDITOR

        // エディタ上でも止める
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
