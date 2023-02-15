using UnityEngine;
using UnityEditor;

namespace Michsky.UI.ModernUIPack
{
    [CustomEditor(typeof(HorizontalSelector))]
    public class HorizontalSelectorEditor : Editor
    {
        private HorizontalSelector hsTarget;
        private UIManagerHSelector tempUIM;
        private int currentTab;

        private void OnEnable()
        {
            hsTarget = (HorizontalSelector)target;

            try { tempUIM = hsTarget.GetComponent<UIManagerHSelector>(); }
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

            GUILayout.Box(new GUIContent(""), customSkin.FindStyle("HS Top Header"));

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

            var itemList = serializedObject.FindProperty("itemList");
            var onValueChanged = serializedObject.FindProperty("onValueChanged");
            var label = serializedObject.FindProperty("label");
            var selectorAnimator = serializedObject.FindProperty("selectorAnimator");
            var labelHelper = serializedObject.FindProperty("labelHelper");
            var labelIcon = serializedObject.FindProperty("labelIcon");
            var labelIconHelper = serializedObject.FindProperty("labelIconHelper");
            var indicatorParent = serializedObject.FindProperty("indicatorParent");
            var indicatorObject = serializedObject.FindProperty("indicatorObject");
            var enableIcon = serializedObject.FindProperty("enableIcon");
            var saveValue = serializedObject.FindProperty("saveValue");
            var selectorTag = serializedObject.FindProperty("selectorTag");
            var enableIndicators = serializedObject.FindProperty("enableIndicators");
            var invokeAtStart = serializedObject.FindProperty("invokeAtStart");
            var invertAnimation = serializedObject.FindProperty("invertAnimation");
            var loopSelection = serializedObject.FindProperty("loopSelection");
            var defaultIndex = serializedObject.FindProperty("defaultIndex");
            var iconScale = serializedObject.FindProperty("iconScale");
            var contentSpacing = serializedObject.FindProperty("contentSpacing");
            var contentLayout = serializedObject.FindProperty("contentLayout");
            var contentLayoutHelper = serializedObject.FindProperty("contentLayoutHelper");
            var enableUIManager = serializedObject.FindProperty("enableUIManager");

            switch (currentTab)
            {
                case 0:
                    GUILayout.Space(6);
                    GUILayout.Box(new GUIContent(""), customSkin.FindStyle("Content Header"));

                    if (hsTarget.itemList.Count != 0)
                    {
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);

                        EditorGUILayout.LabelField(new GUIContent("Default Item Index"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                        defaultIndex.intValue = EditorGUILayout.IntSlider(defaultIndex.intValue, 0, hsTarget.itemList.Count - 1);

                        GUILayout.Space(2);
                        GUILayout.EndHorizontal();
                        // EditorGUILayout.LabelField(new GUIContent(hsTarget.itemList[defaultIndex.intValue].itemTitle), customSkin.FindStyle("Text"));
                    }

                    else
                        EditorGUILayout.HelpBox("There is no item in the list.", MessageType.Warning);

                    if (Application.isPlaying == true)
                    {
                        GUI.enabled = false;
                        GUILayout.BeginHorizontal(EditorStyles.helpBox);

                        EditorGUILayout.LabelField(new GUIContent("Current Index"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                        EditorGUILayout.IntSlider(hsTarget.index, 0, hsTarget.itemList.Count - 1);

                        GUILayout.Space(2);
                        GUILayout.EndHorizontal();
                        GUI.enabled = true;
                    }

                    GUILayout.BeginVertical(EditorStyles.helpBox);
                    EditorGUI.indentLevel = 1;

                    EditorGUILayout.PropertyField(itemList, new GUIContent("Selector Items"), true);
                    itemList.isExpanded = true;

                    EditorGUI.indentLevel = 1;
                    if (GUILayout.Button("+  Add a new item", customSkin.button))
                        hsTarget.AddNewItem();
                   
                    GUILayout.EndVertical();
                    GUILayout.Space(10);
                    GUILayout.Box(new GUIContent(""), customSkin.FindStyle("Events Header"));
                    GUILayout.Space(-8);
                    EditorGUILayout.PropertyField(onValueChanged, new GUIContent("On Value Changed"), true);
                    break;

                case 1:
                    GUILayout.Space(6);
                    GUILayout.Box(new GUIContent(""), customSkin.FindStyle("Core Header"));
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Animator"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(selectorAnimator, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Label"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(label, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Label Helper"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(labelHelper, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Label Icon"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(labelIcon, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Label Icon Helper"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(labelIconHelper, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Indicator Parent"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(indicatorParent, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Indicator Object"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(indicatorObject, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Content Layout"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(contentLayout, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Content Layout Helper"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(contentLayoutHelper, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    break;

                case 2:
                    GUILayout.Space(6);
                    GUILayout.Box(new GUIContent(""), customSkin.FindStyle("Customization Header"));
                    GUILayout.BeginVertical(EditorStyles.helpBox);
                    GUILayout.Space(-3);
                    GUILayout.BeginHorizontal();

                    enableIcon.boolValue = GUILayout.Toggle(enableIcon.boolValue, new GUIContent("Enable Icon"), customSkin.FindStyle("Toggle"));
                    enableIcon.boolValue = GUILayout.Toggle(enableIcon.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                    GUILayout.EndHorizontal();
                    GUILayout.Space(3);

                    if (enableIcon.boolValue == true && hsTarget.labelIcon == null)
                        EditorGUILayout.HelpBox("'Enable Icon' is enabled but 'Label Icon' is not assigned. Go to Resources tab and assign the correct variable.", MessageType.Error);
                    else if (enableIcon.boolValue == true && hsTarget.labelIcon != null)
                        hsTarget.labelIcon.gameObject.SetActive(true);
                    else if (enableIcon.boolValue == false && hsTarget.labelIcon != null)
                        hsTarget.labelIcon.gameObject.SetActive(false);

                    GUILayout.EndVertical();
                    GUILayout.BeginVertical(EditorStyles.helpBox);
                    GUILayout.Space(-3);
                    GUILayout.BeginHorizontal();

                    enableIndicators.boolValue = GUILayout.Toggle(enableIndicators.boolValue, new GUIContent("Enable Indicators"), customSkin.FindStyle("Toggle"));
                    enableIndicators.boolValue = GUILayout.Toggle(enableIndicators.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                    GUILayout.EndHorizontal();
                    GUILayout.Space(3);
                    GUILayout.BeginHorizontal();

                    if (enableIndicators.boolValue == true)
                    {
                        if (hsTarget.indicatorObject == null)
                            EditorGUILayout.HelpBox("'Enable Indicators' is enabled but 'Indicator Object' is not assigned. Go to Resources tab and assign the correct variable.", MessageType.Error);

                        if (hsTarget.indicatorParent == null)
                            EditorGUILayout.HelpBox("'Enable Indicators' is enabled but 'Indicator Parent' is not assigned. Go to Resources tab and assign the correct variable.", MessageType.Error);
                        else
                            hsTarget.indicatorParent.gameObject.SetActive(true);
                    }

                    else if (enableIndicators.boolValue == false && hsTarget.indicatorParent != null)
                        hsTarget.indicatorParent.gameObject.SetActive(false);

                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Icon Scale"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(iconScale, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    EditorGUILayout.LabelField(new GUIContent("Content Spacing"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                    EditorGUILayout.PropertyField(contentSpacing, new GUIContent(""));

                    GUILayout.EndHorizontal();
                    hsTarget.UpdateContentLayout();
                    GUILayout.Space(10);
                    GUILayout.Box(new GUIContent(""), customSkin.FindStyle("Options Header"));

                    if (tempUIM != null)
                        GUI.enabled = false;

                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    invertAnimation.boolValue = GUILayout.Toggle(invertAnimation.boolValue, new GUIContent("Invert Animation"), customSkin.FindStyle("Toggle"));
                    invertAnimation.boolValue = GUILayout.Toggle(invertAnimation.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    loopSelection.boolValue = GUILayout.Toggle(loopSelection.boolValue, new GUIContent("Loop Selection"), customSkin.FindStyle("Toggle"));
                    loopSelection.boolValue = GUILayout.Toggle(loopSelection.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                    GUILayout.EndHorizontal();
                    GUI.enabled = true;
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);

                    invokeAtStart.boolValue = GUILayout.Toggle(invokeAtStart.boolValue, new GUIContent("Invoke At Start"), customSkin.FindStyle("Toggle"));
                    invokeAtStart.boolValue = GUILayout.Toggle(invokeAtStart.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                    GUILayout.EndHorizontal();
                    GUILayout.BeginVertical(EditorStyles.helpBox);
                    GUILayout.Space(-3);
                    GUILayout.BeginHorizontal();

                    saveValue.boolValue = GUILayout.Toggle(saveValue.boolValue, new GUIContent("Save Selection"), customSkin.FindStyle("Toggle"));
                    saveValue.boolValue = GUILayout.Toggle(saveValue.boolValue, new GUIContent(""), customSkin.FindStyle("Toggle Helper"));

                    GUILayout.EndHorizontal();
                    GUILayout.Space(3);

                    if (saveValue.boolValue == true)
                    {
                        GUILayout.BeginHorizontal();                 

                        EditorGUILayout.LabelField(new GUIContent("Selector Tag:"), customSkin.FindStyle("Text"), GUILayout.Width(90));
                        EditorGUILayout.PropertyField(selectorTag, new GUIContent(""));

                        GUILayout.EndHorizontal();
                        EditorGUILayout.HelpBox("Each selector should has its own unique tag.", MessageType.Info);
                    }

                    GUILayout.EndVertical();
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
                                catch { Debug.LogError("<b>[Horizontal Selector]</b> Failed to delete UI Manager connection.", this); }
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