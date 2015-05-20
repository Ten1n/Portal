﻿using System;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.DetailsOfRules
{
    public class NotifyByTimeRuleDetails : RuleDetails
    {
        public string Information { get; set; }
        public DateTime Time { get; set; }
        public DayOfWeek DayOfWeek { get; set; }

        public NotifyByTimeRuleDetails()
        {
            
        }

        public NotifyByTimeRuleDetails(string information, DateTime time, DayOfWeek dayOfWeek)
        {
            Information = information;
            Time = time;
            DayOfWeek = dayOfWeek;
        }
    }
}