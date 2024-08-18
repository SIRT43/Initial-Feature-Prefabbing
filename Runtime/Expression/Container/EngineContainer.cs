using FTGAMEStudio.InitialFramework.Reflection;
using System;
using UnityEngine;

namespace FTGAMEStudio.InitialSolution.Prefabbing
{
    /// <summary>
    /// �����������࣬���ڴ������� <see cref="Component"/> �����ݡ�
    /// 
    /// <para>
    /// ΪʲôҪʹ�ô��ࣿ
    /// <br>���� Unity ���������л����� <see cref="Component"/> �����Ľ��������ʹ�� <see cref="Mapping"/> �����</br>
    /// <br>ͨ������ <see cref="Mapping"/> ��װ��������ʵ����ͨ���������ֶλ����Դ���򸲸� ���� <see cref="Component"/> ��ͬ���ֶλ����ԡ�</br>
    /// </para>
    /// </summary>
    [MapContainer(typeof(Component)), Serializable]
    public abstract class EngineContainer<T> : ObjectExpression<T> where T : Component
    {
        protected EngineContainer(T target) : base(target) { }
    }
}
