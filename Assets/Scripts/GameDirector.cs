using System.Collections.Generic;
using UnityEngine;
using static Define;
using static Define.Card;

public class GameDirector : MonoBehaviour
{
    // *******************************************************
    // メンバ変数
    // *******************************************************

    /// <summary>
    /// カード生成スクリプトへの参照
    /// </summary>
    [SerializeField] 
    private CardGenerator _cardGenerator;

    /// <summary>
    /// デッキ黒
    /// </summary>
    private List<Card> _deckBlack = new List<Card>();

    /// <summary>
    /// デッキ赤
    /// </summary>
    private List<Card> _deckRed = new List<Card>();


    // *******************************************************
    // メソッド
    // *******************************************************

    /// <summary>
    /// 開始時処理
    /// </summary>
    void Start()
    {
        _deckBlack = _cardGenerator.CreateDeck(SuitColorMode.BlackOnly, UseJoker.None);
        _deckRed = _cardGenerator.CreateDeck(SuitColorMode.RedOnly, UseJoker.None);

        _cardGenerator.Shuffle(_deckRed);

        _cardGenerator.GenerateCard(_deckRed, new Vector2(0, 0), true, BackSpriteColor.Red);

        _cardGenerator.GenerateCard(_deckRed, new Vector2(-6, 0), true, BackSpriteColor.Red);

        _cardGenerator.GenerateCard(_deckRed, new Vector2(6, 3), false, BackSpriteColor.Blue);

        /*
        int count = deckBlack.Count;

        for (int i = count - 1; i >= 0; i--)
        {
            cardGenerator.GenerateCardFace(deckBlack);
        }
        */

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    
}
