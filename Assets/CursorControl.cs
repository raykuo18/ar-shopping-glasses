using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorControl : MonoBehaviour
{

    public Texture2D cursor; 
    void Start(){

    }
    // Update is called once per frame
    private void Awake(){
        ChangeCursor(cursor);
        Cursor.lockState = CursorLockMode.Confined;
    }
    private void ChangeCursor(Texture2D  cursorType){
        cursorType = Resize(cursorType, 50, 50);
        Vector2 cursorHotspot = new Vector2 (cursorType.width / 2, cursorType.height / 2);
        Cursor.SetCursor(cursorType, cursorHotspot, CursorMode.Auto);
    }
    Texture2D Resize(Texture2D texture2D,int targetX,int targetY)
    {
        RenderTexture rt=new RenderTexture(targetX, targetY,24);
        RenderTexture.active = rt;
        Graphics.Blit(texture2D,rt);
        Texture2D result=new Texture2D(targetX,targetY);
        result.ReadPixels(new Rect(0,0,targetX,targetY),0,0);
        result.Apply();
        return result;
    }
}
