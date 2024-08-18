using FTGAMEStudio.InitialFramework.IO;
using FTGAMEStudio.InitialSolution.Persistence;
using System.Collections.Generic;
using UnityEngine;

namespace FTGAMEStudio.InitialSolution.Prefabbing
{
    [CreateAssetMenu(fileName = "New Prefabs", menuName = "Initial Solution/Prefabs")]
    public class Prefabs : PersistableObject
    {
        [SerializeField] protected UnityFile fileLocation = new("Prefabbing", "New_Prefabs", FilenameExtension.gamobj);
        public override UnityFile FileLocation => fileLocation;

        public List<GameObjectInfo> gameObjects;
    }
}
