using UnityEngine;

/// <summary>
/// 定義用クラス
/// </summary>
public class Define
{
    /// <summary>
    /// カードを定義するクラス
    /// </summary>
    public class Card
    {
        // *******************************************************
        // 列挙体
        // *******************************************************

        /// <summary>
        /// トランプの絵柄定義
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
        // メンバ変数
        // *******************************************************

        /// <summary>
        /// トランプの絵柄
        /// </summary>
        public Suit suit;
        
        /// <summary>
        /// トランプの数字
        /// </summary>
        public int number;

        // *******************************************************
        // コンストラクタ
        // *******************************************************

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="suit">トランプの絵柄</param>
        /// <param name="number">トランプの数字</param>
        public Card(Suit suit, int number)
        {
            this.suit = suit;
            this.number = number;
        }
    }
}
