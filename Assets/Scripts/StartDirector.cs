using UnityEngine;
using UnityEngine.SceneManagement;

public class StartDirector : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    public void OnStartButton()
    {
        SceneManager.LoadScene("GameScene"); // �V�[�����őJ��
    }

    /// <summary>
    /// 
    /// </summary>
    public void OnCloseButton()
    {
        Application.Quit(); // �A�v���I��
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // �G�f�B�^��ł��~�߂�
        #endif
    }
}
