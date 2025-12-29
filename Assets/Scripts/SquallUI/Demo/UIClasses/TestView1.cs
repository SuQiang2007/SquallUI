using UnityEngine.UI;

public partial class TestView1 : IView
{
    private Button _button;
    protected override void OnInit()
    {
        base.OnInit();
        
        _button = GetChildCompByObj<Button>("ButtonCloseUI");
        _button.onClick.AddListener(OnBtnClicked);

        BindComponent();
        varLabel.text = "This is TextView1 & Label is defiend by bind tool!";
    }

    private void OnBtnClicked()
    { 
        Hide();
    }
}