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
    public class GanzinGuiShowInfo : MonoBehaviour
    {
        public KeyCode ShowHotKey = KeyCode.I;
        private GanzinEyetrackerManager EyeTrackerManager;
        public bool ShowInfo;

        // Start is called before the first frame update
        void Start()
        {
            EyeTrackerManager = FindObjectOfType<GanzinEyetrackerManager>();
            if (EyeTrackerManager == null)
                Debug.LogError("[AP ][Unity] " + "There is no Ganzin Eye Tracker Manager.");
            ShowInfo = true;
        }

        // Update is called once per frame
        void Update()
        {
            // User Interaction
            UserInteract();

            // Check
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
        }
        private void UserInteract()
        {
            if (ShowHotKey != KeyCode.None)
            {
                if (Input.GetKeyDown(ShowHotKey))
                {
                    ShowInfo = !ShowInfo;
                }
            }
        }
        void OnGUI()
        {
            // Constants
            int GUI_PADDING = (int)(Screen.height * 0.01);
            int GUI_Label_WIDTH = (int)(Screen.height * 0.3);
            int GUI_Label_HEIGHT = (int)(Screen.height * 0.03);

            // GUI text for gaze information
            if (ShowInfo)
            {
                EyeTrackerManager.GetGazeData(GazeIndex.COMBINE, out Vector3 gaze_origin, out Vector3 gaze_direction, out Vector3 gaze_position);
                Vector3 global_gaze_position = EyeTrackerManager.gameObject.transform.TransformPoint(gaze_position);
                Vector3 global_gaze_direction = EyeTrackerManager.gameObject.transform.TransformDirection(gaze_direction);
                Vector3 global_gaze_origin = EyeTrackerManager.gameObject.transform.TransformPoint(gaze_origin);

                Vector3 global_gaze_position_mm = global_gaze_position * 1000.0f;
                Vector3 global_gaze_origin_mm = global_gaze_origin * 1000.0f;

                GUIStyle gaze_info_style = new GUIStyle();
                gaze_info_style.fontSize = 20;
                gaze_info_style.normal.textColor = Color.blue;                
                GUI.Label(new Rect(GUI_PADDING, Screen.height - (GUI_PADDING + (GUI_Label_HEIGHT * 5)), GUI_Label_WIDTH, GUI_Label_HEIGHT),
                    "3D gaze position estimated (x, y, z): (" + global_gaze_position_mm.x.ToString("F2") + ", " + global_gaze_position_mm.y.ToString("F2") + ", " + global_gaze_position_mm.z.ToString("F2") + ") (mm)", gaze_info_style);
                GUI.Label(new Rect(GUI_PADDING, Screen.height - (GUI_PADDING + (GUI_Label_HEIGHT * 4)), GUI_Label_WIDTH, GUI_Label_HEIGHT),
                    "3D gaze direction (x, y, z): (" + global_gaze_direction.x.ToString("F4") + ", " + global_gaze_direction.y.ToString("F4") + ", " + global_gaze_direction.z.ToString("F4") + ")", gaze_info_style);
                GUI.Label(new Rect(GUI_PADDING, Screen.height - (GUI_PADDING + (GUI_Label_HEIGHT * 3)), GUI_Label_WIDTH, GUI_Label_HEIGHT),
                    "3D gaze origin (x, y, z): (" + global_gaze_origin_mm.x.ToString("F2") + ", " + global_gaze_origin_mm.y.ToString("F2") + ", " + global_gaze_origin_mm.z.ToString("F2") + ") (mm)", gaze_info_style);
                GUI.Label(new Rect(GUI_PADDING, Screen.height - (GUI_PADDING + (GUI_Label_HEIGHT * 2)), GUI_Label_WIDTH, GUI_Label_HEIGHT),
                    "Gaze data frequency: " + EyeTrackerManager.GazeDataFrequency + "/s", gaze_info_style);
                GUI.Label(new Rect(GUI_PADDING, Screen.height - (GUI_PADDING + (GUI_Label_HEIGHT * 1)), GUI_Label_WIDTH, GUI_Label_HEIGHT),
                    "Gaze data latency: " + EyeTrackerManager.GazeDataLatency + "ms", gaze_info_style);
            }
        }
    }
}