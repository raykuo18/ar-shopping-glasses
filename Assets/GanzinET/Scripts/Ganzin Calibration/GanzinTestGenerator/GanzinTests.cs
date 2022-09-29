using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ganzin.EyeTracker.Unity
{
    /// <summary>
    /// Functions of Keys:
    ///     1: Enable test pattern 1
    ///     2: Enable test pattern 2
    ///     3: Enable test pattern 3
    ///     4: Enable test pattern 4
    ///     5: Enable test pattern 5
    ///     6: Enable test pattern 6
    ///     7: Enable test pattern 7
    ///     8: Enable test pattern 8
    /// </summary>
    /// 
    [DisallowMultipleComponent]
    [ExecuteAlways]
    public class GanzinTests : MonoBehaviour
    {
        public GameObject[] TestPatterns;
        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            // User Interaction
            for (int i = 0; i < TestPatterns.Length; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha0 + i) || Input.GetKeyDown(KeyCode.Keypad0 + i))
                    TestPatterns[i].SetActive(!TestPatterns[i].activeInHierarchy);
            }
        }

        void OnGUI()
        {
            // Constants
            int GUI_PADDING = (int)(Screen.height * 0.01);
            int GUI_BUTTON_WIDTH = (int)(Screen.height * 0.15);
            int GUI_BUTTON_HEIGHT = (int)(Screen.height * 0.03);

            // Create GUI buttons for Test patterns
            for (int i = 0; i < TestPatterns.Length; i++)
            {
                string button_text = "Test Pattern " + i.ToString();
                if (GUI.Button(new Rect(GUI_PADDING, GUI_PADDING + (GUI_BUTTON_HEIGHT * i), GUI_BUTTON_WIDTH, GUI_BUTTON_HEIGHT), button_text))
                {
                    TestPatterns[i].SetActive(!TestPatterns[i].activeInHierarchy);
                }
            }
        }
    }
}