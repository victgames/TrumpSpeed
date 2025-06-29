using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI�Ɋւ��鏈�����`����N���X
/// </summary>
public class UIManager : MonoBehaviour
{
    // *******************************************************
    // �����o�ϐ�
    // *******************************************************

    /// <summary>
    /// �R�D�̈�������������{�^���p�R���|�[�l���g
    /// </summary>
    [SerializeField]
    private GameObject _redistributionButton;

    /// <summary>
    /// �X�^�[�g�V�[���ɐi�ރ{�^���p�R���|�[�l���g
    /// </summary>
    [SerializeField]
    private GameObject _nextButton;

    /// <summary>
    /// �Q�[���I�����ɕ\������p�l��
    /// </summary>
    [SerializeField]
    private GameObject _gameFinishPanel;

    /// <summary>
    /// �Q�[���N���A���Ɏ��ɕ\������e�L�X�g
    /// </summary>
    [SerializeField]
    private GameObject _clearText;

    /// <summary>
    /// �Q�[���I�[�o�[���Ɏ��ɕ\������e�L�X�g
    /// </summary>
    [SerializeField]
    private GameObject _gameOverText;

    // *******************************************************
    // Unity���\�b�h
    // *******************************************************

    /// <summary>
    /// �N��������
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
    // ���\�b�h
    // *******************************************************

    /// <summary>
    /// �Q�[���I���p�l���\��
    /// </summary>
    /// <param name="gameResult">�Q�[���N���A or �Q�[���I�[�o�[</param>
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
    /// �R�D���������{�^���L����
    /// </summary>
    /// <param name="enable">�{�^����L�������邩</param>
    public void DisplayRedistributionButton(bool enable)
    {
        _redistributionButton.GetComponent<Button>().interactable = enable;
    }
}
