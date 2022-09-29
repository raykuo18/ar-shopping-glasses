using UnityEngine;
using Ganzin.EyeTracker.Unity;

namespace Ganzin.EyeTracker.Tests
{
    public class GetEyeRay : MonoBehaviour
    {
        public GazeIndex IndexOfGaze;
        private GanzinEyetrackerManager EyeTrackerManager;
        // private LineRenderer GazeRayRenderer;
        private float LengthOfRay;
 
        void Start()
        {
            // GazeRayRenderer = gameObject.GetComponent<LineRenderer>();
            EyeTrackerManager = FindObjectOfType<GanzinEyetrackerManager>();
        }
 
        void Update()
        {
            if (EyeTrackerManager.Status != GanzinEyetrackerManager.EyetrackerStatus.ACTIVE)
                return;
 
            bool valid = EyeTrackerManager.GetGazeData(IndexOfGaze, out Vector3 localOrigin, out Vector3 localDirection, out Vector3 position);
 
            Vector3 globalOrigin = EyeTrackerManager.gameObject.transform.TransformPoint(localOrigin);
            Vector3 globalDirection = EyeTrackerManager.gameObject.transform.TransformDirection(localDirection);
            Vector3 globalPosition = EyeTrackerManager.gameObject.transform.TransformPoint(position);
            
            // Debug.Log("globalOrigin:\n");
            // Debug.Log(globalOrigin);
            // // Debug.Log("ObjectPosition:\n");
            // // Debug.Log(globalOrigin + globalDirection * LengthOfRay);
            // Debug.Log("ObjectPosition:\n");
            // Debug.Log(globalPosition);
            // LengthOfRay = globalPosition.sqrMagnitude;
            // Debug.Log("LengthOfRay\n");
            // Debug.Log(LengthOfRay);
            
            // GazeRayRenderer.SetPosition(0, globalOrigin);
            // GazeRayRenderer.SetPosition(1, globalOrigin + globalDirection * LengthOfRay);
        }
    }
}  
