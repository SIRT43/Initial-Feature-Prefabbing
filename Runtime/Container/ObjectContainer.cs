using FTGAMEStudio.InitialFramework.Reflection;
using System;

namespace FTGAMEStudio.InitialSolution.Prefabbing
{
    public interface IObjectContainer<T> where T : UnityEngine.Object
    {
        /// <summary>
        /// 记入对象。
        /// </summary>
        public void Credited(T target);
        /// <summary>
        /// 取出数据。
        /// </summary>
        public void Fetched(T target);
    }

    [MapContainer(typeof(UnityEngine.Object)), Serializable]
    public abstract class ObjectContainer<T> : IObjectContainer<T> where T : UnityEngine.Object
    {
        public string name;

        protected ObjectContainer(T target) => Credited(target);

        public virtual void Credited(T target) => Mapping.ReverseMapVariables(this, target);
        public virtual void Fetched(T target) => Mapping.MapVariables(this, target);
    }
}
