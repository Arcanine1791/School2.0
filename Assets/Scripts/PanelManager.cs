using UnityEngine;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
    public ButtonHolder buttons;

    private void Start()
    {
     
        foreach (ButtonInfo xyz in buttons.subjects)
        {
            Instantiate(xyz.btn,transform);
            xyz.btn.GetComponentInChildren<RawImage>().texture = xyz.image;
            xyz.btn.GetComponentInChildren<RawImage>().color = Color.white;
            xyz.btn.GetComponentInChildren<Text>().text = xyz.name;
        }
    }
  
}
