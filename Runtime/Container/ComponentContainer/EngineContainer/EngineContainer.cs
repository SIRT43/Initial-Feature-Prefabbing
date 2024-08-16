using FTGAMEStudio.InitialFramework.Collections.Generic;
using FTGAMEStudio.InitialFramework.ExtensionMethods;
using FTGAMEStudio.InitialFramework.Reflection;
using System;
using UnityEngine;

namespace FTGAMEStudio.InitialSolution.Prefabbing
{
    public interface IEngineContainer<T> where T : UnityEngine.Object
    {
        public void Credited(T target, InquiryMachine<string, Type> containerTypes);
        public void Fetched(T target, InquiryMachine<string, Type> containerTypes);
    }

    /// <summary>
    /// �����������ࡣ
    /// 
    /// <para>������ <see cref="EngineContainer"/> ���ǹ������ع�ϵ��</para>
    /// </summary>
    [MapContainer(typeof(Component)), Serializable]
    public abstract class EngineContainer<T> : ObjectContainer<T> where T : Component
    {
        protected EngineContainer(T target) : base(target) { }
    }

    /// <summary>
    /// �����࣬���ڴ������� <see cref="Component"/>��
    /// 
    /// <para>������ <see cref="EngineContainer{T}"/> ���ǹ������ع�ϵ��</para>
    /// </summary>
    [Serializable]
    public class EngineContainer : ComponentContainer<Component>, IEngineContainer<Component>
    {
        public EngineContainer(Component target, InquiryMachine<string, Type> containerTypes) => Credited(target, containerTypes);
        public EngineContainer(Component target) : base(target) { }

        public void Credited(Component target, InquiryMachine<string, Type> containerTypes)
        {
            name = target.name;

            uniqueName = target.GetType().GetUniqueName();

            ObjectContainer container = PrefabbingPipeline.EngineToContainer(target, containerTypes);
            data = IFJson.ToJson(container);
        }

        public void Fetched(Component target, InquiryMachine<string, Type> containerTypes)
        {
            target.name = name;

            ObjectContainer container = PrefabbingPipeline.JsonToContainer(data, uniqueName, containerTypes);
            container.Fetched(target);
        }

        public override void Credited(Component target) => Credited(target, PrefabbingPipeline.builtinEngineConts);
        public override void Fetched(Component target) => Fetched(target, PrefabbingPipeline.builtinEngineConts);
    }
}
