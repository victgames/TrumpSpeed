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
    private List<CardEntry?> _DeckRed = new List<CardEntry?>();

    /// <summary>
    /// ��D���X�g�i�ԁj
    /// </summary>
    private List<CardEntry?> _FieldRed = new List<CardEntry?>();

    /// <summary>
    /// ��D���X�g�i�ԁj
    /// </summary>
    private List<CardEntry?> _HandRed = new List<CardEntry?>();



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
        //List<Card> cardList = _cardGenerator.GenerateCardList(SuitColorMode.SpadeOnly, UseJoker.One, BackSpriteColor.Red);
        //_DeckRed

        // �V���b�t����R�D��\��
        _cardManager.Shuffle(_DeckRed);
        _cardManager.DisplayDeck(_DeckRed, Position.Deck, Position.DeckOffset);

        // ��D, ��D������
        _cardManager.DrawTopCard(_DeckRed, _FieldRed, CardProperty.Field, Position.Field);
        _cardManager.DrawTopCard(_DeckRed, _HandRed, CardProperty.Hand, Position.Hand);
    }

    private void OnEnable()
    {
        CardController.OnDroppedToField += HandleCardDropped;
    }

    private void OnDisable()
    {
        CardController.OnDroppedToField -= HandleCardDropped;
    }

    // *******************************************************
    // ���\�b�h
    // *******************************************************



    public void HandleCardDropped(CardController controller)
    {
        if (controller == null || controller.Card == null) return;

        // �������X�V
        controller.Card.CardProperty = CardProperty.Field;

        // �\�[�g���͌��݂̏�D���X�g�̖���
        int order = _FieldRed.Count;

        // �\�����E���C���[�ݒ�
        controller.SetSorting(SortLayers.Name(CardProperty.Field), order);

        // �\���ʒu�i�K�v�ɉ����āj
        if (order < Position.Field.Count)
        {
            controller.transform.position = Position.Field[order];
        }

        // �J�[�h�G���g���Ƃ��ĊǗ�
        var entry = new CardEntry(controller.Card, controller, 0);
        _FieldRed.Add(entry);

        Debug.Log($"�J�[�h {controller.Card.ToString()} ����D�ɒǉ��i����: {order}�j");
    }


}
