﻿using System;
using System.Web.UI;
using ConfirmIt.PortalLib.BusinessObjects.RuleEnities.Repositories.DataBaseRepository;

namespace Portal.Controls
{
    public partial class AdminGroupsEditingControl : UserControl
    {
        public event EventHandler<SelectedObjectEventArgs> EventHandler;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var groupRepository = new GroupRepository();
                var currentListOfGroups = groupRepository.GetAllGroups();

                GroupsEditingGridView.DataSource = currentListOfGroups;
                GroupsEditingGridView.DataBind();
            }
            GroupsEditingGridView.SelectedIndexChanged += GroupsEditingGridView_OnSelectedIndexChanged;
        }

        protected void GroupsEditingGridView_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedGroupId == -1)
                throw new Exception("Selected user id equals -1.");

            if (EventHandler != null && SelectedGroupId != -1)
                EventHandler(this, new SelectedObjectEventArgs { ObjectID = SelectedGroupId });
        }

        private int SelectedGroupId
        {
            get
            {
                return GroupsEditingGridView.SelectedDataKey == null
                           ? -1
                           : (int)GroupsEditingGridView.SelectedDataKey.Value;
            }
        }

        public void RefreshGroupList(object sender, EventArgs e)
        {
            var groupRepository = new GroupRepository();
            var currentListOfGroups = groupRepository.GetAllGroups();

            GroupsEditingGridView.DataSource = currentListOfGroups;
            GroupsEditingGridView.DataBind();
        }
    }
}