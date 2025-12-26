using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SquallUI
{
    public class IViewDeleter
    {
        /// <summary>
        /// 删除UI
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="isDestroy"></param>
        public static void DestroyUIObj(GameObject obj, bool isDestroy = true)
        {
            if (obj == null)
                return;

            var list = obj.GetComponentsInChildren<UIBehaviour>(true);
            foreach (var component in list)
            {
                switch (component)
                {
                    case Button btn:
                        btn.onClick?.RemoveAllListeners();
                        // btn.onClick = null;
                        break;
                    case Toggle tog:
                        tog.onValueChanged?.RemoveAllListeners();
                        // tog.onValueChanged = null;
                        break;
                    case Slider slider:
                        slider.onValueChanged?.RemoveAllListeners();
                        // slider.onValueChanged = null;
                        break;
                    case Dropdown dropdown:
                        dropdown.onValueChanged?.RemoveAllListeners();
                        // dropdown.onValueChanged = null;

                        if (isDestroy)
                        {
                            dropdown.image = null;
                            if (dropdown.transition == Selectable.Transition.SpriteSwap)
                            {
                                dropdown.spriteState = new SpriteState();
                            }
                        }

                        break;
                    case TMP_Dropdown tmpDropdown:
                        tmpDropdown.onValueChanged?.RemoveAllListeners();
                        // tmpDropdown.onValueChanged = null;

                        if (isDestroy)
                        {
                            tmpDropdown.image = null;
                            if (tmpDropdown.transition == Selectable.Transition.SpriteSwap)
                            {
                                tmpDropdown.spriteState = new SpriteState();
                            }
                        }
                        break;
                    case TMP_InputField tmpInput:
                        tmpInput.onValueChanged?.RemoveAllListeners();
                        // tmpInput.onValueChanged = null;
                        tmpInput.onEndEdit?.RemoveAllListeners();
                        // tmpInput.onEndEdit = null;
                        tmpInput.onSubmit?.RemoveAllListeners();
                        // tmpInput.onSubmit = null;
                        tmpInput.onSelect?.RemoveAllListeners();
                        // tmpInput.onSelect = null;
                        tmpInput.onDeselect?.RemoveAllListeners();
                        // tmpInput.onDeselect = null;
                        tmpInput.onTextSelection?.RemoveAllListeners();
                        // tmpInput.onTextSelection = null;
                        tmpInput.onEndTextSelection?.RemoveAllListeners();
                        // tmpInput.onEndTextSelection = null;
                        tmpInput.onTouchScreenKeyboardStatusChanged?.RemoveAllListeners();
                        // tmpInput.onTouchScreenKeyboardStatusChanged = null;
                        break;
                    case Scrollbar scrollbar:
                        scrollbar.onValueChanged?.RemoveAllListeners();
                        // scrollbar.onValueChanged = null;
                        break;
                    case ScrollRectLoop scrollRectLoop:
                        scrollRectLoop.onValueChanged?.RemoveAllListeners();
                        // scrollRectLoop.onValueChanged = null;
                        // scrollRectLoop.OnSlideChange = null;
                        break;
                    case ScrollRect scrollRect:
                        scrollRect.onValueChanged?.RemoveAllListeners();
                        //这里不知道为什么要设为null，这玩意儿会被异步调用，如果是null很容易出问题
                        // scrollRect.onValueChanged = null;
                        break;
                    case Mask mask:
                        {
                            // mask存在bug，如果mask对应的隐藏，则会无法释放对应的资源
                            if (mask.gameObject != null)
                            {
                                Image image = mask.gameObject.GetComponent<Image>();
                                if (image != null)
                                {
                                    image.sprite = null;
                                }
                            }

                            break;
                        }
                }
            }

            var eventListeners = obj.GetComponentsInChildren<EventTriggerListener>(true);
            foreach (var eventListener in eventListeners)
            {
                eventListener.Clear();
            }

            var uiCircularScrollView = obj.GetComponentsInChildren<UICircularScrollView>(true);
            foreach (var view in uiCircularScrollView)
            {
                view.Clear();
            }

            if (isDestroy)
                GameObject.Destroy(obj);
        }
    }
}
