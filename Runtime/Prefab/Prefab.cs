using FTGAMEStudio.InitialFramework.Collections.Generic;
using FTGAMEStudio.InitialFramework.ExtensionMethods;
using FTGAMEStudio.InitialFramework.IO;
using FTGAMEStudio.InitialSolution.Persistence;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FTGAMEStudio.InitialSolution.Prefabbing
{
    [AddComponentMenu("Initial Solution/Prefabbing/Prefab")]
    public class Prefab : PersistableBehaviour, IPrefabbing
    {
        [SerializeField] protected UnityFile fileLocation = new(UnityPathType.persistentDataPath, "Prefabbing", "New_Prefab", FilenameExtension.gamobj);
        public GameObjectInfo info;

        [SerializeField] private bool deepOperation = false;
        public bool DeepOperation { get => deepOperation; set => deepOperation = value; }


        public virtual void Enprefab(InquiryMachine<string, Type> containerTypes)
        {
            info = DeepOperation ?
            PrefabbingPipeline.DeepGetGameObjectInfo(gameObject, containerTypes)
            :
            new(gameObject, containerTypes);


            List<MonoInfo> monoInfos = new(info.monos);
            monoInfos.RemoveAll(monoInfo => monoInfo.uniqueName == GetType().GetUniqueName());

            info.monos = monoInfos.ToArray();

            Write();
        }

        public virtual void Deprefab(InquiryMachine<string, Type> containerTypes)
        {
            Read();

            if (DeepOperation) PrefabbingPipeline.DeepFetchedGameObjectInfo(gameObject, info, containerTypes);
            else info.Fetched(gameObject, containerTypes);
        }


        public virtual void Enprefab() => Enprefab(EngineProcessing.builtinEngineConts);
        public virtual void Deprefab() => Deprefab(EngineProcessing.builtinEngineConts);



        public override UnityFile FileLocation => fileLocation;
    }
}
