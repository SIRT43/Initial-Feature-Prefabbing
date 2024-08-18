using FTGAMEStudio.InitialFramework.Collections.Generic;
using FTGAMEStudio.InitialFramework.ExtensionMethods;
using System;
using UnityEngine;

namespace FTGAMEStudio.InitialSolution.Prefabbing
{
    /// <summary>
    /// 持久化管线，用于实现 <see cref="GameObject"/> 持久化。
    /// 
    /// <para>具体来说，本管线实现引擎 <see cref="Component"/> 与 <see cref="MonoBehaviour"/> 的持久化。</para>
    /// </summary>
    public static class PrefabbingPipeline
    {
        /// <summary>
        /// 获取指定 <see cref="GameObject"/> 的所有 <see cref="MonoBehaviour"/> 组件的 <see cref="MonoInfo"/>。
        /// </summary>
        public static MonoInfo[] GetInfosWithGameObject(GameObject gameObject) =>
            MonoProcessing.GetInfos(gameObject.GetMonoComponents());

        /// <summary>
        /// 获取指定 <see cref="GameObject"/> 的所有引擎 <see cref="Component"/> 组件的 <see cref="EngineInfo"/>。
        /// </summary>
        public static EngineInfo[] GetInfosWithGameObject(GameObject gameObject, InquiryMachine<string, Type> containerTypes) =>
            EngineProcessing.GetInfos(gameObject.GetEngineComponents(), containerTypes);


        /// <summary>
        /// 将指定 <see cref="MonoInfo"/> 数组全部取出到指定 <see cref="GameObject"/>。
        /// </summary>
        public static void FetchedInfoWithGameObject(GameObject gameObject, MonoInfo[] monos) =>
            MonoProcessing.FetchedWithMonos(gameObject.GetMonoComponents(), monos);

        /// <summary>
        /// 将指定 <see cref="EngineInfo"/> 数组全部取出到指定 <see cref="GameObject"/>。
        /// </summary>
        public static void FetchedInfoWithGameObject(GameObject gameObject, EngineInfo[] engines, InquiryMachine<string, Type> containerTypes) =>
            EngineProcessing.FetchedWithEngines(gameObject.GetEngineComponents(), engines, containerTypes);



        /// <summary>
        /// 深获取 <see cref="GameObject"/> 的 <see cref="GameObjectInfo"/>。
        /// <br>本方法基于子对象索引。</br>
        /// 
        /// <para>这可能导致性能问题或堆栈溢出。</para>
        /// </summary>
        public static GameObjectInfo DeepGetGameObjectInfo(GameObject root, InquiryMachine<string, Type> containerTypes)
        {
            GameObjectInfo rootInfo = new(root, containerTypes);

            if (root.transform.childCount < 1) return rootInfo;

            for (int index = 0; index < root.transform.childCount; index++)
                rootInfo.child.Add(DeepGetGameObjectInfo(root.transform.GetChild(index).gameObject, containerTypes));

            return rootInfo;
        }

        /// <summary>
        /// 深取出 <see cref="GameObject"/> 的 <see cref="GameObjectInfo"/>。
        /// <br>本方法基于子对象索引。</br>
        /// 
        /// <para>这可能导致性能问题或堆栈溢出。</para>
        /// </summary>
        public static void DeepFetchedGameObjectInfo(GameObject root, GameObjectInfo rootInfo, InquiryMachine<string, Type> containerTypes)
        {
            rootInfo.Fetched(root, containerTypes);

            if (rootInfo.child.Count < 1) return;

            for (int index = 0; index < rootInfo.child.Count; index++)
                rootInfo.child[index].Fetched(root.transform.GetChild(index).gameObject, containerTypes);
        }
    }
}
