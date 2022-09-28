#define Collider_Raycast
//#define Physics_Raycast

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ganzin.EyeTracker.Unity
{
    /// <summary>
    /// Must has a 3D collider to hit.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class GanzinGazeHitDetect : MonoBehaviour
    {
        private GanzinEyetrackerManager EyeTrackerManager;
        [ReadOnly]
        public bool IsHit = false;
        private Collider ThisCollider3D = null;
        [HideInInspector]
        public RaycastHit HitInfo;
        [HideInInspector]
        public Vector3 GlobalGazeRayOrigin;
        [HideInInspector]
        public Vector3 GlobalGazeRayDirection;
        // Start is called before the first frame update
        void Start()
        {
            EyeTrackerManager = FindObjectOfType<GanzinEyetrackerManager>();
            if (EyeTrackerManager == null)
                Debug.LogError("[AP ][Unity] " + "There is no Ganzin Eye Tracker Manager.");

            ThisCollider3D = GetComponent<Collider>();
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

            // Check Hit
            if (ThisCollider3D != null)
            {
                bool valid = EyeTrackerManager.GetGazeData(GazeIndex.COMBINE, out Vector3 combinedOrigin, out Vector3 combinedDirection, out _);
                GlobalGazeRayOrigin = EyeTrackerManager.gameObject.transform.TransformPoint(combinedOrigin);
                GlobalGazeRayDirection = EyeTrackerManager.gameObject.transform.TransformDirection(combinedDirection);
                if (GlobalGazeRayOrigin != Vector3.zero && GlobalGazeRayDirection != Vector3.zero)
                {

                    Ray combined_gaze_ray = new Ray(GlobalGazeRayOrigin, GlobalGazeRayDirection);
#if Collider_Raycast
                    IsHit = ThisCollider3D.Raycast(combined_gaze_ray, out HitInfo, Mathf.Infinity);
                    if (IsHit)
                    {
                        Debug.DrawRay(GlobalGazeRayOrigin, GlobalGazeRayDirection * HitInfo.distance, Color.red);
                    }
#endif
#if Physics_Raycast
                    IsHit = false;
                    if (Physics.Raycast(combined_gaze_ray, out RaycastHit hitInfo, Mathf.Infinity))
                    {
                        if (GameObject.ReferenceEquals(hitInfo.collider.gameObject, gameObject))
                        {
                            IsHit = true;
                            Debug.DrawRay(GlobalGazeRayOrigin, GlobalGazeRayDirection * HitInfo.distance, Color.red);
                        }
                    }
#endif
                }
            }
        }
    }
}