using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// スピードゲームに関する処理
/// </summary>
public class SpeedRule : MonoBehaviour
{
    // *******************************************************
    // メンバ変数
    // *******************************************************

    /// <summary>
    /// カードAとBがSpeedのルール上で連続しているかを判定（±1 or 1-13 or Joker）
    /// </summary>
    /// <param name="handCard">手札数字</param>
    /// <param name="fieldCard">場札数字</param>
    /// <returns>カード重ね判定結果</returns>
    public static bool IsSequential(Card handCard, Card fieldCard)
    {
        int a = handCard.Number;
        int b = fieldCard.Number;

        return (Mathf.Abs(a - b) == 1) || (a == 1 && b == 13) || (a == 13 && b == 1) || (a == 0 || b == 0);
    }

    /// <summary>
    /// 任意の2枚が連続しているかを探索
    /// </summary>
    /// <param name="firstEntries">エントリー（1つ目）</param>
    /// <param name="secondEntries">エントリー（2つ目）</param>
    /// <returns>カード重ねの組み合わせがあるか判定結果</returns>
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
