using FTGAMEStudio.InitialFramework.Classifying;
using FTGAMEStudio.InitialFramework.ExtensionMethods;
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
        /// <summary>
        /// 将 <see cref="MonoBehaviour"/> 按 UniqueName 分类。
        /// </summary>
        public static Dictionary<string, List<MonoBehaviour>> ClassifyMonos(MonoBehaviour[] monos)
        {
            FilterableClassifier<string, MonoBehaviour> classifier = new((MonoBehaviour mono, out string key) =>
            {
                key = mono.GetType().GetUniqueName();
                return true;
            });

            return classifier.Classify(monos);
        }

        /// <summary>
        /// 将 <see cref="PerMono"/> 按 <see cref="PerMono.uniqueName"/> 分类。
        /// </summary>
        public static Dictionary<string, List<MonoContainer>> ClassifyMonoContainers(MonoContainer[] monoContainers)
        {
            FilterableClassifier<string, MonoContainer> classifier = new((MonoContainer monoContainer, out string key) =>
            {
                key = monoContainer.uniqueName;
                return true;
            });

            return classifier.Classify(monoContainers);
        }

        /// <summary>
        /// 不推荐使用此方法，请使用 <see cref="PerToMono(MonoBehaviour[], PerMono[])"/>。
        /// </summary>
        public static void RawContainersToMonos(MonoBehaviour[] monos, MonoContainer[] monoContainers)
        {
            if (monos.Length != monoContainers.Length)
                throw new ArgumentException($"The specified PerMonos cannot be converted to Monos because of the asymmetric parameters. monos: {monos.Length}, perMonos: {monoContainers.Length}", nameof(monoContainers));

            for (int index = 0; index < monos.Length; index++)
                IFJson.FromJson(monoContainers[index].data, monos[index]);
        }

        /// <summary>
        /// 将 <see cref="MonoBehaviour[]"/> 覆盖为辅助类。
        /// </summary>
        public static void ContainersToMonos(MonoBehaviour[] monos, MonoContainer[] monoContainers)
        {
            Dictionary<string, List<MonoBehaviour>> classifyMonos = ClassifyMonos(monos);
            Dictionary<string, List<MonoContainer>> classifyPerMonos = ClassifyMonoContainers(monoContainers);

            foreach (KeyValuePair<string, List<MonoBehaviour>> classifyMono in classifyMonos)
                RawContainersToMonos(classifyMono.Value.ToArray(), classifyPerMonos[classifyMono.Key].ToArray());
        }

        public static void ContainersToMonos(GameObject gameObject, MonoContainer[] monoContainers) => ContainersToMonos(gameObject.GetMonoComponents(), monoContainers);

        /// <summary>
        /// 将指定 <see cref="GameObject"/> 的 <see cref="MonoBehaviour"/> 转换为辅助类。
        /// </summary>
        public static MonoContainer[] MonosToContainers(GameObject gameObject)
        {
            MonoBehaviour[] monos = gameObject.GetMonoComponents();
            MonoContainer[] monoContainers = new MonoContainer[monos.Length];

            for (int index = 0; index < monos.Length; index++)
                monoContainers[index] = new(monos[index]);

            return monoContainers;
        }


        public static void ContainersToChild(GameObject root, GameObjectContainer[] containers)
        {
            for (int index = 0; index < containers.Length; index++) containers[index].Fetched(root.transform.GetChild(index).gameObject);
        }

        public static GameObjectContainer[] ChildToContainers(GameObject root)
        {
            GameObjectContainer[] containers = new GameObjectContainer[root.transform.childCount];

            for (int index = 0; index < root.transform.childCount; index++) containers[index] = new(root.transform.GetChild(index).gameObject);

            return containers;
        }
    }
}
