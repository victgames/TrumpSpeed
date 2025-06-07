using UnityEngine;
using static Define.Card;

public class CardGenerator : MonoBehaviour
{
    // *******************************************************
    // �萔
    // *******************************************************

    public Vector2 spawnPosition = new Vector2(0, 0);

    // *******************************************************
    // �����o�ϐ�
    // *******************************************************

    /// <summary>
    /// �J�[�h�̃v���n�u
    /// </summary>
    public GameObject cardPrefab;

    // *******************************************************
    // ���\�b�h
    // *******************************************************

    /// <summary>
    /// �X�^�[�g�֐�
    /// </summary>
    void Start()
    {
        GenerateCard();
    }

    /// <summary>
    /// �A�b�v�f�[�g�֐�
    /// </summary>
    void Update()
    {
        
    }

    public void GenerateCard()
    {
        // �J�[�h�𐶐�
        GameObject card = Instantiate(cardPrefab, spawnPosition, Quaternion.identity);

        // �����_���ȃX�[�g�Ɛ�����I��
        Suit randomSuit = (Suit)Random.Range(0, 4); // Joker����
        int randomNumber = Random.Range(1, 14);     // 1�`13

        // CardController�ɓn��
        CardController controller = card.GetComponent<CardController>();
        controller.SetCard(randomSuit, randomNumber);
    }
}
