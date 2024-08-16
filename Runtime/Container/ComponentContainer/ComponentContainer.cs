using FTGAMEStudio.InitialFramework.Reflection;
using System;
using UnityEngine;

namespace FTGAMEStudio.InitialSolution.Prefabbing
{
    /// <summary>
    /// �������ࡣ
    /// 
    /// <para>������ <see cref="EngineContainer"/> ���ǹ������ع�ϵ��</para>
    /// </summary>
    [MapContainer(typeof(Component)), Serializable]
    public abstract class ComponentContainer<T> : ObjectContainer<T> where T : Component
    {
        [TextArea(4, 8)] public string data;

        protected ComponentContainer() { }
        protected ComponentContainer(T target) : base(target) { }
    }
}
