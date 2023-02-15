using Evereal.VideoCapture;
using UnityEngine;
using UnityEngine.UI;

public class VideoRec : MonoBehaviour
{
   public VideoCapture videoCapture;
    public Button startRec, stopRec;

    private void Start()
    {
        startRec.onClick.AddListener(()=> RecFunction("Start"));
        stopRec.onClick.AddListener(() => RecFunction("Stop"));
    }

    public void RecFunction(string callingBtn)
    {
        if(callingBtn == "Start")
        {
            videoCapture.StartCapture();
        }
        if(callingBtn == "Stop")
        {
            videoCapture.StopCapture();
        }
        else
        {
            Debug.Log("LOL");
        }
    }
}
