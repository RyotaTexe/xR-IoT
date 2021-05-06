using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;


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

        }
    }
}
