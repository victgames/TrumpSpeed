#nullable enable
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine;
using UnityEngine.UI; // �� �����ǉ��I
using static Define;
using static UnityEngine.EventSystems.EventTrigger;


public class GameDirector : MonoBehaviour
{
    // *******************************************************
    // �����o�ϐ�
    // *******************************************************

    /// <summary>
    /// �J�[�h�𐶐�����R���|�[�l���g
    /// </summary>
    [SerializeField]
    private CardGenerator _cardGenerator;

    /// <summary>
    /// �J�[�h������Ǘ�����R���|�[�l���g
    /// </summary>
    [SerializeField]
    private CardManager _cardManager;

    /// <summary>
    /// UI������Ǘ�����R���|�[�l���g
    /// </summary>
    [SerializeField]
    private UIManager _uiManager;

    /// <summary>
    /// �R�D���X�g�i�ԁj
    /// </summary>
    private List<CardEntry> _entriesDeckRed = new List<CardEntry>();

    /// <summary>
    /// ��D���X�g�i�ԁj
    /// </summary>
    private List<CardEntry> _entriesFieldRed = new List<CardEntry>();

    /// <summary>
    /// ��D���X�g�i�ԁj
    /// </summary>
    private List<CardEntry> _entriesHandRed = new List<CardEntry>();

    /// <summary>
    /// �g�p�ς݃J�[�h���X�g�i�ԁj
    /// </summary>
    private List<CardEntry> _entriesNoneRed = new List<CardEntry>();


    // *******************************************************
    // �v���p�e�B
    // *******************************************************

    /// <summary>
    /// �C���X�^���X
    /// </summary>
    public static GameDirector Instance { get; private set; }


    // *******************************************************
    // Unity���\�b�h
    // *******************************************************

    /// <summary>
    /// �Q�[���J�n�O�̏���������
    /// </summary>
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // ������������Ă�����폜
            return;
        }

        Instance = this;
    }

    /// <summary>
    /// �Q�[���J�n���̏���������
    /// </summary>
    private void Start()
    {
        // �J�[�h���X�g���쐬
        List<Card> cardList = _cardGenerator.InitializeCardList(SuitColorMode.SpadeOnly, UseJoker.None, BackSpriteColor.Red);
        _entriesDeckRed = _cardGenerator.InitializeEntries(cardList);

        // �V���b�t����R�D��\��
        _cardManager.Shuffle(_entriesDeckRed);

        for (int i = 0; i < _entriesDeckRed.Count; i++)
        {
            Vector3 position = Position.Deck + Position.DeckOffset * i;
            _cardManager.UpdateEntry(_entriesDeckRed[i], position, false, CardProperty.Deck, i, 0);
        }

        // ��D, ��D������
        for (int slotIndex = 0; slotIndex < Position.Field.Count; slotIndex++)
        {
            _cardManager.DrawTopCard(_entriesDeckRed, _entriesFieldRed, CardProperty.Field, Position.Field[slotIndex], slotIndex);
        }
        for (int slotIndex = 0; slotIndex < Position.Hand.Count; slotIndex++)
        {
            _cardManager.DrawTopCard(_entriesDeckRed, _entriesHandRed, CardProperty.Hand, Position.Hand[slotIndex], slotIndex);
        }

        MargeHandCardAndDraw();
    }

    /// <summary>
    /// ��D����ւ��{�^���N���b�N������
    /// </summary>
    public void OnRedistributionButtonClick()
    {
        _uiManager.DisplayRedistributionButton(false);

        MargeHandCardAndDraw();
    }

    /// <summary>
    /// �X�^�[�g�V�[���ȍ~�{�^���N���b�N������
    /// </summary>
    public void OnNextButtonClick()
    {
        SceneManager.LoadScene("StartScene"); // �V�[�����őJ��
    }

    // *******************************************************
    // ���\�b�h
    // *******************************************************

    public void TransferCardEntry(GameObject selectedObj, GameObject targetObj)
    {
        int slotIndex = selectedObj.GetComponent<CardController>().Card.SlotIndex;

        CardEntry? selectedEntry = _entriesHandRed.FirstOrDefault(entry => entry.View != null && entry.View.gameObject == selectedObj);
        CardEntry? targetEntry = _entriesFieldRed.FirstOrDefault(entry => entry.View != null && entry.View.gameObject == targetObj);

        _cardManager.UpdateEntry(selectedEntry, targetEntry.View.transform.position, true, CardProperty.Field, targetEntry.View.SpriteRenderer.sortingOrder + 1, targetEntry.Data.SlotIndex);

        _entriesHandRed.Remove(selectedEntry);
        _entriesFieldRed.Add(selectedEntry);

        

        //CardEntry? entry = _entriesFieldRed.FirstOrDefault(entry => entry.View != null && entry.View.gameObject == gameObject);

        //_cardManager.UpdateEntry(entry, Position.None, false, CardProperty.None, 0, 0);

        targetObj.SetActive(false);

        _entriesFieldRed.Remove(targetEntry);
        _entriesNoneRed.Add(targetEntry);

        _cardManager.DrawTopCard(_entriesDeckRed, _entriesHandRed, CardProperty.Hand, Position.Hand[slotIndex], slotIndex);

        JudgeGameFinish();

    }

    /*
    /// <summary>
    /// ��D��1���ǉ�����
    /// </summary>
    public void AddHandCard(int slotIndex)
    {
        _cardManager.DrawTopCard(_entriesDeckRed, _entriesHandRed, CardProperty.Hand, Position.Hand[slotIndex], slotIndex);
    }
    */

    /*
    /// <summary>
    /// None��1���ǉ�����
    /// </summary>
    /// <param name="slotIndex"></param>
    public void AddNoneCard(GameObject gameObject)
    {
        CardEntry? entry = _entriesFieldRed.FirstOrDefault(entry => entry.View != null && entry.View.gameObject == gameObject);

        _cardManager.UpdateEntry(entry, Position.None, false, CardProperty.None, 0, 0);
        gameObject.SetActive(false);
        
        _entriesFieldRed.Remove(entry);
        _entriesNoneRed.Add(entry);

        JudgeGameFinish();
    }
    */

    /// <summary>
    /// 
    /// </summary>
    private void JudgeGameFinish()
    {
        if (_entriesDeckRed.Count == 0 && _entriesHandRed.Count == 0)
        {
            _uiManager.DisplayPanel(true);
            //SceneManager.LoadScene("ClearScene"); // �V�[�����őJ��
        }
        else
        {
            bool judgeDeck = SpeedRule.JudgeSequential(_entriesFieldRed, _entriesDeckRed);
            bool judgeHand = SpeedRule.JudgeSequential(_entriesFieldRed, _entriesHandRed);

            if (!judgeDeck && !judgeHand)
            {
                // ����ȏ�͏d�˂��Ȃ�����GameOver
                _uiManager.DisplayPanel(false);

                //SceneManager.LoadScene("ClearScene"); // �V�[�����őJ��
            }
            else if (!judgeHand)
            {
                _uiManager.DisplayRedistributionButton(true);
            }
        }
    }

    /// <summary>
    /// ��D����ւ��Ǘ�
    /// </summary>
    private void MargeHandCardAndDraw()
    {
        while (!SpeedRule.JudgeSequential(_entriesFieldRed, _entriesHandRed))
        {
            // �R�D�Ǝ�D��������
            _cardManager.MergeCardEntries(_entriesHandRed, _entriesDeckRed);

            // �V���b�t����R�D��\��
            _cardManager.Shuffle(_entriesDeckRed);
            for (int i = 0; i < _entriesDeckRed.Count; i++)
            {
                Vector3 position = Position.Deck + Position.DeckOffset * i;
                _cardManager.UpdateEntry(_entriesDeckRed[i], position, false, CardProperty.Deck, i, 0);
            }

            // ��D������
            for (int slotIndex = 0; slotIndex < Position.Hand.Count; slotIndex++)
            {
                _cardManager.DrawTopCard(_entriesDeckRed, _entriesHandRed, CardProperty.Hand, Position.Hand[slotIndex], slotIndex);
            }
        }

        /*
        do
        {
            // �R�D�Ǝ�D��������
            _cardManager.MergeCardEntries(_entriesHandRed, _entriesDeckRed);

            // �V���b�t����R�D��\��
            _cardManager.Shuffle(_entriesDeckRed);
            for (int i = 0; i < _entriesDeckRed.Count; i++)
            {
                Vector3 position = Position.Deck + Position.DeckOffset * i;
                _cardManager.UpdateEntry(_entriesDeckRed[i], position, false, CardProperty.Deck, i, 0);
            }

            // ��D������
            for (int slotIndex = 0; slotIndex < Position.Hand.Count; slotIndex++)
            {
                _cardManager.DrawTopCard(_entriesDeckRed, _entriesHandRed, CardProperty.Hand, Position.Hand[slotIndex], slotIndex);
            }
        }
        while (!SpeedRule.JudgeSequential(_entriesFieldRed, _entriesHandRed));
        */
    }
    
}
