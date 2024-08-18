using FTGAMEStudio.InitialFramework.ExtensionMethods;
using FTGAMEStudio.InitialFramework.Reflection;
using System;

namespace FTGAMEStudio.InitialSolution.Prefabbing
{
    public interface IObjectExpression<T> where T : UnityEngine.Object
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

    public interface IObjectExpression
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

    /// <summary>
    /// 对象表达基类，用于储存对象信息或覆盖对象信息。
    /// </summary>
    [MapContainer(typeof(UnityEngine.Object)), Serializable]
    public abstract class ObjectExpression : IObjectExpression
    {
        /// <summary>
        /// 当前正在表达对象的唯一名称。
        /// </summary>
        [NonMappable] public string uniqueName;
        public string name;

        protected ObjectExpression() { }
        protected ObjectExpression(UnityEngine.Object target) => Credited(target);


        public virtual void Credited(UnityEngine.Object target)
        {
            uniqueName = target.GetType().GetUniqueName();
            Mapping.ReverseMapVariables(this, ref target);
        }


        public virtual void Fetched(UnityEngine.Object target) =>
            Mapping.MapVariables(this, ref target);
    }

    /// <summary>
    /// 对象表达基类，用于储存对象信息或覆盖对象信息。
    /// </summary>
    [MapContainer(typeof(UnityEngine.Object)), Serializable]
    public abstract class ObjectExpression<T> : ObjectExpression, IObjectExpression<T> where T : UnityEngine.Object
    {
        protected ObjectExpression() { }
        protected ObjectExpression(T target) : base(target) { }


        public override void Credited(UnityEngine.Object target) => Credited(target as T);
        public override void Fetched(UnityEngine.Object target) => Fetched(target as T);


        public virtual void Credited(T target)
        {
            uniqueName = target.GetType().GetUniqueName();
            Mapping.ReverseMapVariables(this, ref target);
        }


        public virtual void Fetched(T target) =>
            Mapping.MapVariables(this, ref target);
    }
}
