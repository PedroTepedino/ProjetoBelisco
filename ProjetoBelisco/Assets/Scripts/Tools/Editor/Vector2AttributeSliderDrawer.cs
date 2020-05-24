using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace GameScripts.Tools.Editor
{
    public class Vector2AttributeSliderDrawer : OdinAttributeDrawer<Vector2SliderAttribute, Vector2>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            GUIHelper.PushLabelWidth(50);
            Rect rect = EditorGUILayout.GetControlRect();

            if (label != null)
            {
                rect = EditorGUI.PrefixLabel(rect, label);
            }
            GUIHelper.PopLabelWidth();

            Vector2 value = this.ValueEntry.SmartValue;

            GUIHelper.PushContextWidth(20);
            GUIHelper.PushLabelWidth(15);
            value.x = EditorGUI.Slider(rect.AlignLeft(rect.width * 0.495f), "X" , value.x, this.Attribute.minValue, this.Attribute.maxValue);
            value.y = EditorGUI.Slider(rect.AlignRight(rect.width * 0.495f), "Y", value.y, this.Attribute.minValue, this.Attribute.maxValue);
            GUIHelper.PopLabelWidth();
            GUIHelper.PopContextWidth();

            this.ValueEntry.SmartValue = value;
        }
    }
}
