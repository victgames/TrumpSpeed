using System.Collections.Generic;
using UnityEngine;
using static Define;

public class CardManager : MonoBehaviour
{
    // *******************************************************
    // ���\�b�h
    // *******************************************************

    /// <summary>
    /// �R�D��\��
    /// </summary>
    /// <param name="entries"></param>
    public void DisplayDeck(List<CardEntry> entries, Vector3 pos, Vector3 offset)
    {
        for (int i = 0; i < entries.Count; i++)
        {
            // �J�[�h�̈ʒu��ύX���čĕ\��
            CardEntry entry = entries[i];
            entry.View.transform.position = pos + offset * i;
            entry.Data.Position = pos + offset * i;

            // �J�[�h�̈ʒu����ύX
            entry.View.GetComponent<CardController>()?.SetSorting(SortLayers.Name(entry.Data.CardProperty), i);
        }
    }

    /// <summary>
    /// �R�D���V���b�t��
    /// </summary>
    /// <param name="deck"></param>
    public void Shuffle(List<CardEntry> deck)
    {
        // i�Ԗڂ�, i�ȍ~�̃����_���ɑI�΂ꂽj�Ԗڂ����ւ���
        for (int i = 0; i < deck.Count; i++)
        {
            int j = UnityEngine.Random.Range(i, deck.Count);
            (deck[i], deck[j]) = (deck[j], deck[i]);
        }
    }

    /// <summary>
    /// �R�D����1������, ��D�܂��͎�D�ɉ�����
    /// </summary>
    /// <param name="oldEntries"></param>
    /// <param name="newEntries"></param>
    /// <param name="cardProperty"></param>
    /// <param name="position"></param>
    public void DrawTopCard(List<CardEntry> oldEntries, List<CardEntry> newEntries, CardProperty cardProperty, IReadOnlyList<Vector3> position)
    {
        for (int i = 0; i < position.Count; i++)
        {
            if (oldEntries.Count == 0) break;

            // �R�D����擪�̃J�[�h�����o��
            CardEntry entry = oldEntries[0];
            oldEntries.RemoveAt(0);

            // �������X�V
            entry.Data.CardProperty = cardProperty;

            // �\���ʒu���X�V
            entry.View.transform.position = position[i];
            entry.Data.Position = position[i];

            // �\�����X�V
            entry.Data.IsFaceUp = true;
            entry.View.GetComponent<CardController>()?.SetSprite(entry.Data.IsFaceUp);

            // �\�[�g�����X�V
            entry.View.GetComponent<CardController>()?.SetSorting(SortLayers.Name(cardProperty), 0);

            // ��D���X�g�ɒǉ�
            newEntries.Add(entry);
        }
    }
}
