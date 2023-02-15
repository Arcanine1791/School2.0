using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;

[System.Serializable]
public class ButtonInfo
{
    public Button btn;
    public string name;
    public Texture2D image;
}

[CreateAssetMenu(fileName = "Panel", menuName = "UIPanels")]
public class ButtonHolder : ScriptableObject
{
    public List<ButtonInfo> subjects;
   
   

}

