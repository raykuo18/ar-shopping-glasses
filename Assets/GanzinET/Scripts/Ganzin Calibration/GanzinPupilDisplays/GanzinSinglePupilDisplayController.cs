using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ganzin.EyeTracker.Unity
{
    /// <summary>
    /// Must be under Canvas
    /// </summary>
    [DisallowMultipleComponent]
    public class GanzinSinglePupilDisplayController : MonoBehaviour
    {
        public EyeIndex IndexOfEye;
        private GanzinEyetrackerManager EyeTrackerManager;
        private bool PupilValid = true;
        public Image DisplayBoarder;
        private RectTransform DisplayRect;
        [ReadOnly]
        public float DisplaySideLength = -1.0f;
        public RectTransform PupilPoint;

        [Header("Config")]
        [Tooltip("The color of display border to present valid.")]
        public Color PupilValidColor = new Color32(255, 255, 225, 255);
        [Tooltip("The color of display border to present invalid.")]
        public Color PupilInvalidColor = new Color32(255, 0, 0, 255);
        [Tooltip("The pupil point's size (ratio to pupil display box).")]
        public float PupilSizeRatio = 0.1f;

        // Start is called before the first frame update
        void Start()
        {
            EyeTrackerManager = FindObjectOfType<GanzinEyetrackerManager>();
            if (EyeTrackerManager == null)
            {
                Debug.LogError("[AP ][Unity] " + "There is no Ganzin Eye Tracker Manager.");
                return;
            }
            DisplayRect = DisplayBoarder.gameObject.GetComponent<RectTransform>();
        }

        // Update is called once per frame
        void Update()
        {
            if (DisplaySideLength <= 0)
            {
                //DisplaySideLength = DisplayRect.sizeDelta.x;
                DisplaySideLength = DisplayRect.rect.width;
                PupilPoint.sizeDelta = new Vector2(DisplaySideLength * PupilSizeRatio, DisplaySideLength * PupilSizeRatio);
            }

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

            PupilValid = EyeTrackerManager.GetPupilData(IndexOfEye, out Vector2 pupilPositionInPropOfTrackingArea, out _, out _);

            if (PupilValid) DisplayBoarder.color = PupilValidColor;
            else DisplayBoarder.color = PupilInvalidColor;

            PupilPoint.anchoredPosition = ConvertPropToLocal2D(pupilPositionInPropOfTrackingArea, DisplaySideLength, DisplaySideLength);
        }

        private Vector2 ConvertPropToLocal2D(Vector2 input, float width, float height)
        {
            float output_x = ((-input.x) + 1.0f) / 2.0f * width;
            float output_y = -((input.y + 1.0f) / 2.0f * height);
            return new Vector2(output_x, output_y);
        }
    }
}