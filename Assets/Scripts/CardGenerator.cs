using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    /// �J�[�h�𐶐�
    /// </summary>
    /// <param name="cardData"></param>
    /// <param name="spawnPosition"></param>
    /// <param name="isFaceUp"></param>
    /// <returns></returns>
    public GameObject GenerateCard(Card cardData, Vector3 spawnPosition, bool isFaceUp, string tag, string sortingLayer, int sortingOrder)
    {
        if (cardData == null)
        {
            Debug.LogWarning("�J�[�h�f�[�^�� null �ł�");
            return null;
        }

        GameObject cardObj = Instantiate(_cardPrefab, spawnPosition, Quaternion.identity);
        CardController controller = cardObj.GetComponent<CardController>();

        if (controller != null)
        {
            controller.Card = cardData;
            controller.UpdateSprite();
            controller.tag = tag;
            controller.SpriteRenderer.sortingLayerName = sortingLayer;
            controller.SpriteRenderer.sortingOrder = sortingOrder;
        }

        return cardObj;
    }

    private Card CreateCardWithSprites(Card baseCard, BackSpriteColor backColor, bool isFaceUp, Vector2 position)
    {
        Sprite faceSprite = GetFaceSprite(baseCard.Suit, baseCard.Number);
        Sprite backSprite = _backSprites[(int)backColor];

        return new Card(
            baseCard.Suit,
            baseCard.Number,
            backColor,
            faceSprite,
            backSprite,
            isFaceUp,
            position
        );
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


    /// <summary>
    /// �f�b�L�쐬
    /// </summary>
    /// <param name="colorMode"></param>
    /// <param name="includeJoker"></param>
    /// <returns></returns>
    public List<Card> CreateDeck(SuitColorMode colorMode, UseJoker includeJoker, BackSpriteColor backColor)
    {
        List<Card> deck = new List<Card>();

        // SuitType���ׂĂ̒l�����ɗ񋓂���suit�ɑ��
        foreach (SuitType suit in Enum.GetValues(typeof(SuitType)))
        {
            // Joker�����O
            if (suit == SuitType.Joker) continue;

            // SuitColorMode�ɕ����ĎR�D�Ɋ܂߂Ȃ��J�[�h�����O
            if (colorMode == SuitColorMode.BlackOnly && (suit != SuitType.Spade && suit != SuitType.Club)) continue;
            if (colorMode == SuitColorMode.RedOnly && (suit != SuitType.Heart && suit != SuitType.Diamond)) continue;
            if (colorMode == SuitColorMode.SpadeOnly && (suit != SuitType.Spade)) continue;
            if (colorMode == SuitColorMode.ClubOnly && (suit != SuitType.Club)) continue;
            if (colorMode == SuitColorMode.DiamondOnly && (suit != SuitType.Diamond)) continue;
            if (colorMode == SuitColorMode.HeartOnly && (suit != SuitType.Heart)) continue;

            // 1�`13�܂ł̐����ŏ�����
            for (int num = 1; num <= 13; num++)
            {
                Sprite faceSprite = GetFaceSprite(suit, num);
                Sprite backSprite = _backSprites[(int)backColor];
                deck.Add(new Card(suit, num, backColor, faceSprite, backSprite, false, Vector2.zero));
            }
        }

        // Joker�̖���������
        for (int i = 0; i < (int)includeJoker; i++)
        {
            // Joker��_faceSprites��53�Ԗڈȍ~�ɔz�u
            Sprite faceSprite = GetFaceSprite(SuitType.Joker, 53 + i);
            Sprite backSprite = _backSprites[(int)backColor];
            deck.Add(new Card(SuitType.Joker, 0, backColor, faceSprite, backSprite, false, Vector2.zero));
        }

        return deck;
    }

    /// <summary>
    /// �V���b�t��
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    public void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int j = UnityEngine.Random.Range(i, list.Count);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}
