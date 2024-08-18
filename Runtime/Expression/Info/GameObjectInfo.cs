using FTGAMEStudio.InitialFramework.Collections.Generic;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FTGAMEStudio.InitialSolution.Prefabbing
{
    /// <summary>
    /// <see cref="GameObject"/> �����࣬���ڴ��������Ϣ�򸲸Ƕ�����Ϣ��
    /// 
    /// <para>�����֧��ǳ�����������ϣ��ʹ�����������ʹ�� <see cref="PrefabbingPipeline"/>��</para>
    /// </summary>
    [Serializable]
    public class GameObjectInfo : ObjectExpression<GameObject>, IEngineInfo<GameObject>
    {
        public EngineInfo[] engines;
        public MonoInfo[] monos;

        [Space]
        [SerializeReference] public List<GameObjectInfo> child = new();

        /// <summary>
        /// ���һ�� <see cref="Credited(GameObject)"/> �� <see cref="Fetched(GameObject)"/> �� <see cref="GameObject"/>��
        /// </summary>
        public GameObject Target { get; private set; }

        public GameObjectInfo(GameObject target, InquiryMachine<string, Type> containerTypes) => Credited(target, containerTypes);
        public GameObjectInfo(GameObject target) : base(target) { }

        public void Credited(GameObject target, InquiryMachine<string, Type> containerTypes)
        {
            Target = target;

            name = target.name;

            engines = PrefabbingPipeline.GetInfosWithGameObject(target, containerTypes);
            monos = PrefabbingPipeline.GetInfosWithGameObject(target);
        }

        public void Fetched(GameObject target, InquiryMachine<string, Type> containerTypes)
        {
            Target = target;
            target.name = name;

            PrefabbingPipeline.FetchedInfoWithGameObject(target, engines, containerTypes);
            PrefabbingPipeline.FetchedInfoWithGameObject(target, monos);
        }

        public override void Credited(GameObject target) => Credited(target, EngineProcessing.builtinEngineConts);
        public override void Fetched(GameObject target) => Fetched(target, EngineProcessing.builtinEngineConts);
    }
}
