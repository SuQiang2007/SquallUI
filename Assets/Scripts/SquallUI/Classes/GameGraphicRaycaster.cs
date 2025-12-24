using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 继承GraphicRaycaster
/// 实现引导点击过滤对象
/// </summary>
public class GameGraphicRaycaster : GraphicRaycaster
{
    /// <summary>
    /// 重写射线检测，用于过滤引导系统需要忽略的UI对象
    /// </summary>
    public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
    {
        base.Raycast(eventData, resultAppendList);
        if (resultAppendList.Count > 0)
        {
            // 从后往前遍历，避免删除时索引错乱
            for (int i = resultAppendList.Count - 1; i >= 0; i--)
            {
                var data = resultAppendList[i];
                if (data.gameObject != null)
                {
                    // TODO: 根据实际需求添加过滤条件
                    // 例如：过滤掉带有特定标记、标签或组件的对象
                    // if (ShouldFilterObject(data.gameObject))
                    // {
                    //     resultAppendList.RemoveAt(i);
                    // }
                    
                    // 当前实现：如果 gameObject 不为 null 就删除（这是错误的！）
                    // 这会导致所有UI都无法接收点击事件
                    // 应该根据实际需求修改过滤条件
                    // resultAppendList.RemoveAt(i);  // 已注释，避免误删
                }
            }
        }
    }
    
    /// <summary>
    /// 判断是否应该过滤掉该对象（供子类或外部扩展使用）
    /// </summary>
    protected virtual bool ShouldFilterObject(GameObject obj)
    {
        // 示例：可以根据标签、组件、名称等条件过滤
        // return obj.CompareTag("IgnoreRaycast") || obj.name.Contains("GuideIgnore");
        return false;
    }

    public void SetEnable(bool isEnable)
    {
        this.enabled = isEnable;
    }

    public bool IsEnable()
    {
        return this.enabled;
    }
}