using System.Collections.Generic;
using System.Threading;
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

    /// <summary>
    /// 任意の2枚が連続しているかを探索
    /// </summary>
    /// <param name="firstEntries"></param>
    /// <param name="secondEntries"></param>
    /// <returns></returns>
    public static bool JudgeSequential(List<CardEntry> firstEntries, List<CardEntry> secondEntries)
    {
        // すべての組み合わせを探索
        for (int i = 0; i < firstEntries.Count; i++)
        {
            for (int j = 0; j < secondEntries.Count; j++)
            {
                // 1つでも重ねる処理が可能な組み合わせがあればtrue
                if (IsSequential(firstEntries[i].Data, secondEntries[j].Data))
                {
                    return true;
                }
            }
        }
        return false;
    }
}
