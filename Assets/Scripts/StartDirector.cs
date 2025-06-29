using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartDirector : MonoBehaviour
{
    // *******************************************************
    // �����o�ϐ�
    // *******************************************************

    /// <summary>
    /// ��Փx
    /// </summary>
    [SerializeField]
    public TMP_Dropdown _difficulty;

    // *******************************************************
    // Unity���\�b�h
    // *******************************************************

    /// <summary>
    /// �Q�[���J�n�O�̏���������
    /// </summary>
    private void Awake()
    {
        // ��Փx�u�ӂ��v�������ɐݒ�
        _difficulty.value = 2;
    }

    /// <summary>
    /// �X�^�[�g�{�^���N���b�N������
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

        SceneManager.LoadScene("GameScene"); // �V�[�����őJ��
    }

    /// <summary>
    /// �N���[�Y�{�^���N���b�N������
    /// </summary>
    public void OnCloseButton()
    {
        Application.Quit(); // �A�v���I��
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // �G�f�B�^��ł��~�߂�
        #endif
    }
}
