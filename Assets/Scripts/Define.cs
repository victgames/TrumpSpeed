using UnityEngine;

/// <summary>
/// 定義用クラス
/// </summary>
public class Define
{

    // *******************************************************
    // 定数
    // *******************************************************

    /// <summary>
    /// カードの生成位置
    /// </summary>
    public Vector2 spawnPos = new Vector2(0, 0);


    /// <summary>
    /// カードを定義するクラス
    /// </summary>
    public class Card
    {
        // *******************************************************
        // 列挙体
        // *******************************************************

        /// <summary>
        /// トランプのスート定義
        /// </summary>
        public enum SuitType
        {
            Spade   = 0,        // スペード
            Club    = 1,        // クラブ
            Diamond = 2,        // ダイヤ
            Heart   = 3,        // ハート
            Joker   = 4         // ジョーカー
        }

        /// <summary>
        /// トランプの使用色
        /// </summary>
        public enum SuitColorMode
        {
            Both        = 0,    // 両方使用
            BlackOnly   = 1,    // 黒のみ
            RedOnly     = 2     // 赤のみ
        }

        /// <summary>
        /// ジョーカーの使用
        /// </summary>
        public enum UseJoker
        {
            None    = 0,        // ジョーカーなし
            One     = 1,        // 1枚使用
            Two     = 2         // 2枚使用
        }

        /// <summary>
        /// カード裏面の絵柄
        /// </summary>
        public enum BackSpriteColor
        {
            Red     = 0,        // 赤
            Blue    = 1         // 青
        }

        // *******************************************************
        // メンバ変数
        // *******************************************************

        /// <summary>
        /// スート
        /// </summary>
        public SuitType Suit { get; private set; }

        /// <summary>
        /// 数字（1～13, Jokerは0）
        /// </summary>
        public int Number { get; private set; }

        /// <summary>
        /// 裏表
        /// </summary>
        public bool IsFaceUp { get; private set; }

        /// <summary>
        /// 位置
        /// </summary>
        public Vector2 SpawnPosition { get; private set; }

        /// <summary>
        /// カード裏面の絵柄
        /// </summary>
        public BackSpriteColor BackColor { get; private set; }


        // *******************************************************
        // コンストラクタ
        // *******************************************************

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="suit">スート</param>
        /// <param name="number">数字</param>
        /// <param name="isFaceUp">裏表</param>
        /// <param name="spawnPosition">位置</param>
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
