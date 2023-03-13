using UnityEngine;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
    public ButtonHolder buttons;

    private void Start()
    {
     
        foreach (ButtonInfo xyz in buttons.subjects)
        {
            Button jar = Instantiate(xyz.btn,transform);
            jar.GetComponentInChildren<RawImage>().texture = xyz.image;
            jar.GetComponentInChildren<RawImage>().color = Color.white;
            jar.GetComponentInChildren<Text>().text = xyz.name;
        }
    }
  
}
