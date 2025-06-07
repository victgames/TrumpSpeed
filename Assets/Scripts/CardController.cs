using System.Collections.Generic;
using UnityEngine;
using static Define;
using static Define.Card;

public class CardController : MonoBehaviour
{
    // *******************************************************
    // 定数
    // *******************************************************

    // *******************************************************
    // メンバ変数
    // *******************************************************

    /// <summary>
    /// カードをセットするインスペクタ（表側54枚分）
    /// spade, club, heart, diamond, joker
    /// </summary>
    [SerializeField]
    private Sprite[] cardSprites;

    /// <summary>
    /// カードをセットするインスペクタ（裏側1種類）
    /// </summary>
    [SerializeField]
    private Sprite[] backSprites;

    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private bool useJoker = true; // InspectorでON/OFF切替できる

    private List<Card> deck;

    /// <summary>
    /// カードの絵柄
    /// </summary>
    public Suit suit;

    /// <summary>
    /// カードの数字
    /// </summary>
    public int number;

    /// <summary>
    /// カードの裏表
    /// </summary>
    private bool isFaceUp = false;



    // *******************************************************
    // メソッド
    // *******************************************************

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        deck = CreateDeck(useJoker);
        //Shuffle(deck);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 
    /// </summary>
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="suit"></param>
    /// <param name="number"></param>
    public void SetCard(Suit suit, int number)
    {
        this.suit = suit;
        this.number = number;

        if (suit == Suit.Joker)
        {
            spriteRenderer.sprite = cardSprites[52 + number]; // number: 0 or 1
        }
        else
        {
            int index = ((int)suit) * 13 + (number - 1); // 0〜51
            spriteRenderer.sprite = cardSprites[index];
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="suit"></param>
    /// <param name="number"></param>
    /// <returns></returns>
    private int GetSpriteIndex(Suit suit, int number)
    {
        // 例：Spade=0〜12, Heart=13〜25...
        return ((int)suit) * 13 + (number - 1);
    }

    public void Flip(bool faceUp)
    {
        isFaceUp = faceUp;

        if (isFaceUp)
        {
            SetCard(suit, number); // 表向き
        }
        else
        {
            spriteRenderer.sprite = backSprites[0]; // 裏向き
        }
    }

    private List<Card> CreateDeck(bool includeJoker)
    {
        var newDeck = new List<Card>();

        // 通常の52枚
        foreach (Suit suit in System.Enum.GetValues(typeof(Suit)))
        {
            if (suit == Suit.Joker) continue;

            for (int number = 1; number <= 13; number++)
            {
                newDeck.Add(new Card(suit, number));
            }
        }

        // Joker追加（同じもの2枚）
        if (includeJoker)
        {
            newDeck.Add(new Card(Suit.Joker, 0));
            newDeck.Add(new Card(Suit.Joker, 0)); // 同じもの
        }

        return newDeck;
    }

}
