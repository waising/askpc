using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFClient.Model
{
    public class AskPen : ObservableObject
    {
        /// <summary>
        /// 大小
        /// </summary>
        private int _size;
        /// <summary>
        /// 笔类型 0:画笔 1 ：橡皮擦 2：清除
        /// </summary>
        private PenStyleEnum _style;
        /// <summary>
        /// 颜色
        /// </summary>
        private string _color;

        public int Size
        {
            get
            {
                return _size;
            }

            set
            {
                _size = value;
                RaisePropertyChanged(() => Size);
            }
        }

        public PenStyleEnum Style
        {
            get
            {
                return _style;
            }

            set
            {
                _style = value;
                RaisePropertyChanged(() => Style);
            }
        }

        public string Color
        {
            get
            {
                return _color;
            }

            set
            {
                _color = value;
                RaisePropertyChanged(() => Color);
            }
        }
    }
}
