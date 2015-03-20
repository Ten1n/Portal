using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web.UI.WebControls;
using ConfirmIt.PortalLib.BAL;
using ConfirmIt.PortalLib.BusinessObjects.UserStatusInfoComparers;
using ConfirmIt.PortalLib.DAL;
using ConfirmIt.PortalLib.DAL.SqlClient;
using UlterSystems.PortalLib.BusinessObjects;

public partial class Controls_UsersList : BaseUserControl
{
	#region ������
	
	/// <summary>
	/// ������ ������ ��������.
	/// </summary>
	public enum Mode
	{
		Standard,
		Admin
	}

	#endregion

	#region ��������

	/// <summary>
	/// ����, �� ������� ������������ �������.
	/// </summary>
	public DateTime Date
	{
		get
		{
			return ViewState["Date"] == null
					   ? DateTime.Today
					   : (DateTime) ViewState["Date"];
		}
		set
		{
			ViewState["Date"] = value;
			FillUsersGrid();
		}
	}

	/// <summary>
	/// ����� ������ ��������.
	/// </summary>
	public Mode ControlMode
	{
		get
		{
			return ViewState["Mode"] == null
					   ? Mode.Standard
					   : (Mode) Enum.Parse(typeof (Mode), (string) ViewState["Mode"]);
		}
		set 
		{
			// �������� ������������.
			if( Page.CurrentUser != null && !Page.CurrentUser.IsInRole( "Administrator" ) )
				value = Mode.Standard;

			ViewState["Mode"] = value.ToString();
			// ��������� ��������� ���������.
			EnableControls();
		}
	}

	/// <summary>
	/// ������ �� �������� ������������ � ����������� ������.
	/// </summary>
	public string StandardNavigateURL
	{
		get
		{
			return ViewState["StandardNavigateURL"] == null
					   ? null
					   : (string) ViewState["StandardNavigateURL"];
		}
		set { ViewState["StandardNavigateURL"] = value; }
	}

	/// <summary>
	/// ������ �� �������� ������������ � ���������������� ������.
	/// </summary>
	public string AdminNavigateURL
	{
		get
		{
			return ViewState["AdminNavigateURL"] == null
					   ? null
					   : (string) ViewState["AdminNavigateURL"];
		}
		set { ViewState["AdminNavigateURL"] = value; }
	}

	/// <summary>
	/// ������ ��������.
	/// </summary>
	public Unit Width
	{
		get { return grdUsersList.Width; }
		set { grdUsersList.Width = value; }
	}

	#endregion

	protected void Page_Load(object sender, EventArgs e)
	{
		// ��������� ������ ������.
		if (Page.CurrentUser != null && !Page.CurrentUser.IsInRole(RolesEnum.Administrator))
			ControlMode = Mode.Standard;

		// ��������� ������� �������������
	    if (!IsPostBack)
	    {
            grdUsersList.DataSource = UserNamesAndStatusesObjectDataSource.Select();
            grdUsersList.DataBind();
            ViewState["isDescendingSortDirection"] = false;
	    }
	}

	/// <summary>
	/// ��������� ��������� ���������.
	/// </summary>
	private void EnableControls()
	{
        switch (ControlMode)
        {
            case Mode.Standard:
                grdUsersList.Columns[2].Visible = false;
                break;

            case Mode.Admin:
                grdUsersList.Columns[2].Visible = true;
                break;
        }
	}

    public void GetUsersStatusInfoByStatus()
    {
        using (var connection = new SqlConnection(@"Data Source=CO-YAR-WS132\SQLEXPRESS;Initial Catalog=Portal;Persist Security Info=True;User ID=sa;Password=Stupw123!"))
        {
            var command = connection.CreateCommand();
            command.CommandText =
                "SELECT DISTINCT LastName FROM Users INNER JOIN UptimeEvents ON (Users.ID=UptimeEvents.UserID) AND (BeginTime >= @BeginTime) AND (BeginTime <= @EndTime);";
            command.Parameters.Add("@BeginTime", SqlDbType.DateTime).Value =
                Date;
            command.Parameters.Add("@EndTime", SqlDbType.DateTime).Value =
                Date.AddDays(1).AddSeconds(-1);
            connection.Open();

            //TODO: remove, just for testing
            using (IDataReader eventsReader = command.ExecuteReader())
            {
                var names = new List<String>();
                while (eventsReader.Read())
                {
                    var currentFirstName = (String)eventsReader["LastName"];
                    names.Add(currentFirstName);
                }
            }
        }
    }

    public UserStatusInfo[] GetUsersStatusInfo(bool isDescendingSortDirection, String propertyName)
    {
        GetUsersStatusInfoByStatus();
        var usersWithFullInformation = UserList.GetStatusesList(Date, isDescendingSortDirection, propertyName);
        return usersWithFullInformation;
    }

	/// <summary>
	/// ��������� ������ �������������.
	/// </summary>
	protected void FillUsersGrid()
	{
        //TODO used in other methods
	}

	/// <summary>
	/// �������� ������ ������������� � ��������� ����������.
	/// </summary>
	protected void OnUserInfoBound(object sender, DataGridItemEventArgs e)
	{
		if (e.Item.DataItem == null || !(e.Item.DataItem is UserStatusInfo))
			return;

		// �������� ���������� � ������� ������������.
		var usInfo = (UserStatusInfo)e.Item.DataItem;

		if (Page.CurrentUser != null && (Page.CurrentUser.ID.HasValue && usInfo.UserID == Page.CurrentUser.ID.Value))
			e.Item.CssClass = "gridview-selectedrow";
	
		// ����� �����������
		var hl = (HyperLink)e.Item.FindControl("hlUserName");
		if (hl == null)
			return;

		// ���������� ��������� ������
		switch (ControlMode)
		{
			case Mode.Standard:
				hl.NavigateUrl = !string.IsNullOrEmpty(StandardNavigateURL)
									 ? StandardNavigateURL + (@"?UserID=" + usInfo.UserID)
									 : string.Empty;
				break;

			case Mode.Admin:
				hl.NavigateUrl = !string.IsNullOrEmpty(AdminNavigateURL)
									 ? AdminNavigateURL + (@"?UserID=" + usInfo.UserID)
									 : string.Empty;
				break;
		}

		// ����� ������ � ���������� ���������.
		ImageButton b = (ImageButton)e.Item.FindControl("btnIll");
		if (b != null)
			b.CommandArgument = usInfo.UserID.ToString();
		
		b = (ImageButton)e.Item.FindControl("btnTrustIll");
		if (b != null)
			b.CommandArgument = usInfo.UserID.ToString();
		
		b = (ImageButton)e.Item.FindControl("btnBusinessTrip");
		if (b != null)
			b.CommandArgument = usInfo.UserID.ToString();
		
		b = (ImageButton)e.Item.FindControl("btnVacation");
		if (b != null)
			b.CommandArgument = usInfo.UserID.ToString();

		b = (ImageButton)e.Item.FindControl("btnLesson");
		if (b != null)
			b.CommandArgument = usInfo.UserID.ToString();

		// ����� ������ �������������� � ���������� ���������.
		var lb = (LinkButton)e.Item.FindControl("lbtnEdit");
		if (lb != null)
			lb.PostBackUrl = hl.NavigateUrl;
	}

	protected virtual void OnSetIll(object sender, EventArgs e)
	{
		createAbsenceEvent(sender, WorkEventType.Ill);
	}

	protected virtual void OnSetTrustIll(object sender, EventArgs e)
	{
		createAbsenceEvent(sender, WorkEventType.TrustIll);
	}

	protected virtual void OnSetBusinessTrip(object sender, EventArgs e)
	{
		createAbsenceEvent(sender, WorkEventType.BusinessTrip);
	}

	protected virtual void OnSetVacation(object sender, EventArgs e)
	{
		createAbsenceEvent(sender, WorkEventType.Vacation);
	}

	protected void OnSetLesson(object sender, EventArgs e)
	{
		try
		{
			if (!(sender is ImageButton))
				return;

			var b = (ImageButton)sender;

			int userID;
			if (!Int32.TryParse(b.CommandArgument, out userID))
				return;

			var userEvents = new UserWorkEvents(userID);

			var duration = TimeSpan.FromMinutes(45);
			userEvents.AddLatestClosedWorkEvent(duration, WorkEventType.TimeOff);

			FillUsersGrid();
		}
		catch (Exception ex)
		{
			lblException.Text = ex.Message;
			lblException.Visible = true;
		}
	}

	/// <summary>
	/// Create absence event for user.
	/// </summary>
	/// <param name="sender">Image button.</param>
	/// <param name="eventType">Type of event.</param>
	private void createAbsenceEvent(Object sender, WorkEventType eventType)
	{
		try
		{
			if (!(sender is ImageButton))
				return;

			var b = (ImageButton)sender;

			int userID;
			if (!Int32.TryParse(b.CommandArgument, out userID))
				return;

			var userEvents = new UserWorkEvents(userID);
			userEvents.CreateAbsenceEvent(eventType, Date);

			FillUsersGrid();
		}
		catch (Exception ex)
		{
			lblException.Text = ex.Message;
			lblException.Visible = true;
		}
	}

    protected void SortingCommand_Click(object sender, GridViewSortEventArgs e)
    {
        bool isDescendingSortDirection;

        if ((bool)ViewState["isDescendingSortDirection"])
        {
            ViewState["isDescendingSortDirection"] = false;
            isDescendingSortDirection = true;
        }
        else
        {
            ViewState["isDescendingSortDirection"] = true;
            isDescendingSortDirection = false;
        }

        var sortedUsers = GetUsersStatusInfo(isDescendingSortDirection, e.SortExpression);
        grdUsersList.DataSource = sortedUsers;
        grdUsersList.DataBind();
    }
}