using FTGAMEStudio.InitialFramework.Collections.Generic;
using FTGAMEStudio.InitialFramework.ExtensionMethods;
using System;
using UnityEngine;

namespace FTGAMEStudio.InitialSolution.Prefabbing
{
    /// <summary>
    /// �־û����ߣ�����ʵ�� <see cref="GameObject"/> �־û���
    /// 
    /// <para>������˵��������ʵ������ <see cref="Component"/> �� <see cref="MonoBehaviour"/> �ĳ־û���</para>
    /// </summary>
    public static class PrefabbingPipeline
    {
        /// <summary>
        /// ��ȡָ�� <see cref="GameObject"/> ������ <see cref="MonoBehaviour"/> ����� <see cref="MonoInfo"/>��
        /// </summary>
        public static MonoInfo[] GetInfosWithGameObject(GameObject gameObject) =>
            MonoProcessing.GetInfos(gameObject.GetMonoComponents());

        /// <summary>
        /// ��ȡָ�� <see cref="GameObject"/> ���������� <see cref="Component"/> ����� <see cref="EngineInfo"/>��
        /// </summary>
        public static EngineInfo[] GetInfosWithGameObject(GameObject gameObject, InquiryMachine<string, Type> containerTypes) =>
            EngineProcessing.GetInfos(gameObject.GetEngineComponents(), containerTypes);


        /// <summary>
        /// ��ָ�� <see cref="MonoInfo"/> ����ȫ��ȡ����ָ�� <see cref="GameObject"/>��
        /// </summary>
        public static void FetchedInfoWithGameObject(GameObject gameObject, MonoInfo[] monos) =>
            MonoProcessing.FetchedWithMonos(gameObject.GetMonoComponents(), monos);

        /// <summary>
        /// ��ָ�� <see cref="EngineInfo"/> ����ȫ��ȡ����ָ�� <see cref="GameObject"/>��
        /// </summary>
        public static void FetchedInfoWithGameObject(GameObject gameObject, EngineInfo[] engines, InquiryMachine<string, Type> containerTypes) =>
            EngineProcessing.FetchedWithEngines(gameObject.GetEngineComponents(), engines, containerTypes);



        /// <summary>
        /// ���ȡ <see cref="GameObject"/> �� <see cref="GameObjectInfo"/>��
        /// <br>�����������Ӷ���������</br>
        /// 
        /// <para>����ܵ�������������ջ�����</para>
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
        /// ��ȡ�� <see cref="GameObject"/> �� <see cref="GameObjectInfo"/>��
        /// <br>�����������Ӷ���������</br>
        /// 
        /// <para>����ܵ�������������ջ�����</para>
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
