using UnityEngine;
using UnityEditor;

public class MenuManager : MonoBehaviour
{
    public ButtonHolder panels;

    private void OnEnable()
    {
        foreach(ButtonInfo xyz in panels.subjects)
        {
            xyz.btn.onClick.AddListener(() => CheckWhichClicked(xyz.btn.name));
        }
    }
    public void CheckWhichClicked(string name)
    {
        Debug.Log(name);
        switch (name)
        {
            case "Physics":
                Debug.Log("Physics");
                break;
            
            case "Literature":
                Debug.Log("Literature");
                break;
            
            case "Chemistry":
                Debug.Log("Chemistry");
                break;
            
            case "Quit":
                Application.Quit();
#if UNITY_EDITOR
                EditorApplication.Exit(0);
#endif
                break;
            case "BacktoMenu":
                
                break;

            default:
                Debug.Log("Invalid Input");
                break;

        }
    }
}
