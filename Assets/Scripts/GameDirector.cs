using System.Collections.Generic;
using UnityEngine;
using static Define;
using static Define.Card;

public class GameDirector : MonoBehaviour
{
    // *******************************************************
    // �����o�ϐ�
    // *******************************************************

    /// <summary>
    /// �J�[�h�𐶐�����R���|�[�l���g
    /// </summary>
    [SerializeField] 
    private CardManager _cardManager;

    /// <summary>
    /// �J�[�h��񃊃X�g
    /// </summary>
    private List<CardEntry> _CardEntriesRed = new List<CardEntry>();

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
        InitializeDeckEntries(_CardEntriesRed, SuitColorMode.Both, UseJoker.One, BackSpriteColor.Red);
        
        // �V���b�t����R�D��\��
        _cardManager.Shuffle(_CardEntriesRed);
        _cardManager.DisplayDeck(_CardEntriesRed, POS_DECK, POS_DECK_OFFSET);

        /*
        for (int i = 0; i < POS_FIELD.Count; i++)
        {
            DrawTopCard(POS_FIELD[i], TAG_FIELD, SORT_LAYER_FIELD);
            FieldCardCounts[i] = 1; // �ŏ���1�����u�����̂�
        }

        for (int i = 0; i < POS_HAND.Count; i++)
        {
            DrawTopCard(POS_HAND[i], TAG_HAND, SORT_LAYER_HAND);
            HandCardCounts[i] = true; // �ŏ���1���u�����̂�
        }
        */
    }

    // *******************************************************
    // ���\�b�h
    // *******************************************************

    /// <summary>
    /// �J�[�h��񃊃X�g�쐬
    /// </summary>
    private void InitializeDeckEntries(List<CardEntry> entries, SuitColorMode mode, UseJoker joker, BackSpriteColor color)
    {
        // �J�[�h��񃊃X�g��������
        entries.Clear();

        // �J�[�h��񃊃X�g���쐬
        List<Card> cardDataList = _cardManager.CreateCardList(mode, joker, color);

        for (int i = 0; i < cardDataList.Count; i++)
        {
            // �J�[�h�𐶐�
            GameObject cardObj = _cardManager.GenerateCard(cardDataList[i], Vector3.zero, TAG_DECK, SORT_LAYER_DECK);

            // �J�[�h��񃊃X�g�Ɋi�[
            if (cardObj != null)
            {
                CardController controller = cardObj.GetComponent<CardController>();
                if (controller != null)
                {
                    CardEntry entry = new CardEntry(cardDataList[i], controller);
                    entries.Add(entry);
                }
            }
        }
    }


}
