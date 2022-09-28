using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ganzin.EyeTracker.Unity
{
    /// <summary>
    /// Must be under Canvas
    /// </summary>
    [DisallowMultipleComponent]
    public class Ganzin2DPointSizeCtrl : MonoBehaviour
    {
        private GanzinEyetrackerManager EyeTrackerManager;
        public float RadiusDegree = 1.5f;
        // Start is called before the first frame update
        void Start()
        {
            EyeTrackerManager = FindObjectOfType<GanzinEyetrackerManager>();
            if (EyeTrackerManager == null)
            {
                Debug.LogError("[AP ][Unity] " + "There is no Ganzin Eye Tracker Manager.");
                return;
            }
            // Change circle radius to a fixed degree range
            float radiusInPixel = Ganzin2DUtility.GetLocalDistance_ByFOV(EyeTrackerManager.gameObject, gameObject, RadiusDegree);
            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(radiusInPixel * 2.0f, radiusInPixel * 2.0f);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
