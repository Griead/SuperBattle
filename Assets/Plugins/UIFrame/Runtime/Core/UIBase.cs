using System;
using System.Collections.Generic;
using UnityEngine;
using Action = System.Action;
using Object = UnityEngine.Object;
#if USING_UNITASK
using GameObjectTask = Cysharp.Threading.Tasks.UniTask<System.Collections.Generic.List<UnityEngine.GameObject>>;
using Task = Cysharp.Threading.Tasks.UniTask;
using Cysharp.Threading.Tasks;
#else
using GameObjectTask = System.Threading.Tasks.Task<System.Collections.Generic.List<UnityEngine.GameObject>>;
using UIBaseTask = System.Threading.Tasks.Task<Feif.UIFramework.UIBase>;
using System.Threading.Tasks;
#endif

public delegate T CustomDelegateOutInUIFrame<D, out T>(D d);

namespace Feif.UIFramework
{
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public abstract class UIBase : MonoBehaviour
    {
        /// <summary>
        /// 是否自动销毁
        /// </summary>
        public bool AutoDestroy = true;

        /// <summary>
        /// 一级父节点
        /// </summary>
        public UIBase Parent;

        /// <summary>
        /// 一级子节点
        /// </summary>
        public List<UIBase> Children = new List<UIBase>();

        /// <summary>
        /// 存储的对象
        /// </summary>
        protected List<UnityEngine.Object> UISaveObjectList = new List<Object>();

        protected internal Task InnerOnCreate() => OnCreate();
        protected internal Task InnerOnRefresh() => OnRefresh();
        protected internal void InnerOnBind() => OnBind();
        protected internal void InnerOnUnbind() => OnUnbind();
        protected internal void InnerOnShow() => OnShow();
        protected internal void InnerOnHide() => OnHide();
        protected internal void InnerOnDied() => OnDied();

        private HashSet<UITimer> timers = new HashSet<UITimer>();

        /// <summary>
        /// 创建时调用，生命周期内只执行一次
        /// </summary>
        protected virtual Task OnCreate() => Task.CompletedTask;

        /// <summary>
        /// 刷新时调用
        /// </summary>
        protected virtual Task OnRefresh() => Task.CompletedTask;

        /// <summary>
        /// 绑定事件
        /// </summary>
        protected virtual void OnBind()
        {
        }

        /// <summary>
        /// 解绑事件
        /// </summary>
        protected virtual void OnUnbind()
        {
        }

        /// <summary>
        /// 显示时调用
        /// </summary>
        protected virtual void OnShow()
        {
        }

        /// <summary>
        /// 隐藏时调用
        /// </summary>
        protected virtual void OnHide()
        {
        }

        /// <summary>
        /// 销毁时调用，生命周期内只执行一次
        /// </summary>
        protected virtual void OnDied()
        {
            for (int i = 0; i < UISaveObjectList.Count; i++)
            {
                Destroy(UISaveObjectList[i]);
            }
        }

        /// <summary>
        /// 创建定时器，gameObject被销毁时会自动Cancel定时器
        /// </summary>
        /// <param name="delay">延迟多少秒后执行callback</param>
        /// <param name="callback">延迟执行的方法</param>
        /// <param name="isLoop">是否是循环定时器</param>
        protected UITimer CreateTimer(float delay, Action callback, bool isLoop = false)
        {
            var timer = UIFrame.CreateTimer(delay, callback, isLoop);
            timers.Add(timer);
            return timer;
        }

        /// <summary>
        /// 取消所有定时器
        /// </summary>
        public void CancelAllTimer()
        {
            foreach (var item in timers)
            {
                item.Cancel();
            }

            timers.Clear();
        }

        public Dictionary<int, List<GameObject>> _instanceItemList = new Dictionary<int, List<GameObject>>();

        /// <summary>
        /// 刷新实例条目
        /// </summary>
        /// <param name="index"> 存储次序 </param>
        /// <param name="count"> 列表数量 </param>
        /// <param name="prefab"> 预制体模板 </param>
        /// <param name="parent"> 生成父级 </param>
        /// <param name="getDataDelegate"> 数据UIData </param>
        /// <typeparam name="T"> UIData </typeparam>
        /// <typeparam name="D"> UIBase </typeparam>
        public async GameObjectTask RefreshInstanceItem<T, D>(int index, int count, GameObject prefab, Transform parent,
            CustomDelegateOutInUIFrame<int, T> getDataDelegate) where D : UIBase where T : UIData
        {
            List<GameObject> existingItem = _instanceItemList.TryGetValue(index, out var value)
                ? value
                : _instanceItemList[index] = new List<GameObject>();

            int existingItemCount = existingItem.Count;
            int requiredItemCount = count;

            // 处理已有对象
            for (int i = 0; i < existingItemCount; i++)
            {
                var item = existingItem[i];

                if (i < requiredItemCount)
                {
                    item.SetActive(true);
                    await UIFrame.Refresh(item.GetComponent<D>());
                }
                else
                {
                    item.SetActive(false);
                }
            }

            // 生成新对象
            if (existingItemCount < requiredItemCount)
            {
                List<Task> tasks = new List<Task>();
                for (int i = existingItemCount; i < requiredItemCount; i++)
                {
                    // 获取数据
                    T data = getDataDelegate(i);
                    
                    // 异步实例化对象并添加到列表
                    tasks.Add(CreateCompatibleTask(async () =>
                    {
                        GameObject newItem = await UIFrame.Instantiate(prefab, parent, data);
                        existingItem.Add(newItem);
                    }));
                }

                await Task.WhenAll(tasks);
            }
            
            // 返回更新后的列表
            return existingItem;
        }

        static Task CreateCompatibleTask(Func<Task> func)
        {
#if USING_UNITASK
    return func(); // UniTask 支持
#else
            return Task.Run(func); // .NET Task 需要 Task.Run 来启动异步方法
#endif
        }
    }
}