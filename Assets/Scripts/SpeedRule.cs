using UnityEngine;

public class SpeedRule : MonoBehaviour
{
    // *******************************************************
    // メンバ変数
    // *******************************************************

    /// <summary>
    /// カードAとBがSpeedのルール上で連続しているかを判定（±1 or 1-13）
    /// </summary>
    public static bool IsSequential(Card handCard, Card fieldCard)
    {
        int a = handCard.Number;
        int b = fieldCard.Number;

        return (Mathf.Abs(a - b) == 1) || (a == 1 && b == 13) || (a == 13 && b == 1) || (a == 0 || b == 0);
    }
}
