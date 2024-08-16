using FTGAMEStudio.InitialFramework.Reflection;
using System;
using UnityEngine;

namespace FTGAMEStudio.InitialSolution.Prefabbing
{
    /// <summary>
    /// 容器基类。
    /// 
    /// <para>本类与 <see cref="EngineContainer"/> 不是功能重载关系。</para>
    /// </summary>
    [MapContainer(typeof(Component)), Serializable]
    public abstract class ComponentContainer<T> : ObjectContainer<T> where T : Component
    {
        [TextArea(4, 8)] public string data;

        protected ComponentContainer() { }
        protected ComponentContainer(T target) : base(target) { }
    }
}
