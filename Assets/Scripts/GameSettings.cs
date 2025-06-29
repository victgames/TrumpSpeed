using static Define;

/// <summary>
/// ゲーム難易度を保存するクラス
/// </summary>
public static class GameSettings
{
    // *******************************************************
    // プロパティ
    // *******************************************************

    /// <summary>
    /// トランプの使用色
    /// </summary>
    public static SuitColorMode SuitColorMode { get; private set; }

    /// <summary>
    /// ジョーカーの使用
    /// </summary>
    public static UseJoker UseJoker { get; private set; }

    /// <summary>
    /// カード裏面の色
    /// </summary>
    public static BackSpriteColor BackSpriteColor { get; private set; }

    // *******************************************************
    // メソッド
    // *******************************************************

    /// <summary>
    /// 難易度情報をプロパティにセット
    /// </summary>
    /// <param name="suitColorMode">トランプの使用色</param>
    /// <param name="useJoker">ジョーカーの使用</param>
    public static void SetGameSettings(SuitColorMode suitColorMode, UseJoker useJoker, BackSpriteColor backSpriteColor)
    {
        SuitColorMode = suitColorMode;
        UseJoker = useJoker;
        BackSpriteColor = backSpriteColor;
    }
}
