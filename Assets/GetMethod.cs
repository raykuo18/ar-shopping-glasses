using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
 
public class GetMethod : MonoBehaviour
{
    GameObject middle;
    TMP_InputField output;
    void Start()
    {
        Debug.Log(this.name);
        middle = GameObject.Find("OutputArea");//.GetComponent<InputField>();
        output = middle.GetComponent<TMP_InputField>();
        GameObject.Find("GetButton").GetComponent<Button>().onClick.AddListener(GetData);
    }
 
    void GetData() => StartCoroutine(GetData_Coroutine());
 
    IEnumerator GetData_Coroutine()
    {
        // Debug.Log("asdasdsadasd")
        output.text = "Loading...";
        string uri = "https://my-json-server.typicode.com/typicode/demo/posts";
        using(UnityWebRequest request = UnityWebRequest.Get(uri))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
                output.text = request.error;
            else
                output.text = request.downloadHandler.text;
        }
    }
}