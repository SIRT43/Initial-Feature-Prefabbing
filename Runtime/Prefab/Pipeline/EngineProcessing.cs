using FTGAMEStudio.InitialFramework.Collections.Generic;
using FTGAMEStudio.InitialFramework.ExtensionMethods;
using FTGAMEStudio.InitialFramework.Reflection;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FTGAMEStudio.InitialSolution.Prefabbing
{
    public static class EngineProcessing
    {
        /// <summary>
        /// 本包内置的所有引擎 <see cref="Component"/> 容器。
        /// </summary>
        public static readonly InquiryMachine<string, Type> builtinEngineConts = new(Mapping.ClassifyMapTargetType(true, typeof(TransformContainer), typeof(RigidbodyContainer)));


        /// <summary>
        /// 将引擎 <see cref="Component"/> 转换为容器。
        /// 
        /// <para>您需要提供一个匹配的容器类型。</para>
        /// </summary>
        public static ObjectExpression EngineToContainer(Component engine, Type containerType)
        {
            Type engineType = engine.GetType();

            if (!Mapping.VerifyMapping(containerType, engineType))
                throw new ArgumentException($"The component does not match the target type of the container. component: {engineType}, container: {Mapping.GetMapTargetType(containerType, true)}");

            return Activator.CreateInstance(containerType, engine as object) as ObjectExpression;
        }

        /// <summary>
        /// 将引擎 <see cref="Component"/> 转换为容器。
        /// 
        /// <para>您需要提供一个容器类型表，本方法将推断可用的容器类型。</para>
        /// </summary>
        public static ObjectExpression EngineToContainer(Component engine, InquiryMachine<string, Type> containerTypes) =>
            EngineToContainer(engine, containerTypes.Inquiry(engine.GetType().GetUniqueName()));

        /// <summary>
        /// 将引擎 <see cref="Component"/> 转换为容器。
        /// 
        /// <para>使用内置的容器类型表。</para>
        /// </summary>
        public static ObjectExpression EngineToContainer(Component engine) =>
            EngineToContainer(engine, builtinEngineConts);


        /// <summary>
        /// 将 Json 转换为容器。
        /// 
        /// <para>您需要提供一个匹配的容器类型。</para>
        /// </summary>
        public static ObjectExpression JsonToContainer(string json, Type containerType) =>
            IFJson.FromJson(json, containerType) as ObjectExpression;

        /// <summary>
        /// 将 Json 转换为容器。
        /// 
        /// <para>本方法用于 <see cref="EngineInfo"/>。</para>
        /// </summary>
        public static ObjectExpression JsonToContainer(string json, string uniqueName, InquiryMachine<string, Type> containerTypes) =>
            JsonToContainer(json, containerTypes.Inquiry(uniqueName));

        /// <summary>
        /// 将 Json 转换为容器。
        /// 
        /// <para>本方法用于 <see cref="EngineInfo"/>。</para>
        /// </summary>
        public static ObjectExpression JsonToContainer(string json, string uniqueName) =>
            JsonToContainer(json, uniqueName, builtinEngineConts);



        /// <summary>
        /// 记入宏，用于批量记入引擎 <see cref="Component"/>。
        /// </summary>
        public static void CreditedMacro(Component[] engines, EngineInfo[] engineInfos, InquiryMachine<string, Type> containerTypes)
        {
            if (engines.Length != engineInfos.Length)
                throw new ArgumentException($"The parameters are asymmetrical and this cannot be done. objects: {engines.Length}, expressions: {engineInfos.Length}");

            for (int index = 0; index < engines.Length; index++) engineInfos[index].Credited(engines[index], containerTypes);
        }

        /// <summary>
        /// 取出宏，用于批量取出 <see cref="EngineInfo"/>。
        /// 
        /// <para>不推荐使用此方法，请使用 <see cref="FetchedWithEngines(Component[], EngineInfo[], InquiryMachine{string, Type})"/></para>
        /// </summary>
        public static void FetchedMacro(Component[] engines, EngineInfo[] engineInfos, InquiryMachine<string, Type> containerTypes)
        {
            if (engines.Length != engineInfos.Length)
                throw new ArgumentException($"The parameters are asymmetrical and this cannot be done. objects: {engines.Length}, expressions: {engineInfos.Length}");

            for (int index = 0; index < engines.Length; index++) engineInfos[index].Fetched(engines[index], containerTypes);
        }

        /// <summary>
        /// 批量取出 <see cref="EngineInfo"/>。
        /// </summary>
        public static void FetchedWithEngines(Component[] engines, EngineInfo[] engineInfos, InquiryMachine<string, Type> containerTypes)
        {
            Dictionary<string, List<Component>> classifyEngines = ReflectionMacro.ClassifyWithUniqueName(engines);
            Dictionary<string, List<EngineInfo>> classifyEngineInfos = ExpressionProcessing.ClassifyExpressions(engineInfos);

            foreach (KeyValuePair<string, List<Component>> classifyEngine in classifyEngines)
            {
                if (!classifyEngineInfos.ContainsKey(classifyEngine.Key)) continue;

                FetchedMacro(classifyEngine.Value.ToArray(), classifyEngineInfos[classifyEngine.Key].ToArray(), containerTypes);
            }
        }

        /// <summary>
        /// 获取指定引擎 <see cref="Component"/> 数组的所有 <see cref="Component"/> 组件的 <see cref="EngineInfo"/>。
        /// </summary>
        public static EngineInfo[] GetInfos(Component[] engines, InquiryMachine<string, Type> containerTypes)
        {
            List<EngineInfo> engineInfos = new();

            foreach (Component engine in engines)
            {
                string uniqueName = engine.GetType().GetUniqueName();

                if (!containerTypes.Has(uniqueName)) continue;

                engineInfos.Add(new(engine, containerTypes));
            }

            return engineInfos.ToArray();
        }
    }
}
