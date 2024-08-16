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
    /// 持久化管线，用于实现 <see cref="GameObject"/> 持久化。
    /// 
    /// <para>具体来说，本管线仅实现 <see cref="Transform"/> 与 <see cref="MonoBehaviour"/> 的持久化。</para>
    /// </summary>
    public static class PrefabbingPipeline
    {
        public static readonly InquiryMachine<string, Type> builtinEngineConts = new(Mapping.ClassifyMapTargetType(true, typeof(TransformContainer), typeof(RigidbodyContainer)));



        /// <summary>
        /// 按 <see cref="ObjectContainer.uniqueName"/> 分类。
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
        /// 记入宏，用于批量记入。
        /// </summary>
        public static void CreditedMacro(UnityEngine.Object[] objects, ObjectContainer[] monoContainers)
        {
            if (objects.Length != monoContainers.Length)
                throw new ArgumentException($"The specified MonoContainers cannot be converted to Monos because of the asymmetric parameters. monos: {objects.Length}, monoContainers: {monoContainers.Length}", nameof(monoContainers));

            for (int index = 0; index < objects.Length; index++) monoContainers[index].Credited(objects[index]);
        }

        /// <summary>
        /// 记入宏，用于批量记入。
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
        /// 记入宏，用于批量记入。
        /// </summary>
        public static void EngineCreditedMacro(Component[] objects, EngineContainer[] monoContainers, InquiryMachine<string, Type> containerTypes)
        {
            if (objects.Length != monoContainers.Length)
                throw new ArgumentException($"The specified MonoContainers cannot be converted to Monos because of the asymmetric parameters. monos: {objects.Length}, monoContainers: {monoContainers.Length}", nameof(monoContainers));

            for (int index = 0; index < objects.Length; index++) monoContainers[index].Credited(objects[index], containerTypes);
        }



        /// <summary>
        /// 取出宏，用于批量取出。
        /// </summary>
        public static void FetchedMacro(UnityEngine.Object[] objects, ObjectContainer[] monoContainers)
        {
            if (objects.Length != monoContainers.Length)
                throw new ArgumentException($"The specified MonoContainers cannot be converted to Monos because of the asymmetric parameters. monos: {objects.Length}, monoContainers: {monoContainers.Length}", nameof(monoContainers));

            for (int index = 0; index < objects.Length; index++) monoContainers[index].Fetched(objects[index]);
        }

        /// <summary>
        /// 取出宏，用于批量取出。
        /// 
        /// <para>不推荐使用此方法，请使用 <see cref="ContainersToMonos(GameObject, MonoContainer[])"/>。</para>
        /// </summary>
        public static void FetchedMacro(GameObject gameObject, MonoContainer[] monoContainers) =>
            FetchedMacro(gameObject.GetMonoComponents(), monoContainers);

        /// <summary>
        /// 取出宏，用于批量取出。
        /// </summary>
        public static void EngineFetchedMacro(Component[] objects, EngineContainer[] monoContainers, InquiryMachine<string, Type> containerTypes)
        {
            if (objects.Length != monoContainers.Length)
                throw new ArgumentException($"The specified MonoContainers cannot be converted to Monos because of the asymmetric parameters. monos: {objects.Length}, monoContainers: {monoContainers.Length}", nameof(monoContainers));

            for (int index = 0; index < objects.Length; index++) monoContainers[index].Fetched(objects[index], containerTypes);
        }



        /// <summary>
        /// 将 <see cref="MonoBehaviour"/> 批量转换为 <see cref="MonoContainer"/>。
        /// </summary>
        public static MonoContainer[] MonosContainerMacro(MonoBehaviour[] monos)
        {
            MonoContainer[] monoContainers = new MonoContainer[monos.Length];

            for (int index = 0; index < monos.Length; index++)
                monoContainers[index] = new(monos[index]);

            return monoContainers;
        }

        /// <summary>
        /// 将指定 <see cref="GameObject"/> 的 <see cref="MonoBehaviour"/> 批量转换为 <see cref="MonoContainer"/>。
        /// </summary>
        public static MonoContainer[] MonosContainerMacro(GameObject gameObject) => MonosContainerMacro(gameObject.GetMonoComponents());


        /// <summary>
        /// 将引擎 <see cref="Component"/> 批量转换为 <see cref="EngineContainer"/>。
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
        /// 将指定 <see cref="GameObject"/> 的引擎 <see cref="Component"/> 批量转换为 <see cref="EngineContainer"/>。
        /// </summary>
        public static EngineContainer[] EnginesContainerMacro(GameObject gameObject, InquiryMachine<string, Type> containerTypes) => 
            EnginesContainerMacro(gameObject.GetEngineComponents(), containerTypes);


        /// <summary>
        /// 将引擎 <see cref="Component"/> 转换为辅助类。
        /// </summary>
        public static ObjectContainer EngineToContainer(Component engine, Type containerType)
        {
            if (!Mapping.VerifyMapping(containerType, engine.GetType()))
                throw new ArgumentException($"The component does not match the target type of the container. component: {engine.GetType()}, container: {Mapping.GetMapTargetType(containerType, true)}");

            return Activator.CreateInstance(containerType, engine as object) as ObjectContainer;
        }

        /// <summary>
        /// 将引擎 <see cref="Component"/> 转换为辅助类。
        /// </summary>
        public static ObjectContainer EngineToContainer(Component engine, InquiryMachine<string, Type> containerTypes) =>
            EngineToContainer(engine, containerTypes.Inquiry(engine.GetType().GetUniqueName()));

        /// <summary>
        /// 将引擎 <see cref="Component"/> 转换为辅助类。
        /// </summary>
        public static ObjectContainer EngineToContainer(Component engine) =>
            EngineToContainer(engine, builtinEngineConts);


        /// <summary>
        /// 通过 Json 转换为辅助类。
        /// </summary>
        public static ObjectContainer JsonToContainer(string json, Type containerType) =>
            IFJson.FromJson(json, containerType) as ObjectContainer;

        /// <summary>
        /// 通过 Json 转换为辅助类。
        /// </summary>
        public static ObjectContainer JsonToContainer(string json, string uniqueName, InquiryMachine<string, Type> containerTypes) =>
            JsonToContainer(json, containerTypes.Inquiry(uniqueName));

        /// <summary>
        /// 通过 Json 转换为辅助类。
        /// </summary>
        public static ObjectContainer JsonToContainer(string json, string uniqueName) =>
            JsonToContainer(json, uniqueName, builtinEngineConts);

        

        /// <summary>
        /// 批量将 <see cref="MonoContainer"/> 取出到 <see cref="MonoBehaviour"/>。
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
        /// 批量将 <see cref="MonoContainer"/> 取出到指定 <see cref="GameObject"/> 的 <see cref="MonoBehaviour"/>。
        /// </summary>
        public static void ContainersToMonos(GameObject gameObject, MonoContainer[] monoContainers) => 
            ContainersToMonos(gameObject.GetMonoComponents(), monoContainers);
        
        
        /// <summary>
        /// 批量将 <see cref="MonoContainer"/> 取出到 <see cref="MonoBehaviour"/>。
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
        /// 批量将 <see cref="MonoContainer"/> 取出到指定 <see cref="GameObject"/> 的 <see cref="MonoBehaviour"/>。
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
