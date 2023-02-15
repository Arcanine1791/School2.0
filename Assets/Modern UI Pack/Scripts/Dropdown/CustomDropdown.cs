using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

namespace Michsky.UI.ModernUIPack
{
    [RequireComponent(typeof(Animator))]
    public class CustomDropdown : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler, IPointerClickHandler
    {
        // Resources
        public Animator dropdownAnimator;
        public GameObject triggerObject;
        public TextMeshProUGUI selectedText;
        public Image selectedImage;
        public Transform itemParent;
        public GameObject itemObject;
        public GameObject scrollbar;
        public VerticalLayoutGroup itemList;
        public Transform listParent;
        public AudioSource soundSource;
        [HideInInspector] public Transform currentListParent;

        // Settings
        public bool enableIcon = true;
        public bool enableTrigger = true;
        public bool enableScrollbar = true;
        public bool setHighPriorty = true;
        public bool outOnPointerExit = false;
        public bool isListItem = false;
        public bool invokeAtStart = false;
        public bool enableDropdownSounds = false;
        public bool useHoverSound = true;
        public bool useClickSound = true;
        public AnimationType animationType;
        [Range(1, 50)] public int itemPaddingTop = 8;
        [Range(1, 50)] public int itemPaddingBottom = 8;
        [Range(1, 50)] public int itemPaddingLeft = 8;
        [Range(1, 50)] public int itemPaddingRight = 25;
        [Range(1, 50)] public int itemSpacing = 8;
        public int selectedItemIndex = 0;

        // Saving
        public bool saveSelected = false;
        public string dropdownTag = "Dropdown";

        // Item list
        [SerializeField]
        public List<Item> dropdownItems = new List<Item>();
        [System.Serializable]
        public class DropdownEvent : UnityEvent<int> { }
        [Space(8)] public DropdownEvent dropdownEvent;

        // Audio
        public AudioClip hoverSound;
        public AudioClip clickSound;

        // Other variables
        [HideInInspector] public bool isOn;
        [HideInInspector] public int index = 0;
        [HideInInspector] public int siblingIndex = 0;
        [HideInInspector] public TextMeshProUGUI setItemText;
        [HideInInspector] public Image setItemImage;
        EventTrigger triggerEvent;
        Sprite imageHelper;
        string textHelper;

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
            public Sprite itemIcon;
            public UnityEvent OnItemSelection = new UnityEvent();
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

            if (saveSelected == true)
            {
                if (invokeAtStart == true)
                    dropdownItems[PlayerPrefs.GetInt("Dropdown" + dropdownTag)].OnItemSelection.Invoke();
                else
                    ChangeDropdownInfo(PlayerPrefs.GetInt("Dropdown" + dropdownTag));
            }

            UpdateItemLayout();
        }

        public void SetupDropdown()
        {
            foreach (Transform child in itemParent)
                Destroy(child.gameObject);

            index = 0;

            for (int i = 0; i < dropdownItems.Count; ++i)
            {
                GameObject go = Instantiate(itemObject, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                go.transform.SetParent(itemParent, false);

                setItemText = go.GetComponentInChildren<TextMeshProUGUI>();
                textHelper = dropdownItems[i].itemName;
                setItemText.text = textHelper;

                Transform goImage;
                goImage = go.gameObject.transform.Find("Icon");
                setItemImage = goImage.GetComponent<Image>();
                imageHelper = dropdownItems[i].itemIcon;
                setItemImage.sprite = imageHelper;

                Button itemButton;
                itemButton = go.GetComponent<Button>();

                itemButton.onClick.AddListener(Animate);
                itemButton.onClick.AddListener(delegate
                {
                    ChangeDropdownInfo(index = go.transform.GetSiblingIndex());
                    dropdownEvent.Invoke(index = go.transform.GetSiblingIndex());

                    if (saveSelected == true)
                        PlayerPrefs.SetInt("Dropdown" + dropdownTag, go.transform.GetSiblingIndex());
                });

                if (dropdownItems[i].OnItemSelection != null)
                    itemButton.onClick.AddListener(dropdownItems[i].OnItemSelection.Invoke);

                if (invokeAtStart == true)
                    dropdownItems[i].OnItemSelection.Invoke();
            }

            if (selectedImage != null && enableIcon == false)
                selectedImage.gameObject.SetActive(false);

            try
            {
                selectedText.text = dropdownItems[selectedItemIndex].itemName;
                selectedImage.sprite = dropdownItems[selectedItemIndex].itemIcon;
                currentListParent = transform.parent;
            }

            catch
            {
                selectedText.text = dropdownTag;
                currentListParent = transform.parent;
                Debug.LogWarning("<b>[Dropdown]</b> There is no dropdown items in the list.", this);
            }
        }

        public void ChangeDropdownInfo(int itemIndex)
        {
            if (selectedImage != null && enableIcon == true)
                selectedImage.sprite = dropdownItems[itemIndex].itemIcon;

            if (selectedText != null)
                selectedText.text = dropdownItems[itemIndex].itemName;

            if (enableDropdownSounds == true && useClickSound == true)
                soundSource.PlayOneShot(clickSound);

            selectedItemIndex = itemIndex;
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

        public void CreateNewItem(string title, Sprite icon)
        {
            Item item = new Item();
            item.itemName = title;
            item.itemIcon = icon;
            dropdownItems.Add(item);
            SetupDropdown();
        }

        public void CreateNewItemFast(string title, Sprite icon)
        {
            Item item = new Item();
            item.itemName = title;
            item.itemIcon = icon;
            dropdownItems.Add(item);
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

        public void OnPointerClick(PointerEventData eventData)
        {
            if (enableDropdownSounds == true && useClickSound == true)
                soundSource.PlayOneShot(clickSound);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (enableDropdownSounds == true && useHoverSound == true)
                soundSource.PlayOneShot(hoverSound);
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