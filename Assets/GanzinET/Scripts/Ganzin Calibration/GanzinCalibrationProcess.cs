//#define KEEP_IF_COLLECT_FAIL
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ganzin.EyeTracker.Unity
{
    [Serializable]
    public class GanzinCalibrationAnimationInfo
    {
        public enum ModeType { Move, Fixed }
        public ModeType Mode;
        [Range(0.0f, 1.0f)]
        public float AnimateProgress;
        public GanzinCalibrationAnimationInfo(ModeType _Mode, float _AnimateProgress)
        {
            Mode = _Mode;
            AnimateProgress = _AnimateProgress;
        }
    }
    [Serializable]
    public class GanzinCalibrationAnimateSection
    {
        public GanzinCalibrationAnimationInfo Animation;
        [Tooltip("Duration of animation (second)")]
        public float Duration;
        private float ElapsedTime;
        public bool Loop;
        public enum IncrementType { ConstGradient, VariableGradient };
        public IncrementType IncrementMethod;

        [Tooltip("Ratio of Width / Height")]
        public Vector2 StartPointPos;
        [Tooltip("Ratio of Width / Height")]
        public Vector2 EndPointPos;
        private Vector2? StartPointPosInPixel = null;
        private Vector2? EndPointPosInPixel = null;
        public Vector2 GetStartPointPosInPixel(float width, float height, float fov_constraint = -1.0f, GameObject view_origin = null, GameObject canvas_obj = null)
        {
            //if (StartPointPosInPixel.HasValue) return StartPointPosInPixel ?? Vector2.zero;
            Vector2 LocalPointPosInPixel = new Vector2(width * (StartPointPos.x - 0.5f), height * (StartPointPos.y - 0.5f));
            if (fov_constraint > 0 && view_origin != null && canvas_obj != null)
                LocalPointPosInPixel = RelocatePointPosByFOV(LocalPointPosInPixel, view_origin, canvas_obj, fov_constraint / 2.0f);
            StartPointPosInPixel = LocalPointPosInPixel;
            return LocalPointPosInPixel;
        }
        public Vector2 GetEndPointPosInPixel(float width, float height, float fov_constraint = -1.0f, GameObject view_origin = null, GameObject canvas_obj = null)
        {
            //if (EndPointPosInPixel.HasValue) return EndPointPosInPixel ?? Vector2.zero;
            Vector2 LocalPointPosInPixel = new Vector2(width * (EndPointPos.x - 0.5f), height * (EndPointPos.y - 0.5f));
            if (fov_constraint > 0 && view_origin != null && canvas_obj != null)
                LocalPointPosInPixel = RelocatePointPosByFOV(LocalPointPosInPixel, view_origin, canvas_obj, fov_constraint / 2.0f);
            EndPointPosInPixel = LocalPointPosInPixel;
            return LocalPointPosInPixel;
        }

        [Tooltip("Ratio of Width / Height")]
        public Vector2 PointPos;
        private Vector2? PointPosInPixel = null;
        public Vector2 GetPointPosInPixel(float width, float height, float fov_constraint = -1.0f, GameObject view_origin = null, GameObject canvas_obj = null)
        {
            //if (PointPosInPixel.HasValue) return PointPosInPixel ?? Vector2.zero;
            Vector2 LocalPointPosInPixel = new Vector2(width * (PointPos.x - 0.5f), height * (PointPos.y - 0.5f));
            if (fov_constraint > 0 && view_origin != null && canvas_obj != null)
                LocalPointPosInPixel = RelocatePointPosByFOV(LocalPointPosInPixel, view_origin, canvas_obj, fov_constraint / 2.0f);
            PointPosInPixel = LocalPointPosInPixel;
            return LocalPointPosInPixel;
        }

        private Vector2 RelocatePointPosByFOV(Vector2 originalPos, GameObject viewOrigin, GameObject canvasObj, float restrictedFOV)
        {
            if (originalPos == Vector2.zero || restrictedFOV >= 90.0f)
                return originalPos;

            float radius = Ganzin2DUtility.GetLocalDistance_ByFOV(viewOrigin, canvasObj, restrictedFOV);
            return (originalPos.normalized * radius);
        }

        public void Update(float deltaTime)
        {
            if (Animation.AnimateProgress >= 1.0f)
            {
                if (Loop)
                    Reset();
                else
                    Animation.AnimateProgress = 1.0f;
                    return;
            }

            ElapsedTime += deltaTime;

            if (IncrementMethod == IncrementType.ConstGradient)
                Animation.AnimateProgress = GetProgress_ConstVelocity(ElapsedTime, Duration);
            else if (IncrementMethod == IncrementType.VariableGradient)
                Animation.AnimateProgress = GetProgress_UniformAcceleration(ElapsedTime, Duration);
        }
        public void Reset()
        {
            Animation.AnimateProgress = 0;
            ElapsedTime = 0;
        }
        private static float GetProgress_ConstVelocity(float elapsedTime, float duration)
        {
            float a = (1.0f / duration);
            return a * elapsedTime;
        }
        private static float GetProgress_UniformAcceleration(float elapsedTime, float duration)
        {
            float a, b, c;
            if (elapsedTime <= duration / 2.0f && elapsedTime >= 0.0f)
            {
                a = 2.0f / (duration * duration);
                b = 0.0f;
                c = 0.0f;
            }
            else if (elapsedTime >= duration / 2.0f && elapsedTime <= duration)
            {
                a = -2.0f / (duration * duration);
                b = 4.0f / duration;
                c = -1.0f;
            }
            else if (elapsedTime > duration)
            {
                a = 0.0f;
                b = 0.0f;
                c = 1.0f;
            }
            else
            {
                a = 0.0f;
                b = 0.0f;
                c = 0.0f;
            }
            return a*(elapsedTime*elapsedTime) + b*elapsedTime + c;
        }
    }
    /// <summary>
    /// Functions of Keys:
    ///     c: Start calibration
    /// </summary>
    [DisallowMultipleComponent]
    public class GanzinCalibrationProcess : MonoBehaviour
    {
        [Header("Input/Output")]
        public KeyCode CalibrateHotKey = KeyCode.C;

        [Header("Controlled Objects")]
        public GameObject TargetPoint;
        private GanzinEyetrackerManager EyeTrackerManager;

        [Header("Event Trigger")]
        [Tooltip("Return orignal status after calibration process is over.")]
        public GameObject[] HiddenDuringCalibration;
        private bool[] HiddenDuringCalibrationPreviousValid;

        public UnityEvent BeginCalibrationActions;
        public UnityEvent EndCalibrationActions;

        public enum TargetPointState
        {
            Idle,
            Run
        }
        [Header("Calibration Process Status")]
        [ReadOnly]
        public TargetPointState TargetState = TargetPointState.Idle;
        [ReadOnly]
        public int MotionIndex = 0;
        [ReadOnly]
        public int CollectIndex = 0;
        /// <summary>
        ///     Contain: CalibrateOnePoint
        /// </summary>
        private bool AlreadyCalledCollect = false;
        /// <summary>
        ///     Contain: EnterCalibration
        /// </summary>
        private bool AlreadyCalledPreTask = false;
        /// <summary>
        ///     Contain: CalibrationSetup, CalibrationCompute, LeaveCalibration
        /// </summary>
        private bool AlreadyCalledPostTask = false;

        [Header("Animation Status")]
        [Tooltip("UI Design can use this ratio to control animation.\n" +
            "0: Animation Start, 1: Animation End")]
        public GanzinCalibrationAnimationInfo CurrentAnimationInfo;

        [Header("Calibration Assembly")]
        public GanzinCalibrationAnimateSection[] MotionSections;

        [Header("Calibration Points Position")]
        public bool ConstraintByFOV = true;
        [Tooltip("The angle of Field of View that constraint the calibration points placements\n" +
            "CalibrationFOV = RadiusDegree x 2")]
        [Range(10.0f, 120.0f)]
        public float CalibrationFOV = 30.0f;

        private int CalibrationPointsNum;
        private float CanvasWidth;
        private float CanvasHeight;
#if UNITY_EDITOR
        [Tooltip("For checking the postions")]
        [ReadOnly]
        public List<Vector2> CalibrationPointsPositions;
#endif

        // Start is called before the first frame update
        void Start()
        {
            EyeTrackerManager = FindObjectOfType<GanzinEyetrackerManager>();
            if (EyeTrackerManager == null)
                Debug.LogError("[AP ][Unity] " + "There is no Ganzin Eye Tracker Manager.");

            if (TargetPoint == null)
                Debug.LogError("[AP ][Unity] " + "There is no Target Point.");

            CanvasWidth = TargetPoint.GetComponentInParent<Canvas>().gameObject.GetComponent<RectTransform>().rect.width;
            CanvasHeight = TargetPoint.GetComponentInParent<Canvas>().gameObject.GetComponent<RectTransform>().rect.height;
            CalibrationPointsNum = 0;
            foreach (var section in MotionSections)
            {
                if (section.Animation.Mode == GanzinCalibrationAnimationInfo.ModeType.Fixed)
                {
                    CalibrationPointsNum++;
                }
            }

            // Turn off Error Measurement on Target Point
            GanzinErrorMeasure targetPointErrorMeasure = TargetPoint.GetComponent<GanzinErrorMeasure>();
            if (targetPointErrorMeasure != null)
                targetPointErrorMeasure.enabled = false;

            HiddenDuringCalibrationPreviousValid = new bool[HiddenDuringCalibration.Length];
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

            // Status
            switch (TargetState)
            {
                case TargetPointState.Idle:
                    TargetPoint.SetActive(false);

                    MotionIndex = 0;
                    CollectIndex = 0;

                    CurrentAnimationInfo = MotionSections[MotionIndex].Animation;

                    AlreadyCalledCollect = false;
                    AlreadyCalledPreTask = false;
                    AlreadyCalledPostTask = false;
                    break;
                case TargetPointState.Run:
                    TargetPoint.SetActive(true);

                    // Flow Control
                    // State
                    if (MotionIndex >= MotionSections.Length)
                    {
                        OverCalibration();
                        break;
                    }

                    // Update 
                    MotionSections[MotionIndex].Update(Time.deltaTime);
                    CurrentAnimationInfo = MotionSections[MotionIndex].Animation;

                    if (MotionSections[MotionIndex].Animation.Mode == GanzinCalibrationAnimationInfo.ModeType.Fixed)
                    {
                        // UI
                        if (ConstraintByFOV)
                            TargetPoint.GetComponent<RectTransform>().anchoredPosition =
                                MotionSections[MotionIndex].GetPointPosInPixel(CanvasWidth, CanvasHeight, CalibrationFOV, EyeTrackerManager.gameObject, TargetPoint);
                        else
                            TargetPoint.GetComponent<RectTransform>().anchoredPosition =
                                MotionSections[MotionIndex].GetPointPosInPixel(CanvasWidth, CanvasHeight);
                        // Eyetracker Calibration
                        Vector3 calibPointPos = EyeTrackerManager.gameObject.transform.InverseTransformPoint(TargetPoint.transform.position);
                        if (!AlreadyCalledCollect)
                            StartCoroutine(Collect(CollectIndex, calibPointPos));
                    }
                    else if (MotionSections[MotionIndex].Animation.Mode == GanzinCalibrationAnimationInfo.ModeType.Move)
                    {
                        // Eyetracker Calibration
                        if (MotionIndex == 0 && !AlreadyCalledPreTask)
                            StartCoroutine(PreTask());
                        if (MotionIndex == (MotionSections.Length - 1) && !AlreadyCalledPostTask)
                            StartCoroutine(PostTask());

                        // UI
                        Vector2 startPointPos, endPointPos;
                        if (ConstraintByFOV)
                        {
                            startPointPos = MotionSections[MotionIndex].GetStartPointPosInPixel(CanvasWidth, CanvasHeight, CalibrationFOV, EyeTrackerManager.gameObject, TargetPoint);
                            endPointPos = MotionSections[MotionIndex].GetEndPointPosInPixel(CanvasWidth, CanvasHeight, CalibrationFOV, EyeTrackerManager.gameObject, TargetPoint);
                        }
                        else
                        {
                            startPointPos = MotionSections[MotionIndex].GetStartPointPosInPixel(CanvasWidth, CanvasHeight);
                            endPointPos = MotionSections[MotionIndex].GetEndPointPosInPixel(CanvasWidth, CanvasHeight);
                        }
                        Vector2 moveVector = endPointPos - startPointPos;
                        TargetPoint.GetComponent<RectTransform>().anchoredPosition =
                                startPointPos + MotionSections[MotionIndex].Animation.AnimateProgress * moveVector;

                        // Flow Control
                        // Motions
                        if (MotionSections[MotionIndex].Animation.AnimateProgress >= 1.0f)
                        {
                            MotionSections[MotionIndex].Reset();
                            MotionIndex++;      // Next Motion
                        }
                    }
                    break;
                default:
                    break;
            }

#if UNITY_EDITOR
            //DefaultCalibrationAssemblyAssignment();
            if (EyeTrackerManager != null)
            {
                CalibrationPointsPositions = new List<Vector2>();
                foreach (var section in MotionSections)
                {
                    if (section.Animation.Mode == GanzinCalibrationAnimationInfo.ModeType.Fixed)
                    {
                        CalibrationPointsPositions.Add(section.GetPointPosInPixel(CanvasWidth, CanvasHeight,
                            CalibrationFOV, EyeTrackerManager.gameObject, TargetPoint));
                    }
                }
            }
#endif
        }

        [Space(10)]
        [Button("StartCalibration")]
        [SerializeField]
        private bool __Dummy_StartCalibration;
        public void StartCalibration()
        {
            if (EyeTrackerManager == null)
            {
                Debug.LogError("[AP ][Unity] " + "There is no Ganzin Eye Tracker Manager.");
                return;
            }
            if (EyeTrackerManager.Status != GanzinEyetrackerManager.EyetrackerStatus.ACTIVE)
            {
                Debug.LogWarning("[AP ][Unity] " + "Ganzin Eye Tracker does not work.");
                return;
            }
            if (TargetState != TargetPointState.Idle)
            {
                Debug.LogWarning("[AP ][Unity] " + "Already Started Calibration.");
                return;
            }

            BeginCalibration();
            TargetState = TargetPointState.Run;
        }
        public void OverCalibration()
        {
            if (TargetState != TargetPointState.Run)
            {
                Debug.LogWarning("[AP ][Unity] " + "Already Overed Calibration.");
                return;
            }

            EndCalibration();
            TargetState = TargetPointState.Idle;
        }
        private IEnumerator Collect(int current_idx, Vector3 calib_point_pos)
        {
            AlreadyCalledCollect = true;

            (bool, bool) collect_result = (false, false);
            Task<(bool, bool)> collect_task = Task.Run(() => {
#if UNITY_ANDROID
                AndroidJNI.AttachCurrentThread();
                Tuple<bool, bool> result = EyeTrackerManager.CalibrateOnePoint(current_idx, calib_point_pos);
                AndroidJNI.DetachCurrentThread();
#else
                Tuple<bool, bool> result = EyeTrackerManager.CalibrateOnePoint(current_idx, calib_point_pos);
#endif
                return (result.Item1, result.Item2);
                    });
            while (!collect_task.IsCompleted) yield return 0;
            if (collect_task.Status == TaskStatus.RanToCompletion)
                collect_result = collect_task.Result;
            
            // Fail and complete
            if (!collect_result.Item1)
            {
                MotionSections[MotionIndex].Reset();
                OverCalibration();
            }
#if KEEP_IF_COLLECT_FAIL
            else if (!collect_result.Item2)
            {
                
            }
#endif
            // Succeed but not complete
            else
            {
                MotionSections[MotionIndex].Reset();

                MotionIndex++;      // Next Motion
                CollectIndex++;
            }
            AlreadyCalledCollect = false;
        }
        private IEnumerator PreTask()
        {
            AlreadyCalledPreTask = true;

            bool enter_result = EyeTrackerManager.EnterCalibration();
            if (!enter_result)
            {
                Debug.LogError("[AP ][Unity] " + "Fail to Enter Calibration!");
                MotionSections[MotionIndex].Reset();
                OverCalibration();
                // throw new InvalidOperationException("Fail to Enter Calibration!");
            }
            yield return 0;
        }
        private IEnumerator PostTask()
        {
            AlreadyCalledPostTask = true;

            bool setup_result = false;
            Task<bool> setup_task = Task.Run(() =>
            {
#if UNITY_ANDROID
                AndroidJNI.AttachCurrentThread();
                bool result = EyeTrackerManager.CalibrationSetup();
                AndroidJNI.DetachCurrentThread();
#else
                bool result = EyeTrackerManager.CalibrationSetup();
#endif
                return result;
            });
            while (!setup_task.IsCompleted) yield return 0;
            if (setup_task.Status == TaskStatus.RanToCompletion)
                setup_result = setup_task.Result;
            if (!setup_result)
            {
                Debug.LogError("[AP ][Unity] " + "Fail to Leave Calibration!");
                MotionSections[MotionIndex].Reset();
                OverCalibration();
                // throw new InvalidOperationException("Fail to Calibration Setup!");
            }
            yield return 0;


            (bool, bool) compute_result = (false, false);
            Task<(bool, bool)> compute_task = Task.Run(() =>
            {
#if UNITY_ANDROID
                AndroidJNI.AttachCurrentThread();
                Tuple<bool, bool> result = EyeTrackerManager.CalibrationCompute();
                AndroidJNI.DetachCurrentThread();
#else
                Tuple<bool, bool> result = EyeTrackerManager.CalibrationCompute();
#endif
                return (result.Item1, result.Item2);
            });
            while (!compute_task.IsCompleted) yield return 0;
            if (compute_task.Status == TaskStatus.RanToCompletion)
                compute_result = compute_task.Result;
            if (!compute_result.Item1)
            {
                Debug.LogError("[AP ][Unity] " + "Fail to Leave Calibration!");
                MotionSections[MotionIndex].Reset();
                OverCalibration();
                // throw new InvalidOperationException("Fail to Calibration Compute!");
            }
            yield return 0;

            bool leave_result = EyeTrackerManager.LeaveCalibration();
            if (!leave_result)
            {
                Debug.LogError("[AP ][Unity] " + "Fail to Leave Calibration!");
                MotionSections[MotionIndex].Reset();
                OverCalibration();
                // throw new InvalidOperationException("Fail to Leave Calibration!");
            }
            yield return 0;
        }
        private void UserInteract()
        {
            if (CalibrateHotKey != KeyCode.None)
            {
                if (Input.GetKeyDown(CalibrateHotKey))
                {
                    StartCalibration();
                }
            }
        }
        private void BeginCalibration()
        {
            for (int i = 0; i < HiddenDuringCalibration.Length; ++i)
            {
                HiddenDuringCalibrationPreviousValid[i] = HiddenDuringCalibration[i].activeInHierarchy;
                HiddenDuringCalibration[i].SetActive(false);
            }
            BeginCalibrationActions?.Invoke();
        }
        private void EndCalibration()
        {
            for (int i = 0; i < HiddenDuringCalibration.Length; i++)
                HiddenDuringCalibration[i].SetActive(HiddenDuringCalibrationPreviousValid[i]);
            EndCalibrationActions?.Invoke();
        }        
    }
}