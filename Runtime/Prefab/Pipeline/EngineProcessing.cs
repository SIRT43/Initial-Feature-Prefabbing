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
        /// �������õ��������� <see cref="Component"/> ������
        /// </summary>
        public static readonly InquiryMachine<string, Type> builtinEngineConts = new(Mapping.ClassifyMapTargetType(true, typeof(TransformContainer), typeof(RigidbodyContainer)));


        /// <summary>
        /// ������ <see cref="Component"/> ת��Ϊ������
        /// 
        /// <para>����Ҫ�ṩһ��ƥ����������͡�</para>
        /// </summary>
        public static ObjectExpression EngineToContainer(Component engine, Type containerType)
        {
            Type engineType = engine.GetType();

            if (!Mapping.VerifyMapping(containerType, engineType))
                throw new ArgumentException($"The component does not match the target type of the container. component: {engineType}, container: {Mapping.GetMapTargetType(containerType, true)}");

            return Activator.CreateInstance(containerType, engine as object) as ObjectExpression;
        }

        /// <summary>
        /// ������ <see cref="Component"/> ת��Ϊ������
        /// 
        /// <para>����Ҫ�ṩһ���������ͱ����������ƶϿ��õ��������͡�</para>
        /// </summary>
        public static ObjectExpression EngineToContainer(Component engine, InquiryMachine<string, Type> containerTypes) =>
            EngineToContainer(engine, containerTypes.Inquiry(engine.GetType().GetUniqueName()));

        /// <summary>
        /// ������ <see cref="Component"/> ת��Ϊ������
        /// 
        /// <para>ʹ�����õ��������ͱ�</para>
        /// </summary>
        public static ObjectExpression EngineToContainer(Component engine) =>
            EngineToContainer(engine, builtinEngineConts);


        /// <summary>
        /// �� Json ת��Ϊ������
        /// 
        /// <para>����Ҫ�ṩһ��ƥ����������͡�</para>
        /// </summary>
        public static ObjectExpression JsonToContainer(string json, Type containerType) =>
            IFJson.FromJson(json, containerType) as ObjectExpression;

        /// <summary>
        /// �� Json ת��Ϊ������
        /// 
        /// <para>���������� <see cref="EngineInfo"/>��</para>
        /// </summary>
        public static ObjectExpression JsonToContainer(string json, string uniqueName, InquiryMachine<string, Type> containerTypes) =>
            JsonToContainer(json, containerTypes.Inquiry(uniqueName));

        /// <summary>
        /// �� Json ת��Ϊ������
        /// 
        /// <para>���������� <see cref="EngineInfo"/>��</para>
        /// </summary>
        public static ObjectExpression JsonToContainer(string json, string uniqueName) =>
            JsonToContainer(json, uniqueName, builtinEngineConts);



        /// <summary>
        /// ����꣬���������������� <see cref="Component"/>��
        /// </summary>
        public static void CreditedMacro(Component[] engines, EngineInfo[] engineInfos, InquiryMachine<string, Type> containerTypes)
        {
            if (engines.Length != engineInfos.Length)
                throw new ArgumentException($"The parameters are asymmetrical and this cannot be done. objects: {engines.Length}, expressions: {engineInfos.Length}");

            for (int index = 0; index < engines.Length; index++) engineInfos[index].Credited(engines[index], containerTypes);
        }

        /// <summary>
        /// ȡ���꣬��������ȡ�� <see cref="EngineInfo"/>��
        /// 
        /// <para>���Ƽ�ʹ�ô˷�������ʹ�� <see cref="FetchedWithEngines(Component[], EngineInfo[], InquiryMachine{string, Type})"/></para>
        /// </summary>
        public static void FetchedMacro(Component[] engines, EngineInfo[] engineInfos, InquiryMachine<string, Type> containerTypes)
        {
            if (engines.Length != engineInfos.Length)
                throw new ArgumentException($"The parameters are asymmetrical and this cannot be done. objects: {engines.Length}, expressions: {engineInfos.Length}");

            for (int index = 0; index < engines.Length; index++) engineInfos[index].Fetched(engines[index], containerTypes);
        }

        /// <summary>
        /// ����ȡ�� <see cref="EngineInfo"/>��
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
        /// ��ȡָ������ <see cref="Component"/> ��������� <see cref="Component"/> ����� <see cref="EngineInfo"/>��
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
