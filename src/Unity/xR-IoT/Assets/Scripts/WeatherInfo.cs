using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;
using LitJson;
using TMPro;


public class WeatherInfo : MonoBehaviour
{
    #region Unity Inspector
    [SerializeField]
    private MeshRenderer weatherIcon;

    [SerializeField]
    private TextMeshPro cityName;

    [SerializeField]
    private TextMeshPro weatherText;

    [SerializeField]
    private TextMeshPro temperature;

    [SerializeField]
    private TextMeshPro maxTemperature;

    [SerializeField]
    private TextMeshPro minTemperature;
    #endregion

    private void Start()
    {
        StartCoroutine(GetWeatherInfo());
    }

    public void GetWeatherInfoButton()
    {
        StartCoroutine(GetWeatherInfo());
    }

    private IEnumerator GetWeatherInfo()
    {
        UnityWebRequest request 
            = UnityWebRequest
            .Get("http://api.openweathermap.org/data/2.5/weather?q=Tokyo,JP&appid=aba5af78ce90de3f752c6e32a4966bb0&lang=ja&units=metric");

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

            var iconName = jsonData["weather"][0]["icon"].ToString();
            var iconUrl = "http://openweathermap.org/img/w/" + iconName + ".png";
            StartCoroutine(GetWeatherIcon(iconUrl));

            UpdateWeatherInfo(jsonData);
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

    private void UpdateWeatherInfo(JsonData jsonData)
    {
        weatherText.text = jsonData["weather"][0]["description"].ToString();
        temperature.text = jsonData["main"]["temp"].ToString() + "Åé";
        maxTemperature.text = "ç≈çÇ:" + jsonData["main"]["temp_max"].ToString() + "Åé";
        minTemperature.text = "ç≈í·:" + jsonData["main"]["temp_min"].ToString() + "Åé";

        Debug.Log(jsonData["weather"][0]["description"]);
        Debug.Log(jsonData["main"]["temp"]);
        Debug.Log(jsonData["main"]["temp_max"]);
        Debug.Log(jsonData["main"]["temp_min"]);
    }
}