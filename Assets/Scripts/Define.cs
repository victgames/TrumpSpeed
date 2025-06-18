#nullable enable
using System.Collections.Generic;
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
    /// 表示位置
    /// </summary>
    public static readonly Vector3 POS_DECK = new Vector3(7.0f, -2.0f, 0.0f);
    public static readonly Vector3 POS_DECK_OFFSET = new Vector3(0.01f, -0.01f, 0.01f);
    public static readonly List<Vector2> POS_FIELD = new List<Vector2>
    {
        new Vector3(-3.6f, 1.0f, 0.0f),
        new Vector3(-1.2f, 1.0f, 0.0f),
        new Vector3( 1.2f, 1.0f, 0.0f),
        new Vector3( 3.6f, 1.0f, 0.0f)
    };
    public static readonly List<Vector2> POS_HAND = new List<Vector2>
    {
        new Vector3(-4.5f, -2.0f, 0.0f),
        new Vector3(-1.5f, -2.0f, 0.0f),
        new Vector3( 1.5f, -2.0f, 0.0f),
        new Vector3( 4.5f, -2.0f, 0.0f)
    };


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
            RedOnly     = 2,    // 赤のみ
            SpadeOnly   = 3,    // スペード
            ClubOnly    = 4,    // クラブ
            DiamondOnly = 5,    // ダイヤ
            HeartOnly   = 6,    // ハート
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
        /// 表面絵柄
        /// </summary>
        public Sprite FaceSprite { get; private set; }

        /// <summary>
        /// 裏面絵柄
        /// </summary>
        public Sprite BackSprite { get; private set; }

        /// <summary>
        /// 裏表
        /// </summary>
        public bool IsFaceUp { get; set; }

        /// <summary>
        /// 位置
        /// </summary>
        public Vector3 Position { get; set; }


        // *******************************************************
        // コンストラクタ
        // *******************************************************

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="suit"></param>
        /// <param name="number"></param>
        /// <param name="backColor"></param>
        /// <param name="faceSprite"></param>
        /// <param name="backSprite"></param>
        /// <param name="isFaceUp"></param>
        /// <param name="spawnPosition"></param>
        public Card(SuitType suit, int number, BackSpriteColor backColor, Sprite faceSprite, Sprite backSprite, bool isFaceUp, Vector3 spawnPosition)
        {
            Suit = suit;
            Number = number;
            BackColor = backColor;
            FaceSprite = faceSprite;
            BackSprite = backSprite;
            IsFaceUp = isFaceUp;
            Position = spawnPosition;
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
