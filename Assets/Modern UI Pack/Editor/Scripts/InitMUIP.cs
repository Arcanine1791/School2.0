using UnityEditor;

namespace Michsky.UI.ModernUIPack
{
    public class InitMUIP
    {
        [InitializeOnLoad]
        public class InitOnLoad
        {
            static InitOnLoad()
            {
                if (!EditorPrefs.HasKey("MUIPv5.Installed"))
                {
                    EditorPrefs.SetInt("MUIPv5.Installed", 1);
                    EditorUtility.DisplayDialog("Hello there!", "Thank you for purchasing Modern UI Pack." +
                        "\r\rFirst of all, import/update TextMesh Pro () from Package Manager if you haven't already." +
                        "\r\rTo use the UI Manager, use Tools > Modern UI Pack > Show UI Manager." +
                        "\r\rIf you need help, feel free to contact us through our support channels or Discord.", "Got it!");
                }

                if (!EditorPrefs.HasKey("MUIP.ObjectCreator.Upgraded"))
                {
                    EditorPrefs.SetInt("MUIP.ObjectCreator.Upgraded", 1);
                    EditorPrefs.SetString("UIManager.RootFolder", "Modern UI Pack/Prefabs/");
                }
            }
        }
    }
}