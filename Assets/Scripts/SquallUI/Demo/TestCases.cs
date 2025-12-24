using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SquallUI.Classes;

namespace SquallUI
{
    /// <summary>
    /// SquallUIMgr 功能测试用例
    /// 使用方法：将此脚本挂载到场景中的任意GameObject上
    /// 测试快捷键：
    /// - F1: 测试显示界面
    /// - F2: 测试隐藏界面
    /// - F3: 测试销毁界面
    /// - F4: 测试界面可见性检查
    /// - F5: 测试UI栈功能
    /// - F6: 测试屏幕背景
    /// - F7: 测试批量操作
    /// - F8: 运行所有测试
    /// </summary>
    public class TestCases : MonoBehaviour
    {
        [Header("测试配置")]
        [Tooltip("测试用的界面名称列表")]
        [SerializeField] private List<string> testViewNames = new List<string> { "TestView1", "TestView2", "TestView3" };

        [Tooltip("是否在Start时自动运行基础测试")]
        [SerializeField] private bool autoRunOnStart = false;

        private void Start()
        {
            if (autoRunOnStart)
            {
                StartCoroutine(RunBasicTests());
            }
        }

        private void Update()
        {
            // 快捷键触发测试
            if (Input.GetKeyDown(KeyCode.F1))
            {
                TestShowView();
            }
            else if (Input.GetKeyDown(KeyCode.F2))
            {
                TestHideView();
            }
            else if (Input.GetKeyDown(KeyCode.F3))
            {
                TestDestroyView();
            }
            else if (Input.GetKeyDown(KeyCode.F4))
            {
                TestViewVisibility();
            }
            else if (Input.GetKeyDown(KeyCode.F5))
            {
                TestUIStack();
            }
            else if (Input.GetKeyDown(KeyCode.F6))
            {
                TestScreenBackground();
            }
            else if (Input.GetKeyDown(KeyCode.F7))
            {
                TestBatchOperations();
            }
            else if (Input.GetKeyDown(KeyCode.F8))
            {
                StartCoroutine(RunAllTests());
            }
        }

        #region 测试用例

        /// <summary>
        /// 测试1: 显示界面功能
        /// </summary>
        [ContextMenu("测试显示界面")]
        public void TestShowView()
        {
            Debug.Log("========== 测试1: 显示界面功能 ==========");
            
            if (testViewNames.Count == 0)
            {
                Debug.LogError("测试失败: 未配置测试界面名称");
                return;
            }

            string viewName = testViewNames[0];
            Debug.Log($"尝试显示界面: {viewName}");

            // 测试同步显示
            SquallUIMgr.Instance.ShowView(viewName, view =>
            {
                if (view != null)
                {
                    Debug.Log($"✓ 界面显示成功: {viewName}, 可见性: {view.IsVisible()}");
                }
                else
                {
                    Debug.LogWarning($"✗ 界面显示失败: {viewName} (可能是预制体未配置)");
                }
            }, true);

            // 测试异步显示
            if (testViewNames.Count > 1)
            {
                string asyncViewName = testViewNames[1];
                Debug.Log($"尝试异步显示界面: {asyncViewName}");
                SquallUIMgr.Instance.ShowView(asyncViewName, view =>
                {
                    if (view != null)
                    {
                        Debug.Log($"✓ 异步界面显示成功: {asyncViewName}");
                    }
                    else
                    {
                        Debug.LogWarning($"✗ 异步界面显示失败: {asyncViewName}");
                    }
                }, false);
            }

            Debug.Log("========== 测试1完成 ==========\n");
        }

        /// <summary>
        /// 测试2: 隐藏界面功能
        /// </summary>
        [ContextMenu("测试隐藏界面")]
        public void TestHideView()
        {
            Debug.Log("========== 测试2: 隐藏界面功能 ==========");

            if (testViewNames.Count == 0)
            {
                Debug.LogError("测试失败: 未配置测试界面名称");
                return;
            }

            string viewName = testViewNames[0];
            
            // 先显示界面
            SquallUIMgr.Instance.ShowView(viewName, view =>
            {
                if (view != null && view.IsVisible())
                {
                    Debug.Log($"界面已显示: {viewName}");
                    
                    // 等待一帧后隐藏
                    StartCoroutine(HideViewAfterDelay(viewName));
                }
                else
                {
                    Debug.LogWarning($"界面未显示，无法测试隐藏功能: {viewName}");
                }
            }, true);
        }

        private IEnumerator HideViewAfterDelay(string viewName)
        {
            yield return new WaitForSeconds(0.5f);
            
            bool wasVisible = SquallUIMgr.Instance.IsViewVisible(viewName);
            SquallUIMgr.Instance.HideView(viewName);
            yield return null;
            
            bool isVisible = SquallUIMgr.Instance.IsViewVisible(viewName);
            
            if (wasVisible && !isVisible)
            {
                Debug.Log($"✓ 界面隐藏成功: {viewName}");
            }
            else
            {
                Debug.LogWarning($"✗ 界面隐藏失败: {viewName}, 之前可见: {wasVisible}, 现在可见: {isVisible}");
            }
            
            Debug.Log("========== 测试2完成 ==========\n");
        }

        /// <summary>
        /// 测试3: 销毁界面功能
        /// </summary>
        [ContextMenu("测试销毁界面")]
        public void TestDestroyView()
        {
            Debug.Log("========== 测试3: 销毁界面功能 ==========");

            if (testViewNames.Count == 0)
            {
                Debug.LogError("测试失败: 未配置测试界面名称");
                return;
            }

            string viewName = testViewNames[0];
            
            // 先显示界面
            SquallUIMgr.Instance.ShowView(viewName, view =>
            {
                if (view != null)
                {
                    Debug.Log($"界面已显示: {viewName}");
                    
                    // 等待一帧后销毁
                    StartCoroutine(DestroyViewAfterDelay(viewName));
                }
                else
                {
                    Debug.LogWarning($"界面未显示，无法测试销毁功能: {viewName}");
                }
            }, true);
        }

        private IEnumerator DestroyViewAfterDelay(string viewName)
        {
            yield return new WaitForSeconds(0.5f);
            
            bool existedBefore = SquallUIMgr.Instance.TryGetView(viewName, out _);
            bool result = SquallUIMgr.Instance.DestroyView(viewName);
            yield return null;
            
            bool existsAfter = SquallUIMgr.Instance.TryGetView(viewName, out _);
            
            if (existedBefore && !existsAfter && result)
            {
                Debug.Log($"✓ 界面销毁成功: {viewName}");
            }
            else
            {
                Debug.LogWarning($"✗ 界面销毁失败: {viewName}, 销毁前存在: {existedBefore}, 销毁后存在: {existsAfter}, 返回值: {result}");
            }
            
            Debug.Log("========== 测试3完成 ==========\n");
        }

        /// <summary>
        /// 测试4: 界面可见性检查
        /// </summary>
        [ContextMenu("测试界面可见性")]
        public void TestViewVisibility()
        {
            Debug.Log("========== 测试4: 界面可见性检查 ==========");

            if (testViewNames.Count == 0)
            {
                Debug.LogError("测试失败: 未配置测试界面名称");
                return;
            }

            string viewName = testViewNames[0];
            
            // 检查不存在的界面
            bool notExistVisible = SquallUIMgr.Instance.IsViewVisible(viewName, false);
            Debug.Log($"不存在的界面可见性(默认false): {notExistVisible} (期望: false)");
            
            bool notExistVisibleTrue = SquallUIMgr.Instance.IsViewVisible(viewName, true);
            Debug.Log($"不存在的界面可见性(默认true): {notExistVisibleTrue} (期望: true)");

            // 显示界面后检查
            SquallUIMgr.Instance.ShowView(viewName, view =>
            {
                if (view != null)
                {
                    StartCoroutine(CheckVisibilityAfterDelay(viewName));
                }
            }, true);
        }

        private IEnumerator CheckVisibilityAfterDelay(string viewName)
        {
            yield return new WaitForSeconds(0.5f);
            
            bool isVisible = SquallUIMgr.Instance.IsViewVisible(viewName);
            bool tryGetResult = SquallUIMgr.Instance.TryGetView(viewName, out IView view);
            
            Debug.Log($"界面可见性: {isVisible}, TryGetView结果: {tryGetResult}");
            
            if (isVisible && tryGetResult && view != null)
            {
                Debug.Log($"✓ 可见性检查正确: {viewName}");
            }
            else
            {
                Debug.LogWarning($"✗ 可见性检查异常: {viewName}");
            }
            
            Debug.Log("========== 测试4完成 ==========\n");
        }

        /// <summary>
        /// 测试5: UI栈功能
        /// </summary>
        [ContextMenu("测试UI栈")]
        public void TestUIStack()
        {
            Debug.Log("========== 测试5: UI栈功能 ==========");

            if (testViewNames.Count < 2)
            {
                Debug.LogError("测试失败: 需要至少2个测试界面名称");
                return;
            }

            StartCoroutine(TestUIStackCoroutine());
        }

        private IEnumerator TestUIStackCoroutine()
        {
            // 清空栈
            SquallUIMgr.Instance.ClearUIStack();
            Debug.Log("已清空UI栈");

            // 显示第一个界面
            SquallUIMgr.Instance.ShowView(testViewNames[0], null, true);
            yield return new WaitForSeconds(0.3f);
            
            string topView1 = SquallUIMgr.Instance.GetTopView();
            List<string> stack1 = SquallUIMgr.Instance.GetUiStack();
            Debug.Log($"显示第一个界面后 - 顶部界面: {topView1}, 栈大小: {stack1.Count}");

            // 显示第二个界面
            SquallUIMgr.Instance.ShowView(testViewNames[1], null, true);
            yield return new WaitForSeconds(0.3f);
            
            string topView2 = SquallUIMgr.Instance.GetTopView();
            List<string> stack2 = SquallUIMgr.Instance.GetUiStack();
            Debug.Log($"显示第二个界面后 - 顶部界面: {topView2}, 栈大小: {stack2.Count}");

            if (topView2 == testViewNames[1] && stack2.Count >= 1)
            {
                Debug.Log($"✓ UI栈功能正常");
            }
            else
            {
                Debug.LogWarning($"✗ UI栈功能异常");
            }

            Debug.Log("========== 测试5完成 ==========\n");
        }

        /// <summary>
        /// 测试6: 屏幕背景功能
        /// </summary>
        [ContextMenu("测试屏幕背景")]
        public void TestScreenBackground()
        {
            Debug.Log("========== 测试6: 屏幕背景功能 ==========");

            // 显示背景
            SquallUIMgr.Instance.ShowScreenBackground(false);
            Debug.Log("已调用 ShowScreenBackground(false)");

            StartCoroutine(TestScreenBackgroundCoroutine());
        }

        private IEnumerator TestScreenBackgroundCoroutine()
        {
            yield return new WaitForSeconds(1f);
            
            // 隐藏背景
            SquallUIMgr.Instance.HideScreenBackground(false);
            Debug.Log("已调用 HideScreenBackground(false)");
            
            yield return new WaitForSeconds(0.5f);
            
            // 测试锁定功能
            SquallUIMgr.Instance.ShowScreenBackground(true);
            Debug.Log("已调用 ShowScreenBackground(true) - 锁定状态");
            
            yield return new WaitForSeconds(0.5f);
            
            // 尝试不强制解锁隐藏（应该失败）
            SquallUIMgr.Instance.HideScreenBackground(false);
            Debug.Log("尝试不强制解锁隐藏（应该失败）");
            
            yield return new WaitForSeconds(0.5f);
            
            // 强制解锁隐藏
            SquallUIMgr.Instance.HideScreenBackground(true);
            Debug.Log("已调用 HideScreenBackground(true) - 强制解锁");
            
            Debug.Log("========== 测试6完成 ==========\n");
        }

        /// <summary>
        /// 测试7: 批量操作功能
        /// </summary>
        [ContextMenu("测试批量操作")]
        public void TestBatchOperations()
        {
            Debug.Log("========== 测试7: 批量操作功能 ==========");

            StartCoroutine(TestBatchOperationsCoroutine());
        }

        private IEnumerator TestBatchOperationsCoroutine()
        {
            // 显示多个界面
            foreach (var viewName in testViewNames)
            {
                SquallUIMgr.Instance.ShowView(viewName, null, true);
                yield return new WaitForSeconds(0.2f);
            }

            yield return new WaitForSeconds(0.5f);
            
            int visibleCount = SquallUIMgr.Instance.GetVisibleViewCount();
            Debug.Log($"显示多个界面后，可见界面数量: {visibleCount}");

            // 隐藏所有界面
            SquallUIMgr.Instance.HideAllView();
            yield return new WaitForSeconds(0.5f);
            
            int visibleCountAfterHide = SquallUIMgr.Instance.GetVisibleViewCount();
            Debug.Log($"隐藏所有界面后，可见界面数量: {visibleCountAfterHide} (期望: 0)");

            if (visibleCountAfterHide == 0)
            {
                Debug.Log($"✓ 批量隐藏功能正常");
            }
            else
            {
                Debug.LogWarning($"✗ 批量隐藏功能异常，仍有 {visibleCountAfterHide} 个可见界面");
            }

            // 重新显示
            foreach (var viewName in testViewNames)
            {
                SquallUIMgr.Instance.ShowView(viewName, null, true);
                yield return new WaitForSeconds(0.2f);
            }

            yield return new WaitForSeconds(0.5f);
            
            // 销毁所有界面
            SquallUIMgr.Instance.DestroyAllView();
            yield return new WaitForSeconds(0.5f);
            
            int visibleCountAfterDestroy = SquallUIMgr.Instance.GetVisibleViewCount();
            Debug.Log($"销毁所有界面后，可见界面数量: {visibleCountAfterDestroy} (期望: 0)");

            if (visibleCountAfterDestroy == 0)
            {
                Debug.Log($"✓ 批量销毁功能正常");
            }
            else
            {
                Debug.LogWarning($"✗ 批量销毁功能异常，仍有 {visibleCountAfterDestroy} 个可见界面");
            }

            Debug.Log("========== 测试7完成 ==========\n");
        }

        /// <summary>
        /// 运行基础测试
        /// </summary>
        private IEnumerator RunBasicTests()
        {
            Debug.Log("========== 开始运行基础测试 ==========");
            yield return new WaitForSeconds(1f);
            
            TestShowView();
            yield return new WaitForSeconds(2f);
            
            TestViewVisibility();
            yield return new WaitForSeconds(2f);
            
            Debug.Log("========== 基础测试完成 ==========");
        }

        /// <summary>
        /// 运行所有测试
        /// </summary>
        [ContextMenu("运行所有测试")]
        public IEnumerator RunAllTests()
        {
            Debug.Log("========== 开始运行所有测试用例 ==========\n");
            
            yield return new WaitForSeconds(0.5f);
            TestShowView();
            yield return new WaitForSeconds(2f);
            
            TestHideView();
            yield return new WaitForSeconds(2f);
            
            TestDestroyView();
            yield return new WaitForSeconds(2f);
            
            TestViewVisibility();
            yield return new WaitForSeconds(2f);
            
            TestUIStack();
            yield return new WaitForSeconds(2f);
            
            TestScreenBackground();
            yield return new WaitForSeconds(3f);
            
            TestBatchOperations();
            yield return new WaitForSeconds(3f);
            
            Debug.Log("========== 所有测试用例运行完成 ==========");
        }

        #endregion

        #region GUI显示测试信息

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, 400, 300));
            GUILayout.Box("SquallUIMgr 测试用例");
            
            GUILayout.Label("快捷键:");
            GUILayout.Label("F1 - 测试显示界面");
            GUILayout.Label("F2 - 测试隐藏界面");
            GUILayout.Label("F3 - 测试销毁界面");
            GUILayout.Label("F4 - 测试界面可见性");
            GUILayout.Label("F5 - 测试UI栈");
            GUILayout.Label("F6 - 测试屏幕背景");
            GUILayout.Label("F7 - 测试批量操作");
            GUILayout.Label("F8 - 运行所有测试");
            
            GUILayout.Space(10);
            
            if (GUILayout.Button("运行所有测试"))
            {
                StartCoroutine(RunAllTests());
            }
            
            GUILayout.Space(10);
            GUILayout.Label($"当前可见界面数: {SquallUIMgr.Instance.GetVisibleViewCount()}");
            GUILayout.Label($"UI栈大小: {SquallUIMgr.Instance.GetUiStack().Count}");
            GUILayout.Label($"顶部界面: {SquallUIMgr.Instance.GetTopView()}");
            
            GUILayout.EndArea();
        }

        #endregion
    }
}
