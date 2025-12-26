using System.Collections;
using System.Collections.Generic;

namespace UnityEngine.UI
{
    public enum ScrollRectLoopType
    {
        //Grid,
        Horizontal,

        Vertical,
    }

    public enum ScrollMoveDir
    {
        None = 0,
        Left,
        Right,
        Up,
        Down,
    }
    /// <summary>
    /// 滚动列表循环组件，可以对显示的列表进行复用，节省性能开销
    /// </summary>
    [AddComponentMenu("UI/Scroll Rect Loop", 40), DisallowMultipleComponent, ExecuteInEditMode, RequireComponent(typeof(RectTransform)), SelectionBase]
    public class ScrollRectLoop : ScrollRect
    {
        /// <summary>
        /// 滑动更改消息
        /// </summary>
        /// <param name="changed"> 更改的预设 </param>
        /// <param name="adjacent"> 相邻的预设 </param>
        /// <param name="adjustIdx"> 需要刷新的数据索引 </param>
        public delegate void SlideChange(GameObject changed, GameObject adjacent, int adjustIdx);

        public RectTransform m_GridRect;

        /// <summary>
        /// 布局类型
        /// </summary>
        [SerializeField]
        public ScrollRectLoopType m_Type;

        /// <summary>
        /// 是否使用Grid布局
        /// </summary>
        public bool isGrid;

        [SerializeField]
        public LayoutGroup m_Layout;

        [SerializeField]
        public GameObject m_Prefab;

        [SerializeField]
        private int m_ItemCount = 0;

        public SlideChange OnSlideChange;

        private RectTransform cache;

        /// <summary>
        /// 创建的prefab集合缓存
        /// </summary>
        private List<GameObject> prefabsList = new List<GameObject>();

        /// <summary>
        /// 所创建的prefab的间距  prefab.Higth / 2 + padding.y
        /// </summary>
        private float interval;

        /// <summary>
        /// 实际加载的Item数量
        /// </summary>
        private int instantiatCount;
        /// <summary>
        /// 记录第一个prefab的位置
        /// </summary>
        private int firstIndex = 0;

        /// <summary>
        /// 记录最后一个prefab的位置
        /// </summary>
        private int lastIndex = 0;

        private int oldMoveIndex = 0;
        private float positionY = 0;
        private float scrollRectHeight;

        private float positionX = 0;
        private float scrollRectWidth;
        /// <summary>
        /// 偏移量
        /// </summary>
        private float offset;

        private RectOffset padding;
        protected override void Start()
        {
            base.Start();
            if (!Application.isPlaying)
                return;
            //InitScrollview(m_ItemCount);
        }

        public void InitScrollView(int itemCount, SlideChange handler)
        {
            m_ItemCount = itemCount;
            RegistSliderChangeEvent(handler);
            InitScrollView();
        }
        /// <summary>
        /// 初始化滚动列表
        /// 根据不同方向的滚动计算创建预设的最大个数
        /// </summary>
        /// /// <param name="itemCount">数据数量</param>
        private void InitScrollView()
        {
            StopMovement();
            cache = GetComponent<RectTransform>();
            scrollRectHeight = cache.sizeDelta.y;
            scrollRectWidth = cache.sizeDelta.x;
            onValueChanged.AddListener(OnScrollValueChanged);
            m_GridRect.anchoredPosition = Vector3.zero;
            m_Layout.enabled = false;
            InitContent();
            switch (m_Type)
            {
                case ScrollRectLoopType.Vertical:
                    instantiatCount = Mathf.CeilToInt(cache.sizeDelta.y / interval) + 1;
                    break;
                case ScrollRectLoopType.Horizontal:
                    instantiatCount = Mathf.CeilToInt(cache.sizeDelta.x / interval) + 1;
                    break;
            }
            if (m_ItemCount < instantiatCount)
            {
                instantiatCount = m_ItemCount;
            }
            for (int i = 0; i < instantiatCount; i++)
            {
                GameObject prefab = Instantiate(m_Prefab, m_GridRect.transform) as GameObject;
                prefabsList.Add(prefab);
                RectTransform rt = prefab.GetComponent<RectTransform>();
                switch (m_Type)
                {
                    case ScrollRectLoopType.Horizontal:
                        rt.anchoredPosition = new Vector2(i* interval+rt.sizeDelta.x/2 + padding.left, -rt.sizeDelta.y / 2 - padding.top);
                        break;
                    case ScrollRectLoopType.Vertical:
                        rt.anchoredPosition = new Vector2(rt.sizeDelta.x / 2 + padding.left, -i * interval-rt.sizeDelta.y/2-padding.top);
                        break;
                }               
                prefab.name = "item_" + i;
                prefab.SetActive(true);
            }
            lastIndex = instantiatCount - 1;
            positionY = m_GridRect.anchoredPosition.y;
            positionX = m_GridRect.anchoredPosition.x;
            switch (m_Type)
            {
                case ScrollRectLoopType.Horizontal:
                    oldMoveIndex = Mathf.FloorToInt(Mathf.Abs(m_GridRect.anchoredPosition.x / interval));
                    break;
                case ScrollRectLoopType.Vertical:
                    oldMoveIndex = Mathf.FloorToInt(Mathf.Abs(m_GridRect.anchoredPosition.y / interval));
                    break;
            }
            UpdateItemsInView();
        }

        /// <summary>
        /// 设置Content的大小
        /// </summary>
        private void InitContent()
        {
            if (m_Prefab != null)
            {
                Vector2 sizeDelta = m_Prefab.GetComponent<RectTransform>().sizeDelta;
                if (m_Layout.GetType() == typeof(VerticalLayoutGroup))
                {
                    interval = sizeDelta.y + (m_Layout as VerticalLayoutGroup).spacing;
                    offset = sizeDelta.y / 2;
                    padding = (m_Layout as VerticalLayoutGroup).padding;
                    m_GridRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, m_ItemCount * interval + padding.top+padding.bottom);
                }
                else if (m_Layout.GetType() == typeof(HorizontalLayoutGroup))
                {
                    interval = sizeDelta.x + (m_Layout as HorizontalLayoutGroup).spacing;
                    offset = sizeDelta.x / 2;
                    padding = (m_Layout as HorizontalLayoutGroup).padding;
                    m_GridRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_ItemCount * interval+padding.left+padding.right);
                }
            }
        }

        private void AddInstantiatItem()
        {
            switch (m_Type)
            {
                case ScrollRectLoopType.Vertical:
                    instantiatCount = Mathf.CeilToInt(cache.sizeDelta.y / interval) + 1;
                    break;
                case ScrollRectLoopType.Horizontal:
                    instantiatCount = Mathf.CeilToInt(cache.sizeDelta.x / interval) + 1;
                    break;
            }
            if (m_ItemCount < instantiatCount)
            {
                instantiatCount = m_ItemCount;
            }
            if (prefabsList.Count < instantiatCount)
            {
                int nowCount = prefabsList.Count;
                for(int i = nowCount; i < instantiatCount; i++)
                {
                    GameObject prefab = Instantiate(m_Prefab, m_GridRect.transform) as GameObject;
                    prefabsList.Add(prefab);
                    RectTransform rt = prefab.GetComponent<RectTransform>();
                    switch (m_Type)
                    {
                        case ScrollRectLoopType.Horizontal:
                            rt.anchoredPosition = new Vector2(i * interval, -rt.sizeDelta.x / 2);
                            break;
                        case ScrollRectLoopType.Vertical:
                            rt.anchoredPosition = new Vector2(rt.sizeDelta.x / 2, -i * interval);
                            break;
                    }
                    prefab.name = "item_" + i;
                    prefab.SetActive(true);                   
                }
                lastIndex = instantiatCount - 1;
            }
        }
        private void RemoveInstantiatItem()
        {
            if (m_ItemCount < instantiatCount)
            {
                for (int i = 0; i < instantiatCount - m_ItemCount; i++)
                {
                    GameObject.Destroy(prefabsList[prefabsList.Count - 1]);
                    prefabsList.RemoveAt(prefabsList.Count - 1);
                }
                instantiatCount = m_ItemCount;
            }
            lastIndex = instantiatCount - 1;
        }
        public void RegistSliderChangeEvent(SlideChange handler)
        {
            OnSlideChange = handler;
        }

        public void UnregistSlideChangeEvent()
        {
            OnSlideChange = null;
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="itemCount"></param>
        public void RefreshDataCount(int itemCount)
        {
            m_ItemCount = itemCount;
            if (m_ItemCount < instantiatCount)
            {
                RemoveInstantiatItem();
            }
            else if(m_ItemCount > instantiatCount)
            {
                AddInstantiatItem();
            }
            InitContent();
            UpdateItemsInView();
        }

        /// <summary>
        /// 增加一个数据
        /// </summary>
        /// <param name="idx"></param>
        public void AddItem(int idx)
        {
            m_ItemCount += 1;
            InitContent();
            if (instantiatCount < m_ItemCount)
            {
                AddInstantiatItem();
            }
            if (idx >= firstIndex && idx <= lastIndex)
            {
                UpdateItemsInView();
            }
        }

        /// <summary>
        /// 删除一个数据
        /// </summary>
        /// <param name="idx"></param>
        public void RemoveItem(int idx)
        {
            m_ItemCount -= 1;
            InitContent();
            if(m_ItemCount < instantiatCount)
            {
                RemoveInstantiatItem();
            }
            lastIndex = instantiatCount - 1;
            if (idx >= firstIndex && idx <= lastIndex)
            {
                UpdateItemsInView();
            }
        }

        /// <summary>
        /// 滚动到某个位置
        /// </summary>
        /// <param name="idx"></param>
        public void ScrollToIdx(int idx)
        {
            StopMovement();
            float itemDelta = idx * interval;
            Vector2 sliderChangeValue = Vector2.zero;
            if (m_Type == ScrollRectLoopType.Horizontal)
            {
                float contentX = -itemDelta - scrollRectHeight/2 - padding.left;
                if(contentX < -(m_GridRect.sizeDelta.x - scrollRectWidth))
                {
                    contentX = -(m_GridRect.sizeDelta.x - scrollRectWidth);
                }
                if (contentX > 0)
                {
                    contentX = 0;
                }
                sliderChangeValue = new Vector2(contentX, 0) - new Vector2(m_GridRect.anchoredPosition.x, 0);
                m_GridRect.anchoredPosition = new  Vector2(contentX, 0);
            }
            else if(m_Type == ScrollRectLoopType.Vertical)
            {
                float contentY = itemDelta - scrollRectHeight/2 + padding.top;
                if(contentY > m_GridRect.sizeDelta.y - scrollRectHeight)
                {
                    contentY = m_GridRect.sizeDelta.y - scrollRectHeight;
                }
                if(contentY < 0)
                {
                    contentY = 0;
                }
                sliderChangeValue = new Vector2(0, contentY) - new Vector2(0, m_GridRect.anchoredPosition.y);
                m_GridRect.anchoredPosition = new Vector2(0, contentY);
            }
            OnScrollValueChanged(sliderChangeValue);
            UpdateItemsInView();
        }

        /// <summary>
        /// 刷新正在显示的数据
        /// </summary>
        public void UpdateItemsInView()
        {
            if(OnSlideChange!= null)
            {
                for(int i = 0; i < prefabsList.Count; i++)
                {
                    int idx = int.Parse(prefabsList[i].name.Replace("item_", ""));
                    OnSlideChange(prefabsList[i], null, idx);
                }
            }          
        }
        /// <summary>
        /// 当进滚动列表进行滑动时触发
        /// 计算滑动方向，预生成下一个预设的位置
        /// </summary>
        /// <param name="vec"></param>
        private void OnScrollValueChanged(Vector2 vec)
        {
            int curMoveIndex = 0;
            int offsetCount = 0;
            ScrollMoveDir dir = ScrollMoveDir.None;
            if (m_Type == ScrollRectLoopType.Vertical)
            {
                float curPosY = m_GridRect.anchoredPosition.y;
                curMoveIndex = Mathf.FloorToInt(Mathf.Abs(curPosY / interval));
                if (curMoveIndex > m_ItemCount-instantiatCount)
                {
                    curMoveIndex = m_ItemCount- instantiatCount;
                }
                offsetCount = Mathf.Abs(curMoveIndex - oldMoveIndex);                
                if (curPosY > interval && curPosY > positionY && lastIndex < m_ItemCount - 1)
                {
                    dir = ScrollMoveDir.Up;
                }
                else if (curPosY < positionY && (curPosY + scrollRectHeight) < (m_ItemCount - 1) * interval && firstIndex > 0)
                {
                    dir = ScrollMoveDir.Down;
                }
                for (int i = 0; i < offsetCount; i++)
                {
                    if(dir == ScrollMoveDir.Up)
                    {
                        firstIndex++;
                        lastIndex++;                        
                    }
                    else if(dir == ScrollMoveDir.Down)
                    {
                        firstIndex--;
                        lastIndex--;
                    }
                    UpdateListviewPos(dir);
                }
                oldMoveIndex = curMoveIndex;
                positionY = curPosY;
            }
            else if(m_Type == ScrollRectLoopType.Horizontal)
            {
                float curPosX = -m_GridRect.anchoredPosition.x;
                curMoveIndex = Mathf.FloorToInt(Mathf.Abs(curPosX / interval));
                if (curMoveIndex > m_ItemCount - instantiatCount)
                {
                    curMoveIndex = m_ItemCount - instantiatCount;
                }
                offsetCount = Mathf.Abs(curMoveIndex - oldMoveIndex);
                if (curPosX > interval && curPosX > positionX && lastIndex < m_ItemCount - 1)
                {
                    dir = ScrollMoveDir.Left;
                }
                else if (curPosX > 0 && curPosX < positionX && (curPosX + scrollRectWidth) < (m_ItemCount - 1) * interval && firstIndex > 0)
                {
                    dir = ScrollMoveDir.Right;
                }
                if (offsetCount > 1)
                {
                    offsetCount -= 1;
                }
                for (int i = 0; i < offsetCount; i++)
                {
                    if (dir == ScrollMoveDir.Left)
                    {
                        firstIndex++;
                        lastIndex++;
                    }
                    else if (dir == ScrollMoveDir.Right)
                    {
                        firstIndex--;
                        lastIndex--;                   
                    }
                    UpdateListviewPos(dir);
                }
                oldMoveIndex = curMoveIndex;
                positionX = curPosX;
            }
        }

        /// <summary>
        /// 更新列表位置
        /// </summary>
        /// <param name="isMoveUp"></param>
        private void UpdateListviewPos(ScrollMoveDir dir)
        {
            int tempIndex = 0;
            GameObject prefab = null;
            GameObject adjacent = null;
            RectTransform rt;
            int adjustIdx = -1;
            switch (dir)
            {
                case ScrollMoveDir.Up:
                    prefab = prefabsList[tempIndex];
                    adjacent = prefabsList[prefabsList.Count - 1];
                    prefabsList.RemoveAt(tempIndex);
                    prefabsList.Add(prefab);
                    rt = prefab.GetComponent<RectTransform>();
                    rt.anchoredPosition = new Vector2(rt.sizeDelta.x / 2 + padding.left, -lastIndex * interval - offset - padding.top);
                    prefab.name = "item_" + lastIndex;
                    adjustIdx = lastIndex;
                    break;
                case ScrollMoveDir.Down:
                    tempIndex = prefabsList.Count - 1;
                    adjacent = prefabsList[0];
                    prefab = prefabsList[tempIndex];
                    prefabsList.RemoveAt(tempIndex);
                    prefabsList.Insert(0, prefab);
                    rt = prefab.GetComponent<RectTransform>();
                    rt.anchoredPosition = new Vector2(rt.sizeDelta.x / 2 + padding.left, -firstIndex * interval - offset - padding.top);
                    prefab.name = "item_" + firstIndex;
                    adjustIdx = firstIndex;
                    break;
                case ScrollMoveDir.Left:
                    prefab = prefabsList[tempIndex];
                    adjacent = prefabsList[prefabsList.Count - 1];
                    prefabsList.RemoveAt(tempIndex);
                    prefabsList.Add(prefab);
                    rt = prefab.GetComponent<RectTransform>();
                    rt.anchoredPosition = new Vector2(lastIndex * interval + offset + padding.left, -rt.sizeDelta.y / 2 - padding.top);
                    prefab.name = "item_" + lastIndex;
                    adjustIdx = lastIndex;
                    break;
                case ScrollMoveDir.Right:
                    tempIndex = prefabsList.Count - 1;
                    adjacent = prefabsList[0];
                    prefab = prefabsList[tempIndex];
                    prefabsList.RemoveAt(tempIndex);
                    prefabsList.Insert(0, prefab);
                    rt = prefab.GetComponent<RectTransform>();
                    rt.anchoredPosition = new Vector2(firstIndex * interval + offset + padding.left, -rt.sizeDelta.y / 2 - padding.top);
                    prefab.name = "item_" + firstIndex;
                    adjustIdx = firstIndex;
                    break;
            }
            if (OnSlideChange != null && adjustIdx >= 0 && adjustIdx< m_ItemCount)
            {
                OnSlideChange(prefab, adjacent, adjustIdx);
            }
        }

        /// <summary>
        /// 清空ScrollView
        /// </summary>
        public void Clear()
        {
            for(int i = 0; i < instantiatCount; i++)
            {
                GameObject.Destroy(prefabsList[i]);
            }
            instantiatCount = 0;
            firstIndex = 0;
            lastIndex = 0;
            m_ItemCount = 0;
            prefabsList.Clear();
        }
    }
}