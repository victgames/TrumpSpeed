using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Define;

/// <summary>
/// �X�^�[�g�V�[���̃��C���������`����N���X
/// </summary>
public class StartDirector : MonoBehaviour
{
    // *******************************************************
    // �����o�ϐ�
    // *******************************************************

    /// <summary>
    /// ��Փx
    /// </summary>
    [SerializeField]
    private TMP_Dropdown _difficulty;

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
    private void OnStartButton()
    {
        int difficulty = _difficulty.value;

        switch (difficulty)
        {
            case 0:     // ����
                GameSettings.SetGameSettings(SuitColorMode.All, UseJoker.None, BackSpriteColor.Red);
                break;

            case 1:     // �ނ�������
                GameSettings.SetGameSettings(SuitColorMode.All, UseJoker.Two, BackSpriteColor.Red);
                break;

            case 2:     // �ӂ�
                GameSettings.SetGameSettings(SuitColorMode.BlackOnly, UseJoker.Two, BackSpriteColor.Red);
                break;

            case 3:     // �₳����
                GameSettings.SetGameSettings(SuitColorMode.SpadeOnly, UseJoker.One, BackSpriteColor.Red);
                break;

            default:    // �G���[��h�����ߑ��
                GameSettings.SetGameSettings(SuitColorMode.BlackOnly, UseJoker.Two, BackSpriteColor.Red);
                break;
        }

        // �Q�[���V�[���ɑJ��
        SceneManager.LoadScene("GameScene");
    }

    /// <summary>
    /// �N���[�Y�{�^���N���b�N������
    /// </summary>
    private void OnCloseButton()
    {
        // �A�v���I��
        Application.Quit();
#if UNITY_EDITOR

        // �G�f�B�^��ł��~�߂�
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
