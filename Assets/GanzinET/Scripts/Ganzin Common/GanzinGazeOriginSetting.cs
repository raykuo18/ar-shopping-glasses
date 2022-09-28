using System;
using UnityEngine;

namespace Ganzin.EyeTracker.Unity
{
    [Serializable]
    public class GanzinGazeOriginSetting
    {
        public bool IsFixed = false;
        public GameObject LeftEyeOrigin = null;
        public GameObject RightEyeOrigin = null;
        [HideInInspector]
        public bool AlreadyFoundLeftRight = false;
        [HideInInspector]
        public string LeftEye_ObjectName = "Left Eye";
        [HideInInspector]
        public string RightEye_ObjectName = "Right Eye";
        public GanzinGazeOriginSetting() {}
        public GanzinGazeOriginSetting(string _LeftEye_ObjectName, string _RightEye_ObjectName)
        {
            LeftEye_ObjectName = _LeftEye_ObjectName;
            RightEye_ObjectName = _RightEye_ObjectName;
        }
        public void FindGazeOrigin(string _LeftEye_ObjectName, string _RightEye_ObjectName)
        {
            LeftEye_ObjectName = _LeftEye_ObjectName;
            RightEye_ObjectName = _RightEye_ObjectName;
            FindGazeOrigin();
        }
        public void FindGazeOrigin()
        {
            if (LeftEyeOrigin != null && RightEyeOrigin != null)
                AlreadyFoundLeftRight = true;
            else
                AlreadyFoundLeftRight = false;

            if (IsFixed && !AlreadyFoundLeftRight)
            {
                LeftEyeOrigin = GameObject.Find(LeftEye_ObjectName);
                RightEyeOrigin = GameObject.Find(RightEye_ObjectName);
                if (LeftEyeOrigin != null && RightEyeOrigin != null)
                    AlreadyFoundLeftRight = true;
            }
        }
    }
}