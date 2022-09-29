using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ganzin.EyeTracker.Unity
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(GanzinCalibrationProcess))]
    public class GanzinCalibPointAnimate : MonoBehaviour
    {
        private GanzinCalibrationProcess calibrationProcess;
        private GameObject targetPoint;
        public float ChangeScale = 1.0f;
        public float ChangeOffset = 0.3f;
        // Start is called before the first frame update
        void Start()
        {
            calibrationProcess = GetComponent<GanzinCalibrationProcess>();
            targetPoint = calibrationProcess.TargetPoint;
        }

        // Update is called once per frame
        void Update()
        {
            float cur_animate_ratio = 1.0f - Mathf.Abs(2.0f * calibrationProcess.CurrentAnimationInfo.AnimateProgress - 1.0f);
            float cur_target_point_scale = (ChangeScale * cur_animate_ratio) + ChangeOffset;
            targetPoint.transform.localScale =  new Vector3(cur_target_point_scale, cur_target_point_scale, targetPoint.transform.localScale.z);
        }
    }
}