using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ganzin.EyeTracker.Unity
{
    /// <summary>
    /// The 2D gaze's origin is the middle of two movable eyes.
    /// If you lock the postion of eyes, the gaze will not match 3D Gaze Ray Line.
    /// </summary>
    [DisallowMultipleComponent]
    public class Ganzin2DGazePointPosCtrl : MonoBehaviour
    {
        private GanzinEyetrackerManager EyeTrackerManager;
        // Start is called before the first frame update
        void Start()
        {
            EyeTrackerManager = FindObjectOfType<GanzinEyetrackerManager>();
            if (EyeTrackerManager == null)
            {
                Debug.LogError("[AP ][Unity] " + "There is no Ganzin Eye Tracker Manager.");
                return;
            }
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
            bool valid = EyeTrackerManager.GetGazeData(GazeIndex.COMBINE, out Vector3 combinedOrigin, out Vector3 combinedDirection, out Vector3 combinedPosition);

            Vector3 globalGazeRayOrigin = EyeTrackerManager.gameObject.transform.TransformPoint(combinedOrigin);
            Vector3 globalGazeRayDirection = EyeTrackerManager.gameObject.transform.TransformDirection(combinedDirection);
            Ray combined_gaze_ray = new Ray(globalGazeRayOrigin, globalGazeRayDirection);
            float enter = 0.0f;
            Plane canvasPlane = new Plane(gameObject.transform.forward, gameObject.transform.position);
            //Debug.DrawRay(combined_gaze_ray.origin, combined_gaze_ray.direction, Color.red);
            if (canvasPlane.Raycast(combined_gaze_ray, out enter))
            {
                Vector3 hitPoint = combined_gaze_ray.GetPoint(enter);
                gameObject.transform.position = hitPoint;
            }
        }
    }
}