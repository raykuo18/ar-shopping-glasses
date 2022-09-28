using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ganzin.EyeTracker.Unity
{
    /// <summary>
    /// Need to set origin.
    /// </summary>
    [RequireComponent(typeof(LineRenderer))]
    public class GanzinGazeRayController : MonoBehaviour
    {
        public GazeIndex IndexOfGaze;
        public GanzinGazeOriginSetting OriginSetting;
        private GanzinEyetrackerManager EyeTrackerManager;
        private LineRenderer GazeRayRenderer;
        private const int LengthOfRay = 100;
        // Start is called before the first frame update
        void Start()
        {
            GazeRayRenderer = gameObject.GetComponent<LineRenderer>();
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

            bool valid = EyeTrackerManager.GetGazeData(IndexOfGaze, out Vector3 localOrigin, out Vector3 localDirection, out _);

            Vector3 globalOrigin = Vector3.zero;
            Vector3 globalDirection = Vector3.forward;

            if (OriginSetting.IsFixed)
            {
                if (!OriginSetting.AlreadyFoundLeftRight)
                {
                    Debug.LogError("[AP ][Unity] " + "There is no position of left eye and right eye.");
                }

                if (IndexOfGaze == GazeIndex.LEFT)
                {
                    globalOrigin = OriginSetting.LeftEyeOrigin.transform.position;
                    globalDirection = EyeTrackerManager.gameObject.transform.TransformDirection(localDirection);
                }
                else if (IndexOfGaze == GazeIndex.RIGHT)
                {
                    globalOrigin = OriginSetting.RightEyeOrigin.transform.position;
                    globalDirection = EyeTrackerManager.gameObject.transform.TransformDirection(localDirection);
                }
                else if (IndexOfGaze == GazeIndex.COMBINE)
                {
                    globalOrigin = new Vector3((OriginSetting.LeftEyeOrigin.transform.position.x + OriginSetting.RightEyeOrigin.transform.position.x) / 2.0f,
                        (OriginSetting.LeftEyeOrigin.transform.position.y + OriginSetting.RightEyeOrigin.transform.position.y) / 2.0f,
                        (OriginSetting.LeftEyeOrigin.transform.position.z + OriginSetting.RightEyeOrigin.transform.position.z) / 2.0f);
                    globalDirection = EyeTrackerManager.gameObject.transform.TransformDirection(localDirection);
                }
            }
            else
            {
                globalOrigin = EyeTrackerManager.gameObject.transform.TransformPoint(localOrigin);
                globalDirection = EyeTrackerManager.gameObject.transform.TransformDirection(localDirection);
            }
            GazeRayRenderer.SetPosition(0, globalOrigin);
            GazeRayRenderer.SetPosition(1, globalOrigin + globalDirection * LengthOfRay);

        }
    }
}