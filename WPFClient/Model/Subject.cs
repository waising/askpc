using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFClient.Model
{
    public class Subject : ObservableObject
    {
        /// <summary>
        /// 科目：数学为M，物理为P，化学为C
        /// </summary
        private string _subjectName;
        /// <summary>
        /// 年级：7-9为初中，10-12为高中
        /// </summary>
        private int _grade;
        /// <summary>
        /// 
        /// </summary>
        private List<ImageInfo> _images;

        [JsonProperty("subject")]
        public string SubjectName
        {
            get
            {
                return _subjectName;
            }

            set
            {
                _subjectName = value;
            }
        }

        public int Grade
        {
            get
            {
                return _grade;
            }

            set
            {
                _grade = value;
            }
        }

        public List<ImageInfo> Images
        {
            get
            {
                return _images;
            }

            set
            {
                _images = value;
            }
        }
    }

    public class ImageInfo : ObservableObject
    {
        /// <summary>
        /// 图片地址
        /// </summary>
        private string _url;
        /// <summary>
        /// 搜题文本
        /// </summary>
        private string _parse;

        public string Url
        {
            get
            {
                return _url;
            }

            set
            {
                _url = value;
            }
        }

        public string Parse
        {
            get
            {
                return _parse;
            }

            set
            {
                _parse = value;
            }
        }
    }
}
