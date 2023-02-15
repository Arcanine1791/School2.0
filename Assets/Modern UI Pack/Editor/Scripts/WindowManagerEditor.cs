using UnityEngine;
using UnityEditor;

namespace Michsky.UI.ModernUIPack
{
    [CustomEditor(typeof(WindowManager))]
    public class WindowManagerEditor : Editor
    {
        private WindowManager wmTarget;
        private UIManagerWindowManager tempUIM;
        private int currentTab;

        private void OnEnable()
        {
            wmTarget = (WindowManager)target;

            try { tempUIM = wmTarget.GetComponent<UIManagerWindowManager>(); }
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

            GUILayout.Box(new GUIContent(""), customSkin.FindStyle("WM Top Header"));

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

            var windows = serializedObject.FindProperty("windows");
            var currentWindowIndex = serializedObject.FindProperty("currentWindowIndex");
            var editMode = serializedObject.FindProperty("editMode");
            var onWindowChange = serializedObject.FindProperty("onWindowChange");

            switch (currentTab)
            {
                case 0:
                    GUILayout.Space(6);
                    GUILayout.Box(new GUIContent(""), customSkin.FindStyle("Content Header"));
                    GUILayout.BeginVertical(EditorStyles.helpBox);
                    EditorGUI.indentLevel = 1;

                    EditorGUILayout.PropertyField(windows, new GUIContent("Window Items"), true);
                    windows.isExpanded = true;

                    if (GUILayout.Button("+  Add a new window", customSkin.button))
                        wmTarget.AddNewItem();

                    GUILayout.EndVertical();
                    GUILayout.Space(10);
                    GUILayout.Box(new GUIContent(""), customSkin.FindStyle("Events Header"));
                    GUILayout.Space(-8);
                    EditorGUILayout.PropertyField(onWindowChange, new GUIContent("On Window Change"), true);
                    break;

                case 1:
                    GUILayout.Space(6);
                    GUILayout.Box(new GUIContent(""), customSkin.FindStyle("Options Header"));
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    editMode.boolValue = GUILayout.Toggle(editMode.boolValue, new GUIContent("Edit Mode"), customSkin.FindStyle("Toggle"));
                    editMode.boolValue = GUILayout.Toggle(editMode.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                    GUILayout.EndHorizontal();

                    if (wmTarget.windows.Count != 0)
                    {
                        GUILayout.BeginVertical(EditorStyles.helpBox);

                        EditorGUILayout.LabelField(new GUIContent("Selected Window:"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                        currentWindowIndex.intValue = EditorGUILayout.IntSlider(currentWindowIndex.intValue, 0, wmTarget.windows.Count - 1);

                        GUILayout.Space(2);
                        EditorGUILayout.LabelField(new GUIContent(wmTarget.windows[currentWindowIndex.intValue].windowName), customSkin.FindStyle("Text"));

                        if (editMode.boolValue == true)
                        {
                            EditorGUILayout.HelpBox("While in edit mode, you can change the visibility of windows by changing the selected window.", MessageType.Info);

                            for (int i = 0; i < wmTarget.windows.Count; i++)
                            {
                                if (i == currentWindowIndex.intValue)
                                    wmTarget.windows[currentWindowIndex.intValue].windowObject.GetComponent<CanvasGroup>().alpha = 1;
                                else
                                    wmTarget.windows[i].windowObject.GetComponent<CanvasGroup>().alpha = 0;
                            }
                        }

                        GUILayout.EndVertical();
                    }

                    else
                        EditorGUILayout.HelpBox("Window List is empty. Create a new item to see more options.", MessageType.Info);

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
                                catch { Debug.LogError("<b>[Window Manager]</b> Failed to delete UI Manager connection.", this); }
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