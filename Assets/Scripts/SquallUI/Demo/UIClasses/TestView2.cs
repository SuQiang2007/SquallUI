using UnityEngine.UI;

public class TestView2 : IView
{
    private Button _button;
    protected override void OnInit()
    {
        base.OnInit();
        
        _button = GetChildCompByObj<Button>("ButtonCloseUI");
        _button.onClick.AddListener(OnBtnClicked);
    }

    private void OnBtnClicked()
    { 
        Hide();
    }
}