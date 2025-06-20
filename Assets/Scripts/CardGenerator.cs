#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using static UnityEngine.EventSystems.EventTrigger;

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
    /// �J�[�h���X�g�쐬
    /// </summary>
    /// <param name="colorMode"></param>
    /// <param name="includeJoker"></param>
    /// <returns></returns>
    public List<Card> InitializeCardList(SuitColorMode colorMode, UseJoker includeJoker, BackSpriteColor backColor)
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
                Sprite? faceSprite = GetFaceSprite(suit, num);
                Sprite? backSprite = GetBackSprite(backColor);
                deck.Add(new Card(suit, num, backColor, faceSprite, backSprite, false, CardProperty.None, null));
            }
        }

        // Joker�̖���������
        for (int i = 0; i < (int)includeJoker; i++)
        {
            // Joker��_faceSprites��53�Ԗڈȍ~�ɔz�u
            Sprite? faceSprite = GetFaceSprite(SuitType.Joker, 53 + i);
            Sprite? backSprite = GetBackSprite(backColor);
            deck.Add(new Card(SuitType.Joker, 0, backColor, faceSprite, backSprite, false, CardProperty.None, null));
        }

        return deck;
    }

    /// <summary>
    /// �J�[�h��񃊃X�g�쐬
    /// </summary>
    public List<CardEntry?> InitializeEntries(List<Card> cardDataList)
    {
        // �J�[�h��񃊃X�g��������
        List<CardEntry?> entries = new List<CardEntry?>();

        for (int i = 0; i < cardDataList.Count; i++)
        {
            // �J�[�h�𐶐�
            GameObject? cardObj = GenerateCard(cardDataList[i]);

            // �J�[�h��񃊃X�g�Ɋi�[
            if (cardObj != null)
            {
                CardController controller = cardObj.GetComponent<CardController>();
                if (controller != null)
                {
                    // ����:None, �C���f�b�N�X:0�Ƃ��ď�����
                    CardEntry entry = new CardEntry(cardDataList[i], controller);//, CardProperty.None, 0);
                    entries.Add(entry);
                }
            }
        }
        return entries;
    }

    /// <summary>
    /// �J�[�h�𐶐�
    /// </summary>
    /// <param name="cardData"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public GameObject? GenerateCard(Card cardData)
    {
        if (cardData == null)
        {
            return null;
        }

        // �J�[�h�C���X�^���X�쐬
        GameObject cardObj = Instantiate(_cardPrefab, Vector3.zero, Quaternion.identity);

        // �C���X�^���X�̏����X�V
        var cardCtrl = cardObj.GetComponent<CardController>();
        if (cardCtrl != null)
        {
            cardCtrl.Card = cardData;
        }
        cardObj.GetComponent<CardController>()?.UpdateSprite();
        cardObj.GetComponent<CardController>()?.SetSorting(SortLayers.Name(CardProperty.None), 0);

        return cardObj;
    }

    /// <summary>
    /// �\�ʃX�v���C�g��T��
    /// </summary>
    /// <param name="suit"></param>
    /// <param name="number"></param>
    /// <returns></returns>
    private Sprite? GetFaceSprite(SuitType suit, int number)
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
    /// ���ʃX�v���C�g��T��
    /// </summary>
    /// <param name="backColor"></param>
    /// <returns></returns>
    private Sprite? GetBackSprite(BackSpriteColor backColor)
    {
        return _backSprites[(int)backColor];
    }
}
