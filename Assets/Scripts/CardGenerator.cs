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

    public GameObject GenerateCard(List<Card> deck, Vector2 spawnPosition, bool isFaceUp, BackSpriteColor backColor)
    {
        if (deck.Count == 0)
        {
            Debug.LogWarning("�f�b�L����ł�");
            return null;
        }

        Card drawCard = deck[0];
        deck.RemoveAt(0);

        Card updatedCard = new Card(
            drawCard.Suit,
            drawCard.Number,
            isFaceUp,
            spawnPosition,
            backColor
        );

        GameObject cardObj = Instantiate(_cardPrefab, spawnPosition, Quaternion.identity);
        CardController controller = cardObj.GetComponent<CardController>();
        if (controller != null)
        {
            // �� 1���������v�Z���ēn�� ��
            Sprite faceSprite = GetFaceSprite(drawCard.Suit, drawCard.Number);
            Sprite backSprite = _backSprites[(int)backColor];

            controller.SetCard(updatedCard, faceSprite, backSprite);
        }

        return cardObj;
    }

    // �\�X�v���C�g��1�������擾
    private Sprite GetFaceSprite(SuitType suit, int number)
    {
        if (suit == SuitType.Joker)
        {
            return _faceSprites[_faceSprites.Length - 1]; // �W���[�J�[�͍Ō�
        }

        int index = ((int)suit) * 13 + (number - 1);
        if (index >= 0 && index < _faceSprites.Length)
        {
            return _faceSprites[index];
        }

        Debug.LogWarning($"GetFaceSprite: �����ȃC���f�b�N�X {index}");
        return null;
    }


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

        for (int i = 0; i < (int)includeJoker; i++)
        {
            deck.Add(new Card(SuitType.Joker, 0, false, Vector2.zero, BackSpriteColor.Red));
        }

        return deck;
    }

    public void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int j = UnityEngine.Random.Range(i, list.Count);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}
