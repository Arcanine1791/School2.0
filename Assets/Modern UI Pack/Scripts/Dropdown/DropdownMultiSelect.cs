using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

namespace Michsky.UI.ModernUIPack
{
    public class DropdownMultiSelect : MonoBehaviour, IPointerExitHandler
    {
        // Resources
        public GameObject triggerObject;
        public Transform itemParent;
        public GameObject itemObject;
        public GameObject scrollbar;
        private VerticalLayoutGroup itemList;
        private Transform currentListParent;
        public Transform listParent;
        private Animator dropdownAnimator;
        public TextMeshProUGUI setItemText;

        // Settings
        public bool enableIcon = true;
        public bool enableTrigger = true;
        public bool enableScrollbar = true;
        public bool setHighPriorty = true;
        public bool outOnPointerExit = false;
        public bool isListItem = false;
        public AnimationType animationType;
        [Range(1, 50)] public int itemPaddingTop = 8;
        [Range(1, 50)] public int itemPaddingBottom = 8;
        [Range(1, 50)] public int itemPaddingLeft = 8;
        [Range(1, 50)] public int itemPaddingRight = 25;
        [Range(1, 50)] public int itemSpacing = 8;

        // Saving
        public bool saveSelected = false;
        public bool invokeAtStart = false;
        public string toggleTag = "Multi Dropdown";

        // Items
        [SerializeField]
        public List<Item> dropdownItems = new List<Item>();

        // Other variables
        string textHelper;
        string newItemTitle;
        bool isOn;
        public int iHelper = 0;
        public int siblingIndex = 0;
        EventTrigger triggerEvent;

        [System.Serializable]
        public class ToggleEvent : UnityEvent<bool> { }

        public enum AnimationType
        {
            FADING,
            SLIDING,
            STYLISH
        }

        [System.Serializable]
        public class Item
        {
            public string itemName = "Dropdown Item";
            public bool isOn = false;
            [SerializeField] public ToggleEvent onValueChanged = new ToggleEvent();
        }

        void Start()
        {
            try
            {
                if (itemList == null)
                    itemList = itemParent.GetComponent<VerticalLayoutGroup>();

                if (dropdownItems.Count != 0)
                    SetupDropdown();

                currentListParent = transform.parent;

                if (enableTrigger == true && triggerObject != null)
                {
                    // triggerButton = gameObject.GetComponent<Button>();
                    triggerEvent = triggerObject.AddComponent<EventTrigger>();
                    EventTrigger.Entry entry = new EventTrigger.Entry();
                    entry.eventID = EventTriggerType.PointerClick;
                    entry.callback.AddListener((eventData) => { Animate(); });
                    triggerEvent.GetComponent<EventTrigger>().triggers.Add(entry);
                }
            }

            catch { Debug.LogError("<b>[Dropdown]</b> Cannot initalize the object due to missing resources.", this); return; }

            if (dropdownAnimator == null)
                dropdownAnimator = gameObject.GetComponent<Animator>();

            if (enableScrollbar == false && scrollbar != null)
                Destroy(scrollbar);

            if (setHighPriorty == true)
                transform.SetAsLastSibling();

            UpdateItemLayout();
        }

        public void SetupDropdown()
        {
            foreach (Transform child in itemParent)
                Destroy(child.gameObject);

            for (int i = 0; i < dropdownItems.Count; ++i)
            {
                GameObject go = Instantiate(itemObject, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                go.transform.SetParent(itemParent, false);

                setItemText = go.GetComponentInChildren<TextMeshProUGUI>();
                textHelper = dropdownItems[i].itemName;
                setItemText.text = textHelper;

                Toggle itemToggle;
                itemToggle = go.GetComponent<Toggle>();

                iHelper = i;

                itemToggle.onValueChanged.AddListener(delegate { UpdateToggle(go.transform.GetSiblingIndex()); });
              //  ChangeDropdownInfo(index = go.transform.GetSiblingIndex());

                if (dropdownItems[i].onValueChanged != null)
                    itemToggle.onValueChanged.AddListener(dropdownItems[i].onValueChanged.Invoke);

                if (saveSelected == true)
                {
                    if (invokeAtStart == true)
                    {
                        if (PlayerPrefs.GetInt("DropdownMS" + toggleTag) == 1)
                            dropdownItems[i].onValueChanged.Invoke(true);
                        else
                            dropdownItems[i].onValueChanged.Invoke(false);
                    }

                    else
                        itemToggle.onValueChanged.AddListener(SaveToggleData);
                }

                else
                {
                    if (invokeAtStart == true)
                    {
                        if (dropdownItems[i].isOn == true)
                            dropdownItems[i].onValueChanged.Invoke(true);
                        else
                            dropdownItems[i].onValueChanged.Invoke(false);
                    }

                    else
                    {
                        if (dropdownItems[i].isOn == true)
                            itemToggle.isOn = true;
                        else
                            itemToggle.isOn = false;
                    }
                }

                if (invokeAtStart == true)
                {
                    if (dropdownItems[i].isOn == true)
                        dropdownItems[i].onValueChanged.Invoke(true);
                    else
                        dropdownItems[i].onValueChanged.Invoke(false);
                }
            }

            currentListParent = transform.parent;
        }

        void UpdateToggle(int itemIndex)
        {
            if (dropdownItems[itemIndex].isOn == true)
                dropdownItems[itemIndex].isOn = false;
            else
                dropdownItems[itemIndex].isOn = true;
        }

        void SaveToggleData(bool isOn)
        {
            if (isOn == true)
                PlayerPrefs.SetInt("DropdownMS" + toggleTag + iHelper, 1);
            else
                PlayerPrefs.SetInt("DropdownMS" + toggleTag + iHelper, 0);
        }

        public void Animate()
        {
            if (isOn == false && animationType == AnimationType.FADING)
            {
                dropdownAnimator.Play("Fading In");
                isOn = true;

                if (isListItem == true)
                {
                    siblingIndex = transform.GetSiblingIndex();
                    gameObject.transform.SetParent(listParent, true);
                }
            }

            else if (isOn == true && animationType == AnimationType.FADING)
            {
                dropdownAnimator.Play("Fading Out");
                isOn = false;

                if (isListItem == true)
                {
                    gameObject.transform.SetParent(currentListParent, true);
                    gameObject.transform.SetSiblingIndex(siblingIndex);
                }
            }

            else if (isOn == false && animationType == AnimationType.SLIDING)
            {
                dropdownAnimator.Play("Sliding In");
                isOn = true;

                if (isListItem == true)
                {
                    siblingIndex = transform.GetSiblingIndex();
                    gameObject.transform.SetParent(listParent, true);
                }
            }

            else if (isOn == true && animationType == AnimationType.SLIDING)
            {
                dropdownAnimator.Play("Sliding Out");
                isOn = false;

                if (isListItem == true)
                {
                    gameObject.transform.SetParent(currentListParent, true);
                    gameObject.transform.SetSiblingIndex(siblingIndex);
                }
            }

            else if (isOn == false && animationType == AnimationType.STYLISH)
            {
                dropdownAnimator.Play("Stylish In");
                isOn = true;

                if (isListItem == true)
                {
                    siblingIndex = transform.GetSiblingIndex();
                    gameObject.transform.SetParent(listParent, true);
                }
            }

            else if (isOn == true && animationType == AnimationType.STYLISH)
            {
                dropdownAnimator.Play("Stylish Out");
                isOn = false;
                if (isListItem == true)
                {
                    gameObject.transform.SetParent(currentListParent, true);
                    gameObject.transform.SetSiblingIndex(siblingIndex);
                }
            }

            if (setHighPriorty == true)
                transform.SetAsLastSibling();

            if (enableTrigger == true && isOn == false)
                triggerObject.SetActive(false);
            else if (enableTrigger == true && isOn == true)
                triggerObject.SetActive(true);

            if (enableTrigger == true && outOnPointerExit == true)
                triggerObject.SetActive(false);
        }

        public void CreateNewItem()
        {
            Item item = new Item();
            item.itemName = newItemTitle;
            dropdownItems.Add(item);
            SetupDropdown();
        }

        public void RemoveItem(string itemTitle)
        {
            var item = dropdownItems.Find(x => x.itemName == itemTitle);
            dropdownItems.Remove(item);
            SetupDropdown();
        }

        public void AddNewItem()
        {
            Item item = new Item();
            dropdownItems.Add(item);
        }

        public void UpdateItemLayout()
        {
            if (itemList != null)
            {
                itemList.spacing = itemSpacing;
                itemList.padding.top = itemPaddingTop;
                itemList.padding.bottom = itemPaddingBottom;
                itemList.padding.left = itemPaddingLeft;
                itemList.padding.right = itemPaddingRight;
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (outOnPointerExit == true && isOn == true)
            {
                Animate();
                isOn = false;

                if (isListItem == true)
                    gameObject.transform.SetParent(currentListParent, true);
            }
        }
    }
}