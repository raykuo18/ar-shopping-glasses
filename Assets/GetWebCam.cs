// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using Dynamsoft;
// using Dynamsoft.DBR;

// using OpenCvSharp;
// using OpenCVForUnity;
// using OpenCVForUnity.CoreModule;
// using OpenCVForUnity.UnityUtils;
// using System.Runtime.InteropServices;

// public class GetWebCam : MonoBehaviour
// {
//     WebCamTexture webCamTexture;
//     public RawImage imgDisplayForPhotoSnap;
//     private string errorMsg;

//     BarcodeReader reader;
//     PublicRuntimeSettings runtimeSettings;
//     TextResult[] results;
//     // BarcodeResult[] results_;

//     // Start is called before the first frame update
//     void Start()
//     {   
//         WebCamDevice[] devices = WebCamTexture.devices;
//         webCamTexture = new WebCamTexture(devices[2].name);
//         GetComponent<Renderer>().material.mainTexture = webCamTexture;
//         webCamTexture.Play();

//         BarcodeReader.InitLicense("DLS2eyJoYW5kc2hha2VDb2RlIjoiMjAwMDAxLTE2NDk4Mjk3OTI2MzUiLCJvcmdhbml6YXRpb25JRCI6IjIwMDAwMSIsInNlc3Npb25QYXNzd29yZCI6IndTcGR6Vm05WDJrcEQ5YUoifQ==", out errorMsg);
//         reader = new BarcodeReader();
//         runtimeSettings = reader.GetRuntimeSettings();
//         runtimeSettings.BarcodeFormatIds = (int)EnumBarcodeFormat.BF_ALL;
//         runtimeSettings.BarcodeFormatIds_2 = (int)(EnumBarcodeFormat_2.BF2_POSTALCODE | EnumBarcodeFormat_2.BF2_DOTCODE);
//         runtimeSettings.ExpectedBarcodesCount = 32;
//         reader.UpdateRuntimeSettings(runtimeSettings);

//     }

//     // Update is called once per frame
//     void Update()
//     {
//         if (imgDisplayForPhotoSnap.texture != null)
//         {
            
//             Mat frameMat = new Mat(webCamTexture.height, webCamTexture.width, CvType.CV_8UC4);
//             Color32[] colors = new Color32[webCamTexture.width * webCamTexture.height];
//             Utils.webCamTextureToMat(webCamTexture, frameMat, colors, false);

//             var bytes = new byte[frameMat.Total()*3];
//             Marshal.Copy(frameMat.Data, bytes, 0, bytes.length);

//             try
//             {
//                 // IntPtr data = frameMat.Data;
//                 // int width = frameMat.Width;
//                 // int height = frameMat.Height;
//                 // int elemSize = frameMat.ElemSize();
//                 // int buffer_size = width * height * elemSize;
//                 // byte[] buffer = new byte[buffer_size];
//                 // Marshal.Copy(data, buffer, 0, buffer_size);
//                 // results_ = reader.DecodeBuffer(buffer, width, height, width * elemSize, ImagePixelFormat.IPF_RGB_888);
//                 // if (results_ != null)
//                 // {
//                 //     Console.WriteLine("Total result count: " + results_.Length);
//                 //     foreach (BarcodeResult result in results_)
//                 //     {
//                 //         Console.WriteLine(result.BarcodeText);
//                 //     }
//                 // }

//                 results = reader.DecodeFile(@"[INSTALLATION FOLDER]/Images/AllSupportedBarcodeTypes.png", "");
//                 results = reader.DecodeBuffer(bytes, webCamTexture.width, webCamTexture.height, 1, EnumImagePixelFormat.IPF_ARGB_8888, "");
//                 Debug.Log("Total barcodes found: " + results.Length);
//                 for (int iIndex = 0; iIndex < results.Length; ++iIndex)
//                 {
//                     Debug.Log("Barcode " + (iIndex + 1));
//                     if (results[iIndex].BarcodeFormat != 0)
//                     {
//                         Debug.Log("    Barcode Format: " + results[iIndex].BarcodeFormatString);
//                     }
//                     else
//                     {
//                         Debug.Log("    Barcode Format: " + results[iIndex].BarcodeFormatString_2);
//                     }
//                     Debug.Log("    Barcode Text: " + results[iIndex].BarcodeText);
//                 }
            
//             }
//             catch (BarcodeReaderException exp)
//             {
//                 Debug.Log(exp.Message);
//             }
//         }
        
//     }
// }
