using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ganzin.EyeTracker.Unity
{
    /// <summary>
    /// Need to set origin.
    /// </summary>
    public class Ganzin3DGazePointPosCtrl : MonoBehaviour
    {
        public GanzinGazeOriginSetting OriginSetting;
        private GanzinEyetrackerManager EyeTrackerManager;
        // Start is called before the first frame update
        void Start()
        {
            EyeTrackerManager = FindObjectOfType<GanzinEyetrackerManager>();
            if (EyeTrackerManager == null)
                Debug.LogError("[AP ][Unity] " + "There is no Ganzin Eye Tracker Manager.");
        }

        // Update is called once per frame
        void Update()
        {
            if (EyeTrackerManager == null)
            {
                Debug.LogError("[AP ][Unity] " + "There is no Ganzin Eye Tracker Manager.");
                EyeTrackerManager = FindObjectOfType<GanzinEyetrackerManager>();
                return;
            }
            if (EyeTrackerManager.Status != GanzinEyetrackerManager.EyetrackerStatus.ACTIVE)
            {
                Debug.LogWarning("[AP ][Unity] " + "Ganzin Eye Tracker does not work.");
                return;
            }

            // Find left eye and right eye
            OriginSetting.FindGazeOrigin();

            bool valid = EyeTrackerManager.GetGazeData(GazeIndex.COMBINE, out Vector3 combinedOrigin, out Vector3 combinedDirection, out Vector3 combinedPosition);

            if (OriginSetting.IsFixed)
            {
                if (!OriginSetting.AlreadyFoundLeftRight)
                {
                    Debug.LogError("[AP ][Unity] " + "There is no position of left eye and right eye.");
                }
                Vector3 globalCombinedOrigin = new Vector3((OriginSetting.LeftEyeOrigin.transform.position.x + OriginSetting.RightEyeOrigin.transform.position.x) / 2.0f,
                (OriginSetting.LeftEyeOrigin.transform.position.y + OriginSetting.RightEyeOrigin.transform.position.y) / 2.0f,
                (OriginSetting.LeftEyeOrigin.transform.position.z + OriginSetting.RightEyeOrigin.transform.position.z) / 2.0f);

                gameObject.transform.position = globalCombinedOrigin + EyeTrackerManager.gameObject.transform.TransformDirection(combinedPosition - combinedOrigin);
            }
            else
            {
                gameObject.transform.position = EyeTrackerManager.gameObject.transform.TransformPoint(combinedPosition);
            }
        }
    }
}