using UnityEngine;

/// <summary>
/// ��`�p�N���X
/// </summary>
public class Define
{
    /// <summary>
    /// �J�[�h���`����N���X
    /// </summary>
    public class Card
    {
        // *******************************************************
        // �񋓑�
        // *******************************************************

        /// <summary>
        /// �g�����v�̊G����`
        /// </summary>
        public enum Suit
        {
            Spade   = 0,
            Heart   = 1,
            Diamond = 2,
            Club    = 3,
            Joker   = 4
        }

        // *******************************************************
        // �����o�ϐ�
        // *******************************************************

        /// <summary>
        /// �g�����v�̊G��
        /// </summary>
        public Suit suit;
        
        /// <summary>
        /// �g�����v�̐���
        /// </summary>
        public int number;

        // *******************************************************
        // �R���X�g���N�^
        // *******************************************************

        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        /// <param name="suit">�g�����v�̊G��</param>
        /// <param name="number">�g�����v�̐���</param>
        public Card(Suit suit, int number)
        {
            this.suit = suit;
            this.number = number;
        }
    }
}
