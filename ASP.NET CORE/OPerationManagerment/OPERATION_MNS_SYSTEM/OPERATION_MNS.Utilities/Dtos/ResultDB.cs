using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace OPERATION_MNS.Utilities.Dtos
{
    [Serializable]
    public class ResultDB
    {
        private int returnInt = 0;
        private string returnString = string.Empty;
        private object obj;
        private DataSet returnDataSet = new DataSet();

        public int ReturnInt
        {
            get
            {
                return returnInt;
            }
            set
            {
                returnInt = value;
            }
        }
        public string ReturnString
        {
            get
            {
                return returnString;
            }
            set
            {
                returnString = value;
            }
        }
        public DataSet ReturnDataSet
        {
            get
            {
                return returnDataSet;
            }
            set
            {
                returnDataSet = value.Copy();
            }
        }

        public object Obj
        {
            get => obj;
            set => obj = value;
        }
    }
}
