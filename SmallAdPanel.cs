using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using UnityEngine.UI;
using Tentuplay.FashionAd;
using System.Threading;

public class SmallAdPanel : MonoBehaviour
{
    static string clientId; // 클라이언트(게임사) 아이디 세팅하기 
    static string userId = "james"; // 유저 아이디 갖고오기
    GameObject goToBuyButton;
    GameObject titleText;
    GameObject descText;

    private void Update()
    {
        goToBuyButton = this.transform.root.Find("GoToBuyButton").gameObject;
        titleText = this.transform.root.Find("Title").gameObject;
        descText = this.transform.root.Find("Desc").gameObject;

        if (goToBuyButton.activeSelf)
        {
            titleText.SetActive(true);
            descText.SetActive(true);
        }
        else
        {
            titleText.SetActive(false);
            descText.SetActive(false);
        }

        if (queue.Count > 0 && !isUserInTrigArea)
        {
            this.GetComponent<RawImage>().texture = null;
            this.transform.root.Find("GoToBuyButton").gameObject.SetActive(false);
            isAdPanelSet = false;
            queue.Dequeue();
        }
    }

    public TentuAdApi.NativeAdData panelAdData;

    public void ShowAd()
    {
        string url = "";
        if (Persona.persona == "communication")
        {
            url = "https://global.appnext.com/offerWallApi.aspx?tid=API&did=03382e13-716f-47c0-8b40-10ca1ad1abe1&id=99dd343b-c93a-418f-b393-19f968a387f3&ip=92.38.148.61&lockcat=Social&uagent=Dalvik%2f2.1.0+(Linux%3b+U%3b+Android+9%3b+Redmi+Note+7+Pro+MIUI%2fV10.3.9.0.PFHINXM)";
        }
        else if (Persona.persona == "fashionItemPrefer")
        {
            url = "https://global.appnext.com/offerWallApi.aspx?tid=API&did=03382e13-716f-47c0-8b40-10ca1ad1abe1&id=99dd343b-c93a-418f-b393-19f968a387f3&ip=92.38.148.61&lockcat=Shopping&uagent=Dalvik%2f2.1.0+(Linux%3b+U%3b+Android+9%3b+Redmi+Note+7+Pro+MIUI%2fV10.3.9.0.PFHINXM)";
        }
        else if(Persona.persona == "tracker")
        {
            url = "https://global.appnext.com/offerWallApi.aspx?tid=API&did=03382e13-716f-47c0-8b40-10ca1ad1abe1&id=99dd343b-c93a-418f-b393-19f968a387f3&ip=92.38.148.61&lockcat=Travel&uagent=Dalvik%2f2.1.0+(Linux%3b+U%3b+Android+9%3b+Redmi+Note+7+Pro+MIUI%2fV10.3.9.0.PFHINXM)";
        }
        else
        {
            url = "https://global.appnext.com/offerWallApi.aspx?tid=API&did=03382e13-716f-47c0-8b40-10ca1ad1abe1&id=99dd343b-c93a-418f-b393-19f968a387f3&ip=92.38.148.61&lockcat=Sports&uagent=Dalvik%2f2.1.0+(Linux%3b+U%3b+Android+9%3b+Redmi+Note+7+Pro+MIUI%2fV10.3.9.0.PFHINXM)";
        }

        StartCoroutine(ShowAdChain(url ,userId, (x) => panelAdData = x)); ;
    }

    /*광고 정보를 받아온 후에 광고판의 이미지를 변경시키기 위해 IEnumerator를 사용*/
    IEnumerator ShowAdChain(string url,string userId, System.Action<TentuAdApi.NativeAdData> var)
    {
        TentuAdApi fApi = gameObject.AddComponent<TentuAdApi>();
        yield return StartCoroutine(fApi.sendSetImageAdData(url ,userId, var));//먼저 광고 정보를 갖고온다
        StartCoroutine(fApi.sendShowAd(panelAdData.apps[0].urlImg, "Small"));//광고 정보 갖고오면 이미지url넣어서 보여준다.
        this.transform.root.Find("Title").GetComponent<Text>().text = panelAdData.apps[0].title;
        this.transform.root.Find("Desc").GetComponent<Text>().text = panelAdData.apps[0].desc;
    }


    /*광고판 광고에서 구매하러 가기 클릭시 연결된 URL로 이동*/
    public void GoPanelAdSite()
    {
        TentuAdApi fApi = gameObject.AddComponent<TentuAdApi>();
        if (panelAdData.apps[0].urlApp != null)
        {
            string url = panelAdData.apps[0].urlApp;
            Application.OpenURL(url);
        }
    }

    private bool isUserInTrigArea; //트리거 지역에 있는지 없는지
    private Queue<string> queue = new Queue<string>(); // 5초 지나고 올 메시지 담을 큐
    private int trigNum; // 몇번째 트리거 안에 들어왔는지
    private bool isAdPanelSet = false; //광고판에 광고가 세팅되어있는지 아닌지

    public void OnAdTriggerEnter()
    {
        trigNum++;

        if (!isAdPanelSet)
        {
            this.ShowAd();
        }

        isUserInTrigArea = true;
        isAdPanelSet = true;
    }

    public void OnAdTriggerExit()
    {
        //5초 뒤에 사라지게 하는 쓰레드
        Thread td = new Thread(new ParameterizedThreadStart(Run));
        td.Start(trigNum);
        isUserInTrigArea = false;
    }

    /*
     * 5초 뒤 광고판 리셋하라는 메세지를 보낼 메소드
     * 다시 트리거 안에 들어왔을 경우 기존에 돌아가던 쓰레드를
     * 무력화 시키기 위해 이 쓰레드가 시작될 때의 trignumber와 현재가
     * 동일한지 비교함.
     * 이렇게 하지 않으면 유저가 트리거 밖을 나갔다가 다시 5초 안에 다시 들어온 후에
     * 바로 다시 나갔을 때는 5초 뒤에 광고가 사라지지 않고 5초가 되기 전에 사라짐.
     * 기존에 돌아가던 쓰레드가 실행이 끝나면서 메세지를 날리기 때문 
     */
    private void Run(object trigNumber)
    {
        Thread.Sleep(1000);
        if ((int)trigNumber == trigNum)
        {
            string message = "resetAdPanel";
            queue.Enqueue(message);
        }
    }
}
