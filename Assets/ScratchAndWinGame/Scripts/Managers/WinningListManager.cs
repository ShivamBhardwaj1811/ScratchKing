using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

public class WinningListManager : Singleton<WinningListManager>
{
    #region Private Members

    /// <summary>
    /// The list of enter animations for the page
    /// </summary>
    [SerializeField]
    private List<AnimClipPlayer> AnimIn = new List<AnimClipPlayer>();

    /// <summary>
    /// The list of exit animations for the page
    /// </summary>
    [SerializeField]
    private List<AnimClipPlayer> AnimOut = new List<AnimClipPlayer>();

    [SerializeField]
    private float TimeBetweenAnimations = 0.5f;

    /// <summary>
    /// The actual item to display in the scroll views
    /// </summary>
    [SerializeField]
    private GameObject Item;

    #endregion

    #region Public Members

    /// <summary>
    /// The lists which will contain data about the items
    /// </summary>
    public List<GameObject> ContentLists = new List<GameObject>();

    /// <summary>
    /// The lists which will contain data about the items
    /// </summary>
    public List<GameObject> Content = new List<GameObject>();

    /// <summary>
    /// The gold icon to display
    /// </summary>
    public Sprite GoldIcon;

    /// <summary>
    /// The money icon to display on the list
    /// </summary>
    public Sprite MoneyIcon;

    /// <summary>
    /// The panel to display thw winning list
    /// </summary>
    [SerializeField]
    public GameObject WinningPanel;

    #endregion

    #region Private Methods

    /// <summary>
    /// Awake is fired before any start method is fired
    /// </summary>
    private void Awake()
    {
        Initialize(this);
    }

    /// <summary>
    /// Temporarily populates the content
    /// </summary>
    private void CreateItems(int ContentIndex, List<DisplayItemModel> itemsData)
    {
        GameObject temp;
        RectTransform rectTransform;
        RectTransform rect = Content[ContentIndex].GetComponent<RectTransform>();
        Vector2 contentPosition = rect.anchoredPosition;
        float height = Item.GetComponent<RectTransform>().rect.height;
        float contentHeight = itemsData.Count * height + height * itemsData.Count / 6;
        rect.sizeDelta = new Vector2(rect.rect.width, contentHeight);
        Vector2 position = new Vector2(0, contentHeight / 2 - height / 2 - 20);
        for (int i = 0; i < itemsData.Count; i++)
        {
            temp = Instantiate(Item, Content[ContentIndex].transform);
            rectTransform = temp.GetComponent<RectTransform>();
            if(i==0)
            {
                Image[] images = temp.GetComponentsInChildren<Image>(true);
                foreach(Image image in images)
                {
                    if (image.tag == "UserImage")
                        image.gameObject.SetActive(false);
                    else if (image.tag == "KingImage")
                        image.gameObject.SetActive(true);
                }
            }
            rectTransform.anchoredPosition = position;
            setData(itemsData[i], temp);
	        position.y -= height + 50;
        }
        contentPosition.y = 0;
    }

    /// <summary>
    /// Populates the data in the fields
    /// </summary>
    /// <param name="itemModel"></param>
    /// <param name="displayItem"></param>
    private void setData(DisplayItemModel itemModel, GameObject displayItem)
    {
        List<Text> textBoxes = displayItem.GetComponentsInChildren<Text>().ToList();
        textBoxes[0].text = itemModel.Username;
        textBoxes[1].text = itemModel.Country == null ? "Not Known" : itemModel.Country;
        textBoxes[2].text = $"{itemModel.Earning}";
        List<Image> images = displayItem.GetComponentsInChildren<Image>().ToList();
        if (itemModel.AreaType == AreaType.Money)
            images[images.Count - 1].sprite = MoneyIcon;
        else if (itemModel.AreaType == AreaType.Gold)
            images[images.Count - 1].sprite = GoldIcon;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Starts the loading sequence for the winning panel
    /// </summary>
    /// <returns></returns>
    public IEnumerator WinningPanelLoadSequence()
    {
        LoadScreenManager.instance.DisplayLoadingScreen();
        WinningListButtonManager.instance.ActivateDeactivateSelectionLines(0);
        WinningPanel.SetActive(true);
        yield return Utility.runCoroutinesAllAtOnceWaitToFinish(this, AnimIn.convertTo<AnimClipPlayer, IEnumerator>(), TimeBetweenAnimations);
        yield return WebRequestHandler.PostRequest(ApiPathManager.BestPlayersUrl, new ApiRequest<AreaType>() { Content = AreaType.Gold }, SaveLoadManager.instance.getToken());
        APIResponse<List<DisplayItemModel>> apiResponse = WebRequestHandler.Response<APIResponse<List<DisplayItemModel>>>();
        if (apiResponse != null)
        {
            if (apiResponse.isOk)
            {
                CreateItems(0, apiResponse.Response);
            }
        }
        yield return WebRequestHandler.PostRequest(ApiPathManager.BestPlayersUrl, new ApiRequest<AreaType>() { Content = AreaType.Money }, SaveLoadManager.instance.getToken());
        apiResponse = WebRequestHandler.Response<APIResponse<List<DisplayItemModel>>>();
        if (apiResponse != null)
        {
            if (apiResponse.isOk)
                CreateItems(1, apiResponse.Response);
        }

        //List<DisplayItemModel> items = new List<DisplayItemModel>();

        //for (int i = 0; i < 15; i++) 
        //{
        //    items.Add(new DisplayItemModel()
        //    {
        //        Username = "Asim",
        //        Earning = 2,
        //        AreaType = AreaType.Gold,
        //        Country = "Pakistan"
        //    });
        //}

        //CreateItems(0, items);
        //items.Clear();
        //for (int i = 0; i < 8; i++)
        //{
        //    items.Add(new DisplayItemModel()
        //    {
        //        Username = "Asim",
        //        Earning = 2,
        //        AreaType = AreaType.Money,
        //        Country = "Pakistan"
        //    });
        //}

        //CreateItems(1, items);

        LoadScreenManager.instance.StopLoadingScreen();
        yield return null;
    }

    public IEnumerator WinningPanelUnloadSequence()
    {
        yield return Utility.runCoroutinesAllAtOnceWaitToFinish(this, AnimOut.convertTo<AnimClipPlayer, IEnumerator>(), TimeBetweenAnimations);
        WinningPanel.SetActive(false);
    }

    #endregion

}
