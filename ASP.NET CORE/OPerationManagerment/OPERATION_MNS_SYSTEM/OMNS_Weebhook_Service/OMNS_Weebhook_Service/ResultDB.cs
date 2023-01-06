using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMNS_Weebhook_Service
{
    [Serializable]
    public class ResultDB
    {
        private int returnInt = 0;
        private string returnString = string.Empty;
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
    }
}
