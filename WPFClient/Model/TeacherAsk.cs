using GalaSoft.MvvmLight;

namespace WPFClient.Model
{
    public class TeacherAsk : ObservableObject
    {

        /// <summary>
        /// 经验值
        /// </summary>
        private int _exprience;
        /// <summary>
        /// 经验值升级
        /// </summary>
        private int _exprienceMax = 100;
        /// <summary>
        /// 教师职称
        /// </summary>
        private string _level;
        /// <summary>
        /// 获得的总星星数
        /// </summary>
        private int _score;
        /// <summary>
        /// 星星（百分比）
        /// </summary>
        private double _star;
        /// <summary>
        /// 回答次数
        /// </summary>
        private int _answerTimes;
        /// <summary>
        /// 从该字段可分解出该教师的学科和年级
        ///如CM_09004：
        ///第一个字母C为初中、G为高中
        ///第二个字母M为数学、P为物理、C为化学
        ///下划线后面的前两位数为年级信息、07-09为初中、10-12为高中
        ///第二个字母(subject)和年级(grade)信息应在登陆后保存用于搜题时使用
        /// </summary>
        private string _code;
        /// <summary>
        /// 补充说明年级信息
        /// </summary>
        private string _subject;
        /// <summary>
        /// 
        /// 阿思币
        /// </summary>
        private int _askcoin;

        /// <summary>
        /// 头像
        /// </summary>
        private string _avatar;

        public int Exprience
        {
            get
            {
                return _exprience;
            }

            set
            {
                _exprience = value;
                RaisePropertyChanged(() => Exprience);
            }
        }

        public string Level
        {
            get
            {
                return _level;
            }

            set
            {
                _level = value;
                RaisePropertyChanged(() => Level);
            }
        }

        public int Score
        {
            get
            {
                return _score;
            }

            set
            {
                _score = value;
                RaisePropertyChanged(() => Score);
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
                RaisePropertyChanged(() => Star);
            }
        }

        public int AnswerTimes
        {
            get
            {
                return _answerTimes;
            }

            set
            {
                _answerTimes = value;
                
                RaisePropertyChanged(() => AnswerTimes);
            }
        }

        public string Code
        {
            get
            {
                return _code;
            }

            set
            {
                _code = value;
                RaisePropertyChanged(() => Code);
            }
        }

        public string Subject
        {
            get
            {
                return _subject;
            }

            set
            {
                _subject = value;
                RaisePropertyChanged(() => Subject);
            }
        }

        public int Askcoin
        {
            get
            {
                return _askcoin;
            }

            set
            {
                _askcoin = value;
                RaisePropertyChanged(() => Askcoin);
            }
        }

        public string Avatar
        {
            get
            {
                return _avatar;
            }

            set
            {
                _avatar = value;
                RaisePropertyChanged(() => Avatar);
            }
        }

        public int ExprienceMax
        {
            get
            {
                return _exprienceMax;
            }

            set
            {
                _exprienceMax = value;
                RaisePropertyChanged(() => ExprienceMax);
            }
        }
    }
}
