using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpireBuilding.CodeFiles
{
    class Leaderboard
    {
        private string pvRankNum;
        public string RankNum
        {
            get { return pvRankNum; }
            set { pvRankNum = value; }
        }

        private string pvLBUserName;
        public string LBUserName
        {
            get { return pvLBUserName; }
            set { pvLBUserName = value; }
        }

        private string pvLBValue;
        public string LBValue
        {
            get { return pvLBValue; }
            set { pvLBValue = value; }
        }

        private string pvLBBackground;
        public string LBBackground
        {
            get { return pvLBBackground; }
            set { pvLBBackground = value; }
        }

        private string pvLBForeground;
        public string LBForeground
        {
            get { return pvLBForeground; }
            set { pvLBForeground = value; }
        }

        private int pvLBValueFormat;
        public int LBValueFormat
        {
            get { return pvLBValueFormat; }
            set { pvLBValueFormat = value; }
        }

        public Leaderboard(string rank, string value, int valueformat, string username, string background, string foreground)
        {
            this.RankNum = rank;
            if (valueformat == 1)
            {
                this.LBValue = Convert.ToDouble(value).ToString("C0");
            }
            else if (valueformat == 2)
            {
                this.LBValue = Convert.ToDouble(value).ToString("N");
            }
            else if (valueformat == 3)
            {
                TimeSpan elapsedTime = new TimeSpan(0, 0, Convert.ToInt32(value));
                this.LBValue = elapsedTime.TotalHours.ToString("N0") + ":" + elapsedTime.ToString(@"mm\:ss");
                //this.LBValue = elapsedTime.ToString(@"hh\:mm\:ss");
            }
            else
            {
                this.LBValue = Convert.ToDouble(value).ToString("N0");
            }
            this.LBUserName = username;
            this.LBBackground = background;
            this.LBForeground = foreground;
        }
    }
}
