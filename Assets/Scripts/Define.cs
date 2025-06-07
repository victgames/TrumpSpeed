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
    /// �J�[�h�̐����ʒu
    /// </summary>
    public Vector2 spawnPos = new Vector2(0, 0);


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
        // �����o�ϐ�
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
        /// ���\
        /// </summary>
        public bool IsFaceUp { get; private set; }

        /// <summary>
        /// �ʒu
        /// </summary>
        public Vector2 SpawnPosition { get; private set; }

        /// <summary>
        /// �J�[�h���ʂ̊G��
        /// </summary>
        public BackSpriteColor BackColor { get; private set; }


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
        public Card(SuitType suit, int number, bool isFaceUp, Vector2 spawnPosition, BackSpriteColor backColor)
        {
            Suit = suit;
            Number = number;
            IsFaceUp = isFaceUp;
            SpawnPosition = spawnPosition;
            BackColor = backColor;
        }
    }
}
