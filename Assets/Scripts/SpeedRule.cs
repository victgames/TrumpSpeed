using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SpeedRule : MonoBehaviour
{
    // *******************************************************
    // �����o�ϐ�
    // *******************************************************

    /// <summary>
    /// �J�[�hA��B��Speed�̃��[����ŘA�����Ă��邩�𔻒�i�}1 or 1-13�j
    /// </summary>
    public static bool IsSequential(Card handCard, Card fieldCard)
    {
        int a = handCard.Number;
        int b = fieldCard.Number;

        return (Mathf.Abs(a - b) == 1) || (a == 1 && b == 13) || (a == 13 && b == 1) || (a == 0 || b == 0);
    }

    /// <summary>
    /// �C�ӂ�2�����A�����Ă��邩��T��
    /// </summary>
    /// <param name="firstEntries"></param>
    /// <param name="secondEntries"></param>
    /// <returns></returns>
    public static bool JudgeSequential(List<CardEntry> firstEntries, List<CardEntry> secondEntries)
    {
        // ���ׂĂ̑g�ݍ��킹��T��
        for (int i = 0; i < firstEntries.Count; i++)
        {
            for (int j = 0; j < secondEntries.Count; j++)
            {
                // 1�ł��d�˂鏈�����\�ȑg�ݍ��킹�������true
                if (IsSequential(firstEntries[i].Data, secondEntries[j].Data))
                {
                    return true;
                }
            }
        }
        return false;
    }
}
