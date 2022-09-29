using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class data {
    public string id;
    public string title;
}
 
public class GetMethod : MonoBehaviour
{
    TMP_InputField outputField;
    void Start()
    {
        outputField = GameObject.Find("OutputArea").GetComponent<TMP_InputField>();
        GameObject.Find("Button_yes").GetComponent<Button>().onClick.AddListener(GetData);
    }
 
    void GetData() => StartCoroutine(GetData_Coroutine());
 
    IEnumerator GetData_Coroutine()
    {
        outputField.text = "Loading...";
        string uri = "https://my-json-server.typicode.com/typicode/demo/posts";
        using(UnityWebRequest request = UnityWebRequest.Get(uri))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError) {
                outputField.text = request.error;
                Debug.Log("error");
                Debug.Log(outputField.text);
            }
            else {
                string infoText = request.downloadHandler.text;
                outputField.text = infoText;
                // string infoText = request.result.text;
                // string parsed = JsonUtility.FromJson<data>(infoText);
                // output.text = parsed.text;
                Debug.Log("success");
                Debug.Log(infoText);
            }
        }
    }
}