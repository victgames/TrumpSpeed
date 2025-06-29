#nullable enable
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;
using static Define;

public class CardManager : MonoBehaviour
{
    // *******************************************************
    // メソッド
    // *******************************************************

    /// <summary>
    /// 山札から1枚引き, 場札または手札に加える
    /// </summary>
    /// <param name="deckEntries">山札リスト</param>
    /// <param name="handEntries">手札リスト</param>
    /// <param name="cardProperty">カードの所属</param>
    /// <param name="position">位置</param>
    /// <param name="slotIndex">インデックス</param>
    private void DrawTopCard(List<CardEntry> deckEntries, List<CardEntry> handEntries, CardProperty cardProperty, Vector3 position, int slotIndex)
    {
        if (deckEntries.Count == 0) return;

        // 山札から先頭のカードを取り出す
        int lastIndex = deckEntries.Count - 1;
        CardEntry entry = deckEntries[lastIndex];

        UpdateEntry(entry, position, true, cardProperty, 0, slotIndex);

        // 山札リストから削除, 場札リストに追加
        deckEntries.RemoveAt(lastIndex);
        handEntries.Add(entry);
    }

    /// <summary>
    /// 山札, 手札, 場札を表示する処理
    /// </summary>
    /// <param name="deckEntries">山札リスト</param>
    /// <param name="fieldEntries">場札リスト</param>
    /// <param name="handEntries">手札リスト</param>
    /// <param name="runFirstOnly">開始時処理か</param>
    public void ArrangeFieldCards(List<CardEntry> deckEntries, List<CardEntry> fieldEntries, List<CardEntry> handEntries, bool runFirstOnly)
    {
        do
        {
            // 山札と手札を混ぜる
            MergeCardEntries(handEntries, deckEntries);

            // 山札を表示
            DisplayDeck(deckEntries);

            // 手札を引く
            for (int slotIndex = 0; slotIndex < Define.Position.Hand.Count; slotIndex++)
            {
                DrawTopCard(deckEntries, handEntries, CardProperty.Hand, Define.Position.Hand[slotIndex], slotIndex);
            }

            // 最初のみ実行
            if (runFirstOnly)
            {
                // 場札を引く
                for (int slotIndex = 0; slotIndex < Define.Position.Field.Count; slotIndex++)
                {
                    DrawTopCard(deckEntries, fieldEntries, CardProperty.Field, Define.Position.Field[slotIndex], slotIndex);
                }
            }
        }
        // 重ねられるかを判定
        while (!SpeedRule.JudgeSequential(fieldEntries, handEntries));
    }

    /// <summary>
    /// 場のカードの位置, 役割を変更
    /// </summary>
    /// <param name="selectedObj">選択したカード</param>
    /// <param name="targetObj">重ねる対象のカード</param>
    public void TransferCardEntry(List<CardEntry> deckEntries, List<CardEntry> fieldEntries, List<CardEntry> handEntries, List<CardEntry> noneEntries, GameObject selectedObj, GameObject targetObj)
    {
        // 手札の位置を保存
        int slotIndex = selectedObj.GetComponent<CardController>().Card.SlotIndex;

        // 選択したカードの情報を更新
        CardEntry? selectedEntry = handEntries.FirstOrDefault(entry => entry.View != null && entry.View.gameObject == selectedObj);
        CardEntry? targetEntry = fieldEntries.FirstOrDefault(entry => entry.View != null && entry.View.gameObject == targetObj);
        UpdateEntry(selectedEntry, targetEntry.View.transform.position, true, CardProperty.Field, 0, targetEntry.Data.SlotIndex);

        // 選択したカードを場札リストに移動
        handEntries.Remove(selectedEntry);
        fieldEntries.Add(selectedEntry);

        // 重ねる対象のカードを非表示リストに移動
        targetObj.SetActive(false);
        fieldEntries.Remove(targetEntry);
        noneEntries.Add(targetEntry);

        // 手札に新しいカードを追加
        DrawTopCard(deckEntries, handEntries, CardProperty.Hand, Define.Position.Hand[slotIndex], slotIndex);
    }

    /// <summary>
    /// 山札と手札を混ぜる処理
    /// </summary>
    /// <param name="fromEntries">要素を移動させるリスト</param>
    /// <param name="toEntries">要素が移動してくるリスト</param>
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
        }
    }

    /// <summary>
    /// 山札を表示
    /// </summary>
    /// <param name="entries">リスト</param>
    private void DisplayDeck(List<CardEntry> entries)
    {
        // シャッフル後山札を表示
        Shuffle(entries);

        for (int i = 0; i < entries.Count; i++)
        {
            Vector3 position = Define.Position.Deck + Define.Position.DeckOffset * i;
            UpdateEntry(entries[i], position, false, CardProperty.Deck, i, 0);
        }
    }

    /// <summary>
    /// 山札をシャッフル
    /// </summary>
    /// <param name="entries">リスト</param>
    private void Shuffle(List<CardEntry> entries)
    {
        for (int i = entries.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1); // jは0〜iの範囲
            (entries[i], entries[j]) = (entries[j], entries[i]);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="entry">エントリー</param>
    /// <param name="position">位置</param>
    /// <param name="isFaceUp">表裏</param>
    /// <param name="cardProperty">カードの所属</param>
    /// <param name="order">レンダラーオーダー順</param>
    /// <param name="slotIndex">インデックス</param>
    public void UpdateEntry(CardEntry entry, Vector3 position, bool isFaceUp, CardProperty cardProperty, int order, int slotIndex)
    {
        // カードの位置を変更して再表示
        entry.View.transform.position = position;

        // カードの情報を更新
        entry.View.GetComponent<CardController>()?.SetSprite(isFaceUp);
        entry.View.GetComponent<CardController>()?.SetCardProperty(cardProperty);
        entry.View.GetComponent<CardController>()?.SetSortingOrder(order);
        entry.View.GetComponent<CardController>()?.SetSlotIndex(slotIndex);
    }
}
