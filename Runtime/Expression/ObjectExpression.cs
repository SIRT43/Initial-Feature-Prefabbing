using FTGAMEStudio.InitialFramework.ExtensionMethods;
using FTGAMEStudio.InitialFramework.Reflection;
using System;

namespace FTGAMEStudio.InitialSolution.Prefabbing
{
    public interface IObjectExpression<T> where T : UnityEngine.Object
    {
        /// <summary>
        /// �������
        /// </summary>
        public void Credited(T target);
        /// <summary>
        /// ȡ�����ݡ�
        /// </summary>
        public void Fetched(T target);
    }

    public interface IObjectExpression
    {
        /// <summary>
        /// �������
        /// </summary>
        public void Credited(UnityEngine.Object target);
        /// <summary>
        /// ȡ�����ݡ�
        /// </summary>
        public void Fetched(UnityEngine.Object target);
    }

    /// <summary>
    /// ��������࣬���ڴ��������Ϣ�򸲸Ƕ�����Ϣ��
    /// </summary>
    [MapContainer(typeof(UnityEngine.Object)), Serializable]
    public abstract class ObjectExpression : IObjectExpression
    {
        /// <summary>
        /// ��ǰ���ڱ������Ψһ���ơ�
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
    /// ��������࣬���ڴ��������Ϣ�򸲸Ƕ�����Ϣ��
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
