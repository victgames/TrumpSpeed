#nullable enable
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using static Define;

/// <summary>
/// 定義用クラス
/// </summary>
public class Define
{
    // *******************************************************
    // 列挙体
    // *******************************************************

    /// <summary>
    /// カードの所属
    /// </summary>
    public enum CardProperty
    {
        Deck,
        Field,
        Hand
    }

    /// <summary>
    /// トランプのスート定義
    /// </summary>
    public enum SuitType
    {
        Spade = 0,        // スペード
        Club = 1,        // クラブ
        Diamond = 2,        // ダイヤ
        Heart = 3,        // ハート
        Joker = 4         // ジョーカー
    }

    /// <summary>
    /// トランプの使用色
    /// </summary>
    public enum SuitColorMode
    {
        Both = 0,    // 両方使用
        BlackOnly = 1,    // 黒のみ
        RedOnly = 2,    // 赤のみ
        SpadeOnly = 3,    // スペード
        ClubOnly = 4,    // クラブ
        DiamondOnly = 5,    // ダイヤ
        HeartOnly = 6,    // ハート
    }

    /// <summary>
    /// ジョーカーの使用
    /// </summary>
    public enum UseJoker
    {
        None = 0,        // ジョーカーなし
        One = 1,        // 1枚使用
        Two = 2         // 2枚使用
    }

    /// <summary>
    /// カード裏面の絵柄
    /// </summary>
    public enum BackSpriteColor
    {
        Red = 0,        // 赤
        Blue = 1         // 青
    }

    
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
    public class Position
    {
        /// <summary>山札の基準位置</summary>
        public static readonly Vector3 Deck = new Vector3(7.0f, -2.0f, 0.0f);

        /// <summary>山札内のカード間のオフセット</summary>
        public static readonly Vector3 DeckOffset = new Vector3(0.01f, -0.01f, 0.01f);

        /// <summary>場札の表示位置リスト</summary>
        public static readonly IReadOnlyList<Vector3> Field = new List<Vector3>
        {
            new Vector3(-3.6f, 1.0f, 0.0f),
            new Vector3(-1.2f, 1.0f, 0.0f),
            new Vector3( 1.2f, 1.0f, 0.0f),
            new Vector3( 3.6f, 1.0f, 0.0f)
        };

        /// <summary>手札の表示位置リスト</summary>
        public static readonly IReadOnlyList<Vector3> Hand = new List<Vector3>
        {
            new Vector3(-4.5f, -2.0f, 0.0f),
            new Vector3(-1.5f, -2.0f, 0.0f),
            new Vector3( 1.5f, -2.0f, 0.0f),
            new Vector3( 4.5f, -2.0f, 0.0f)
        };
    }


}

/// <summary>
/// ソートレイヤー名を定義するクラス
/// </summary>
public static class SortLayers
{
    public static string Name(CardProperty layer)
    {
        return layer switch
        {
            CardProperty.Deck => "Deck",
            CardProperty.Field => "Field",
            CardProperty.Hand => "Hand",
            _ => "Default"
        };
    }
}


/// <summary>
/// カードを定義するクラス
/// </summary>
public class Card
{
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
    /// カードの所属
    /// </summary>
    public CardProperty CardProperty { get; set; }

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
    /// <param name="cardProperty"></param>
    /// <param name="position"></param>
    public Card(SuitType suit, int number, BackSpriteColor backColor, Sprite faceSprite, Sprite backSprite, bool isFaceUp, CardProperty cardProperty, Vector3 position)
    {
        Suit = suit;
        Number = number;
        BackColor = backColor;
        FaceSprite = faceSprite;
        BackSprite = backSprite;
        IsFaceUp = isFaceUp;
        CardProperty = cardProperty;
        Position = position;
    }
}

/// <summary>
/// カードの表示順を定義するクラス
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

    /// <summary>
    /// 場のスロット位置（0〜3など）
    /// </summary>
    public int SlotIndex { get; set; }

    // *******************************************************
    // コンストラクタ
    // *******************************************************

    /// <summary>
    /// カードのデータと見た目（表示オブジェクト）を1セットで保持する
    /// </summary>
    /// <param name="data">カードのデータ</param>
    /// <param name="view">カード表示用コンポーネント</param>
    public CardEntry(Card data, CardController view, int slotIndex)
    {
        Data = data;
        View = view;
        SlotIndex = slotIndex;
    }
}