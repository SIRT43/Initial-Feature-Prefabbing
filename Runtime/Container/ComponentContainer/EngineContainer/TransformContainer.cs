using FTGAMEStudio.InitialFramework.Reflection;
using System;
using UnityEngine;

namespace FTGAMEStudio.InitialSolution.Prefabbing
{
    /// <summary>
    /// ∏®÷˙¿‡£¨”√”⁄¥¢¥Ê <see cref="Transform"/>°£
    /// </summary>
    [MapContainer(typeof(Transform)), Serializable]
    public class TransformContainer : EngineContainer<Transform>
    {
        public Vector3 position;
        public Vector3 eulerAngles;

        public TransformContainer(Transform transform) : base(transform) { }
    }
}
