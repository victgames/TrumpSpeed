using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartDirector : MonoBehaviour
{
    // *******************************************************
    // メンバ変数
    // *******************************************************

    /// <summary>
    /// 難易度
    /// </summary>
    [SerializeField]
    public TMP_Dropdown _difficulty;

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
    public void OnStartButton()
    {
        int difficulty = _difficulty.value;

        switch (difficulty)
        {
            case 0:
                GameSettings.SuitColorMode = Define.SuitColorMode.All;
                GameSettings.UseJoker = Define.UseJoker.None;
                break;
            case 1:
                GameSettings.SuitColorMode = Define.SuitColorMode.All;
                GameSettings.UseJoker = Define.UseJoker.Two;
                break;
            case 2:
                GameSettings.SuitColorMode = Define.SuitColorMode.BlackOnly;
                GameSettings.UseJoker = Define.UseJoker.Two;
                break;
            case 3:
                GameSettings.SuitColorMode = Define.SuitColorMode.SpadeOnly;
                GameSettings.UseJoker = Define.UseJoker.One;
                break;
            default:
                break;
        }

        SceneManager.LoadScene("GameScene"); // シーン名で遷移
    }

    /// <summary>
    /// クローズボタンクリック時操作
    /// </summary>
    public void OnCloseButton()
    {
        Application.Quit(); // アプリ終了
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // エディタ上でも止める
        #endif
    }
}
