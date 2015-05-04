﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using ConfirmIt.PortalLib.BusinessObjects.Rules.Interfaces;
using ConfirmIt.PortalLib.Rules;
using Core.ORM.Attributes;

namespace ConfirmIt.PortalLib.BusinessObjects.Rules.RealizationViaOneTable
{

    [Serializable]
    [DBTable("Rules")]
    public abstract class Rule : ObjectDataBase
    {
        protected string _xmlInformation;

        private DateTime _beginTime = new DateTime(1753,1,1,12,0,0);
        private DateTime _endTime = new DateTime(9999,12,31,11,59,59);

        [DBRead("BeginTime")]
        public DateTime BeginTime
        {
            get { return _beginTime; }
            set { _beginTime = value; }
        }

        [DBRead("EndTime")]
        public DateTime EndTime
        {
            get { return _endTime; }
            set { _endTime = value; }
        }

        [DBRead("IdType")]
        public int IdType
        {
            get { return GetIdType(); }
            protected set { }
        }

        [DBRead("XmlInformation")]
        public string XmlInformation
        {
            get
            {
                return _xmlInformation;
            }
            set
            {
                _xmlInformation = value;
                LoadFromXlm();
            }
        }

        protected List<int> GroupsId { get; set; }

        public const string TableAccordName = "AccordRules";

        public void AddGroupId(int id)
        {
            GroupsId.Add(id);
        }

        public void RemoveGroupId(int id)
        {
            GroupsId.Remove(id);
        }

        public override void Save()
        {
            LoadToXml();
            base.Save();

            var usersFromDataBase = GetGroupsIdFromDataBase();

            var nonAddingGroups = GroupsId.Except(usersFromDataBase);
            var nonDeletingGroups = usersFromDataBase.Except(GroupsId);

            AddGroupsInDataBase(nonAddingGroups);
            DeleteGroupsFromDataBase(nonDeletingGroups);
        }

        private void AddGroupsInDataBase(IEnumerable<int> groupsId)
        {
            if (groupsId.Count() == 0) return;

            using (SqlConnection connection = new SqlConnection(Connection))
            {
                connection.Open();

                foreach (var idGroup in groupsId)
                {
                    SqlCommand command = connection.CreateCommand();

                    command.CommandText =
                        string.Format("INSERT INTO {0} (idRule, idUserGroup) VALUES  (@idRule, @idUserGroup)", TableAccordName);
                    command.Parameters.AddWithValue("@idRule", ID);
                    command.Parameters.AddWithValue("@idUserGroup", idGroup);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        private void DeleteGroupsFromDataBase(IEnumerable<int> groupsId)
        {
            if (groupsId.Count() == 0) return;

            var groupsIdForDeleting = string.Join(",", groupsId);

            using (SqlConnection connection = new SqlConnection(Connection))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandText =
                    string.Format("DELETE FROM {0} WHERE idRule = @idRule and idUserGroup in ({1})", TableAccordName, groupsIdForDeleting);
                command.Parameters.AddWithValue("@idRule", ID);
                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public override void Delete()
        {
            if (ID == null)
                return;

            base.Delete();
        }

        public List<IUserGroup> GetUserGroups()
        {
            if (ID == null)
                throw new NullReferenceException("ID of instance is null");

            var userGroups = new List<IUserGroup>();

            if (GroupsId.Count != 0)
            {
                GroupsId = GetGroupsIdFromDataBase();
            }

            foreach (var id in GroupsId)
            {
                var userGroup = new UserGroup();
                if (userGroup.Load(id))
                {
                    userGroups.Add(userGroup);
                }
            }
            return userGroups;
        }

        private List<int> GetGroupsIdFromDataBase()
        {
            var groupsId = new List<int>();
            using (SqlConnection connection = new SqlConnection(Connection))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandText =
                    string.Format("Select idUserGroup FROM {0} WHERE idRule = @idRule", TableAccordName);
                command.Parameters.AddWithValue("@idRule", ID);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        groupsId.Add((int)reader["idUserGroup"]);
                    }
                }

                connection.Close();
            }
            return groupsId;
        }


        protected abstract void LoadToXml();

        protected abstract void LoadFromXlm();

        public abstract int GetIdType();
    }
}