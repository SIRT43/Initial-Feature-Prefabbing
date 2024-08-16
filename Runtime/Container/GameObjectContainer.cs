using System;
using UnityEngine;

namespace FTGAMEStudio.InitialSolution.Prefabbing
{
    [Serializable]
    public class GameObjectContainer : ObjectContainer<GameObject>
    {
        /// <summary>
        /// 最后一次 <see cref="Credited(GameObject)"/> 或 <see cref="Fetched(GameObject)"/> 的 <see cref="GameObject"/>。
        /// </summary>
        public GameObject Target { get; private set; }

        public TransformContainer transform;
        public MonoContainer[] monos;

        [Space]
        [SerializeField] private bool hasRigidbody;
        public RigidbodyContainer rigidbody;

        public bool HasRigidbody => hasRigidbody;

        public GameObjectContainer(GameObject target) : base(target) { }

        public override void Credited(GameObject target)
        {
            Target = target;

            name = target.name;

            transform = new(target.transform);
            monos = PrefabbingPipeline.MonosToContainers(target);

            hasRigidbody = target.TryGetComponent(out Rigidbody rigidbody);
            if (hasRigidbody) this.rigidbody = new(rigidbody);
        }

        public override void Fetched(GameObject target)
        {
            Target = target;

            target.name = name;

            transform.Fetched(target.transform);
            PrefabbingPipeline.ContainersToMonos(target, monos);

            if (!hasRigidbody) return;
            if (!target.TryGetComponent(out Rigidbody rigidbody)) return;

            this.rigidbody.Fetched(rigidbody);
        }
    }
}
