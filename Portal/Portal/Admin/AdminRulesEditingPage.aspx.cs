﻿using System;
using System.Collections.Generic;
using System.Linq;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.DataBaseRepository;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Rules;
using ConfirmIt.PortalLib.BusinessObjects.Rules;
using Portal.Controls.RulesControls;

namespace Portal.Admin
{
    public partial class AdminRulesEditingPage : BaseWebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var isShowRuleCreatorControl = string.IsNullOrEmpty(Request.QueryString["RuleID"]);
            if (isShowRuleCreatorControl)
            {
                RuleCreatorControl.Visible = RuleEditingControlPlaceHolder.Visible = true;
                GroupsManipulationControlID.Visible = false;
            }
            else
            {
                var ruleId = int.Parse(Request.QueryString["RuleID"]);
                RuleCreatorControl.Visible = false;
                GroupsManipulationControlID.Visible = true;
                GroupsManipulationControlID.CurrentWrapperEntityId = ruleId;
                GroupsManipulationControlID.AddEntitiesToWrapperEntityAction += AddEntitiesToWrapperEntity;
                GroupsManipulationControlID.RemoveEntitiesToWrapperEntityAction += RemoveEntitiesToWrapperEntity;
                GroupsManipulationControlID.GetIncludedEntities += GetIncludedEntitiesForBinding;
                GroupsManipulationControlID.GetNotIncludedEntities += GetNotIncludedEntitiesForBinding;

                var groupRepository = new GroupRepository();
                var ruleRepository = new RuleRepository(groupRepository);

                var ruleKind = Request.QueryString["RuleKind"];
                RuleKind parsedRuleKind;
                Enum.TryParse(ruleKind, out parsedRuleKind);

                //RuleEditingControlPlaceHolder.Controls.Clear();
                Rule editingRule;
                switch (parsedRuleKind)
                {
                    //TODO AddWorkTime
                    case RuleKind.AddWorkTime:
                        editingRule = ruleRepository.GetRuleById<NotifyByTimeRule>(ruleId);
                        break;
                    case RuleKind.NotReportToMoscow:
                        editingRule = ruleRepository.GetRuleById<NotReportToMoscowRule>(ruleId);
                        var ruleConfigurationControl = (NotReportToMoscowRuleConfigurationControl)
                             LoadControl("~/Controls/RulesControls/NotReportToMoscowRuleConfigurationControl.ascx");
                        ruleConfigurationControl.ID = "CurrentRuleConfigurationControl";
                        ruleConfigurationControl.RuleId = ruleId;
                        ruleConfigurationControl.SetDateTime(editingRule.TimeInformation.BeginTime, editingRule.TimeInformation.EndTime);
                        ViewState["CurrentRuleArguments"] = new RuleArguments
                        {
                            RuleId = ruleId,
                            CurrentRuleKind = RuleKind.NotReportToMoscow
                        };
                        RuleEditingControlPlaceHolder.Controls.Add(ruleConfigurationControl);
                        break;
                    case RuleKind.NotifyByTime:
                        editingRule = ruleRepository.GetRuleById<NotifyByTimeRule>(ruleId);
                        break;
                    case RuleKind.NotifyLastUser:
                        editingRule = ruleRepository.GetRuleById<NotifyLastUserRule>(ruleId);
                        break;
                    default:
                        throw new ArgumentException();
                }
            }
        }

        public IList<object> GetIncludedEntitiesForBinding(int wrapperEntityId)
        {
            var groupRepository = new GroupRepository();
            var allGroupsByRule = new RuleRepository(groupRepository).GetAllGroupsByRule(wrapperEntityId);

            var entities =
               allGroupsByRule.Select(
                   group => new { group.ID, group.Description }).ToArray();
            return entities;
        }

        public IList<object> GetNotIncludedEntitiesForBinding(int wrapperEntityId)
        {
            var groupRepository = new GroupRepository();
            var allGroupsByRule = new RuleRepository(groupRepository).GetAllGroupsByRule(wrapperEntityId);
            var allGroups = new GroupRepository().GetAllGroups();
            var userGroupsNotContainingInCurrentRule = allGroups
                .Where(userGroupFromAllGroups => !allGroupsByRule.Any(userGroupByRule => userGroupByRule.ID.Value == userGroupFromAllGroups.ID.Value)).ToList();

            var entities =
                userGroupsNotContainingInCurrentRule.Select(
                    group => new { group.ID, group.Description }).ToArray();
            return entities;
        }

        public void AddEntitiesToWrapperEntity(int wrapperEntityId, IList<int> idsSelectedEntities)
        {
            var groupRepository = new GroupRepository();
            var ruleRepository = new RuleRepository(groupRepository);
            ruleRepository.AddGroupIdsToRule(wrapperEntityId, idsSelectedEntities.ToArray());
        }

        public void RemoveEntitiesToWrapperEntity(int wrapperEntityId, IList<int> idsSelectedEntities)
        {
            var groupRepository = new GroupRepository();
            var ruleRepository = new RuleRepository(groupRepository);
            ruleRepository.DeleteGroupIdsFromRule(wrapperEntityId, idsSelectedEntities.ToArray());
        }
    }
}