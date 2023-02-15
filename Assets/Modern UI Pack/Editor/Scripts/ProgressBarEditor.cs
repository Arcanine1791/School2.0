using UnityEngine;
using UnityEditor;

namespace Michsky.UI.ModernUIPack
{
    [CustomEditor(typeof(ProgressBar))]
    public class ProgressBarEditor : Editor
    {
        private ProgressBar pbTarget;
        private UIManagerProgressBar tempUIM;
        private UIManagerProgressBarLoop tempFilledUIM;
        private int currentTab;

        private void OnEnable()
        {
            pbTarget = (ProgressBar)target;

            try
            {
                if (pbTarget.isFilled == false) { tempUIM = pbTarget.GetComponent<UIManagerProgressBar>(); }
                else { tempFilledUIM = pbTarget.GetComponent<UIManagerProgressBarLoop>(); }
            }

            catch { }
        }

        public override void OnInspectorGUI()
        {
            GUISkin customSkin;
            Color defaultColor = GUI.color;

            if (EditorGUIUtility.isProSkin == true) { customSkin = (GUISkin)Resources.Load("Editor\\MUI Skin Dark"); }
            else { customSkin = (GUISkin)Resources.Load("Editor\\MUI Skin Light"); }

            GUILayout.BeginHorizontal();
            GUI.backgroundColor = defaultColor;

            GUILayout.Box(new GUIContent(""), customSkin.FindStyle("PB Top Header"));

            GUILayout.EndHorizontal();
            GUILayout.Space(-42);

            GUIContent[] toolbarTabs = new GUIContent[3];
            toolbarTabs[0] = new GUIContent("Content");
            toolbarTabs[1] = new GUIContent("Resources");
            toolbarTabs[2] = new GUIContent("Settings");

            GUILayout.BeginHorizontal();
            GUILayout.Space(17);

            currentTab = GUILayout.Toolbar(currentTab, toolbarTabs, customSkin.FindStyle("Tab Indicator"));

            GUILayout.EndHorizontal();
            GUILayout.Space(-40);
            GUILayout.BeginHorizontal();
            GUILayout.Space(17);

            if (GUILayout.Button(new GUIContent("Content", "Content"), customSkin.FindStyle("Tab Content")))
                currentTab = 0;
            if (GUILayout.Button(new GUIContent("Resources", "Resources"), customSkin.FindStyle("Tab Resources")))
                currentTab = 1;
            if (GUILayout.Button(new GUIContent("Settings", "Settings"), customSkin.FindStyle("Tab Settings")))
                currentTab = 2;

            GUILayout.EndHorizontal();

            var currentPercent = serializedObject.FindProperty("currentPercent");
            var speed = serializedObject.FindProperty("speed");
            var minValue = serializedObject.FindProperty("minValue");
            var maxValue = serializedObject.FindProperty("maxValue");
            var valueLimit = serializedObject.FindProperty("valueLimit");
            var loadingBar = serializedObject.FindProperty("loadingBar");
            var textPercent = serializedObject.FindProperty("textPercent");
            var isOn = serializedObject.FindProperty("isOn");
            var restart = serializedObject.FindProperty("restart");
            var invert = serializedObject.FindProperty("invert");
            var addPrefix = serializedObject.FindProperty("addPrefix");
            var addSuffix = serializedObject.FindProperty("addSuffix");
            var prefix = serializedObject.FindProperty("prefix");
            var suffix = serializedObject.FindProperty("suffix");
            var decimals = serializedObject.FindProperty("decimals");
            var onValueChanged = serializedObject.FindProperty("onValueChanged");

            switch (currentTab)
            {
                case 0:
                    GUILayout.Space(6);
                    GUILayout.Box(new GUIContent(""), customSkin.FindStyle("Content Header"));
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Current Percent"), customSkin.FindStyle("Text"), GUILayout.Width(100));
                    pbTarget.currentPercent = EditorGUILayout.Slider(pbTarget.currentPercent, minValue.floatValue, maxValue.floatValue);
                    currentPercent.floatValue = pbTarget.currentPercent;
                 
                    GUILayout.EndHorizontal();

                    if (pbTarget.loadingBar != null && pbTarget.textPercent != null) { pbTarget.UpdateUI(); }
                    else
                    {
                        if (pbTarget.loadingBar == null || pbTarget.textPercent == null)
                        {
                            GUILayout.BeginHorizontal();
                            EditorGUILayout.HelpBox("One or more resources needs to be assigned.", MessageType.Error);
                            GUILayout.EndHorizontal();
                        }
                    }

                    GUILayout.BeginVertical(EditorStyles.helpBox);
                    EditorGUILayout.LabelField(new GUIContent("Min / Max Value"), customSkin.FindStyle("Text"), GUILayout.Width(110));
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(2);

                    minValue.floatValue = EditorGUILayout.Slider(minValue.floatValue, 0, maxValue.floatValue - 1);
                    maxValue.floatValue = EditorGUILayout.Slider(maxValue.floatValue, minValue.floatValue + 1, valueLimit.floatValue);

                    GUILayout.EndHorizontal();
                    GUILayout.Space(2);
                    EditorGUILayout.HelpBox("You can increase the max value limit by changing 'Value Limit' in the settings tab.", MessageType.Info);
                    GUILayout.EndVertical();
                   
                    GUILayout.Space(10);
                    GUILayout.Box(new GUIContent(""), customSkin.FindStyle("Events Header"));
                    EditorGUILayout.PropertyField(onValueChanged, new GUIContent("On Value Changed"));
                    break;

                case 1:
                    GUILayout.Space(6);
                    GUILayout.Box(new GUIContent(""), customSkin.FindStyle("Core Header"));
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Loading Bar"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(loadingBar, new GUIContent(""));

                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Text Indicator"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(textPercent, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    break;

                case 2:
                    GUILayout.Space(6);
                    GUILayout.Box(new GUIContent(""), customSkin.FindStyle("Options Header"));
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    isOn.boolValue = GUILayout.Toggle(isOn.boolValue, new GUIContent("Is On"), customSkin.FindStyle("Toggle"));
                    isOn.boolValue = GUILayout.Toggle(isOn.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                    GUILayout.EndHorizontal();                 
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    restart.boolValue = GUILayout.Toggle(restart.boolValue, new GUIContent("Restart / Loop"), customSkin.FindStyle("Toggle"));
                    restart.boolValue = GUILayout.Toggle(restart.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    invert.boolValue = GUILayout.Toggle(invert.boolValue, new GUIContent("Invert"), customSkin.FindStyle("Toggle"));
                    invert.boolValue = GUILayout.Toggle(invert.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginVertical(EditorStyles.helpBox);
                    GUILayout.Space(-3);
                    GUILayout.BeginHorizontal();

                    addPrefix.boolValue = GUILayout.Toggle(addPrefix.boolValue, new GUIContent("Add Prefix"), customSkin.FindStyle("Toggle"));
                    addPrefix.boolValue = GUILayout.Toggle(addPrefix.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                    GUILayout.EndHorizontal();
                    GUILayout.Space(3);

                    if (addPrefix.boolValue == true)
                    {
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(new GUIContent("Prefix:"), customSkin.FindStyle("Text"), GUILayout.Width(40));
                        EditorGUILayout.PropertyField(prefix, new GUIContent(""));
                        GUILayout.EndHorizontal();
                    }

                    GUILayout.EndVertical();
                    GUILayout.BeginVertical(EditorStyles.helpBox);
                    GUILayout.Space(-3);
                    GUILayout.BeginHorizontal();

                    addSuffix.boolValue = GUILayout.Toggle(addSuffix.boolValue, new GUIContent("Add Suffix"), customSkin.FindStyle("Toggle"));
                    addSuffix.boolValue = GUILayout.Toggle(addSuffix.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                    GUILayout.EndHorizontal();
                    GUILayout.Space(3);

                    if (addSuffix.boolValue == true) 
                    {
                        GUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(new GUIContent("Suffix:"), customSkin.FindStyle("Text"), GUILayout.Width(40));
                        EditorGUILayout.PropertyField(suffix, new GUIContent(""));
                        GUILayout.EndHorizontal();
                    }

                    GUILayout.EndVertical();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Value Limit"), customSkin.FindStyle("Text"), GUILayout.Width(80));
                    EditorGUILayout.PropertyField(valueLimit, new GUIContent(""));
                    if (valueLimit.floatValue <= minValue.floatValue) { valueLimit.floatValue = minValue.floatValue + 1; }

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Decimals"), customSkin.FindStyle("Text"), GUILayout.Width(80));
                    EditorGUILayout.PropertyField(decimals, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Speed"), customSkin.FindStyle("Text"), GUILayout.Width(80));
                    EditorGUILayout.PropertyField(speed, new GUIContent(""));

                    GUILayout.EndHorizontal();

                    GUILayout.Space(10);
                    GUILayout.Box(new GUIContent(""), customSkin.FindStyle("UIM Header"));

                    if (tempUIM != null && pbTarget.isFilled == false)
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
                                catch { Debug.LogError("<b>[Progress Bar]</b> Failed to delete UI Manager connection.", this); }
                            }
                        }
                    }

                    else if (tempFilledUIM != null && pbTarget.isFilled == true)
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
                                catch { Debug.LogError("<b>[Progress Bar]</b> Failed to delete UI Manager connection.", this); }
                            }
                        }
                    }

                    else if (tempUIM == null && tempFilledUIM == null)
                        EditorGUILayout.HelpBox("This object does not have any connection with UI Manager.", MessageType.Info);

                    break;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}