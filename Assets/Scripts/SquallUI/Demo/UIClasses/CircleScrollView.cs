using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleScrollView : IView
{
    private UICircularScrollView _circleScrollView;
    protected override void OnInit()
    {
        base.OnInit();
        _circleScrollView = GetChildCompByObj<UICircularScrollView>("Scroll View");
        _circleScrollView.Init(1000, (go, idx) =>
        {
            go.GetComponent<Text>().text = "这是第"+idx.ToString()+"条信息";
        });
    }
}
