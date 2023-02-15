using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Michsky.UI.ModernUIPack
{
    public class WindowManager : MonoBehaviour
    {
        // Content
        public List<WindowItem> windows = new List<WindowItem>();

        // Settings
        public int currentWindowIndex = 0;
        private int currentButtonIndex = 0;
        private int newWindowIndex;
        [HideInInspector] public bool editMode = false;
        bool isFirstTime = true;

        // Events
        [System.Serializable] public class WindowChangeEvent : UnityEvent<int> { }
        [Space(8)] public WindowChangeEvent onWindowChange;

        // Hidden vars
        private GameObject currentWindow;
        private GameObject nextWindow;
        private GameObject currentButton;
        private GameObject nextButton;
        private Animator currentWindowAnimator;
        private Animator nextWindowAnimator;
        private Animator currentButtonAnimator;
        private Animator nextButtonAnimator;

        // Animation states
        string windowFadeIn = "In";
        string windowFadeOut = "Out";
        string buttonFadeIn = "Normal to Pressed";
        string buttonFadeOut = "Pressed to Normal";

        [System.Serializable]
        public class WindowItem
        {
            public string windowName = "My Window";
            public GameObject windowObject;
            public GameObject buttonObject;
        }

        void Start()
        {
            try
            {
                currentButton = windows[currentWindowIndex].buttonObject;
                currentButtonAnimator = currentButton.GetComponent<Animator>();
                currentButtonAnimator.Play(buttonFadeIn);
            }

            catch { }

            currentWindow = windows[currentWindowIndex].windowObject;
            currentWindowAnimator = currentWindow.GetComponent<Animator>();
            currentWindowAnimator.Play(windowFadeIn);
            onWindowChange.Invoke(currentWindowIndex);
            isFirstTime = false;
        }

        void OnEnable()
        {
            if (isFirstTime == false && nextWindowAnimator == null)
            {
                currentWindowAnimator.Play(windowFadeIn);
                currentButtonAnimator.Play(buttonFadeIn);
            }

            else if (isFirstTime == false && nextWindowAnimator != null)
            {
                nextWindowAnimator.Play(windowFadeIn);
                nextButtonAnimator.Play(buttonFadeIn);
            }
        }

        public void OpenFirstTab()
        {
            if (currentWindowIndex != 0)
            {
                currentWindow = windows[currentWindowIndex].windowObject;
                currentWindowAnimator = currentWindow.GetComponent<Animator>();
                currentWindowAnimator.Play(windowFadeOut);

                try
                {
                    currentButton = windows[currentWindowIndex].buttonObject;
                    currentButtonAnimator = currentButton.GetComponent<Animator>();
                    currentButtonAnimator.Play(buttonFadeOut);
                }

                catch { }

                currentWindowIndex = 0;
                currentButtonIndex = 0;
                currentWindow = windows[currentWindowIndex].windowObject;
                currentWindowAnimator = currentWindow.GetComponent<Animator>();
                currentWindowAnimator.Play(windowFadeIn);

                try
                {
                    currentButton = windows[currentButtonIndex].buttonObject;
                    currentButtonAnimator = currentButton.GetComponent<Animator>();
                    currentButtonAnimator.Play(buttonFadeIn);
                }

                catch { }

                onWindowChange.Invoke(currentWindowIndex);
            }

            else if (currentWindowIndex == 0)
            {
                currentWindow = windows[currentWindowIndex].windowObject;
                currentWindowAnimator = currentWindow.GetComponent<Animator>();
                currentWindowAnimator.Play(windowFadeIn);

                try
                {
                    currentButton = windows[currentButtonIndex].buttonObject;
                    currentButtonAnimator = currentButton.GetComponent<Animator>();
                    currentButtonAnimator.Play(buttonFadeIn);
                }

                catch { }
            }
        }

        public void OpenPanel(string newPanel)
        {
            for (int i = 0; i < windows.Count; i++)
            {
                if (windows[i].windowName == newPanel)
                    newWindowIndex = i;
            }

            if (newWindowIndex != currentWindowIndex)
            {
                currentWindow = windows[currentWindowIndex].windowObject;

                try { currentButton = windows[currentWindowIndex].buttonObject; }
                catch { }

                currentWindowIndex = newWindowIndex;
                nextWindow = windows[currentWindowIndex].windowObject;
                currentWindowAnimator = currentWindow.GetComponent<Animator>();
                nextWindowAnimator = nextWindow.GetComponent<Animator>();
                currentWindowAnimator.Play(windowFadeOut);
                nextWindowAnimator.Play(windowFadeIn);

                try
                {
                    currentButtonIndex = newWindowIndex;
                    nextButton = windows[currentButtonIndex].buttonObject;
                    currentButtonAnimator = currentButton.GetComponent<Animator>();
                    nextButtonAnimator = nextButton.GetComponent<Animator>();
                    currentButtonAnimator.Play(buttonFadeOut);
                    nextButtonAnimator.Play(buttonFadeIn);
                }

                catch { }

                onWindowChange.Invoke(currentWindowIndex);
            }
        }

        public void NextPage()
        {
            if (currentWindowIndex <= windows.Count - 2)
            {
                currentWindow = windows[currentWindowIndex].windowObject;

                try
                {
                    currentButton = windows[currentButtonIndex].buttonObject;
                    nextButton = windows[currentButtonIndex + 1].buttonObject;
                    currentButtonAnimator = currentButton.GetComponent<Animator>();
                    currentButtonAnimator.Play(buttonFadeOut);
                }

                catch { }

                currentWindowAnimator = currentWindow.GetComponent<Animator>();
                currentWindowAnimator.Play(windowFadeOut);
                currentWindowIndex += 1;
                currentButtonIndex += 1;
                nextWindow = windows[currentWindowIndex].windowObject;
                nextWindowAnimator = nextWindow.GetComponent<Animator>();
                nextWindowAnimator.Play(windowFadeIn);

                try
                {
                    nextButtonAnimator = nextButton.GetComponent<Animator>();
                    nextButtonAnimator.Play(buttonFadeIn);
                }

                catch { }

                onWindowChange.Invoke(currentWindowIndex);
            }
        }

        public void PrevPage()
        {
            if (currentWindowIndex >= 1)
            {
                currentWindow = windows[currentWindowIndex].windowObject;

                try
                {
                    currentButton = windows[currentButtonIndex].buttonObject;
                    nextButton = windows[currentButtonIndex - 1].buttonObject;
                    currentButtonAnimator = currentButton.GetComponent<Animator>();
                    currentButtonAnimator.Play(buttonFadeOut);
                }

                catch { }

                currentWindowAnimator = currentWindow.GetComponent<Animator>();
                currentWindowAnimator.Play(windowFadeOut);
                currentWindowIndex -= 1;
                currentButtonIndex -= 1;
                nextWindow = windows[currentWindowIndex].windowObject;
                nextWindowAnimator = nextWindow.GetComponent<Animator>();
                nextWindowAnimator.Play(windowFadeIn);

                try
                {
                    nextButtonAnimator = nextButton.GetComponent<Animator>();
                    nextButtonAnimator.Play(buttonFadeIn);
                }

                catch { }

                onWindowChange.Invoke(currentWindowIndex);
            }
        }

        public void ShowCurrentWindow()
        {
            if (nextWindowAnimator == null)
                currentWindowAnimator.Play(windowFadeIn);
            else
                nextWindowAnimator.Play(windowFadeIn);
        }

        public void HideCurrentWindow()
        {
            if (nextWindowAnimator == null)
                currentWindowAnimator.Play(windowFadeOut);
            else
                nextWindowAnimator.Play(windowFadeOut);
        }

        public void ShowCurrentButton()
        {
            if (nextButtonAnimator == null)
                currentButtonAnimator.Play(buttonFadeIn);
            else
                nextButtonAnimator.Play(buttonFadeIn);
        }

        public void HideCurrentButton()
        {
            if (nextButtonAnimator == null)
                currentButtonAnimator.Play(buttonFadeOut);
            else
                nextButtonAnimator.Play(buttonFadeOut);
        }

        public void AddNewItem()
        {
            WindowItem window = new WindowItem();

            if (windows.Count != 0 && windows[windows.Count - 1].windowObject != null)
            {
                int tempIndex = windows.Count - 1;
               
                GameObject tempWindow = windows[tempIndex].windowObject.transform.parent.GetChild(tempIndex).gameObject;
                GameObject newWindow = Instantiate(tempWindow, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
               
                newWindow.transform.SetParent(windows[tempIndex].windowObject.transform.parent, false);
                newWindow.gameObject.name = "New Window " + tempIndex.ToString();
               
                window.windowName = "New Window " + tempIndex.ToString();
                window.windowObject = newWindow;

                if (windows[tempIndex].buttonObject != null)
                {
                    GameObject tempButton = windows[tempIndex].buttonObject.transform.parent.GetChild(tempIndex).gameObject;
                    GameObject newButton = Instantiate(tempButton, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;

                    newButton.transform.SetParent(windows[tempIndex].buttonObject.transform.parent, false);
                    newButton.gameObject.name = "New Window " + tempIndex.ToString();

                    window.buttonObject = newButton;
                }
            }

            windows.Add(window);
        }
    }
}