using UnityEngine;
using UnityEditor;

namespace Michsky.UI.ModernUIPack
{
    [CustomEditor(typeof(PieChart))]
    public class PieChartEditor : Editor
    {
        private PieChart pieTarget;
        private UIManagerPieChart tempUIM;
        private int currentTab;

        private void OnEnable()
        {
            pieTarget = (PieChart)target;

            try { tempUIM = pieTarget.GetComponent<UIManagerPieChart>(); }
            catch { }
        }

        public override void OnInspectorGUI()
        {
            GUISkin customSkin;
            Color defaultColor = GUI.color;

            if (EditorGUIUtility.isProSkin == true)
                customSkin = (GUISkin)Resources.Load("Editor\\MUI Skin Dark");
            else
                customSkin = (GUISkin)Resources.Load("Editor\\MUI Skin Light");

            GUILayout.BeginHorizontal();
            GUI.backgroundColor = defaultColor;

            GUILayout.Box(new GUIContent(""), customSkin.FindStyle("PC Top Header"));

            GUILayout.EndHorizontal();
            GUILayout.Space(-42);

            GUIContent[] toolbarTabs = new GUIContent[2];
            toolbarTabs[0] = new GUIContent("Content");
            toolbarTabs[1] = new GUIContent("Settings");

            GUILayout.BeginHorizontal();
            GUILayout.Space(17);

            currentTab = GUILayout.Toolbar(currentTab, toolbarTabs, customSkin.FindStyle("Tab Indicator"));

            GUILayout.EndHorizontal();
            GUILayout.Space(-40);
            GUILayout.BeginHorizontal();
            GUILayout.Space(17);

            if (GUILayout.Button(new GUIContent("Content", "Content"), customSkin.FindStyle("Tab Content")))
                currentTab = 0;
            if (GUILayout.Button(new GUIContent("Settings", "Settings"), customSkin.FindStyle("Tab Settings")))
                currentTab = 1;

            GUILayout.EndHorizontal();

            var chartData = serializedObject.FindProperty("chartData");
            var borderThickness = serializedObject.FindProperty("borderThickness");
            var borderColor = serializedObject.FindProperty("borderColor");
            var enableBorderColor = serializedObject.FindProperty("enableBorderColor");
            var addValueToIndicator = serializedObject.FindProperty("addValueToIndicator");
            var indicatorParent = serializedObject.FindProperty("indicatorParent");
            var valuePrefix = serializedObject.FindProperty("valuePrefix");
            var valueSuffix = serializedObject.FindProperty("valueSuffix");

            switch (currentTab)
            {
                case 0:
                    GUILayout.Space(6);
                    GUILayout.Box(new GUIContent(""), customSkin.FindStyle("Content Header"));
                    GUILayout.BeginVertical(EditorStyles.helpBox);
                    EditorGUI.indentLevel = 1;
                  
                    EditorGUILayout.PropertyField(chartData, new GUIContent("Chart Items"));
                    chartData.isExpanded = true;

                    if (GUILayout.Button("+  Add a new item", customSkin.button))
                        pieTarget.AddNewItem();

                    EditorGUI.indentLevel = 0;
                    GUILayout.EndHorizontal();

                    if (pieTarget.gameObject.activeInHierarchy == true)
                        pieTarget.UpdateIndicators();

                    break;

                case 1:
                    GUILayout.Space(6);
                    GUILayout.Box(new GUIContent(""), customSkin.FindStyle("Customization Header"));
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Indicator Parent"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(indicatorParent, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Border Thickness"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(borderThickness, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    addValueToIndicator.boolValue = GUILayout.Toggle(addValueToIndicator.boolValue, new GUIContent("Add Value To Indicator"), customSkin.FindStyle("Toggle"));
                    addValueToIndicator.boolValue = GUILayout.Toggle(addValueToIndicator.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                    GUILayout.EndHorizontal();

                    if (addValueToIndicator.boolValue == true)
                    {
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);

                        EditorGUILayout.LabelField(new GUIContent("Value Prefix:"), customSkin.FindStyle("Text"), GUILayout.Width(75));
                        EditorGUILayout.PropertyField(valuePrefix, new GUIContent(""));

                        GUILayout.Space(10);

                        EditorGUILayout.LabelField(new GUIContent("Value Suffix:"), customSkin.FindStyle("Text"), GUILayout.Width(75));
                        EditorGUILayout.PropertyField(valueSuffix, new GUIContent(""));

                        GUILayout.EndHorizontal();
                    }

                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    enableBorderColor.boolValue = GUILayout.Toggle(enableBorderColor.boolValue, new GUIContent("Enable Border Color (Experimental)"), customSkin.FindStyle("Toggle"));
                    enableBorderColor.boolValue = GUILayout.Toggle(enableBorderColor.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                    GUILayout.EndHorizontal();

                    if (enableBorderColor.boolValue == true)
                    {
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);

                        EditorGUILayout.LabelField(new GUIContent("Border Color"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                        EditorGUILayout.PropertyField(borderColor, new GUIContent(""));

                        GUILayout.EndHorizontal();
                    }

                    GUILayout.Space(10);
                    GUILayout.Box(new GUIContent(""), customSkin.FindStyle("UIM Header"));

                    if (tempUIM != null)
                    {
                        EditorGUILayout.HelpBox("This object is connected with UI Manager. Some parameters (such as colors, " +
                            "fonts or booleans) are managed by the manager.", MessageType.Info);

                        if (GUILayout.Button("Open UI Manager", customSkin.button))
                            EditorApplication.ExecuteMenuItem("Tools/Modern UI Pack/Show UI Manager");

                        if (GUILayout.Button("Disable UI Manager Connection", customSkin.button))
                        {
                            if (EditorUtility.DisplayDialog("Modern UI Pack", "Are you sure you want to disable UI Manager connection with the object? " +
                                "This operation cannot be undone.", "Yes", "Cancel"))
                            {
                                try { DestroyImmediate(tempUIM); }
                                catch { Debug.LogError("<b>[Pie Chart]</b> Failed to delete UI Manager connection.", this); }
                            }
                        }
                    }

                    else if (tempUIM == null)
                        EditorGUILayout.HelpBox("This object does not have any connection with UI Manager.", MessageType.Info);

                    break;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}