#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(TimelineEvent))]
public class TimelineEventDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        position.height = EditorGUIUtility.singleLineHeight;
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 1;
        EditorGUI.PropertyField(position, property.FindPropertyRelative("triggerTime"));
        Rect typePos = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing, position.width, position.height);
        EditorGUI.PropertyField(typePos, property.FindPropertyRelative("type"));
        Rect OtherPosition = new Rect(position.x, typePos.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing, position.width, position.height);
        switch ((TimelineEventType)property.FindPropertyRelative("type").intValue)
        {
            case TimelineEventType.GameEvent:
                EditorGUI.PropertyField(OtherPosition, property.FindPropertyRelative("toTrigger"));
                break;
            case TimelineEventType.FlagSetTrue:
                EditorGUI.PropertyField(OtherPosition, property.FindPropertyRelative("flag"));
                break;
            case TimelineEventType.FlagSetFalse:
                EditorGUI.PropertyField(OtherPosition, property.FindPropertyRelative("flag"));
                break;
            case TimelineEventType.WaitForFlag:
                EditorGUI.PropertyField(OtherPosition, property.FindPropertyRelative("flag"));
                break;
            case TimelineEventType.SwitchTimeline:
                EditorGUI.PropertyField(OtherPosition, property.FindPropertyRelative("timeline"));
                break;
            case TimelineEventType.PlayOtherTimelineAsync:
                EditorGUI.PropertyField(OtherPosition, property.FindPropertyRelative("timeline"));
                break;
            case TimelineEventType.PlayAnimation:
                EditorGUI.PropertyField(OtherPosition, property.FindPropertyRelative("animatorID"));
                Rect OtherOtherPosition = new Rect(position.x, OtherPosition.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing, position.width, position.height);
                EditorGUI.PropertyField(OtherOtherPosition, property.FindPropertyRelative("stateName"));
                break;
        }
        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        int lineCount = 3;
        if ((TimelineEventType)property.FindPropertyRelative("type").intValue == TimelineEventType.PlayAnimation) lineCount++;
        return EditorGUIUtility.singleLineHeight * lineCount + EditorGUIUtility.standardVerticalSpacing * (lineCount - 1);
    }
}
#endif