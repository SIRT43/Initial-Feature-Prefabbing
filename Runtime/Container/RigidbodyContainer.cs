using FTGAMEStudio.InitialFramework.Reflection;
using System;
using UnityEngine;

namespace FTGAMEStudio.InitialSolution.Prefabbing
{
    [MapContainer(typeof(Rigidbody)), Serializable]
    public class RigidbodyContainer : ObjectContainer<Rigidbody>
    {
        public Vector3 velocity;
        public Vector3 angularVelocity;

        public RigidbodyContainer(Rigidbody target) : base(target) { }
    }
}
