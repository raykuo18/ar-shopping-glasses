using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Ganzin.EyeTracker.Unity
{
    [CustomPropertyDrawer(typeof(GanzinCalibrationAnimateSection))]
    public class GanzinCalibrationAnimateSectionDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            int LineNum = 0;
            var Animation = property.FindPropertyRelative("Animation");
            var Mode = Animation.FindPropertyRelative("Mode");
            if (Mode.enumValueIndex == (int)GanzinCalibrationAnimationInfo.ModeType.Move)
                LineNum = 6;
            else if (Mode.enumValueIndex == (int)GanzinCalibrationAnimationInfo.ModeType.Fixed)
                LineNum = 5;
            float AdditionalHeight = EditorGUIUtility.singleLineHeight * LineNum;
            return base.GetPropertyHeight(property, label) + AdditionalHeight;
        }
        public override void OnGUI(Rect totalPosition, SerializedProperty property, GUIContent label)
        {
            var Animation = property.FindPropertyRelative("Animation");
            var Mode = Animation.FindPropertyRelative("Mode");
            var Duration = property.FindPropertyRelative("Duration");
            var Loop = property.FindPropertyRelative("Loop");
            var IncrementMethod = property.FindPropertyRelative("IncrementMethod");
            var StartPointPos = property.FindPropertyRelative("StartPointPos");
            var EndPointPos = property.FindPropertyRelative("EndPointPos");
            var PointPos = property.FindPropertyRelative("PointPos");

            //EditorGUI.BeginProperty(totalPosition, label, property);

            EditorGUI.PrefixLabel(totalPosition, label);

            var ModeRect = new Rect(totalPosition.position.x, totalPosition.position.y + EditorGUIUtility.singleLineHeight,
                totalPosition.width, EditorGUIUtility.singleLineHeight);

            var DurationRect = new Rect(totalPosition.position.x, totalPosition.position.y + EditorGUIUtility.singleLineHeight * 2,
                totalPosition.width, EditorGUIUtility.singleLineHeight);
            var LoopRect = new Rect(totalPosition.position.x, totalPosition.position.y + EditorGUIUtility.singleLineHeight * 3,
                totalPosition.width, EditorGUIUtility.singleLineHeight);
            var IncrementMethodRect = new Rect(totalPosition.position.x, totalPosition.position.y + EditorGUIUtility.singleLineHeight * 4,
                totalPosition.width, EditorGUIUtility.singleLineHeight);
            var StartPointPosRect = new Rect(totalPosition.position.x, totalPosition.position.y + EditorGUIUtility.singleLineHeight * 5,
                totalPosition.width, EditorGUIUtility.singleLineHeight);
            var EndPointPosRect = new Rect(totalPosition.position.x, totalPosition.position.y + EditorGUIUtility.singleLineHeight * 6,
                totalPosition.width, EditorGUIUtility.singleLineHeight);
            var PointPosRect = new Rect(totalPosition.position.x, totalPosition.position.y + EditorGUIUtility.singleLineHeight * 5,
                totalPosition.width, EditorGUIUtility.singleLineHeight);

            EditorGUI.indentLevel++;

            EditorGUI.PropertyField(ModeRect, Mode);
            EditorGUI.PropertyField(DurationRect, Duration);
            EditorGUI.PropertyField(LoopRect, Loop);
            EditorGUI.PropertyField(IncrementMethodRect, IncrementMethod);
            if (Mode.enumValueIndex == (int)GanzinCalibrationAnimationInfo.ModeType.Move)
            {
                
                EditorGUI.PropertyField(StartPointPosRect, StartPointPos);
                EditorGUI.PropertyField(EndPointPosRect, EndPointPos);                
            }
            else if (Mode.enumValueIndex == (int)GanzinCalibrationAnimationInfo.ModeType.Fixed)
            {
                EditorGUI.PropertyField(PointPosRect, PointPos);
            }

            EditorGUI.indentLevel--;

            //EditorGUI.EndProperty();
        }
    }
}