using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPFClient.Model
{
    public class Evaluation : ObservableObject
    {
        private int _reward;
        private string _suggest;
        private double _star;

        public int Reward
        {
            get
            {
                return _reward;
            }

            set
            {
                _reward = value;
            }
        }

        public string Suggest
        {
            get
            {
                return _suggest;
            }

            set
            {
                _suggest = value;
            }
        }

        public double Star
        {
            get
            {
                return _star;
            }

            set
            {
                _star = value;
            }
        }

    }
}
