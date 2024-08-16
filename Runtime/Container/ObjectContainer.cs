using FTGAMEStudio.InitialFramework.ExtensionMethods;
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

    public interface IObjectContainer
    {
        /// <summary>
        /// 记入对象。
        /// </summary>
        public void Credited(UnityEngine.Object target);
        /// <summary>
        /// 取出数据。
        /// </summary>
        public void Fetched(UnityEngine.Object target);
    }

    [MapContainer(typeof(UnityEngine.Object)), Serializable]
    public abstract class ObjectContainer : IObjectContainer
    {
        [NonMappable] public string uniqueName;
        public string name;

        protected ObjectContainer() { }
        protected ObjectContainer(UnityEngine.Object target) => Credited(target);

        public virtual void Credited(UnityEngine.Object target)
        {
            uniqueName = target.GetType().GetUniqueName();
            Mapping.ReverseMapVariables(this, target);
        }

        public virtual void Fetched(UnityEngine.Object target)
        {
            Mapping.MapVariables(this, target);
        }
    }

    [MapContainer(typeof(UnityEngine.Object)), Serializable]
    public abstract class ObjectContainer<T> : ObjectContainer, IObjectContainer<T> where T : UnityEngine.Object
    {
        protected ObjectContainer() { }
        protected ObjectContainer(T target) : base(target) { }

        public override void Credited(UnityEngine.Object target) => Credited(target as T);
        public override void Fetched(UnityEngine.Object target) => Fetched(target as T);

        public virtual void Credited(T target)
        {
            uniqueName = target.GetType().GetUniqueName();
            Mapping.ReverseMapVariables(this, target);
        }

        public virtual void Fetched(T target)
        {
            Mapping.MapVariables(this, target);
        }
    }
}
