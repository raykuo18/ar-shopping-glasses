using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ganzin.EyeTracker.Unity
{
    /// <summary>
    /// Must has a GanzinGazeHitDetect.
    /// </summary>
    [RequireComponent(typeof(GanzinGazeHitDetect))]
    [RequireComponent(typeof(Button))]
    public class GanzinButtonGazeSelect : MonoBehaviour
    {
        private GanzinGazeHitDetect GazeHitDetector;
        private Button ThisButton;
        // Start is called before the first frame update
        void Start()
        {
            GazeHitDetector = GetComponent<GanzinGazeHitDetect>();
            if (GazeHitDetector == null)
            {
                Debug.LogError("[AP ][Unity] " + "There is no Ganzin Gaze Hit Detect.");
                return;
            }
            ThisButton = GetComponent<Button>();
            if (ThisButton == null)
            {
                Debug.LogError("[AP ][Unity] " + "There is no Button.");
                return;
            }
        }

        // Update is called once per frame
        void Update()
        {
            EventSystem.current.SetSelectedGameObject(null);
            if (GazeHitDetector.IsHit)
                ThisButton.Select();
        }
    }
}