using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFClient.Common
{
    public class ResponseModel
    {
        private int flag = 0;
        private object content;
        private string msg;

        public ResponseModel()
        {

        }

        public ResponseModel(int flag,string msg)
        {
            Flag = flag;
            Msg = msg;
        }

        public int Flag
        {
            get
            {
                return flag;
            }

            set
            {
                flag = value;
            }
        }

        public string Msg
        {
            get
            {
                return msg;
            }

            set
            {
                msg = value;
            }
        }

        public object Content
        {
            get
            {
                return content;
            }

            set
            {
                content = value;
            }
        }


        public T ResModel<T>(string json = "")
        {
            json = json == "" ? content.ToString() : json;
            return JsonConvert.DeserializeObject<T>(json);
        }

        public List ResModeList<List>(string json = "")
        {
            json = json == "" ? content.ToString() : json;
            return JsonConvert.DeserializeObject<List>(json);
        }
    }
}
