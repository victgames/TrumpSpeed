using UnityEngine;
using UnityEngine.SceneManagement;

public class StartDirector : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    public void OnStartButton()
    {
        SceneManager.LoadScene("GameScene"); // シーン名で遷移
    }

    /// <summary>
    /// 
    /// </summary>
    public void OnCloseButton()
    {
        Application.Quit(); // アプリ終了
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // エディタ上でも止める
        #endif
    }
}
