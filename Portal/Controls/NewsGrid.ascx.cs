using System;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using Core;
using UlterSystems.PortalLib.NewsTape;
using Confirmit.Portal;

public partial class NewsTape_NewsGrid : BaseUserControl
{
	#region Свойства, транслирующиеся из внутреннего грида.

	/// <summary>
	/// Отображать столбец статуса новоти.
	/// </summary>
	public bool ShowNewsStatus
	{
		set
		{
			innerGrid.Columns[0].Visible = value;
		}
		get
		{
			return innerGrid.Columns[0].Visible;
		}
	}
	///// <summary>
	///// Отображать столбец типа новости.
	///// </summary>
	//public bool ShowNewsType
	//{
	//    set
	//    {
	//        innerGrid.Columns[1].Visible = value;
	//    }
	//    get
	//    {
	//        return innerGrid.Columns[1].Visible;
	//    }
	//}

	/// <summary>
	/// Событие, возникающее, когда гриду необходимо получить данные.
	/// </summary>
	public event GridRequestDatasourceHandler RequestDatasource;
	#endregion
	/// <summary>
	/// Обрабатывает формирование строк.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			News news = e.Row.DataItem as News;
			if (ShowNewsStatus)
			{
				Image imgNewsLocation = e.Row.FindControl("imgNewsLocation") as Image;
				if (news.ExpireTime >= DateTime.Now)
				{
					imgNewsLocation.ImageUrl = "~/Images/actualNews.gif";
					imgNewsLocation.ToolTip = GetGlobalResourceObject("NewsTape", "actualNews").ToString();
				}
				else
				{
					imgNewsLocation.ImageUrl = "~/Images/archiveNews.gif";
					imgNewsLocation.ToolTip = this.GetGlobalResourceObject("NewsTape", "archiveNews").ToString();
				}
			}

			Image imgNewsType = e.Row.FindControl("imgNewsType") as Image;
			String strOffice;

			if (news.OfficeID == 0)         // общие новости.
			{
				imgNewsType.ImageUrl = "~/Images/generalNewsImage.gif";
				strOffice = GetGlobalResourceObject("NewsTape", "generalNews").ToString();

			}
			else                            //новости офисов.
			{
				strOffice = news.OfficeName;
				imgNewsType.ImageUrl = ConfigurationManager.AppSettings["officeNewsImage" + news.OfficeName];
			}
			imgNewsType.ToolTip = strOffice;
			imgNewsType.AlternateText = strOffice;

			LinkButton lbCaption = e.Row.FindControl("lbCaption") as LinkButton;
			lbCaption.Text = news.Caption;
			lbCaption.PostBackUrl = lbCaption.PostBackUrl + news.ID;

			String strText;
			int length;
			length = news.Text.Length > 25 ? 25 : news.Text.Length;
			if (length == 25)
				strText = news.Text.Substring(0, length) + "...";
			else strText = news.Text;
			((HtmlContainerControl)e.Row.FindControl("divText")).InnerHtml = strText;

				// Set time of news.
				TableCell cell = e.Row.Cells[ 7 ];
				cell.Text = news.CreateTime.ToShortDateString();
				cell.Text += Environment.NewLine + news.CreateTime.ToShortTimeString();
		}
	}

	/// <summary>
	/// Поднимает событие биндинга внутреннего грида.
	/// </summary>
	protected PagingResult OnRequestDataSource(object sender, PagingArgs args)
	{
		// поднимаем событие биндинга.
		if (RequestDatasource != null)
			return RequestDatasource(this, args);
		return null;
	}

	public void RefreshData(bool resetPagingAndSelection)
	{
		this.innerGrid.RefreshData(resetPagingAndSelection);
	}


	protected void Page_Load(object sender, EventArgs e)
	{
		innerGrid.SearchMode = true;

	}
}
