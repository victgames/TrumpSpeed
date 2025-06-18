using static Define.Card;
using static Define;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class GameDirector : MonoBehaviour
{
    // *******************************************************
    // �����o�ϐ�
    // *******************************************************

    [SerializeField] 
    private CardGenerator _cardGenerator;

    private List<CardEntry> _deckEntries = new List<CardEntry>();

    // *******************************************************
    // �v���p�e�B
    // *******************************************************

    public bool[] HandCardCounts { get; private set; } = new bool[4];

    public int[] FieldCardCounts { get; private set; } = new int[4];


    // *******************************************************
    // ��{���\�b�h
    // *******************************************************

    /// <summary>
    /// �J�n������
    /// </summary>
    void Start()
    {
        InitializeDeck();
        //DisplayDeck();

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
    /// �f�b�L�쐬
    /// </summary>
    private void InitializeDeck()
    {
        // �f�b�L�G���g���[��������
        _deckEntries.Clear();

        // �f�b�L���X�g���쐬
        List<Card> cardDataList = _cardGenerator.CreateDeck(SuitColorMode.Both, UseJoker.One, BackSpriteColor.Red);
        _cardGenerator.Shuffle(cardDataList);

        for (int i = 0; i < cardDataList.Count; i++)
        {
            // �J�[�h�𐶐�
            Vector3 pos = POS_DECK + POS_DECK_OFFSET * i;
            GameObject cardObj = _cardGenerator.GenerateCard(cardDataList[i], pos, false, TAG_DECK, SORT_LAYER_DECK, i);

            // �f�b�L�G���g���[�Ɋi�[
            if (cardObj != null)
            {
                CardController controller = cardObj.GetComponent<CardController>();
                if (controller != null)
                {
                    CardEntry entry = new CardEntry(cardDataList[i], controller);
                    _deckEntries.Add(entry);
                }
            }
        }

    }

    /// <summary>
    /// �f�b�L�\��
    /// </summary>
    private void DisplayDeck()
    {
        for (int i = 0; i < _deckEntries.Count; i++)
        {
            // CardEntry��T��
            CardEntry entry = _deckEntries[i];

            // �ʒu��ύX
            Vector3 pos = POS_DECK + POS_DECK_OFFSET * i;
            entry.View.transform.position = pos;
            entry.Data.Position = pos;

            // SpriteRenderer�̃p�����[�^�ύX
            entry.View.GetComponent<CardController>()?.SetSorting(SORT_LAYER_DECK, i);
        }
    }

    /// <summary>
    /// �f�b�L1���ڂ���������
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="tag"></param>
    /// <param name="sortLayer"></param>
    /// <returns></returns>
    public CardEntry DrawTopCard(Vector2 pos, string tag, string sortLayer)
    {
        if (_deckEntries.Count == 0) return null;

        CardEntry topCard = _deckEntries[0];
        _deckEntries.RemoveAt(0);

        DisplayDeck();

        topCard.View.transform.position = pos;
        topCard.View.tag = tag;

        SpriteRenderer sr = topCard.View.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sortingLayerName = sortLayer;
            sr.sortingOrder = 0; // �K�v�ɉ����Ē������Ă�������
        }

        CardController controller = topCard.View.GetComponent<CardController>();
        if (controller != null)
        {
            //controller.SetFaceUp(true);
            controller.Card.Position = pos;
        }

        return topCard;
    }

}
