using FTGAMEStudio.InitialFramework.Collections.Generic;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FTGAMEStudio.InitialSolution.Prefabbing
{
    [Serializable]
    public class GameObjectContainer : ObjectContainer<GameObject>, IEngineContainer<GameObject>
    {
        public EngineContainer[] engines;
        public MonoContainer[] monos;

        [Space]
        [SerializeReference] public List<GameObjectContainer> child = new();

        /// <summary>
        /// 最后一次 <see cref="Credited(GameObject)"/> 或 <see cref="Fetched(GameObject)"/> 的 <see cref="GameObject"/>。
        /// </summary>
        public GameObject Target { get; private set; }

        public GameObjectContainer(GameObject target, InquiryMachine<string, Type> containerTypes) => Credited(target, containerTypes);
        public GameObjectContainer(GameObject target) : base(target) { }

        public void Credited(GameObject target, InquiryMachine<string, Type> containerTypes)
        {
            Target = target;

            name = target.name;

            engines = PrefabbingPipeline.EnginesContainerMacro(target, containerTypes);
            monos = PrefabbingPipeline.CreditedMacro(target);
        }

        public void Fetched(GameObject target, InquiryMachine<string, Type> containerTypes)
        {
            Target = target;
            target.name = name;

            PrefabbingPipeline.ContainersToEngines(target, engines, containerTypes);
            PrefabbingPipeline.ContainersToMonos(target, monos);
        }

        public override void Credited(GameObject target) => Credited(target, PrefabbingPipeline.builtinEngineConts);
        public override void Fetched(GameObject target) => Fetched(target, PrefabbingPipeline.builtinEngineConts);
    }
}
