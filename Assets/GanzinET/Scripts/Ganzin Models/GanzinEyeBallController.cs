using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ganzin.EyeTracker.Unity
{
    /// <summary>
    /// If you want to lock the eye ball position, you can disable "EyeBallMovable".
    /// </summary>
    public class GanzinEyeBallController : MonoBehaviour
    {
        public bool EyeBallMovable = true;
        public EyeIndex IndexOfEye;
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
            bool valid = EyeTrackerManager.GetGazeData((GazeIndex)IndexOfEye, out Vector3 origin, out Vector3 direction, out _);
            if (valid)
            {
                Vector3 localDirection = gameObject.transform.parent.InverseTransformDirection(EyeTrackerManager.gameObject.transform.TransformDirection(direction));
                gameObject.transform.localRotation = Quaternion.FromToRotation(Vector3.forward, localDirection);
                if (EyeBallMovable)
                    gameObject.transform.localPosition = gameObject.transform.parent.InverseTransformPoint(EyeTrackerManager.gameObject.transform.TransformPoint(origin));
            }
        }        
    }
}
