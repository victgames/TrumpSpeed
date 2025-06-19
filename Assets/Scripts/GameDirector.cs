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
    private List<CardEntry> _DeckRed = new List<CardEntry>();

    /// <summary>
    /// ��D���X�g�i�ԁj
    /// </summary>
    private List<CardEntry> _FieldRed = new List<CardEntry>();

    /// <summary>
    /// ��D���X�g�i�ԁj
    /// </summary>
    private List<CardEntry> _HandRed = new List<CardEntry>();



    // *******************************************************
    // �v���p�e�B
    // *******************************************************

    /// <summary>
    /// ��D�������
    /// </summary>
    public bool[] HandCardCounts { get; private set; } = new bool[4];

    /// <summary>
    /// ��D�������
    /// </summary>
    public int[] FieldCardCounts { get; private set; } = new int[4];


    // *******************************************************
    // ��{���\�b�h
    // *******************************************************

    /// <summary>
    /// �J�n������
    /// </summary>
    void Start()
    {
        // �J�[�h���X�g���쐬
        _DeckRed = _cardGenerator.InitializeEntries(SuitColorMode.SpadeOnly, UseJoker.One, BackSpriteColor.Red, CardProperty.Deck);

        // �V���b�t����R�D��\��
        _cardManager.Shuffle(_DeckRed);
        _cardManager.DisplayDeck(_DeckRed, Position.Deck, Position.DeckOffset);

        // ��D, ��D������
        _cardManager.DrawTopCard(_DeckRed, _FieldRed, CardProperty.Field, Position.Field);
        _cardManager.DrawTopCard(_DeckRed, _HandRed, CardProperty.Hand, Position.Hand);
    }

    // *******************************************************
    // ���\�b�h
    // *******************************************************

    


}
