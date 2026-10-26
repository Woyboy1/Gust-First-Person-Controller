using GustFPC;
using UnityEditor;
using UnityEngine;

namespace GustFPC.Editor
{
    [CustomEditor(typeof(Player))]
    public class GustEditor : UnityEditor.Editor
    {
        #region Foldouts

        private bool dependenciesExpandeed = true; // Recommended true incase you missed an assignment
        private bool movementExpanded = false;
        private bool cameraExpanded = false;
        private bool inputExpanded = false;
        private bool interactionExpanded = false;
        private bool soundExpanded = false;
        private bool headbobExpanded = false;

        #endregion

        #region Inspector

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            Player player = (Player)target;

            DrawMainHeader();

            DrawDocumentationButton();

            GUILayout.Space(15);

            // Settings
            DrawComponentSection(
                player,
                "Player (Master) Settings",
                ref playerSettingsExpanded
            );

            // Dependencies
            DrawComponentSection(
                player.Dependencies,
                "Player Dependencies",
                ref dependenciesExpandeed
            );

            // Movement
            DrawComponentSection(
                player.Dependencies.MovementController,
                "Movement Settings",
                ref movementExpanded
            );

            // Camera
            DrawComponentSection(
                player.Dependencies.CameraController,
                "Camera Settings",
                ref cameraExpanded
            );

            // Input
            DrawComponentSection(
                player.Dependencies.PlayerInputHandler,
                "Player Input",
                ref inputExpanded
            );

            // Interaction
            DrawComponentSection(
                player.Dependencies.InteractionController,
                "Player Interaction",
                ref interactionExpanded
            );

            // Sound
            DrawComponentSection(
                player.Dependencies.PlayerSoundController,
                "Sound Settings",
                ref soundExpanded
            );

            // Headbob
            DrawComponentSection(
                player.Dependencies.HeadBobController,
                "Headbob Settings",
                ref headbobExpanded
            );

            serializedObject.ApplyModifiedProperties();
        }

        #endregion

        #region Shared Design

        private bool playerSettingsExpanded = true;

        private void DrawMainHeader()
        {
            GUILayout.Space(8);

            GUIStyle titleStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 25,
                alignment = TextAnchor.MiddleLeft
            };

            titleStyle.normal.textColor = Color.lightBlue;

            GUIStyle descriptionStyle = new GUIStyle(EditorStyles.miniLabel)
            {
                fontSize = 15,
                wordWrap = true,
                richText = true
            };

            EditorGUILayout.LabelField("Gust First-Person Controller", titleStyle);

            GUILayout.Space(15);

            EditorGUILayout.LabelField(
                "A general first-person controller made by Woyboy. A miniature, but very customizable" +
                " package. Please refer to documentation for questions.",
                descriptionStyle
            );

            GUILayout.Space(8);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }

        private void DrawComponentSection(
            Component component,
            string title,
            ref bool expanded)
        {
            GUILayout.Space(5);

            EditorGUILayout.BeginVertical("box");

            expanded = EditorGUILayout.Foldout(
                expanded,
                title,
                true,
                EditorStyles.foldoutHeader
            );

            if (expanded)
            {
                if (component == null)
                {
                    EditorGUILayout.HelpBox(
                        $"No component found for {title}.",
                        MessageType.Warning
                    );
                }
                else
                {
                    DrawComponentFields(component);
                }
            }

            EditorGUILayout.EndVertical();
        }

        private void DrawComponentFields(Component component)
        {
            SerializedObject componentObject =
                new SerializedObject(component);

            componentObject.Update();

            SerializedProperty property = componentObject.GetIterator();
            bool enterChildren = true;

            while (property.NextVisible(enterChildren))
            {
                enterChildren = false;

                if (property.propertyPath == "m_Script")
                    continue;

                EditorGUILayout.PropertyField(property, true);
            }

            componentObject.ApplyModifiedProperties();
        }

        #endregion

        #region Documentation Button

        private void DrawDocumentationButton()
        {
            GUILayout.Space(5);

            if (GUILayout.Button("View Documentation", GUILayout.Height(30)))
            {
                Application.OpenURL("https://your-documentation-url.com");
            }
        }

        #endregion
    }
}