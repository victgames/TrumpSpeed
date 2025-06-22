#nullable enable
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
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
    public void DisplayDeck(List<CardEntry> entries, Vector3 basisPosition, Vector3 offset)
    {
        for (int i = 0; i < entries.Count; i++)
        {
            CardEntry entry = entries[i];

            Vector3 position = basisPosition + offset * i;

            UpdateEntry(entry, position, false, CardProperty.Deck, i, 0);

            /*
            // �J�[�h�̈ʒu��ύX���čĕ\��
            entry.View.transform.position = pos + offset * i;

            // �J�[�h�̗��\���X�V
            entry.View.GetComponent<CardController>()?.SetSprite(false);
            entry.View.GetComponent<CardController>()?.SetCardProperty(CardProperty.Deck);
            entry.View.GetComponent<CardController>()?.SetSortingOrder(i);
            entry.View.GetComponent<CardController>()?.SetSlotIndex(0);
            */
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

        UpdateEntry(entry, position, true, cardProperty, 0, slotIndex);

        /*
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
        */

        // �R�D���X�g����폜, ��D���X�g�ɒǉ�
        oldEntries.RemoveAt(lastIndex);
        newEntries.Add(entry);
    }

    public void MergeCardEntries(List<CardEntry> fromEntries, List<CardEntry> toEntries)
    {
        // 1. fromEntries�̑S�v�f��toEntries�Ɉړ��i���Z�b�g�͂܂����Ȃ��j
        for (int i = 0; i < fromEntries.Count; i++)
        {
            CardEntry entry = fromEntries[i];
            fromEntries.RemoveAt(i);
            toEntries.Add(entry);
        }

        // 2. toEntries�̑S�v�f�ɑ΂��ă��Z�b�g���������s
        for (int i = 0; i < toEntries.Count; i++)
        {
            CardEntry entry = toEntries[i];

            UpdateEntry(entry, Define.Position.None, false, CardProperty.None, -1, -1);

            /*
            entry.View.transform.position = Vector3.zero;
            entry.Data.CardProperty = CardProperty.None;
            entry.Data.SlotIndex = null;
            entry.View.GetComponent<CardController>()?.SetSorting(SortLayers.Name(CardProperty.None), 0);
            entry.Data.IsFaceUp = false;
            entry.View.GetComponent<CardController>()?.UpdateSprite();
            */
        }
    }

    private void UpdateEntry(CardEntry entry, Vector3 position, bool isFaceUp, CardProperty cardProperty, int order, int slotIndex)
    {
        // �J�[�h�̈ʒu��ύX���čĕ\��
        entry.View.transform.position = position;

        // �J�[�h�̗��\���X�V
        entry.View.GetComponent<CardController>()?.SetSprite(isFaceUp);
        entry.View.GetComponent<CardController>()?.SetCardProperty(cardProperty);
        entry.View.GetComponent<CardController>()?.SetSortingOrder(order);
        entry.View.GetComponent<CardController>()?.SetSlotIndex(slotIndex);
    }
}
