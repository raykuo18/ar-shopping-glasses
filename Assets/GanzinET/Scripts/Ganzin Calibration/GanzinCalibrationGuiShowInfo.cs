using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ganzin.EyeTracker.Unity
{
    /// <summary>
    /// Functions of Keys:
    ///     i: Show Gaze Information
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(GanzinGuiShowInfo))]
    public class GanzinCalibrationGuiShowInfo : MonoBehaviour
    {
        private GanzinGuiShowInfo GuiShowInfo;
        public GameObject GazePoint;
        // Start is called before the first frame update
        void Start()
        {
            GuiShowInfo = gameObject.GetComponent<GanzinGuiShowInfo>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnGUI()
        {
            // Constants
            int GUI_PADDING = (int)(Screen.height * 0.01);
            int GUI_Label_WIDTH = (int)(Screen.height * 0.3);
            int GUI_Label_HEIGHT = (int)(Screen.height * 0.03);

            // GUI text for gaze information
            if (GuiShowInfo != null)
            {
                if (GuiShowInfo.ShowInfo)
                {
                    Vector3 global_gaze_position_on_screen_mm = GazePoint.transform.position * 1000.0f;

                    GUIStyle gaze_info_style = new GUIStyle();
                    gaze_info_style.fontSize = 20;
                    gaze_info_style.normal.textColor = Color.blue;
                    GUI.Label(new Rect(GUI_PADDING, Screen.height - (GUI_PADDING + (GUI_Label_HEIGHT * 7)), GUI_Label_WIDTH, GUI_Label_HEIGHT),
                        "2D gaze position on screen (x, y): (" + GazePoint.transform.localPosition.x.ToString("F2") + ", " + GazePoint.transform.localPosition.y.ToString("F2") + ")(px)", gaze_info_style);
                    GUI.Label(new Rect(GUI_PADDING, Screen.height - (GUI_PADDING + (GUI_Label_HEIGHT * 6)), GUI_Label_WIDTH, GUI_Label_HEIGHT),
                        "3D gaze position on screen (x, y, z): (" + global_gaze_position_on_screen_mm.x.ToString("F2") + ", " + global_gaze_position_on_screen_mm.y.ToString("F2") + ", " + global_gaze_position_on_screen_mm.z.ToString("F2") + ")(mm)", gaze_info_style);
                }
            }
        }
    }
}