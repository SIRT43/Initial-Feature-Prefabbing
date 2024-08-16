using FTGAMEStudio.InitialFramework.IO;
using FTGAMEStudio.InitialSolution.Persistence;
using System.Collections.Generic;
using UnityEngine;

namespace FTGAMEStudio.InitialSolution.Prefabbing
{
    [CreateAssetMenu(fileName = "New Prefab", menuName = "Initial Solution/Prefab")]
    public class Prefab : PersistableObject
    {
        [SerializeField] protected FilePath fileLocation = new("Prefabbing", "New_Prefab", FilenameExtension.gamobj);
        public override FilePath FileLocation => fileLocation;

        public List<GameObjectContainer> gameObjects;
    }
}
