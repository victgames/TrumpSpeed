#nullable enable
using System.Collections.Generic;
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


    // *******************************************************
    // �v���p�e�B
    // *******************************************************

    /// <summary>
    /// 
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
    }

    /// <summary>
    /// ��D����ւ��{�^���N���b�N������
    /// </summary>
    public void OnButtonClick()
    {
        // �J�[�h���X�g���쐬
        _cardManager.MergeCardEntries(_entriesHandRed, _entriesDeckRed);

        // �V���b�t����R�D��\��
        _cardManager.Shuffle(_entriesDeckRed);
        _cardManager.DisplayDeck(_entriesDeckRed, Position.Deck, Position.DeckOffset);

        // ��D������
        for (int slotIndex = 0; slotIndex < Position.Hand.Count; slotIndex++)
        {
            _cardManager.DrawTopCard(_entriesDeckRed, _entriesHandRed, CardProperty.Hand, Position.Hand[slotIndex], slotIndex);
        }
    }

    // *******************************************************
    // ���\�b�h
    // *******************************************************

    /// <summary>
    /// ��D��1���ǉ�����
    /// </summary>
    public void AddHandCard(int slotIndex)
    {
        _cardManager.DrawTopCard(_entriesDeckRed, _entriesHandRed, CardProperty.Hand, Position.Hand[slotIndex], slotIndex);
    }
}
