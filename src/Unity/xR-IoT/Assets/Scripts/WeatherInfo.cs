using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;
using LitJson;


public class WeatherInfo : MonoBehaviour
{
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

            Debug.Log(jsonData["title"]);
            Debug.Log(jsonData["description"]["text"]);
            Debug.Log(jsonData["forecasts"][0]["date"]);
            Debug.Log(jsonData["forecasts"][0]["telop"]);
            Debug.Log(jsonData["forecasts"][0]["temperature"]["max"]["celsius"]);
            Debug.Log(jsonData["forecasts"][0]["chanceOfRain"]["18-24"]);
            Debug.Log(jsonData["forecasts"][1]["date"]);
            Debug.Log(jsonData["forecasts"][1]["telop"]);
            Debug.Log(jsonData["forecasts"][1]["temperature"]["max"]["celsius"]);
            Debug.Log(jsonData["forecasts"][1]["chanceOfRain"]["18-24"]);
            Debug.Log(jsonData["location"]["city"]);

        }
    }
}
