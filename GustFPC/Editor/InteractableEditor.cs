using UnityEditor;
using UnityEngine;

namespace GustFPC.Editor
{
    [CustomEditor(typeof(Interactable), true)]
    public class InteractableEditor : UnityEditor.Editor
    {
        #region Foldouts

        private bool interactionExpanded = false;
        private bool debugExpanded = false;
        private bool audioExpanded = false;
        private bool eventsExpanded = false;
        private bool customExpanded = true;
        private bool cooldownExpanded = false;

        #endregion

        #region Inspector
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawMainHeader();

            DrawSection("Interaction Settings", ref interactionExpanded,
                "canInteract", "disableAfterInteraction");

            DrawSection("Cooldown Settings", ref cooldownExpanded,
                "useCooldown", "interactionCooldown");

            DrawSection("Debug Settings", ref debugExpanded,
                "logInteraction", "debugMessage");

            DrawSection("Audio Settings", ref audioExpanded,
                "audioSource", "interactionSound");

            DrawSection("Events", ref eventsExpanded,
                "onInteract");

            DrawCustomFields();

            serializedObject.ApplyModifiedProperties();
        }
        #endregion

        #region UI Elements
        private void DrawMainHeader()
        {
            GUIStyle titleStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 25,
                alignment = TextAnchor.MiddleLeft
            };
            titleStyle.normal.textColor = Color.lightBlue;

            GUILayout.Space(5);

            EditorGUILayout.LabelField("Interactable", titleStyle);

            GUILayout.Space(5);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }

        private void DrawSection(
            string title,
            ref bool expanded,
            params string[] propertyNames)
        {
            EditorGUILayout.BeginVertical("box");

            expanded = EditorGUILayout.Foldout(
                expanded, title, true, EditorStyles.foldoutHeader);

            if (expanded)
            {
                EditorGUI.indentLevel++;

                foreach (string propertyName in propertyNames)
                {
                    SerializedProperty property =
                        serializedObject.FindProperty(propertyName);

                    if (property != null)
                        EditorGUILayout.PropertyField(property, true);
                }

                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndVertical();
            GUILayout.Space(3);
        }

        private void DrawCustomFields()
        {
            EditorGUILayout.BeginVertical("box");

            customExpanded = EditorGUILayout.Foldout(
                customExpanded, "Custom Behavior", true,
                EditorStyles.foldoutHeader);

            if (customExpanded)
            {
                EditorGUI.indentLevel++;

                SerializedProperty property =
                    serializedObject.GetIterator();

                bool enterChildren = true;

                while (property.NextVisible(enterChildren))
                {
                    enterChildren = false;

                    if (property.propertyPath == "m_Script" ||
                        IsBaseProperty(property.propertyPath))
                        continue;

                    EditorGUILayout.PropertyField(property, true);
                }

                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndVertical();
        }

        private bool IsBaseProperty(string propertyPath)
        {
            return propertyPath == "canInteract" ||
                   propertyPath == "disableAfterInteraction" ||
                   propertyPath == "logInteraction" ||
                   propertyPath == "debugMessage" ||
                   propertyPath == "audioSource" ||
                   propertyPath == "interactionSound" ||
                   propertyPath == "useCooldown" ||
                   propertyPath == "interactionCooldown" ||
                   propertyPath == "onInteract";
        }
        #endregion
    }
}