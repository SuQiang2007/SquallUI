using UnityEngine;

public class WidgetsTestCases : MonoBehaviour
{
    [Header("UI位置")]
    [SerializeField] private Rect areaRect = new Rect(10, 10, 260, 120);

    [Header("按钮文案")]
    [SerializeField] private string buttonText = "Run Widget Test";

    private void OnGUI()
    {
        GUILayout.BeginArea(areaRect, GUI.skin.box);
        GUILayout.Label("Widgets TestCases");

        if (GUILayout.Button(buttonText, GUILayout.Height(40)))
        {
            OnClickTestButton();
        }

        GUILayout.EndArea();
    }

    /// <summary>
    /// 点击按钮后的逻辑你自己写
    /// </summary>
    private void OnClickTestButton()
    {
        SquallUIMgr.Instance.ShowView("CircleScrollView");
    }
}
