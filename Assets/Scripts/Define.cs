using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

/// <summary>
/// ��`�p�N���X
/// </summary>
public class Define
{
    // *******************************************************
    // �萔
    // *******************************************************

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
    /// �J�[�h���`����N���X
    /// </summary>
    public class Card
    {
        // *******************************************************
        // �񋓑�
        // *******************************************************

        /// <summary>
        /// �g�����v�̃X�[�g��`
        /// </summary>
        public enum SuitType
        {
            Spade   = 0,        // �X�y�[�h
            Club    = 1,        // �N���u
            Diamond = 2,        // �_�C��
            Heart   = 3,        // �n�[�g
            Joker   = 4         // �W���[�J�[
        }

        /// <summary>
        /// �g�����v�̎g�p�F
        /// </summary>
        public enum SuitColorMode
        {
            Both        = 0,    // �����g�p
            BlackOnly   = 1,    // ���̂�
            RedOnly     = 2     // �Ԃ̂�
        }

        /// <summary>
        /// �W���[�J�[�̎g�p
        /// </summary>
        public enum UseJoker
        {
            None    = 0,        // �W���[�J�[�Ȃ�
            One     = 1,        // 1���g�p
            Two     = 2         // 2���g�p
        }

        /// <summary>
        /// �J�[�h���ʂ̊G��
        /// </summary>
        public enum BackSpriteColor
        {
            Red     = 0,        // ��
            Blue    = 1         // ��
        }

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
        /// ���\
        /// </summary>
        public bool IsFaceUp { get; set; }

        /// <summary>
        /// �ʒu
        /// </summary>
        public Vector2 SpawnPosition { get; set; }

        /// <summary>
        /// �^�O
        /// </summary>
        //public string Tag { get; set; }

        /// <summary>
        /// �\�[�e�B���O���C���[
        /// </summary>
        //public string SortingLayerName { get; set; }


        // *******************************************************
        // �R���X�g���N�^
        // *******************************************************

        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        /// <param name="suit">�X�[�g</param>
        /// <param name="number">����</param>
        /// <param name="isFaceUp">���\</param>
        /// <param name="spawnPosition">�ʒu</param>
        public Card(SuitType suit, int number, BackSpriteColor backColor, bool isFaceUp, Vector2 spawnPosition)//, string tag, string sortingLayer)
        {
            Suit = suit;
            Number = number;
            BackColor = backColor;
            IsFaceUp = isFaceUp;
            SpawnPosition = spawnPosition;
            //Tag = tag;
            //SortingLayerName = sortingLayer;
        }
    }

    /// <summary>
    /// �J�[�h�̕\�������`
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
}
