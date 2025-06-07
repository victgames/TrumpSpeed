using UnityEngine;
using static Define.Card;

public class CardGenerator : MonoBehaviour
{
    // *******************************************************
    // 定数
    // *******************************************************

    public Vector2 spawnPosition = new Vector2(0, 0);

    // *******************************************************
    // メンバ変数
    // *******************************************************

    /// <summary>
    /// カードのプレハブ
    /// </summary>
    public GameObject cardPrefab;

    // *******************************************************
    // メソッド
    // *******************************************************

    /// <summary>
    /// スタート関数
    /// </summary>
    void Start()
    {
        GenerateCard();
    }

    /// <summary>
    /// アップデート関数
    /// </summary>
    void Update()
    {
        
    }

    public void GenerateCard()
    {
        // カードを生成
        GameObject card = Instantiate(cardPrefab, spawnPosition, Quaternion.identity);

        // ランダムなスートと数字を選ぶ
        Suit randomSuit = (Suit)Random.Range(0, 4); // Joker除く
        int randomNumber = Random.Range(1, 14);     // 1～13

        // CardControllerに渡す
        CardController controller = card.GetComponent<CardController>();
        controller.SetCard(randomSuit, randomNumber);
    }
}
