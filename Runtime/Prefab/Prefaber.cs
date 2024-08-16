using FTGAMEStudio.InitialFramework.IO;
using FTGAMEStudio.InitialSolution.Persistence;
using FTGAMEStudio.InitialFramework.Replicator;
using System.Collections.Generic;
using UnityEngine;

namespace FTGAMEStudio.InitialSolution.Prefabbing
{
    public interface IPrefaber : IGameObjectReplicator
    {
        /// <summary>
        /// 将所有子对象储存到 <see cref="Prefab"/>。
        /// </summary>
        public void ToPrefab();
        /// <summary>
        /// 加载 <see cref="Prefab"/> 的对象。
        /// 
        /// <para>请注意，本方法会先调用 Read 方法。</para>
        /// </summary>
        public void LoadPrefab();

        /// <summary>
        /// 本方法将在对象实例化后持久化对象。
        /// </summary>
        public GameObject SingleInstantiate();
        public GameObject[] MultipleInstantiate(int count);

        /// <summary>
        /// 销毁对象与持久化数据。
        /// 
        /// <para>如果您直接使用 <see cref="Object.Destroy(Object)"/> 销毁 <see cref="GameObject"/> 那么它的持久化数据不会被移除，它将在下一个加载时机被实例化。</para>
        /// </summary>
        public void DestroyInstantiate(GameObjectContainer target);
    }

    [AddComponentMenu("Initial Solution/Prefabbing/Prefaber")]
    public class Prefaber : MonoBehaviour, IPersistableObject, IPrefaber
    {
        public Prefab data;
        public GameObject original;


        public virtual GameObject SingleObject() => Instantiate(original, transform);

        public virtual GameObject[] MultipleObject(int count)
        {
            List<GameObject> gameObjects = new();

            for (int current = 0; current < count; current++) gameObjects.Add(SingleObject());

            return gameObjects.ToArray();
        }


        public virtual void ToPrefab()
        {
            data.gameObjects = new(PrefabbingPipeline.ChildToContainers(gameObject));
            data.Write();
        }

        public virtual void LoadPrefab()
        {
            data.Read();

            if (transform.childCount < data.gameObjects.Count) MultipleObject(data.gameObjects.Count - transform.childCount);

            PrefabbingPipeline.ContainersToChild(gameObject, data.gameObjects.ToArray());
        }

        public virtual GameObject SingleInstantiate()
        {
            GameObject gameObject = SingleObject();
            data.gameObjects.Add(new(gameObject));

            data.Write();

            return gameObject;
        }

        public virtual GameObject[] MultipleInstantiate(int count)
        {
            List<GameObject> gameObjects = new();

            for (int current = 0; current < count; current++) gameObjects.Add(SingleInstantiate());

            return gameObjects.ToArray();
        }

        public virtual void DestroyInstantiate(GameObjectContainer target)
        {
            Destroy(target.Target);
            data.gameObjects.Remove(target);

            data.Write();
        }



        public FilePath FileLocation => data.FileLocation;

        public bool Read() => data.Read();
        public bool Write() => data.Write();
        public bool Delete() => data.Delete();
#if UNITY_EDITOR
        public void DisplayInExplorer() => data.DisplayInExplorer();
#endif
    }
}
