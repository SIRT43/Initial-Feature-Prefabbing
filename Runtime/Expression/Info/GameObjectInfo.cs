using FTGAMEStudio.InitialFramework.Collections.Generic;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FTGAMEStudio.InitialSolution.Prefabbing
{
    /// <summary>
    /// <see cref="GameObject"/> 表达基类，用于储存对象信息或覆盖对象信息。
    /// 
    /// <para>本类仅支持浅操作，如果您希望使用深操作，请使用 <see cref="PrefabbingPipeline"/>。</para>
    /// </summary>
    [Serializable]
    public class GameObjectInfo : ObjectExpression<GameObject>, IEngineInfo<GameObject>
    {
        public EngineInfo[] engines;
        public MonoInfo[] monos;

        [Space]
        [SerializeReference] public List<GameObjectInfo> child = new();

        /// <summary>
        /// 最后一次 <see cref="Credited(GameObject)"/> 或 <see cref="Fetched(GameObject)"/> 的 <see cref="GameObject"/>。
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
