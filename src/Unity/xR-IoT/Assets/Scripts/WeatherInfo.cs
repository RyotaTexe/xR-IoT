using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;
using LitJson;


public class WeatherInfo : MonoBehaviour
{
    #region Unity Inspector
    [SerializeField]
    private MeshRenderer weatherIcon;
    #endregion

    private void Start()
    {
        
    }

    public void GetWeatherInfoButton()
    {
        StartCoroutine(GetWeatherInfo());
    }

    private IEnumerator GetWeatherInfo()
    {
        UnityWebRequest request 
            = UnityWebRequest
            .Get("https://weather.tsukumijima.net/api/forecast?city=130010");

        yield return request.SendWebRequest();

        if(request.isHttpError || request.isNetworkError)
        {
            Debug.Log("Error : Failed to get weather infomation");
        }
        else
        {
            Debug.Log("Success : Succeeded to get weather infomation");

            Debug.Log(request.downloadHandler.text);

            JsonData jsonData = JsonMapper.ToObject(request.downloadHandler.text);

            //weatherIcon.material.mainTexture = 


            Debug.Log(jsonData["title"]);
            Debug.Log(jsonData["forecasts"][0]["image"]["url"]);

            var iconUrl = jsonData["forecasts"][0]["image"]["url"].ToString();
            StartCoroutine(GetWeatherIcon(iconUrl));

        }
    }

    private IEnumerator GetWeatherIcon(string url)
    {
        UnityWebRequest request
            = UnityWebRequestTexture.GetTexture(url);

        yield return request.SendWebRequest();

        if(request.isHttpError || request.isNetworkError)
        {
            Debug.Log(request.error);
        }
        else
        {
            weatherIcon.material.mainTexture
                = ((DownloadHandlerTexture)request.downloadHandler).texture;
        }
    }
}
