using FTGAMEStudio.InitialFramework.Reflection;
using System;
using UnityEngine;

namespace FTGAMEStudio.InitialSolution.Prefabbing
{
    /// <summary>
    /// 组件表达基类，用于储存对象信息或覆盖对象信息。
    /// </summary>
    [MapContainer(typeof(Component)), Serializable]
    public abstract class ComponentExpression<T> : ObjectExpression<T> where T : Component
    {
        [TextArea(4, 16)] public string data;

        protected ComponentExpression() { }
        protected ComponentExpression(T target) : base(target) { }
    }
}
