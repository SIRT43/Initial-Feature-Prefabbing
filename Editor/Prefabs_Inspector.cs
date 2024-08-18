#if UNITY_EDITOR
using FTGAMEStudio.InitialSolution.Persistence;
using UnityEditor;

namespace FTGAMEStudio.InitialSolution.Prefabbing
{
    [CustomEditor(typeof(Prefabs))]
    public class Prefabs_Inspector : IPersistableObject_Inspector { }
}
#endif