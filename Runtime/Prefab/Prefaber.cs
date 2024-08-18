using FTGAMEStudio.InitialFramework.Collections.Generic;
using FTGAMEStudio.InitialFramework.IO;
using FTGAMEStudio.InitialFramework.Replicator;
using FTGAMEStudio.InitialSolution.Persistence;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FTGAMEStudio.InitialSolution.Prefabbing
{
    public interface IPrefaber
    {
        public GameObjectInfo Instantiate(InquiryMachine<string, Type> containerTypes);
        public GameObjectInfo Instantiate();
    }

    [AddComponentMenu("Initial Solution/Prefabbing/Prefaber")]
    public class Prefaber : MonoBehaviour, IPrefabbing, IPersistableObject, IPrefaber
    {
        public GameObjectReplicator replicator;
        public Prefabs prefabs;

        [SerializeField] private bool deepOperation = false;
        public bool DeepOperation { get => deepOperation; set => deepOperation = value; }


        /// <summary>
        /// 获取调用 <see cref="GameObjectReplicator.SingleObject"/> 的 <see cref="GameObjectInfo"/> 并 <see cref="Write"/>。
        /// </summary>
        public virtual GameObjectInfo Instantiate(InquiryMachine<string, Type> containerTypes)
        {
            GameObject gameObject = replicator.SingleObject();

            GameObjectInfo gameObjectInfo = DeepOperation ?
                PrefabbingPipeline.DeepGetGameObjectInfo(gameObject, containerTypes)
                :
                new(gameObject, containerTypes);

            prefabs.gameObjects.Add(gameObjectInfo);
            Write();

            return gameObjectInfo;
        }


        /// <summary>
        /// 序列化 <see cref="GameObjectInfo"/> 并 <see cref="Write"/>。
        /// </summary>
        public virtual void Enprefab(InquiryMachine<string, Type> containerTypes)
        {
            if (replicator.Parent == null)
                throw new NullReferenceException("Enprefab could not be made because the root transform in replicator could not be found.");

            List<GameObjectInfo> infos = new();

            foreach (Transform child in replicator.Parent)
            {
                GameObjectInfo gameObjectInfo = DeepOperation ?
                    PrefabbingPipeline.DeepGetGameObjectInfo(child.gameObject, containerTypes)
                    :
                    new(child.gameObject, containerTypes);

                infos.Add(gameObjectInfo);
            }

            prefabs.gameObjects = infos;

            Write();
        }

        /// <summary>
        /// <see cref="Read"/> 并反序列化 <see cref="GameObjectInfo"/>。
        /// </summary>
        public virtual void Deprefab(InquiryMachine<string, Type> containerTypes)
        {
            if (replicator.Parent == null)
                throw new NullReferenceException("Deprefab could not be made because the root transform in replicator could not be found.");


            Read();

            int dap = prefabs.gameObjects.Count - replicator.Parent.childCount;
            if (dap > 0) replicator.MultipleObject(dap);


            for (int index = 0; index < prefabs.gameObjects.Count; index++)
            {
                GameObject gameObject = replicator.Parent.GetChild(index).gameObject;

                if (DeepOperation) PrefabbingPipeline.DeepFetchedGameObjectInfo(gameObject, prefabs.gameObjects[index], containerTypes);
                else prefabs.gameObjects[index].Fetched(gameObject, containerTypes);
            }
        }


        public virtual GameObjectInfo Instantiate() => Instantiate(EngineProcessing.builtinEngineConts);
        public virtual void Enprefab() => Enprefab(EngineProcessing.builtinEngineConts);
        public virtual void Deprefab() => Deprefab(EngineProcessing.builtinEngineConts);



        public virtual UnityFile FileLocation => prefabs.FileLocation;

        public virtual bool Read() => prefabs.Read();
        public virtual void Write() => prefabs.Write();
        public virtual bool Delete() => prefabs.Delete();

#if UNITY_EDITOR
        public virtual void DisplayInExplorer() => prefabs.DisplayInExplorer();
#endif
    }
}
