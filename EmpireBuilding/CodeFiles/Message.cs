using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpireBuilding.CodeFiles
{
    public class Message
    {
        private int pvMessageID;
        public int MessageID
        {
            get { return pvMessageID; }
            set { pvMessageID = value; }
        }

        private string pvMessageTitle;
        public string MessageTitle
        {
            get { return pvMessageTitle; }
            set { pvMessageTitle = value; }
        }

        private string pvMessageDescription;
        public string MessageDescription
        {
            get { return pvMessageDescription; }
            set { pvMessageDescription = value; }
        }

        private string pvMessageImage;
        public string MessageImage
        {
            get { return pvMessageImage; }
            set { pvMessageImage = value; }
        }

        private string pvMessageCount;
        public string MessageCount
        {
            get { return pvMessageCount; }
            set { pvMessageCount = value; }
        }

        private string pvMessageXPVisible;
        public string MessageXPVisible
        {
            get { return pvMessageXPVisible; }
            set { pvMessageXPVisible = value; }
        }

        private int pvMessageCurLevel;
        public int MessageCurLevel
        {
            get { return pvMessageCurLevel; }
            set { pvMessageCurLevel = value; }
        }

        private int pvMessageNextLevel;
        public int MessageNextLevel
        {
            get { return pvMessageNextLevel; }
            set { pvMessageNextLevel = value; }
        }

        private long pvMessageCurXP;
        public long MessageCurXP
        {
            get { return pvMessageCurXP; }
            set { pvMessageCurXP = value; }
        }

        private long pvMessageNextXP;
        public long MessageNextXP
        {
            get { return pvMessageNextXP; }
            set { pvMessageNextXP = value; }
        }

        private int pvMessageAddedXPDisplay;
        public int MessageAddedXPDisplay
        {
            get { return pvMessageAddedXPDisplay; }
            set { pvMessageAddedXPDisplay = value; }
        }

        private double pvMessageAddedXP;
        public double MessageAddedXP
        {
            get { return pvMessageAddedXP; }
            set { pvMessageAddedXP = value; }
        }

        private double pvMessageCurXPPCT;
        public double MessageCurXPPCT
        {
            get { return pvMessageCurXPPCT; }
            set { pvMessageCurXPPCT = value; }
        }

        private double pvMessageCurXPPCT2;
        public double MessageCurXPPCT2
        {
            get { return pvMessageCurXPPCT2; }
            set { pvMessageCurXPPCT2 = value; }
        }

        public Message(int messageid, string messagetitle, string messagedescription, string messageimage, string xpvisible, Machine whichmachine, int addedxp, int DidSkillGainLevel)
        {
            this.MessageID = messageid;
            this.MessageTitle = messagetitle;
            this.MessageDescription = messagedescription;
            this.MessageImage = messageimage;
            this.MessageXPVisible = xpvisible;
            if (this.MessageXPVisible == "Visible")
            {
                this.MessageCurLevel = whichmachine.Level;
                this.MessageNextLevel = whichmachine.Level + 1;
                this.MessageCurXP = helper.GetNextLevelXP(whichmachine.ID, whichmachine.Level - 1);
                this.MessageNextXP = helper.GetNextLevelXP(whichmachine.ID, whichmachine.Level);
                long XPforCurrentLevel = this.MessageNextXP - this.MessageCurXP;
                this.MessageAddedXPDisplay = whichmachine.TotalEXP;
                double PrevCurXP = 0.0;

                if (DidSkillGainLevel == 1)
                {
                    // ====================================================================================== //
                    // ==================== First Message during Level Up - Red Bar ========================= //
                    this.MessageCurLevel = whichmachine.Level - 1;
                    this.MessageNextLevel = whichmachine.Level;
                    long MachineCurrentXP = this.MessageCurXP;
                    this.MessageCurXP = helper.GetNextLevelXP(whichmachine.ID, (whichmachine.Level - 2));
                    this.MessageNextXP = helper.GetNextLevelXP(whichmachine.ID, whichmachine.Level - 1);
                    XPforCurrentLevel = this.MessageNextXP - this.MessageCurXP;

                    long Temp4 = helper.GetNextLevelXP(whichmachine.ID, whichmachine.Level - 1);
                    double LeftOverAddedXP2 = addedxp - (this.MessageAddedXPDisplay - Temp4);
                    this.MessageAddedXP = (LeftOverAddedXP2 / XPforCurrentLevel) * 200; // Width of the Green Bar
                    PrevCurXP = XPforCurrentLevel - LeftOverAddedXP2;

                    // ====================================================================================== //
                    // ====================================================================================== //

                    if (this.MessageID == 6)
                    {
                        // ====================================================================================== //
                        // ==================== Second Message during Level Up - Green Bar ====================== //

                        PrevCurXP = 0;
                        this.MessageCurLevel = whichmachine.Level; // Have to add 1 here to Offset the MessageCurLevel subtraction above
                        this.MessageNextLevel = whichmachine.Level + 1;
                        this.MessageCurXP = helper.GetNextLevelXP(whichmachine.ID, whichmachine.Level - 1);
                        this.MessageNextXP = helper.GetNextLevelXP(whichmachine.ID, whichmachine.Level);
                        XPforCurrentLevel = this.MessageNextXP - this.MessageCurXP;
                        double LeftOverAddedXP = 0.0;
                        LeftOverAddedXP = (this.MessageAddedXPDisplay - this.MessageCurXP);
                        this.MessageAddedXP = Math.Round((LeftOverAddedXP / XPforCurrentLevel) * 200); // Width of the Green Bar
                        // ====================================================================================== //
                        // ====================================================================================== //
                    }

                    // ====================================================================================== //
                    // ==================== Red Bar ========================================================= //
                    double temp1 = (Convert.ToDouble(PrevCurXP) / Convert.ToDouble(XPforCurrentLevel)) * 200;
                    this.MessageCurXPPCT = temp1; // Width of the Red Bar
                    this.MessageCurXPPCT2 = temp1 + 52;
                    // ====================================================================================== //
                    // ====================================================================================== //
                }
                else
                {
                    // ====================================================================================== //
                    // ==================== Non Level Up - Green Bar ======================================== //
                    double LeftOverAddedXP = 0.0;
                    PrevCurXP = (this.MessageAddedXPDisplay - this.MessageCurXP) - addedxp;
                    LeftOverAddedXP = addedxp;
                    this.MessageAddedXP = Math.Round((LeftOverAddedXP / XPforCurrentLevel) * 200); // Width of the Green Bar
                    // ====================================================================================== //
                    // ====================================================================================== //

                    // ====================================================================================== //
                    // ==================== Red Bar ========================================================= //
                    double temp1 = (Convert.ToDouble(PrevCurXP) / Convert.ToDouble(XPforCurrentLevel)) * 200;
                    this.MessageCurXPPCT = temp1; // Width of the Red Bar
                    this.MessageCurXPPCT2 = temp1 + 52;
                    // ====================================================================================== //
                    // ====================================================================================== //
                }



            }
            
        }

        public Message() { }
    }
}
