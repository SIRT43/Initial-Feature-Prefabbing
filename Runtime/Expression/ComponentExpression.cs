using FTGAMEStudio.InitialFramework.Reflection;
using System;
using UnityEngine;

namespace FTGAMEStudio.InitialSolution.Prefabbing
{
    /// <summary>
    /// ��������࣬���ڴ��������Ϣ�򸲸Ƕ�����Ϣ��
    /// </summary>
    [MapContainer(typeof(Component)), Serializable]
    public abstract class ComponentExpression<T> : ObjectExpression<T> where T : Component
    {
        [TextArea(4, 16)] public string data;

        protected ComponentExpression() { }
        protected ComponentExpression(T target) : base(target) { }
    }
}
