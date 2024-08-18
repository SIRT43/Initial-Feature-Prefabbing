using FTGAMEStudio.InitialFramework.Collections.Generic;
using System;

namespace FTGAMEStudio.InitialSolution.Prefabbing
{
    public interface IPrefabbing
    {
        public bool DeepOperation { get; set; }

        /// <summary>
        /// ���л���
        /// </summary>
        public void Enprefab(InquiryMachine<string, Type> containerTypes);
        public void Enprefab();

        /// <summary>
        /// �����л���
        /// </summary>
        public void Deprefab(InquiryMachine<string, Type> containerTypes);
        public void Deprefab();
    }
}
