using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;
//Main Game Loop
public class GameManager : Singleton<GameManager>
{

    [SerializeField]
    private GameObject messageBox;

    public TicketHolder ticketHolder;
    private Ticket CurrentTicket;

    private void Awake()
    {
        Initialize(this);
    }

    public void StartGameplay()
    {
        StartCoroutine(UpdateTicketSettings());
    }

    public IEnumerator UpdateTicketSettings()
    {
        if (SaveLoadManager.instance.currentUser.Tickets > 0)
        {
            if (messageBox.activeSelf)
                messageBox.SetActive(false);
            CurrentTicket = ticketHolder.ActivateRandomTicket();
            CurrentTicket.PrepareTicket();
            yield return CurrentTicket.ProcessTicket();
            WinPanelManager.instance.SetWinningUI(new WinSettings(null, AreaType.Ticket), false);
            SaveLoadManager.instance.increaseValues(AreaType.Ticket, -1);
            StartCoroutine(WinPanelManager.instance.PanelFinishAnimation(SaveLoadManager.instance.currentUser.TicketsOld, SaveLoadManager.instance.currentUser.Tickets, ScoreBoardManager.instance.TicketPart, AreaType.Ticket, 1, false, ScoreBoardManager.instance.TicketPart.SpriteEndTransform));
            StartCoroutine(UpdateTicketSettings());
        }
        else
        {
            messageBox.SetActive(true);
            CurrentTicket = ticketHolder.ActivateRandomTicket();
            CurrentTicket.DisplayScoreBoard();
        }
    }
}
