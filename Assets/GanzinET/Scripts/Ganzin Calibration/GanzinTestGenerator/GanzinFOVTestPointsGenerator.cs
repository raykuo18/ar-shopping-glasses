using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ganzin.EyeTracker.Unity
{
    /// <summary>
    /// Must be under Canvas
    /// </summary>
    public class GanzinFOVTestPointsGenerator : MonoBehaviour
    {
        private GanzinEyetrackerManager EyeTrackerManager = null;
        public GameObject Prefab;
        public int Number = 7;
        public bool HasCenterPoint = true;
        public float FOV = 15.0f;
        public float OffsetDegree = 0.0f;
        [ReadOnly]
        public float CentrifugalRadius = 200.0f;
        [ReadOnly]
        public Vector2[] TestPointsPosition;

        // Start is called before the first frame update
        void Start()
        {
            EyeTrackerManager = FindObjectOfType<GanzinEyetrackerManager>();
            if (EyeTrackerManager == null)
            {
                Debug.LogError("[AP ][Unity] " + "There is no Ganzin Eye Tracker Manager.");
                return;
            }
            
            if (Number < 1) return;
            TestPointsPosition = new Vector2[Number];
            CentrifugalRadius = Ganzin2DUtility.GetLocalDistance_ByFOV(EyeTrackerManager.gameObject, gameObject, (FOV / 2.0f));

            // Position
            int CircleArrangeStartIndex = 0;
            int CircleArrangeNumber = Number;
            if (HasCenterPoint)
            {
                TestPointsPosition[0] = new Vector2(0, 0);
                CircleArrangeStartIndex = 1;
                CircleArrangeNumber = Number - 1;
            }
            for (int i = CircleArrangeStartIndex; i < Number; i++)
            {
                float angle = (i - 1) * (Mathf.PI * 2 / CircleArrangeNumber) + (Mathf.Deg2Rad * OffsetDegree);
                TestPointsPosition[i] = new Vector2(Mathf.Cos(angle) * CentrifugalRadius, Mathf.Sin(angle) * CentrifugalRadius);
            }

            // Generate
            for (int i = 0; i < Number; i++)
            {
                GameObject testPoint = Instantiate(Prefab, transform);
                testPoint.GetComponent<RectTransform>().anchoredPosition = TestPointsPosition[i];
                testPoint.transform.localScale = Vector3.one;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}