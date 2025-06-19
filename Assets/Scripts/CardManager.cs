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
            // カードの位置を変更して再表示
            CardEntry entry = entries[i];
            entry.View.transform.position = pos + offset * i;
            entry.Data.Position = pos + offset * i;

            // カードの位置情報を変更
            entry.View.GetComponent<CardController>()?.SetSorting(SortLayers.Name(entry.Data.CardProperty), i);
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
            int j = UnityEngine.Random.Range(i, deck.Count);
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
    public void DrawTopCard(List<CardEntry> oldEntries, List<CardEntry> newEntries, CardProperty cardProperty, IReadOnlyList<Vector3> position)
    {
        for (int i = 0; i < position.Count; i++)
        {
            if (oldEntries.Count == 0) break;

            // 山札から先頭のカードを取り出す
            CardEntry entry = oldEntries[0];
            oldEntries.RemoveAt(0);

            // 所属を更新
            entry.Data.CardProperty = cardProperty;

            // 表示位置を更新
            entry.View.transform.position = position[i];
            entry.Data.Position = position[i];

            // 表裏を更新
            entry.Data.IsFaceUp = true;
            entry.View.GetComponent<CardController>()?.SetSprite(entry.Data.IsFaceUp);

            // ソート情報を更新
            entry.View.GetComponent<CardController>()?.SetSorting(SortLayers.Name(cardProperty), 0);

            // 場札リストに追加
            newEntries.Add(entry);
        }
    }
}
