using Unity.VisualScripting.Antlr3.Runtime;
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
    /// タグ定義
    /// </summary>
    public const string TAG_DECK = "DeckCard";
    public const string TAG_FIELD = "FieldCard";
    public const string TAG_HAND = "HandCard";

    /// <summary>
    /// ソーティングレイヤー定義
    /// </summary>
    public const string SORT_LAYER_DECK = "Deck";
    public const string SORT_LAYER_FIELD = "Field";
    public const string SORT_LAYER_HAND = "Hand";


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
        // プロパティ
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
        /// カード裏面の絵柄
        /// </summary>
        public BackSpriteColor BackColor { get; private set; }

        /// <summary>
        /// 裏表
        /// </summary>
        public bool IsFaceUp { get; set; }

        /// <summary>
        /// 位置
        /// </summary>
        public Vector2 SpawnPosition { get; set; }

        /// <summary>
        /// タグ
        /// </summary>
        //public string Tag { get; set; }

        /// <summary>
        /// ソーティングレイヤー
        /// </summary>
        //public string SortingLayerName { get; set; }


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
    /// カードの表示順を定義
    /// </summary>
    public class CardEntry
    {
        // *******************************************************
        // プロパティ
        // *******************************************************

        /// <summary>
        /// カードのデータ（スート・数字・裏表など）
        /// </summary>
        public Card Data { get; private set; }

        /// <summary>
        /// カードの見た目を制御するコンポーネント（GameObjectにアタッチ）
        /// </summary>
        public CardController View { get; private set; }


        // *******************************************************
        // コンストラクタ
        // *******************************************************

        /// <summary>
        /// カードのデータと見た目（表示オブジェクト）を1セットで保持する
        /// </summary>
        /// <param name="data">カードのデータ</param>
        /// <param name="view">カード表示用コンポーネント</param>
        public CardEntry(Card data, CardController view)
        {
            Data = data;
            View = view;
        }
    }
}
