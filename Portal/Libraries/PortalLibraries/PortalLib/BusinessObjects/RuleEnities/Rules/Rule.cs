﻿using System;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Processor;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules.DetailsOfRules;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Utilities;
using ConfirmIt.PortalLib.BusinessObjects.Rules;
using Core;
using Core.ORM.Attributes;

namespace ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules
{
    [DBTable("Rules")]
    public abstract class Rule : BasePlainObject
    {

        public RuleDetails RuleDetails;

        private DateTime _beginTime = DateTime.Now;
        [DBRead("BeginTime")]
        public DateTime BeginTime
        {
            get { return _beginTime; }
            set { _beginTime = value; }
        }

        private DateTime _endTime = DateTime.Now;
        [DBRead("EndTime")]
        public DateTime EndTime
        {
            get { return _endTime; }
            set { _endTime = value; }
        }

        [DBRead("TypeId")]
        public int TypeId
        {
            get { return (int)RuleType; }
            set { }
        }

        private string _xmlInformation;
        [DBRead("XmlInformation")]
        public string XmlInformation
        {
            get { return _xmlInformation; }
            set { _xmlInformation = value; }
        }

        public override bool Load(int id)
        {
            var success = base.Load(id);
            if (success) DeserializeInstance();
            return success;
        }

        public override void Save()
        {
            _xmlInformation = new SerializeHelper<RuleDetails>().GetXml(RuleDetails);
            base.Save();
        }

        public abstract void DeserializeInstance();

        public abstract RuleKind RuleType { get; }

        public abstract void Visit(RuleVisitor ruleVisitor, RuleInstance ruleInstance);
    }
}
