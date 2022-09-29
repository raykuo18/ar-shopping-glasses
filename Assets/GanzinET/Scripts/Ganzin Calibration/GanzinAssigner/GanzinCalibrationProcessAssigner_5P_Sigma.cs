using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ganzin.EyeTracker.Unity
{
    [RequireComponent(typeof(GanzinCalibrationProcess))]
    [ExecuteAlways]
    public class GanzinCalibrationProcessAssigner_5P_Sigma : MonoBehaviour
    {
        private GanzinCalibrationProcess CalibrationProcess;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            CalibrationProcess = GetComponent<GanzinCalibrationProcess>();
            DefaultCalibrationAssemblyAssignment();
        }
        private void DefaultCalibrationAssemblyAssignment()
        {
            int total_section_num = 12;
            CalibrationProcess.MotionSections = new GanzinCalibrationAnimateSection[total_section_num];
            for (int i = 0; i < total_section_num; i++)
            {
                CalibrationProcess.MotionSections[i] = new GanzinCalibrationAnimateSection();
                if (i == 0 || i == 2 || i == 4 || i == 6 || i == 8 || i == 10 || i == 11) // Animation
                {
                    CalibrationProcess.MotionSections[i].Animation = new GanzinCalibrationAnimationInfo(GanzinCalibrationAnimationInfo.ModeType.Move, 0.0f);
                    CalibrationProcess.MotionSections[i].Duration = 2;
                    CalibrationProcess.MotionSections[i].Loop = false;
                    CalibrationProcess.MotionSections[i].IncrementMethod = GanzinCalibrationAnimateSection.IncrementType.VariableGradient;
                    switch (i)
                    {
                        case 0:
                            CalibrationProcess.MotionSections[i].StartPointPos = new Vector2(0.5f, 0.5f);
                            CalibrationProcess.MotionSections[i].EndPointPos = new Vector2(0.5f, 0.5f);
                            break;
                        case 2:
                            CalibrationProcess.MotionSections[i].StartPointPos = new Vector2(0.5f, 0.5f);
                            CalibrationProcess.MotionSections[i].EndPointPos = new Vector2(0.0f, 0.0f);
                            break;
                        case 4:
                            CalibrationProcess.MotionSections[i].StartPointPos = new Vector2(0.0f, 0.0f);
                            CalibrationProcess.MotionSections[i].EndPointPos = new Vector2(1.0f, 0.0f);
                            break;
                        case 6:
                            CalibrationProcess.MotionSections[i].StartPointPos = new Vector2(1.0f, 0.0f);
                            CalibrationProcess.MotionSections[i].EndPointPos = new Vector2(1.0f, 1.0f);
                            break;
                        case 8:
                            CalibrationProcess.MotionSections[i].StartPointPos = new Vector2(1.0f, 1.0f);
                            CalibrationProcess.MotionSections[i].EndPointPos = new Vector2(0.0f, 1.0f);
                            break;
                        case 10:
                            CalibrationProcess.MotionSections[i].StartPointPos = new Vector2(0.0f, 1.0f);
                            CalibrationProcess.MotionSections[i].EndPointPos = new Vector2(0.5f, 0.5f);
                            break;
                        case 11:
                            CalibrationProcess.MotionSections[i].StartPointPos = new Vector2(0.5f, 0.5f);
                            CalibrationProcess.MotionSections[i].EndPointPos = new Vector2(0.5f, 0.5f);
                            break;
                    }
                }
                else // Collect
                {
                    CalibrationProcess.MotionSections[i].Animation = new GanzinCalibrationAnimationInfo(GanzinCalibrationAnimationInfo.ModeType.Fixed, 0.0f);
                    CalibrationProcess.MotionSections[i].Duration = 2;
                    CalibrationProcess.MotionSections[i].Loop = true;
                    CalibrationProcess.MotionSections[i].IncrementMethod = GanzinCalibrationAnimateSection.IncrementType.ConstGradient;
                    switch ((int)(i / 2.0f))
                    {
                        case 0:
                            CalibrationProcess.MotionSections[i].PointPos = new Vector2(0.5f, 0.5f);
                            break;
                        case 1:
                            CalibrationProcess.MotionSections[i].PointPos = new Vector2(0.0f, 0.0f);
                            break;
                        case 2:
                            CalibrationProcess.MotionSections[i].PointPos = new Vector2(1.0f, 0.0f);
                            break;
                        case 3:
                            CalibrationProcess.MotionSections[i].PointPos = new Vector2(1.0f, 1.0f);
                            break;
                        case 4:
                            CalibrationProcess.MotionSections[i].PointPos = new Vector2(0.0f, 1.0f);
                            break;
                    }
                }
            }
        }
    }
}