using System;
using System.Collections.Generic;
using ConfirmIt.PortalLib.BAL;
using SLService;

/// <summary>
/// ������� ���������� ��� �������� �������� ������� �������������.
/// </summary>
public partial class NewDay : BaseUserControl
{
	#region Fields

	protected ControlState CurrentControlState = ControlState.WorkFinished;

	#endregion

	#region ControlState enum

	/// <summary>
	/// States of control.
	/// </summary>
	protected enum ControlState
	{
		WorkGoes,
		WorkFinished,
		Absent
	}

	#endregion

	#region Properties

	/// <summary>
	/// State of control.
	/// </summary>
	protected ControlState State
	{
		get
		{
			return ViewState["State"] == null
					   ? ControlState.WorkFinished
					   : (ControlState) ViewState["State"];
		}
		set
		{
			ViewState["State"] = value;
			EnableControls();
		}
	}

	#endregion

	#region Event handlers

	/// <summary>
	/// ���������� �������� ��������.
	/// </summary>
	protected void Page_Load( object sender, EventArgs e )
	{
		if (Page.CurrentUser == null || !Page.CurrentUser.ID.HasValue)
		{
			Visible = false;
			return;
		}

		// ���������� ������� ��������� ������������.
		DefineCurrentState();
	}

	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);
		
		SLService.SLService service = new SLService.SLService();
		gridViewUserDayEvents.DataSource = service.GetTodayWorkEventsOfUser(Page.CurrentUser.ID.Value);
		gridViewUserDayEvents.DataBind();

		showTimes();
	}

	#region ����������� ������� �� ������

	/// <summary>
	/// ���������� ������� �� ������ ������ ������.
	/// </summary>
	protected void OnWork_Click(object sender, EventArgs e)
	{
		if (!IsPostBack && !WebHelpers.IsRequestIPAllowed())
			return;

		if (State == ControlState.WorkFinished)
		{
			SetUserWorkEvent(true, WorkEventType.MainWork);
			State = ControlState.WorkGoes;
			return;
		}

		SetUserWorkEvent(false, WorkEventType.MainWork);
		State = ControlState.WorkFinished;
		
		informLastUser();
	}

	/// <summary>
	/// ���������� ������� �� ������ ����������.
	/// </summary>
	protected void OnTime_Click(object sender, EventArgs e)
	{
		if (!IsPostBack && !WebHelpers.IsRequestIPAllowed())
			return;

		switch (State)
		{
		    case ControlState.WorkGoes:
		        SetUserWorkEvent(true, WorkEventType.TimeOff);
		        State = ControlState.Absent;
		        return;
		    case ControlState.Absent:
		        SetUserWorkEvent(false, WorkEventType.TimeOff);
		        State = ControlState.WorkGoes;
		        return;
		}
	}

	#endregion

	#endregion

	#region Methods

	private void SetUserWorkEvent(bool isOpenAction, WorkEventType eventType)
	{
		if (!WebHelpers.IsRequestIPAllowed())
			return;

		SLService.SLService service = new SLService.SLService();
		service.SetUserWorkEvent(Page.CurrentUser.ID.Value, isOpenAction, eventType);
	}

	/// <summary>
	/// Shows and hides controls depending on state.
	/// </summary>
	private void EnableControls()
	{
		SetLocalization();

		if (!WebHelpers.IsRequestIPAllowed())
		{
			btTime.Visible = btWork.Visible = false;
			return;
		}

        switch (State)
		{
			case ControlState.WorkGoes:
				btWork.Visible = btTime.Visible = true;
				break;

			case ControlState.WorkFinished:
				btWork.Visible = true;
				btTime.Visible = false;
				break;

			case ControlState.Absent:
				btWork.Visible = btTime.Visible = true;
				break;
		}
	}

	/// <summary>
	/// Set text to controls.
	/// </summary>
	private void SetLocalization()
	{
		btWork.Text = State == ControlState.WorkFinished
						  ? (string)GetLocalResourceObject("btnWorkBegin.Text")
						  : (string)GetLocalResourceObject("btnWorkEnd.Text");

		btTime.Text = State == ControlState.WorkGoes
							  ? (string)GetLocalResourceObject("btnTimeOff.Text")
							  : (string)GetLocalResourceObject("btnTimeOn.Text");
	}

	/// <summary>
	/// ���������� ������� ��������� ������������.
	/// </summary>
	protected virtual void DefineCurrentState()
	{
		SLService.SLService service = new SLService.SLService();
		IList<WorkEvent> mainAndLastWorkEvent = service.GetMainWorkAndLastEvent(Page.CurrentUser.ID.Value);
		WorkEvent TodayWorkEvent = mainAndLastWorkEvent[0];
		WorkEvent LastEvent = mainAndLastWorkEvent[1];

		if (TodayWorkEvent == null ||
			(TodayWorkEvent.BeginTime != TodayWorkEvent.EndTime))
		{
			State = ControlState.WorkFinished;
			return;
		}

		switch (LastEvent.EventType)
		{
			case WorkEventType.MainWork:
				State = ControlState.WorkGoes;
				break;

			case WorkEventType.TimeOff:
				State = LastEvent.BeginTime == LastEvent.EndTime
							? ControlState.Absent
							: ControlState.WorkGoes;
				break;
		}
	}

	/// <summary>
	/// ���������� ��������� �������.
	/// </summary>
	private void showTimes()
	{
        ReloadLables();

		SLService.SLService service = new SLService.SLService();
		IDictionary<TimeKey, TimeSpan> timeDict = service.GetFullDayTimes(Page.CurrentUser.ID.Value);

		int hours = (int)timeDict[TimeKey.TodayWork].TotalHours;
		int minutes = (int)(timeDict[TimeKey.TodayWork] - new TimeSpan(hours, 0, 0)).TotalMinutes;
		lblTime.Text = String.Format(lblTime.Text, hours, minutes);

		// ����� �� ��������� ���.
		hours = (int)timeDict[TimeKey.TodayRest].TotalHours;
		minutes = (int)(timeDict[TimeKey.TodayRest] - new TimeSpan(hours, 0, 0)).TotalMinutes;
		lblRemainToday.Text = String.Format(lblRemainToday.Text, hours, minutes);

		// ����� ��������� ������.
		DateTime endWork = DateTime.Now.Add(timeDict[TimeKey.TodayRest]);
		lblEndDay.Text = String.Format(lblEndDay.Text, endWork.ToShortTimeString());

		// ����� �� ��������� ������.
		hours = (int)timeDict[TimeKey.WeekRest].TotalHours;
		minutes = (int)(timeDict[TimeKey.WeekRest] - new TimeSpan(hours, 0, 0)).TotalMinutes;
		lblRemainWeek.Text = String.Format(lblRemainWeek.Text, hours, minutes);

		// ����� �� ��������� ������.
		hours = (int)timeDict[TimeKey.MonthRest].TotalHours;
		minutes = (int)(timeDict[TimeKey.MonthRest] - new TimeSpan(hours, 0, 0)).TotalMinutes;
		lblRemainMonth.Text = String.Format(lblRemainMonth.Text, hours, minutes);
	}
    private void ReloadLables()
    {
        lblTime.Text = GetLocalResourceObject("lblTime.Text").ToString();
        lblRemainToday.Text = GetLocalResourceObject("lblRemainToday.Text").ToString();
        lblEndDay.Text = GetLocalResourceObject("lblEndDay.Text").ToString();
        lblRemainMonth.Text = GetLocalResourceObject("lblRemainMonth.Text").ToString();
    }

	/// <summary>
	/// ������������� ���������� ������������.
	/// </summary>
	private void informLastUser()
	{
		SLService.SLService service = new SLService.SLService();
		int usersCount = service.GetNumberOfActiveUsers();

		if (usersCount >= 0 && usersCount <= 2)
		{
			String scriptAllert = "<script type='text/javascript'> alert('";
			switch (usersCount)
			{
				case 0:
					scriptAllert += "�� ��������� ������� �� �����!";
					break;

				case 1:
					scriptAllert += "� ����� ������� ������ ���� �������!";
					break;

				case 2:
					scriptAllert += "� ����� �������� ������ ����!";
					break;
			}

			scriptAllert += "\\n�����, ��������� ���� � ����� � ��������!'); </script>";
			Page.ClientScript.RegisterClientScriptBlock(GetType(),
														"NewDay", scriptAllert);
		}
	}

	#endregion
}