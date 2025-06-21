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
            CardEntry entry = entries[i];

            // �J�[�h�̈ʒu��ύX���čĕ\��
            entry.View.transform.position = pos + offset * i;

            // �������X�V
            entry.Data.CardProperty = CardProperty.Deck;

            // �R�D�̓X���b�g1�̂���0�ɕύX
            entry.Data.SlotIndex = null;

            // �J�[�h�̈ʒu����ύX
            entry.View.GetComponent<CardController>()?.SetSorting(SortLayers.Name(CardProperty.Deck), i);
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
            int j = Random.Range(i, deck.Count);
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
    /// <param name="slotIndex"></param>
    public void DrawTopCard(List<CardEntry> oldEntries, List<CardEntry> newEntries, CardProperty cardProperty, Vector3 position, int slotIndex)
    {
        if (oldEntries.Count == 0) return;

        // �R�D����擪�̃J�[�h�����o��
        int lastIndex = oldEntries.Count - 1;
        CardEntry entry = oldEntries[lastIndex];

        // �\���ʒu���X�V
        entry.View.transform.position = position;

        // �������X�V
        entry.Data.CardProperty = cardProperty;

        // ��̃X���b�g�ʒu���X�V�i1�`4�j
        entry.Data.SlotIndex = slotIndex;

        // �\�[�g�����X�V
        entry.View.GetComponent<CardController>()?.SetSorting(SortLayers.Name(cardProperty), 0);

        // �\�����X�V
        entry.Data.IsFaceUp = true;
        entry.View.GetComponent<CardController>()?.UpdateSprite();

        // �R�D���X�g����폜, ��D���X�g�ɒǉ�
        oldEntries.RemoveAt(lastIndex);
        newEntries.Add(entry);
    }

    /*
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
            int lastIndex = oldEntries.Count - 1;
            CardEntry entry = oldEntries[lastIndex];

            // �\���ʒu���X�V
            entry.View.transform.position = position[i];

            // �������X�V
            entry.Data.CardProperty = cardProperty;

            // ��̃X���b�g�ʒu���X�V�i1�`4�j
            entry.Data.SlotIndex = i;

            // �\�[�g�����X�V
            entry.View.GetComponent<CardController>()?.SetSorting(SortLayers.Name(cardProperty), 0);

            // �\�����X�V
            entry.Data.IsFaceUp = true;
            entry.View.GetComponent<CardController>()?.UpdateSprite();

            // �R�D���X�g����폜, ��D���X�g�ɒǉ�
            oldEntries.RemoveAt(lastIndex);
            newEntries.Add(entry);
        }
    }*/

}
