using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpireBuilding.CodeFiles
{
    public class Skills
    {
        private string pvSkillTitlePlusLevel;
        public string SkillTitlePlusLevel
        {
            get { return pvSkillTitlePlusLevel; }
            set { pvSkillTitlePlusLevel = value; }
        }

        private string pvSkillDescription;
        public string SkillDescription
        {
            get { return pvSkillDescription; }
            set { pvSkillDescription = value; }
        }

        private string pvSkillBonusText;
        public string SkillBonusText
        {
            get { return pvSkillBonusText; }
            set { pvSkillBonusText = value; }
        }

        private string pvSkillBonusNumber;
        public string SkillBonusNumber
        {
            get { return pvSkillBonusNumber; }
            set { pvSkillBonusNumber = value; }
        }

        private string pvImagePath;
        public string ImagePath
        {
            get { return pvImagePath; }
            set { pvImagePath = value; }
        }

        private int pvLevel;
        public int Level
        {
            get { return pvLevel; }
            set { pvLevel = value; }
        }

        private int pvNextLevel;
        public int NextLevel
        {
            get { return pvNextLevel; }
            set { pvNextLevel = value; }
        }

        private int pvEXP;
        public int EXP
        {
            get { return pvEXP; }
            set { pvEXP = value; }
        }

        private long pvNextEXP;
        public long NextEXP
        {
            get { return pvNextEXP; }
            set { pvNextEXP = value; }
        }

        private double pvNextLevelPCT;
        public double NextLevelPCT
        {
            get { return pvNextLevelPCT; }
            set { pvNextLevelPCT = value; }
        }
    }
}
