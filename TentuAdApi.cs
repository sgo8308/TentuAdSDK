using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Networking;
using UnityEngine.UI;
using WebP;

namespace Tentuplay.FashionAd
{
    public class TentuAdApi : MonoBehaviour
    {
        //ShowAd 메소드의 IEnumerator, 광고판에 광고이미지를 세팅해 줌
        public IEnumerator sendShowAd(string url,string adType)
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                //큰 광고판 이미지는 webp확장자로 오기 때문에 작은 광고판이랑 나누어서 처리
                if (adType == "BigAd")
                {
                    var bytes = www.downloadHandler.data;
                    Texture2D texture = Texture2DExt.CreateTexture2DFromWebP(bytes, lMipmaps: true, lLinear: true, lError: out Error lError);
                    if (lError == Error.Success)
                    {
                        this.GetComponent<RawImage>().texture = texture;
                        this.transform.root.Find("GoToBuyButton").gameObject.SetActive(true);
                    }
                    else
                    {
                        Debug.LogError("Webp Load Error : " + lError.ToString());
                    }
                }
                else{

                    this.GetComponent<RawImage>().texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                    this.transform.root.Find("GoToBuyButton").gameObject.SetActive(true);
                }
            }
        }

        //광고데이터를 불러와서 세팅함
        public IEnumerator sendSetImageAdData(string url, string userId, System.Action<TentuAdApi.NativeAdData> var)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.isNetworkError || webRequest.isHttpError)
                {
                    Debug.Log(webRequest.error);
                }
                else
                {
                    Debug.Log("Form upload complete!");
                    string data = webRequest.downloadHandler.text;
                    Debug.Log(data);
                    NativeAdData nativeAdData = JsonConvert.DeserializeObject<NativeAdData>(data);
                    var(nativeAdData);
                }
            }
        }

        //받아온 json 형태 광고정보를 변환할 클래스들
        public class NativeAdData
        {
            public appsInfo[] apps;
        }

        public class appsInfo
        {
            public string pixelImp;
            public string title;
            public string desc;
            public string urlImg;
            public string urlImgWide;
            public string urlApp;
            public string androidPackage;
            public string revenueType;
            public string revenueRate;
            public string campaignGoal;
            public string buttonText;
            public string categories;
            public string idx;
            public string targetedDevices;
            public string targetedOSver;
            public string urlVideo;
            public string urlVideoHigh;
            public string urlVideo30Sec;
            public string urlVideo30SecHigh;
            public string bannerId;
            public string campaignId;
            public string supportedVersion;
            public string storeRating;
            public string storeDownloads;
            public string appSize;
            public string min_ecpm;
            public string market_url;
            public string sid;
            public string lurl;
            public string domain;
        }
    }
}


