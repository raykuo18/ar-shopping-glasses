using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Ganzin.EyeTracker.Unity
{
    [CustomPropertyDrawer(typeof(GanzinGazeOriginSetting))]
    public class GanzinGazeOriginSettingDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float AdditionalHeight = EditorGUIUtility.singleLineHeight * 3;
            return base.GetPropertyHeight(property, label) + AdditionalHeight;
        }
        public override void OnGUI(Rect totalPosition, SerializedProperty property, GUIContent label)
        {
            var IsFixed = property.FindPropertyRelative("IsFixed");
            var LeftEyeOrigin = property.FindPropertyRelative("LeftEyeOrigin");
            var RightEyeOrigin = property.FindPropertyRelative("RightEyeOrigin");

            //EditorGUI.BeginProperty(totalPosition, label, property);

            EditorGUI.PrefixLabel(totalPosition, label);            

            var IsFixedRect = new Rect(totalPosition.position.x, totalPosition.position.y + EditorGUIUtility.singleLineHeight, 
                totalPosition.width, EditorGUIUtility.singleLineHeight);
            var LeftEyeOriginRect = new Rect(totalPosition.position.x, totalPosition.position.y + EditorGUIUtility.singleLineHeight * 2, 
                totalPosition.width, EditorGUIUtility.singleLineHeight);
            var RightEyeOriginRect = new Rect(totalPosition.position.x, totalPosition.position.y + EditorGUIUtility.singleLineHeight * 3, 
                totalPosition.width, EditorGUIUtility.singleLineHeight);

            EditorGUI.indentLevel++;
            
            EditorGUI.PropertyField(IsFixedRect, IsFixed);
            EditorGUI.BeginDisabledGroup(!IsFixed.boolValue);
            {
                EditorGUI.PropertyField(LeftEyeOriginRect, LeftEyeOrigin);
                EditorGUI.PropertyField(RightEyeOriginRect, RightEyeOrigin);
            }
            EditorGUI.EndDisabledGroup();
            EditorGUI.indentLevel--;

            //EditorGUI.EndProperty();
        }
    }
}