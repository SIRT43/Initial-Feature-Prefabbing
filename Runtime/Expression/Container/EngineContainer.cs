using FTGAMEStudio.InitialFramework.Reflection;
using System;
using UnityEngine;

namespace FTGAMEStudio.InitialSolution.Prefabbing
{
    /// <summary>
    /// 引擎容器基类，用于储存引擎 <see cref="Component"/> 的数据。
    /// 
    /// <para>
    /// 为什么要使用此类？
    /// <br>由于 Unity 不允许序列化引擎 <see cref="Component"/> 本包的解决方案是使用 <see cref="Mapping"/> 解决。</br>
    /// <br>通过基于 <see cref="Mapping"/> 封装，本基类实现了通过派生类字段或属性储存或覆盖 引擎 <see cref="Component"/> 的同名字段或属性。</br>
    /// </para>
    /// </summary>
    [MapContainer(typeof(Component)), Serializable]
    public abstract class EngineContainer<T> : ObjectExpression<T> where T : Component
    {
        protected EngineContainer(T target) : base(target) { }
    }
}
