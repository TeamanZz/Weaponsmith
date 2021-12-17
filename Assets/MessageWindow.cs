using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageWindow : MonoBehaviour
{
    //public MessageDATA messageDATA;
    //public MessageInfo messageInfo;
    //public GameObject messagePanel;
    //public bool isActive = false;

    //[Header("View settings")]
    //public GridLayoutGroup layoutGroup;
    //public Scrollbar scrollbar;
    //public int newsNumber = 0;
    //public RectTransform content;
    //public GameObject newsPrefab;
    //public bool instatiate = false;
    //public float reloadTime = 0.2f;

    //[Header("Notification settings")]
    //public List<NewsPanel> notificationPanel = new List<NewsPanel>();
    //public GameObject notificationObject;
    //public bool notification = false;
    //public Text notificationCount;

    //public void Awake()
    //{
    //    messagePanel.SetActive(false);
    //    notificationObject.SetActive(false);
    //    instatiate = false;
    //    isActive = false;
    //    messageInfo = messageDATA.messageDat[0];
    //    scrollbar.size = 1;
    //    Invoke("InstatiateNews", 0.10f);
    //    Invoke("InstatiateNews", 2f);
    //    Invoke("InstatiateNews", 3f);

    //}

    //public void Update()
    //{
    //    if(Input.GetKey(KeyCode.Alpha4))
    //    {
    //        InstatiateNews();
    //    }
    //}

    //public void OpenPanel()
    //{
    //    messagePanel.SetActive(true);
    //    isActive = true;
    //    Debug.Log("Open");
    //}

    //public void InstatiateNews()
    //{
    //    if (newsNumber >= messageInfo.messages.Count)// || isActive == false)
    //        return;

    //    if (instatiate == true)
    //        return;

    //    Debug.Log("Instatiate");
    //    if(newsNumber == 0)
    //        content.sizeDelta = new Vector2(content.sizeDelta.x, 800f);
    //    else
    //        content.sizeDelta = new Vector2(content.sizeDelta.x, content.sizeDelta.y + 800f);

    //    GameObject newsPan = Instantiate(newsPrefab, content.transform);
    //    newsPan.transform.parent = content;
    //    NewsPanel news = newsPan.GetComponent<NewsPanel>();
    //    news.Initialization(messageInfo.messages[newsNumber], this);
    //    Debug.Log(messageInfo.messages[newsNumber]);

    //    newsNumber++;
    //    scrollbar.size = 1 / newsNumber;
    //    instatiate = true;
    //    InitializationNotification(news);
    //    Invoke("NewNews", reloadTime);
    //}

    //public void InitializationNotification(NewsPanel newsPanel)
    //{
    //    notificationPanel.Add(newsPanel);
    //    if (notificationPanel.Count > 0)
    //        notificationObject.SetActive(true);

    //    notificationCount.text = notificationPanel.Count.ToString();
    //}

    //public void DeinitializationNotification(NewsPanel newsPanel)
    //{
    //    if(notificationPanel.Count > 0)
    //        notificationPanel.Remove(newsPanel);
    //    if (notificationPanel.Count <= 0)
    //        notificationObject.SetActive(false);

    //    notificationCount.text = notificationPanel.Count.ToString();
    //}

    //public void NewNews()
    //{
    //    if (instatiate == true)
    //        instatiate = false;
    //}

    //public void ClosedPanel()
    //{
    //    messagePanel.SetActive(false);
    //    isActive = false;
    //}
}
