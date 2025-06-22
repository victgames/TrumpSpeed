using UnityEngine;
using UnityEngine.SceneManagement;

public class StartDirector : MonoBehaviour
{
    // Start�{�^���Ɋ��蓖�Ă�
    public void OnStartButton()
    {
        SceneManager.LoadScene("GameScene"); // �V�[�����őJ��
    }

    // Close�{�^���Ɋ��蓖�Ă�
    public void OnCloseButton()
    {
        Application.Quit(); // �A�v���I��
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // �G�f�B�^��ł��~�߂�
        #endif
    }
}
