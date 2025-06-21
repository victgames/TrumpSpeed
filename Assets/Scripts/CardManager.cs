using System.Collections.Generic;
using UnityEngine;
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
    public void DisplayDeck(List<CardEntry> entries, Vector3 pos, Vector3 offset)
    {
        for (int i = 0; i < entries.Count; i++)
        {
            CardEntry entry = entries[i];

            // カードの位置を変更して再表示
            entry.View.transform.position = pos + offset * i;

            // 所属を更新
            entry.Data.CardProperty = CardProperty.Deck;

            // 山札はスロット1つのため0に変更
            entry.Data.SlotIndex = null;

            // カードの位置情報を変更
            entry.View.GetComponent<CardController>()?.SetSorting(SortLayers.Name(CardProperty.Deck), i);
        }
    }

    /// <summary>
    /// 山札をシャッフル
    /// </summary>
    /// <param name="deck"></param>
    public void Shuffle(List<CardEntry> deck)
    {
        // i番目と, i以降のランダムに選ばれたj番目を入れ替える
        for (int i = 0; i < deck.Count; i++)
        {
            int j = Random.Range(i, deck.Count);
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

        // 山札リストから削除, 場札リストに追加
        oldEntries.RemoveAt(lastIndex);
        newEntries.Add(entry);
    }

    /*
    /// <summary>
    /// 山札から1枚引き, 場札または手札に加える
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

            // 山札から先頭のカードを取り出す
            int lastIndex = oldEntries.Count - 1;
            CardEntry entry = oldEntries[lastIndex];

            // 表示位置を更新
            entry.View.transform.position = position[i];

            // 所属を更新
            entry.Data.CardProperty = cardProperty;

            // 場のスロット位置を更新（1～4）
            entry.Data.SlotIndex = i;

            // ソート情報を更新
            entry.View.GetComponent<CardController>()?.SetSorting(SortLayers.Name(cardProperty), 0);

            // 表裏を更新
            entry.Data.IsFaceUp = true;
            entry.View.GetComponent<CardController>()?.UpdateSprite();

            // 山札リストから削除, 場札リストに追加
            oldEntries.RemoveAt(lastIndex);
            newEntries.Add(entry);
        }
    }*/

}
