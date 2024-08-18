using FTGAMEStudio.InitialFramework.Collections.Generic;
using FTGAMEStudio.InitialFramework.ExtensionMethods;
using System;
using UnityEngine;

namespace FTGAMEStudio.InitialSolution.Prefabbing
{
    public interface IEngineInfo<T> where T : UnityEngine.Object
    {
        public void Credited(T target, InquiryMachine<string, Type> containerTypes);
        public void Fetched(T target, InquiryMachine<string, Type> containerTypes);
    }

    /// <summary>
    /// 引擎 <see cref="Component"/> 表达基类，用于储存对象信息或覆盖对象信息。
    /// </summary>
    [Serializable]
    public class EngineInfo : ComponentExpression<Component>, IEngineInfo<Component>
    {
        public EngineInfo(Component target, InquiryMachine<string, Type> containerTypes) => Credited(target, containerTypes);
        public EngineInfo(Component target) : base(target) { }

        /// <summary>
        /// 为什么要提供 containerTypes？
        /// 
        /// <para><see cref="EngineInfo"/> 必须通过它推断 target 应该用哪个容器。</para>
        /// </summary>
        public void Credited(Component target, InquiryMachine<string, Type> containerTypes)
        {
            name = target.name;

            uniqueName = target.GetType().GetUniqueName();

            ObjectExpression container = EngineProcessing.EngineToContainer(target, containerTypes);
            data = IFJson.ToJson(container);
        }

        /// <summary>
        /// 为什么要提供 containerTypes？
        /// 
        /// <para><see cref="EngineInfo"/> 必须通过它推断 target 应该用哪个容器。</para>
        /// </summary>
        public void Fetched(Component target, InquiryMachine<string, Type> containerTypes)
        {
            target.name = name;

            ObjectExpression container = EngineProcessing.JsonToContainer(data, uniqueName, containerTypes);
            container.Fetched(target);
        }

        public override void Credited(Component target) => Credited(target, EngineProcessing.builtinEngineConts);
        public override void Fetched(Component target) => Fetched(target, EngineProcessing.builtinEngineConts);
    }
}
