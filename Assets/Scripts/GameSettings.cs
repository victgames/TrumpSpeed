using static Define;

/// <summary>
/// �Q�[����Փx��ۑ�����N���X
/// </summary>
public static class GameSettings
{
    // *******************************************************
    // �v���p�e�B
    // *******************************************************

    /// <summary>
    /// �g�����v�̎g�p�F
    /// </summary>
    public static SuitColorMode SuitColorMode { get; private set; }

    /// <summary>
    /// �W���[�J�[�̎g�p
    /// </summary>
    public static UseJoker UseJoker { get; private set; }

    /// <summary>
    /// �J�[�h���ʂ̐F
    /// </summary>
    public static BackSpriteColor BackSpriteColor { get; private set; }

    // *******************************************************
    // ���\�b�h
    // *******************************************************

    /// <summary>
    /// ��Փx�����v���p�e�B�ɃZ�b�g
    /// </summary>
    /// <param name="suitColorMode">�g�����v�̎g�p�F</param>
    /// <param name="useJoker">�W���[�J�[�̎g�p</param>
    public static void SetGameSettings(SuitColorMode suitColorMode, UseJoker useJoker, BackSpriteColor backSpriteColor)
    {
        SuitColorMode = suitColorMode;
        UseJoker = useJoker;
        BackSpriteColor = backSpriteColor;
    }
}
