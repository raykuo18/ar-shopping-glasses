using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ganzin.EyeTracker.Unity
{
    /// <summary>
    /// Attach this component at any GameObject you want to measure error.
    /// Must has a GanzinGazeHitDetect.
    /// </summary>
    [RequireComponent(typeof(GanzinGazeHitDetect))]
    public class GanzinErrorMeasure : MonoBehaviour
    {
        private GanzinGazeHitDetect GazeHitDetector;
        public Text ErrorText = null;
        public int ErrorSampleTimes = 150;
        private int ErrorSampleCount = 0;
        private float AccumulatedError = 0.0f;
        private float UpdatedError = 0.0f;
        // Start is called before the first frame update
        void Start()
        {
            GazeHitDetector = GetComponent<GanzinGazeHitDetect>();
            if (GazeHitDetector == null)
            {
                Debug.LogError("[AP ][Unity] " + "There is no Ganzin Gaze Hit Detect.");
                return;
            }

            if (ErrorText != null)
                ErrorText.gameObject.SetActive(false);            
        }
        void OnDisable()
        {
            if (ErrorText != null)
                ErrorText.gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            // Check Hit
            if (GazeHitDetector.IsHit && GazeHitDetector.HitInfo.collider != null)
            {
                Vector3 globalGroundTruthDirection = GazeHitDetector.HitInfo.collider.gameObject.transform.position - GazeHitDetector.GlobalGazeRayOrigin;
                float CurrentError = Vector3.Angle(globalGroundTruthDirection, GazeHitDetector.GlobalGazeRayDirection);

                AccumulatedError += CurrentError;
                ErrorSampleCount++;

                if (ErrorSampleCount >= ErrorSampleTimes)
                {
                    UpdatedError = AccumulatedError / ErrorSampleCount;
                    AccumulatedError = 0.0f;
                    ErrorSampleCount = 0;

                    ErrorText.gameObject.SetActive(true);
                    ErrorText.text = UpdatedError.ToString("F3") + " Deg.";
                }
            }
        }
    }
}