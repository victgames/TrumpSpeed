#nullable enable
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static Define;

public class CardManager : MonoBehaviour
{
    // *******************************************************
    // メソッド
    // *******************************************************

    /// <summary>
    /// 山札を表示
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
            // カードの位置を変更して再表示
            entry.View.transform.position = pos + offset * i;

            // カードの裏表を更新
            entry.View.GetComponent<CardController>()?.SetSprite(false);
            entry.View.GetComponent<CardController>()?.SetCardProperty(CardProperty.Deck);
            entry.View.GetComponent<CardController>()?.SetSortingOrder(i);
            entry.View.GetComponent<CardController>()?.SetSlotIndex(0);
            */
        }
    }

    /// <summary>
    /// 山札をシャッフル
    /// </summary>
    /// <param name="deck"></param>
    public void Shuffle(List<CardEntry> deck)
    {
        for (int i = deck.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1); // jは0〜iの範囲
            (deck[i], deck[j]) = (deck[j], deck[i]);
        }
    }


    /// <summary>
    /// 山札から1枚引き, 場札または手札に加える
    /// </summary>
    /// <param name="oldEntries"></param>
    /// <param name="newEntries"></param>
    /// <param name="cardProperty"></param>
    /// <param name="position"></param>
    /// <param name="slotIndex"></param>
    public void DrawTopCard(List<CardEntry> oldEntries, List<CardEntry> newEntries, CardProperty cardProperty, Vector3 position, int slotIndex)
    {
        if (oldEntries.Count == 0) return;

        // 山札から先頭のカードを取り出す
        int lastIndex = oldEntries.Count - 1;
        CardEntry entry = oldEntries[lastIndex];

        UpdateEntry(entry, position, true, cardProperty, 0, slotIndex);

        /*
        // 表示位置を更新
        entry.View.transform.position = position;

        // 所属を更新
        entry.Data.CardProperty = cardProperty;

        // 場のスロット位置を更新（1～4）
        entry.Data.SlotIndex = slotIndex;

        // ソート情報を更新
        entry.View.GetComponent<CardController>()?.SetSorting(SortLayers.Name(cardProperty), 0);

        // 表裏を更新
        entry.Data.IsFaceUp = true;
        entry.View.GetComponent<CardController>()?.UpdateSprite();
        */

        // 山札リストから削除, 場札リストに追加
        oldEntries.RemoveAt(lastIndex);
        newEntries.Add(entry);
    }

    public void MergeCardEntries(List<CardEntry> fromEntries, List<CardEntry> toEntries)
    {
        // fromEntriesの要素をすべて移動させる
        for (int i = fromEntries.Count - 1; i >= 0; i--)
        {
            CardEntry entry = fromEntries[i];
            fromEntries.RemoveAt(i);
            toEntries.Add(entry);
        }

        // 2. toEntriesの全要素に対してリセット処理を実行
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

    public void UpdateEntry(CardEntry entry, Vector3 position, bool isFaceUp, CardProperty cardProperty, int order, int slotIndex)
    {
        // カードの位置を変更して再表示
        entry.View.transform.position = position;

        // カードの裏表を更新
        entry.View.GetComponent<CardController>()?.SetSprite(isFaceUp);
        entry.View.GetComponent<CardController>()?.SetCardProperty(cardProperty);
        entry.View.GetComponent<CardController>()?.SetSortingOrder(order);
        entry.View.GetComponent<CardController>()?.SetSlotIndex(slotIndex);
    }
}
