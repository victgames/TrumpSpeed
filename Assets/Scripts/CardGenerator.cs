using System;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using static Define.Card;

public class CardGenerator : MonoBehaviour
{
    // *******************************************************
    // �����o�ϐ�
    // *******************************************************

    /// <summary>
    /// �J�[�h�̃v���n�u
    /// </summary>
    [SerializeField]
    private GameObject _cardPrefab;

    /// <summary>
    /// 52���{�W���[�J�[���̕\�ʃX�v���C�g�i�C���X�y�N�^�ŃZ�b�g�j
    /// </summary>
    [SerializeField]
    private Sprite[] _faceSprites;

    /// <summary>
    /// �J�[�h���ʃX�v���C�g�i0:��, 1:�j�i�C���X�y�N�^�ŃZ�b�g�j
    /// </summary>
    [SerializeField]
    private Sprite[] _backSprites;


    // *******************************************************
    // ���\�b�h
    // *******************************************************

    /// <summary>
    /// �f�b�L����J�[�h��1�������Đ����i�\/�����ʁj
    /// </summary>
    public void GenerateCard(List<Card> deck, Vector2 spawnPosition, bool isFaceUp, BackSpriteColor backColor)
    {
        if (deck.Count == 0)
        {
            Debug.LogWarning("�f�b�L����ł�");
            return;
        }

        Card drawCard = deck[0];
        deck.RemoveAt(0);

        // �V�����J�[�h���Ƃ��ď�Ԃ��X�V����Card���Đ���
        Card updatedCard = new Card(
            drawCard.Suit,
            drawCard.Number,
            isFaceUp,
            spawnPosition,
            backColor
        );

        GameObject cardObj = Instantiate(_cardPrefab, spawnPosition, Quaternion.identity);
        CardController controller = cardObj.GetComponent<CardController>();
        controller.SetCard(updatedCard, _faceSprites, _backSprites);
    }

    /// <summary>
    /// �f�b�L����
    /// </summary>
    public List<Card> CreateDeck(SuitColorMode colorMode, UseJoker includeJoker)
    {
        List<Card> deck = new List<Card>();

        foreach (SuitType suit in Enum.GetValues(typeof(SuitType)))
        {
            if (suit == SuitType.Joker) continue;

            if (colorMode == SuitColorMode.BlackOnly && (suit != SuitType.Spade && suit != SuitType.Club)) continue;
            if (colorMode == SuitColorMode.RedOnly && (suit != SuitType.Heart && suit != SuitType.Diamond)) continue;

            for (int num = 1; num <= 13; num++)
            {
                deck.Add(new Card(suit, num, false, Vector2.zero, BackSpriteColor.Red));
            }
        }

        // Joker �̒ǉ��i0�Ƃ���j
        for (int i = 0; i < (int)includeJoker; i++)
        {
            deck.Add(new Card(SuitType.Joker, 0, false, Vector2.zero, BackSpriteColor.Red));
        }

        return deck;
    }

    /// <summary>
    /// �f�b�L���V���b�t������iFisher-Yates�A���S���Y���j
    /// </summary>
    public void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int j = UnityEngine.Random.Range(i, list.Count);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}
