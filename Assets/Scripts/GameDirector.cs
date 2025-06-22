#nullable enable
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine;
using static Define;


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
    /// �R�D�̈�������������{�^���p�R���|�[�l���g
    /// </summary>
    [SerializeField]
    private GameObject _redistributionButton;

    /// <summary>
    /// �R�D���X�g�i�ԁj
    /// </summary>
    private List<CardEntry> _entriesDeckRed = new List<CardEntry>();

    /// <summary>
    /// ��D���X�g�i�ԁj
    /// </summary>
    private List<CardEntry> _entriesFieldRed = new List<CardEntry>();

    /// <summary>
    /// ��D���X�g�i�ԁj
    /// </summary>
    private List<CardEntry> _entriesHandRed = new List<CardEntry>();

    /// <summary>
    /// �g�p�ς݃J�[�h���X�g�i�ԁj
    /// </summary>
    private List<CardEntry> _entriesNoneRed = new List<CardEntry>();


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
        _redistributionButton.SetActive(false);

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // ������������Ă�����폜
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
        List<Card> cardList = _cardGenerator.InitializeCardList(SuitColorMode.SpadeOnly, UseJoker.One, BackSpriteColor.Red);
        _entriesDeckRed = _cardGenerator.InitializeEntries(cardList);

        // �V���b�t����R�D��\��
        _cardManager.Shuffle(_entriesDeckRed);
        _cardManager.DisplayDeck(_entriesDeckRed, Position.Deck, Position.DeckOffset);

        // ��D, ��D������
        for (int slotIndex = 0; slotIndex < Position.Field.Count; slotIndex++)
        {
            _cardManager.DrawTopCard(_entriesDeckRed, _entriesFieldRed, CardProperty.Field, Position.Field[slotIndex], slotIndex);
        }
        for (int slotIndex = 0; slotIndex < Position.Hand.Count; slotIndex++)
        {
            _cardManager.DrawTopCard(_entriesDeckRed, _entriesHandRed, CardProperty.Hand, Position.Hand[slotIndex], slotIndex);
        }

        //MargeManager();
    }

    /// <summary>
    /// ��D����ւ��{�^���N���b�N������
    /// </summary>
    public void OnButtonClick()
    {
        // ��D����ւ��{�^�����\���ɂ���
        _redistributionButton.SetActive(false);

        //MargeManager();
    }

    // *******************************************************
    // ���\�b�h
    // *******************************************************

    public void TransferCardEntry(GameObject selectedObj, GameObject targetObj)
    {
        CardEntry? selectedEntry = _entriesHandRed.FirstOrDefault(entry => entry.View != null && entry.View.gameObject == selectedObj);
        CardEntry? targetEntry = _entriesFieldRed.FirstOrDefault(entry => entry.View != null && entry.View.gameObject == targetObj);

        _cardManager.UpdateEntry(selectedEntry, targetEntry.View.transform.position, true, CardProperty.Field, targetEntry.View.SpriteRenderer.sortingOrder + 1, targetEntry.Data.SlotIndex);

        _entriesHandRed.Remove(selectedEntry);
        _entriesFieldRed.Add(selectedEntry);
    }

    /// <summary>
    /// ��D��1���ǉ�����
    /// </summary>
    public void AddHandCard(int slotIndex)
    {
        _cardManager.DrawTopCard(_entriesDeckRed, _entriesHandRed, CardProperty.Hand, Position.Hand[slotIndex], slotIndex);

        if (_entriesDeckRed.Count == 0 && _entriesHandRed.Count == 0)
        {
            SceneManager.LoadScene("ClearScene"); // �V�[�����őJ��
        }
    }

    /// <summary>
    /// None��1���ǉ�����
    /// </summary>
    /// <param name="slotIndex"></param>
    public void AddNoneCard(GameObject Obj)
    {
        CardEntry? entry = _entriesFieldRed.FirstOrDefault(entry => entry.View != null && entry.View.gameObject == Obj);

        _cardManager.UpdateEntry(entry, Position.None, false, CardProperty.None, 0, 0);
        Obj.SetActive(false);
        
        _entriesFieldRed.Remove(entry);
        _entriesNoneRed.Add(entry);

        JudgeGameOver();

    }


    private void JudgeGameOver()
    {
        bool judgeDeck = SpeedRule.JudgeSequential(_entriesFieldRed, _entriesDeckRed);
        bool judgeHand = SpeedRule.JudgeSequential(_entriesFieldRed, _entriesHandRed);

        if (judgeDeck == false && judgeHand == false)
        {
            // ����ȏ�͏d�˂��Ȃ�����GameOver
            SceneManager.LoadScene("GameOverScene"); // �V�[�����őJ��
        }

        if (judgeHand == false)
        {
            _redistributionButton.SetActive(true);
        }
    }
    private void MargeManager()
    {
        bool judgeHand = false;
        do
        {
            // �R�D�Ǝ�D��������
            _cardManager.MergeCardEntries(_entriesHandRed, _entriesDeckRed);

            // �V���b�t����R�D��\��
            _cardManager.Shuffle(_entriesDeckRed);
            _cardManager.DisplayDeck(_entriesDeckRed, Position.Deck, Position.DeckOffset);

            // ��D������
            for (int slotIndex = 0; slotIndex < Position.Hand.Count; slotIndex++)
            {
                _cardManager.DrawTopCard(_entriesDeckRed, _entriesHandRed, CardProperty.Hand, Position.Hand[slotIndex], slotIndex);
            }
            judgeHand = SpeedRule.JudgeSequential(_entriesFieldRed, _entriesHandRed);
        }
        while (judgeHand == true);
    }
    
}
