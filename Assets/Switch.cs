using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Switch : MonoBehaviour
{
   
    public GameObject[] background;
    int index;

    void Start()
    {
        index = 0;
        background[index].gameObject.SetActive(true);
    }

    public void Next()
     {
         index += 1;
         if (index == background.Length){
            index = 0;
         }
         for(int i = 0 ; i < background.Length; i++)
         {
            background[i].gameObject.SetActive(false);
         }
         background[index].gameObject.SetActive(true);
         Debug.Log(index);
     }
    
     public void Previous()
     {
         index -= 1;
         if (index == -1){
            index = background.Length -1;
         }
         for(int i = 0 ; i < background.Length; i++)
         {
            background[i].gameObject.SetActive(false);
         }
         background[index].gameObject.SetActive(true);
         Debug.Log(index);
     }

   
}