using System;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using static Define.Card;

public class CardManager : MonoBehaviour
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
    /// �J�[�h���X�g�쐬
    /// </summary>
    /// <param name="colorMode"></param>
    /// <param name="includeJoker"></param>
    /// <returns></returns>
    public List<Card> CreateCardList(SuitColorMode colorMode, UseJoker includeJoker, BackSpriteColor backColor)
    {
        List<Card> deck = new List<Card>();

        // SuitType���ׂĂ̒l�����ɗ񋓂���suit�ɑ��
        foreach (SuitType suit in Enum.GetValues(typeof(SuitType)))
        {
            // Joker�����O
            if (suit == SuitType.Joker) continue;

            // SuitColorMode�ɕ����ăJ�[�h���X�g�Ɋ܂߂Ȃ��J�[�h�����O
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
    /// �J�[�h�𐶐�
    /// </summary>
    /// <param name="cardData"></param>
    /// <param name="position"></param>
    /// <param name="isFaceUp"></param>
    /// <returns></returns>
    public GameObject GenerateCard(Card cardData, Vector3 position, string tag, string sortingLayer)
    {
        if (cardData == null)
        {
            return null;
        }

        GameObject cardObj = Instantiate(_cardPrefab, position, Quaternion.identity);
        CardController controller = cardObj.GetComponent<CardController>();

        if (controller != null)
        {
            controller.Card = cardData;
            controller.UpdateSprite();
            controller.tag = tag;
            controller.SpriteRenderer.sortingLayerName = sortingLayer;
        }

        return cardObj;
    }


    /// <summary>
    /// �\�ʃX�v���C�g��T��
    /// </summary>
    /// <param name="suit"></param>
    /// <param name="number"></param>
    /// <returns></returns>
    private Sprite GetFaceSprite(SuitType suit, int number)
    {
        // �W���[�J�[�̏ꍇ�͓��͒l�̃X�v���C�g���擾
        if (suit == SuitType.Joker)
        {
            return _faceSprites[number];
        }

        // �����̏ꍇ��suit�Ɛ����ō��v����X�v���C�g��T��
        int index = ((int)suit) * 13 + (number - 1);
        if (index >= 0 && index < _faceSprites.Length)
        {
            return _faceSprites[index];
        }

        return null;
    }
    

    /// <summary>
    /// �R�D��\��
    /// </summary>
    /// <param name="entries"></param>
    public void DisplayDeck(List<CardEntry> entries, Vector3 pos, Vector3 offset)
    {
        for (int i = 0; i < entries.Count; i++)
        {
            // �J�[�h�̈ʒu��ύX���čĕ\��
            CardEntry entry = entries[i];

            entry.View.transform.position = pos + offset * i;
            entry.Data.Position = pos + offset * i;

            // �J�[�h�̈ʒu����ύX
            entry.View.GetComponent<CardController>()?.SetSorting(SORT_LAYER_DECK, i);
        }
    }

    /// <summary>
    /// �R�D���V���b�t��
    /// </summary>
    /// <param name="deck"></param>
    public void Shuffle(List<CardEntry> deck)
    {
        for (int i = 0; i < deck.Count; i++)
        {
            int j = UnityEngine.Random.Range(i, deck.Count);
            (deck[i], deck[j]) = (deck[j], deck[i]);
        }
    }


    /// <summary>
    /// �f�b�L1���ڂ���������
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="tag"></param>
    /// <param name="sortLayer"></param>
    /// <returns></returns>
    public CardEntry DrawTopCard(List<CardEntry> entries, Vector3 pos, string tag, string sortLayer)
    {
        if (entries.Count == 0) return null;

        CardEntry topCard = entries[0];
        entries.RemoveAt(0);

        //DisplayDeck();

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
