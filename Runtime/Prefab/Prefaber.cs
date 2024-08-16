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
        /// �������Ӷ��󴢴浽 <see cref="Prefab"/>��
        /// </summary>
        public void ToPrefab();
        /// <summary>
        /// ���� <see cref="Prefab"/> �Ķ���
        /// 
        /// <para>��ע�⣬���������ȵ��� Read ������</para>
        /// </summary>
        public void LoadPrefab();

        /// <summary>
        /// ���������ڶ���ʵ������־û�����
        /// </summary>
        public GameObject SingleInstantiate();
        public GameObject[] MultipleInstantiate(int count);

        /// <summary>
        /// ���ٶ�����־û����ݡ�
        /// 
        /// <para>�����ֱ��ʹ�� <see cref="Object.Destroy(Object)"/> ���� <see cref="GameObject"/> ��ô���ĳ־û����ݲ��ᱻ�Ƴ�����������һ������ʱ����ʵ������</para>
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
