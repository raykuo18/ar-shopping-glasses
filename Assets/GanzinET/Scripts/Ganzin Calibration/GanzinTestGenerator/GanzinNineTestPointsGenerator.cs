using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ganzin.EyeTracker.Unity
{
    /// <summary>
    /// Must be under Canvas
    /// </summary>
    public class GanzinNineTestPointsGenerator : MonoBehaviour
    {
        public GameObject Prefab;
        [ReadOnly]
        public float ScreenWidth = 200;
        [ReadOnly]
        public float ScreenHeight = 100;
        [ReadOnly]
        public float BetweenSpaceX = 200;
        [ReadOnly]
        public float BetweenSpaceY = 100;
        private const int Number = 9;
        [ReadOnly]
        public Vector2[] TestPointsPosition = new Vector2[Number];

        // Start is called before the first frame update
        void Start()
        {
            ScreenWidth = gameObject.GetComponentInParent<Canvas>().gameObject.GetComponent<RectTransform>().rect.width;
            ScreenHeight = gameObject.GetComponentInParent<Canvas>().gameObject.GetComponent<RectTransform>().rect.height;

            BetweenSpaceX = ScreenWidth / 4.0f;
            BetweenSpaceY = ScreenHeight / 4.0f;
            
            // Position
            TestPointsPosition[0] = new Vector2(-BetweenSpaceX, BetweenSpaceY);
            TestPointsPosition[1] = new Vector2(0, BetweenSpaceY);
            TestPointsPosition[2] = new Vector2(BetweenSpaceX, BetweenSpaceY);
            TestPointsPosition[3] = new Vector2(-BetweenSpaceX, 0);
            TestPointsPosition[4] = new Vector2(0, 0);
            TestPointsPosition[5] = new Vector2(BetweenSpaceX, 0);
            TestPointsPosition[6] = new Vector2(-BetweenSpaceX, -BetweenSpaceY);
            TestPointsPosition[7] = new Vector2(0, -BetweenSpaceY);
            TestPointsPosition[8] = new Vector2(BetweenSpaceX, -BetweenSpaceY);

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