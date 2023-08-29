using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinningListButtonManager : Singleton<WinningListButtonManager>
{
    #region Private Members

    /// <summary>
    /// The lines to display under selected itom
    /// </summary>
    [SerializeField]
    private List<GameObject> selectionLines = new List<GameObject>();

    /// <summary>
    /// The currently selected object
    /// </summary>
    [SerializeField]
    private int SelectedItem = 0;

    #endregion

    #region Private Methods

    private void Awake()
    {
        Initialize(this);
    }

    /// <summary>
    /// Returns to the main page
    /// </summary>
    /// <returns></returns>
    private IEnumerator WinningPageReturnSequence()
    {
        yield return WinningListManager.instance.WinningPanelUnloadSequence();
        yield return StartSequenceManager.instance.PlayAnimation();
        ComponentManager.instance.Gameplay.SetActive(true);
        StartCoroutine(GameManager.instance.UpdateTicketSettings());
    }

    public void ActivateDeactivateSelectionLines(int index)
    {
        WinningListManager.instance.ContentLists[SelectedItem].SetActive(false);
        selectionLines[SelectedItem].SetActive(false);
        SelectedItem = index;
        selectionLines[SelectedItem].SetActive(true);
        WinningListManager.instance.ContentLists[SelectedItem].SetActive(true);
    }

    #endregion

    #region Public Methods

    public void BestGoldPlayer()
    {
        if (SelectedItem == 0)
            return;
        ActivateDeactivateSelectionLines(0);
    }

    public void BestMoneyPlayer()
    {
        if (SelectedItem == 1)
            return;
        ActivateDeactivateSelectionLines(1);
    }

    public void ReturnButton()
    {
        StartCoroutine(WinningPageReturnSequence());
    }

    #endregion

}
