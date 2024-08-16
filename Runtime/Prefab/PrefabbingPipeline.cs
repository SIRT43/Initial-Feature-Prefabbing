using FTGAMEStudio.InitialFramework.Classifying;
using FTGAMEStudio.InitialFramework.Collections.Generic;
using FTGAMEStudio.InitialFramework.ExtensionMethods;
using FTGAMEStudio.InitialFramework.Reflection;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FTGAMEStudio.InitialSolution.Prefabbing
{
    /// <summary>
    /// �־û����ߣ�����ʵ�� <see cref="GameObject"/> �־û���
    /// 
    /// <para>������˵�������߽�ʵ�� <see cref="Transform"/> �� <see cref="MonoBehaviour"/> �ĳ־û���</para>
    /// </summary>
    public static class PrefabbingPipeline
    {
        public static readonly InquiryMachine<string, Type> builtinEngineConts = new(Mapping.ClassifyMapTargetType(true, typeof(TransformContainer), typeof(RigidbodyContainer)));



        /// <summary>
        /// �� <see cref="ObjectContainer.uniqueName"/> ���ࡣ
        /// </summary>
        public static Dictionary<string, List<T>> ClassifyContainers<T>(T[] containers) where T : ObjectContainer
        {
            FilterableClassifier<string, T> classifier = new((T monoContainer, out string key) =>
            {
                key = monoContainer.uniqueName;
                return true;
            });

            return classifier.Classify(containers);
        }



        /// <summary>
        /// ����꣬�����������롣
        /// </summary>
        public static void CreditedMacro(UnityEngine.Object[] objects, ObjectContainer[] monoContainers)
        {
            if (objects.Length != monoContainers.Length)
                throw new ArgumentException($"The specified MonoContainers cannot be converted to Monos because of the asymmetric parameters. monos: {objects.Length}, monoContainers: {monoContainers.Length}", nameof(monoContainers));

            for (int index = 0; index < objects.Length; index++) monoContainers[index].Credited(objects[index]);
        }

        /// <summary>
        /// ����꣬�����������롣
        /// </summary>
        public static MonoContainer[] CreditedMacro(GameObject gameObject)
        {
            MonoBehaviour[] monoBehaviours = gameObject.GetMonoComponents();
            MonoContainer[] monoContainers = new MonoContainer[monoBehaviours.Length];

            for (int index = 0; index < monoBehaviours.Length; index++)
                monoContainers[index] = new(monoBehaviours[index]);

            CreditedMacro(gameObject.GetMonoComponents(), monoContainers);

            return monoContainers;
        }

        /// <summary>
        /// ����꣬�����������롣
        /// </summary>
        public static void EngineCreditedMacro(Component[] objects, EngineContainer[] monoContainers, InquiryMachine<string, Type> containerTypes)
        {
            if (objects.Length != monoContainers.Length)
                throw new ArgumentException($"The specified MonoContainers cannot be converted to Monos because of the asymmetric parameters. monos: {objects.Length}, monoContainers: {monoContainers.Length}", nameof(monoContainers));

            for (int index = 0; index < objects.Length; index++) monoContainers[index].Credited(objects[index], containerTypes);
        }



        /// <summary>
        /// ȡ���꣬��������ȡ����
        /// </summary>
        public static void FetchedMacro(UnityEngine.Object[] objects, ObjectContainer[] monoContainers)
        {
            if (objects.Length != monoContainers.Length)
                throw new ArgumentException($"The specified MonoContainers cannot be converted to Monos because of the asymmetric parameters. monos: {objects.Length}, monoContainers: {monoContainers.Length}", nameof(monoContainers));

            for (int index = 0; index < objects.Length; index++) monoContainers[index].Fetched(objects[index]);
        }

        /// <summary>
        /// ȡ���꣬��������ȡ����
        /// 
        /// <para>���Ƽ�ʹ�ô˷�������ʹ�� <see cref="ContainersToMonos(GameObject, MonoContainer[])"/>��</para>
        /// </summary>
        public static void FetchedMacro(GameObject gameObject, MonoContainer[] monoContainers) =>
            FetchedMacro(gameObject.GetMonoComponents(), monoContainers);

        /// <summary>
        /// ȡ���꣬��������ȡ����
        /// </summary>
        public static void EngineFetchedMacro(Component[] objects, EngineContainer[] monoContainers, InquiryMachine<string, Type> containerTypes)
        {
            if (objects.Length != monoContainers.Length)
                throw new ArgumentException($"The specified MonoContainers cannot be converted to Monos because of the asymmetric parameters. monos: {objects.Length}, monoContainers: {monoContainers.Length}", nameof(monoContainers));

            for (int index = 0; index < objects.Length; index++) monoContainers[index].Fetched(objects[index], containerTypes);
        }



        /// <summary>
        /// �� <see cref="MonoBehaviour"/> ����ת��Ϊ <see cref="MonoContainer"/>��
        /// </summary>
        public static MonoContainer[] MonosContainerMacro(MonoBehaviour[] monos)
        {
            MonoContainer[] monoContainers = new MonoContainer[monos.Length];

            for (int index = 0; index < monos.Length; index++)
                monoContainers[index] = new(monos[index]);

            return monoContainers;
        }

        /// <summary>
        /// ��ָ�� <see cref="GameObject"/> �� <see cref="MonoBehaviour"/> ����ת��Ϊ <see cref="MonoContainer"/>��
        /// </summary>
        public static MonoContainer[] MonosContainerMacro(GameObject gameObject) => MonosContainerMacro(gameObject.GetMonoComponents());


        /// <summary>
        /// ������ <see cref="Component"/> ����ת��Ϊ <see cref="EngineContainer"/>��
        /// </summary>
        public static EngineContainer[] EnginesContainerMacro(Component[] engines, InquiryMachine<string, Type> containerTypes)
        {
            List<EngineContainer> containers = new();

            foreach (Component engine in engines)
            {
                if (!containerTypes.Has(engine.GetType().GetUniqueName())) continue;

                containers.Add(new(engine, containerTypes));
            }

            return containers.ToArray();
        }

        /// <summary>
        /// ��ָ�� <see cref="GameObject"/> ������ <see cref="Component"/> ����ת��Ϊ <see cref="EngineContainer"/>��
        /// </summary>
        public static EngineContainer[] EnginesContainerMacro(GameObject gameObject, InquiryMachine<string, Type> containerTypes) => 
            EnginesContainerMacro(gameObject.GetEngineComponents(), containerTypes);


        /// <summary>
        /// ������ <see cref="Component"/> ת��Ϊ�����ࡣ
        /// </summary>
        public static ObjectContainer EngineToContainer(Component engine, Type containerType)
        {
            if (!Mapping.VerifyMapping(containerType, engine.GetType()))
                throw new ArgumentException($"The component does not match the target type of the container. component: {engine.GetType()}, container: {Mapping.GetMapTargetType(containerType, true)}");

            return Activator.CreateInstance(containerType, engine as object) as ObjectContainer;
        }

        /// <summary>
        /// ������ <see cref="Component"/> ת��Ϊ�����ࡣ
        /// </summary>
        public static ObjectContainer EngineToContainer(Component engine, InquiryMachine<string, Type> containerTypes) =>
            EngineToContainer(engine, containerTypes.Inquiry(engine.GetType().GetUniqueName()));

        /// <summary>
        /// ������ <see cref="Component"/> ת��Ϊ�����ࡣ
        /// </summary>
        public static ObjectContainer EngineToContainer(Component engine) =>
            EngineToContainer(engine, builtinEngineConts);


        /// <summary>
        /// ͨ�� Json ת��Ϊ�����ࡣ
        /// </summary>
        public static ObjectContainer JsonToContainer(string json, Type containerType) =>
            IFJson.FromJson(json, containerType) as ObjectContainer;

        /// <summary>
        /// ͨ�� Json ת��Ϊ�����ࡣ
        /// </summary>
        public static ObjectContainer JsonToContainer(string json, string uniqueName, InquiryMachine<string, Type> containerTypes) =>
            JsonToContainer(json, containerTypes.Inquiry(uniqueName));

        /// <summary>
        /// ͨ�� Json ת��Ϊ�����ࡣ
        /// </summary>
        public static ObjectContainer JsonToContainer(string json, string uniqueName) =>
            JsonToContainer(json, uniqueName, builtinEngineConts);

        

        /// <summary>
        /// ������ <see cref="MonoContainer"/> ȡ���� <see cref="MonoBehaviour"/>��
        /// </summary>
        public static void ContainersToMonos(MonoBehaviour[] monos, MonoContainer[] monoContainers)
        {
            Dictionary<string, List<MonoBehaviour>> classifyMonos = ReflectionMacro.ClassifyWithUniqueName(monos);
            Dictionary<string, List<MonoContainer>> classifyMonoContainers = ClassifyContainers(monoContainers);

            foreach (KeyValuePair<string, List<MonoBehaviour>> classifyMono in classifyMonos)
            {
                if (!classifyMonoContainers.ContainsKey(classifyMono.Key)) continue;

                FetchedMacro(classifyMono.Value.ToArray(), classifyMonoContainers[classifyMono.Key].ToArray());
            }
        }

        /// <summary>
        /// ������ <see cref="MonoContainer"/> ȡ����ָ�� <see cref="GameObject"/> �� <see cref="MonoBehaviour"/>��
        /// </summary>
        public static void ContainersToMonos(GameObject gameObject, MonoContainer[] monoContainers) => 
            ContainersToMonos(gameObject.GetMonoComponents(), monoContainers);
        
        
        /// <summary>
        /// ������ <see cref="MonoContainer"/> ȡ���� <see cref="MonoBehaviour"/>��
        /// </summary>
        public static void ContainersToEngines(Component[] engines, EngineContainer[] engineContainers, InquiryMachine<string, Type> containerTypes)
        {
            Dictionary<string, List<Component>> classifyEngines = ReflectionMacro.ClassifyWithUniqueName(engines);
            Dictionary<string, List<EngineContainer>> classifyEngineContainers = ClassifyContainers(engineContainers);

            foreach (KeyValuePair<string, List<Component>> classifyEngine in classifyEngines)
            {
                if (!classifyEngineContainers.ContainsKey(classifyEngine.Key)) continue;

                EngineFetchedMacro(classifyEngine.Value.ToArray(), classifyEngineContainers[classifyEngine.Key].ToArray(), containerTypes);
            }
        }

        /// <summary>
        /// ������ <see cref="MonoContainer"/> ȡ����ָ�� <see cref="GameObject"/> �� <see cref="MonoBehaviour"/>��
        /// </summary>
        public static void ContainersToEngines(GameObject gameObject, EngineContainer[] engineContainers, InquiryMachine<string, Type> containerTypes) => 
            ContainersToEngines(gameObject.GetEngineComponents(), engineContainers, containerTypes);



        public static GameObjectContainer[] ChildToContainers(GameObject root, InquiryMachine<string, Type> containerTypes)
        {
            GameObjectContainer[] containers = new GameObjectContainer[root.transform.childCount];

            for (int index = 0; index < root.transform.childCount; index++) 
                containers[index] = new(root.transform.GetChild(index).gameObject, containerTypes);

            return containers;
        }

        public static GameObjectContainer[] ChildToContainers(GameObject root) =>
            ChildToContainers(root, builtinEngineConts);

        public static void ContainersToChild(GameObject root, GameObjectContainer[] containers, InquiryMachine<string, Type> containerTypes)
        {
            for (int index = 0; index < root.transform.childCount; index++) 
                containers[index].Fetched(root.transform.GetChild(index).gameObject, containerTypes);
        }

        public static void ContainersToChild(GameObject root, GameObjectContainer[] containers) =>
            ContainersToChild(root, containers, builtinEngineConts);
    }
}
