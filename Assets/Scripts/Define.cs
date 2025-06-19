#nullable enable
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using static Define;

/// <summary>
/// ��`�p�N���X
/// </summary>
public class Define
{
    // *******************************************************
    // �񋓑�
    // *******************************************************

    /// <summary>
    /// �J�[�h�̏���
    /// </summary>
    public enum CardProperty
    {
        Deck,
        Field,
        Hand
    }

    /// <summary>
    /// �g�����v�̃X�[�g��`
    /// </summary>
    public enum SuitType
    {
        Spade = 0,        // �X�y�[�h
        Club = 1,        // �N���u
        Diamond = 2,        // �_�C��
        Heart = 3,        // �n�[�g
        Joker = 4         // �W���[�J�[
    }

    /// <summary>
    /// �g�����v�̎g�p�F
    /// </summary>
    public enum SuitColorMode
    {
        Both = 0,    // �����g�p
        BlackOnly = 1,    // ���̂�
        RedOnly = 2,    // �Ԃ̂�
        SpadeOnly = 3,    // �X�y�[�h
        ClubOnly = 4,    // �N���u
        DiamondOnly = 5,    // �_�C��
        HeartOnly = 6,    // �n�[�g
    }

    /// <summary>
    /// �W���[�J�[�̎g�p
    /// </summary>
    public enum UseJoker
    {
        None = 0,        // �W���[�J�[�Ȃ�
        One = 1,        // 1���g�p
        Two = 2         // 2���g�p
    }

    /// <summary>
    /// �J�[�h���ʂ̊G��
    /// </summary>
    public enum BackSpriteColor
    {
        Red = 0,        // ��
        Blue = 1         // ��
    }

    
    /// <summary>
    /// �^�O��`
    /// </summary>
    public const string TAG_DECK = "DeckCard";
    public const string TAG_FIELD = "FieldCard";
    public const string TAG_HAND = "HandCard";

    /// <summary>
    /// �\�[�e�B���O���C���[��`
    /// </summary>
    public const string SORT_LAYER_DECK = "Deck";
    public const string SORT_LAYER_FIELD = "Field";
    public const string SORT_LAYER_HAND = "Hand";
    

    /// <summary>
    /// �\���ʒu
    /// </summary>
    public class Position
    {
        /// <summary>�R�D�̊�ʒu</summary>
        public static readonly Vector3 Deck = new Vector3(7.0f, -2.0f, 0.0f);

        /// <summary>�R�D���̃J�[�h�Ԃ̃I�t�Z�b�g</summary>
        public static readonly Vector3 DeckOffset = new Vector3(0.01f, -0.01f, 0.01f);

        /// <summary>��D�̕\���ʒu���X�g</summary>
        public static readonly IReadOnlyList<Vector3> Field = new List<Vector3>
        {
            new Vector3(-3.6f, 1.0f, 0.0f),
            new Vector3(-1.2f, 1.0f, 0.0f),
            new Vector3( 1.2f, 1.0f, 0.0f),
            new Vector3( 3.6f, 1.0f, 0.0f)
        };

        /// <summary>��D�̕\���ʒu���X�g</summary>
        public static readonly IReadOnlyList<Vector3> Hand = new List<Vector3>
        {
            new Vector3(-4.5f, -2.0f, 0.0f),
            new Vector3(-1.5f, -2.0f, 0.0f),
            new Vector3( 1.5f, -2.0f, 0.0f),
            new Vector3( 4.5f, -2.0f, 0.0f)
        };
    }


}

/// <summary>
/// �\�[�g���C���[�����`����N���X
/// </summary>
public static class SortLayers
{
    public static string Name(CardProperty layer)
    {
        return layer switch
        {
            CardProperty.Deck => "Deck",
            CardProperty.Field => "Field",
            CardProperty.Hand => "Hand",
            _ => "Default"
        };
    }
}


/// <summary>
/// �J�[�h���`����N���X
/// </summary>
public class Card
{
    // *******************************************************
    // �v���p�e�B
    // *******************************************************

    /// <summary>
    /// �X�[�g
    /// </summary>
    public SuitType Suit { get; private set; }

    /// <summary>
    /// �����i1�`13, Joker��0�j
    /// </summary>
    public int Number { get; private set; }

    /// <summary>
    /// �J�[�h���ʂ̊G��
    /// </summary>
    public BackSpriteColor BackColor { get; private set; }

    /// <summary>
    /// �\�ʊG��
    /// </summary>
    public Sprite FaceSprite { get; private set; }

    /// <summary>
    /// ���ʊG��
    /// </summary>
    public Sprite BackSprite { get; private set; }

    /// <summary>
    /// ���\
    /// </summary>
    public bool IsFaceUp { get; set; }

    /// <summary>
    /// �J�[�h�̏���
    /// </summary>
    public CardProperty CardProperty { get; set; }

    /// <summary>
    /// �ʒu
    /// </summary>
    public Vector3 Position { get; set; }


    // *******************************************************
    // �R���X�g���N�^
    // *******************************************************

    /// <summary>
    /// �R���X�g���N�^
    /// </summary>
    /// <param name="suit"></param>
    /// <param name="number"></param>
    /// <param name="backColor"></param>
    /// <param name="faceSprite"></param>
    /// <param name="backSprite"></param>
    /// <param name="isFaceUp"></param>
    /// <param name="cardProperty"></param>
    /// <param name="position"></param>
    public Card(SuitType suit, int number, BackSpriteColor backColor, Sprite faceSprite, Sprite backSprite, bool isFaceUp, CardProperty cardProperty, Vector3 position)
    {
        Suit = suit;
        Number = number;
        BackColor = backColor;
        FaceSprite = faceSprite;
        BackSprite = backSprite;
        IsFaceUp = isFaceUp;
        CardProperty = cardProperty;
        Position = position;
    }
}

/// <summary>
/// �J�[�h�̕\�������`����N���X
/// </summary>
public class CardEntry
{
    // *******************************************************
    // �v���p�e�B
    // *******************************************************

    /// <summary>
    /// �J�[�h�̃f�[�^�i�X�[�g�E�����E���\�Ȃǁj
    /// </summary>
    public Card Data { get; private set; }

    /// <summary>
    /// �J�[�h�̌����ڂ𐧌䂷��R���|�[�l���g�iGameObject�ɃA�^�b�`�j
    /// </summary>
    public CardController View { get; private set; }

    // *******************************************************
    // �R���X�g���N�^
    // *******************************************************

    /// <summary>
    /// �J�[�h�̃f�[�^�ƌ����ځi�\���I�u�W�F�N�g�j��1�Z�b�g�ŕێ�����
    /// </summary>
    /// <param name="data">�J�[�h�̃f�[�^</param>
    /// <param name="view">�J�[�h�\���p�R���|�[�l���g</param>
    public CardEntry(Card data, CardController view)
    {
        Data = data;
        View = view;
    }
}