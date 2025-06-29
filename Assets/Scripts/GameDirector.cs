#nullable enable
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// �Q�[���V�[���̃��C������
/// </summary>
public class GameDirector : MonoBehaviour
{
    // *******************************************************
    // �����o�ϐ�
    // *******************************************************

    /// <summary>
    /// �J�[�h�𐶐�����R���|�[�l���g
    /// </summary>
    [SerializeField]
    private CardGenerator _cardGenerator;

    /// <summary>
    /// �J�[�h������Ǘ�����R���|�[�l���g
    /// </summary>
    [SerializeField]
    private CardManager _cardManager;

    /// <summary>
    /// UI������Ǘ�����R���|�[�l���g
    /// </summary>
    [SerializeField]
    private UIManager _uiManager;

    /// <summary>
    /// �R�D���X�g
    /// </summary>
    private List<CardEntry> _entriesDeck = new List<CardEntry>();

    /// <summary>
    /// ��D���X�g
    /// </summary>
    private List<CardEntry> _entriesField = new List<CardEntry>();

    /// <summary>
    /// ��D���X�g
    /// </summary>
    private List<CardEntry> _entriesHand = new List<CardEntry>();

    /// <summary>
    /// �g�p�ς݃J�[�h���X�g
    /// </summary>
    private List<CardEntry> _entriesNone = new List<CardEntry>();

    /// <summary>
    /// �Q�[��BGM���I�[�f�B�I�\�[�X
    /// </summary>
    public AudioSource _gameBGMAudioSource;

    /// <summary>
    /// �������I�[�f�B�I�\�[�X
    /// </summary>
    public AudioSource _correctAudioSource;

    /// <summary>
    /// �N���A���I�[�f�B�I�\�[�X
    /// </summary>
    public AudioSource _clearAudioSource;

    /// <summary>
    /// �Q�[���I�[�o�[���I�[�f�B�I�\�[�X
    /// </summary>
    public AudioSource _gameOverAudioSource;


    // *******************************************************
    // �v���p�e�B
    // *******************************************************

    /// <summary>
    /// �C���X�^���X
    /// </summary>
    public static GameDirector Instance { get; private set; }

    // *******************************************************
    // Unity���\�b�h
    // *******************************************************

    /// <summary>
    /// �Q�[���J�n�O�̏���������
    /// </summary>
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            // ������������Ă�����폜
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    /// <summary>
    /// �Q�[���J�n���̏���������
    /// </summary>
    private void Start()
    {
        // �J�[�h���X�g���쐬
        _entriesDeck = _cardGenerator.InitializeEntries(GameSettings.SuitColorMode, GameSettings.UseJoker, GameSettings.BackSpriteColor);

        // �R�D, ��D, ��D��\��
        _cardManager.ArrangeFieldCards(_entriesDeck, _entriesField, _entriesHand, true);
    }

    /// <summary>
    /// ��D����ւ��{�^���N���b�N������
    /// </summary>
    private void OnRedistributionButtonClick()
    {
        _uiManager.DisplayRedistributionButton(false);

        _cardManager.ArrangeFieldCards(_entriesDeck, _entriesField, _entriesHand, false);
    }

    /// <summary>
    /// �X�^�[�g�V�[���ȍ~�{�^���N���b�N������
    /// </summary>
    private void OnNextButtonClick()
    {
        SceneManager.LoadScene("StartScene"); // �V�[�����őJ��
    }

    // *******************************************************
    // ���\�b�h
    // *******************************************************

    /// <summary>
    /// �Q�[���I���𔻒�
    /// ��D����D�ɏd�˂�����ɌĂяo��
    /// </summary>
    /// <param name="selectedObj">�I�����ꂽ�J�[�h</param>
    /// <param name="targetObj">�d�˂�Ώۂ̃J�[�h</param>
    public void JudgeGameFinish(GameObject selectedObj, GameObject targetObj)
    {
        // ��̃J�[�h�̈ʒu�Ɩ������X�V
        _cardManager.TransferCardEntry(_entriesDeck, _entriesField, _entriesHand, _entriesNone, selectedObj, targetObj);

        // �R�D�Ǝ�D�����ׂĎg���؂��Ă��邩
        if (_entriesDeck.Count == 0 && _entriesHand.Count == 0)
        {
            // �u�Q�[���N���A�v��\��
            _uiManager.DisplayPanel(true);

            // �Q�[��BGM���~�߂�
            _gameBGMAudioSource.Stop();

            // �N���ABGM���Đ�
            _clearAudioSource.Play();
        }
        else
        {
            // �d�˂���J�[�h�����邩����
            bool judgeHand = SpeedRule.JudgeSequential(_entriesField, _entriesHand);
            bool judgeDeck = SpeedRule.JudgeSequential(_entriesField, _entriesDeck);

            // �R�D�Ǝ�D�ɏd�˂���J�[�h���Ȃ�
            if (!judgeHand && !judgeDeck)
            {
                // ����ȏ�͏d�˂��Ȃ�����GameOver
                _uiManager.DisplayPanel(false);

                // �Q�[��BGM���~�߂�
                _gameBGMAudioSource.Stop();

                // �Q�[���I�[�o�[BGM���Đ�
                _gameOverAudioSource.Play();
            }
            else
            {
                if (!judgeHand)
                {
                    // �u�R�D�؂蒼���v�{�^���\��
                    _uiManager.DisplayRedistributionButton(true);
                }
                // �������Đ�
                _correctAudioSource.Play();
            }
        }
    }
}
