using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ganzin.EyeTracker.Unity
{
    /// <summary>
    /// Must be under Canvas
    /// </summary>
    public class GanzinCornerTestPointsGenerator : MonoBehaviour
    {
        public GameObject Prefab;
        [ReadOnly]
        public float ScreenWidth = 200;
        [ReadOnly]
        public float ScreenHeight = 100;
        [ReadOnly]
        public float PrefabWidth = 20;
        [ReadOnly]
        public float PrefabHeight = 10;
        [ReadOnly]
        public float OffsetX = 200;
        [ReadOnly]
        public float OffsetY = 100;
        private const int Number = 4;
        [ReadOnly]
        public Vector2[] TestPointsPosition = new Vector2[Number];

        // Start is called before the first frame update
        void Start()
        {
            ScreenWidth = gameObject.GetComponentInParent<Canvas>().gameObject.GetComponent<RectTransform>().rect.width;
            ScreenHeight = gameObject.GetComponentInParent<Canvas>().gameObject.GetComponent<RectTransform>().rect.height;
            PrefabWidth = Prefab.GetComponent<RectTransform>().rect.width;
            PrefabHeight = Prefab.GetComponent<RectTransform>().rect.height;
            OffsetX = (ScreenWidth / 2.0f) - (PrefabWidth / 2.0f);
            OffsetY = (ScreenHeight / 2.0f) - (PrefabHeight / 2.0f);

            // Position
            TestPointsPosition[0] = new Vector2(-OffsetX, OffsetY);
            TestPointsPosition[1] = new Vector2(OffsetX, OffsetY);
            TestPointsPosition[2] = new Vector2(-OffsetX, -OffsetY);
            TestPointsPosition[3] = new Vector2(OffsetX, -OffsetY);

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