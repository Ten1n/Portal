using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Controls.DatePicker
{

    public enum DatePickerLanguage
    {
        En,
        Ru
    }
    /// <summary>
    ///	Represents the control to get the date and time.
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:DatePicker runat=server></{0}:DatePicker>")]
    public class DatePicker : WebControl, INamingContainer
    {
        private readonly TextBox _txtDate = new TextBox();
        private const string DateFormat = "mm.dd.yyyy";

        private string Selector
        {
            get { return "$(\"#" + _txtDate.ClientID + "\")"; }
        }

        private string PlaceHolder
        {
            set { _txtDate.Attributes.Add("placeholder", value); }
        }

        [DefaultValue(typeof(DateTime), ""),
        Category("Date Selection"),
        Bindable(true, BindingDirection.TwoWay)]
        public DateTime SelectedDate
        {
            get { return GetSelectedDate(); }
            set { SetSelectedDate(value); }
        }

        [Description("")]
        [Category("Date Selection"), DefaultValue("en")]
        [Browsable(true)]
        public DatePickerLanguage Language
        {
            get { return _language; }
            set
            {
                _language = value;
                PlaceHolder = DateFormat;
            }
        }
        private DatePickerLanguage _language = DatePickerLanguage.Ru;

        [Description("")]
        [Category("Date Selection"), DefaultValue(true)]
        [Browsable(true)]
        public bool HideAfterSelect
        {
            get { return _hideAfterSelect; }
            set{_hideAfterSelect = value;}
        }
        private bool _hideAfterSelect = true;

        public void ChangeLocale()
        {
            var result = Selector + ".pickmeup('set_locale', 'en');";

            Page.ClientScript.RegisterClientScriptBlock(GetType(), "css" + UniqueID, result, true);
        }
      
        private DateTime GetSelectedDate()
        {
            DateTime result;

            if (DateTime.TryParseExact(_txtDate.Text, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
                return result;

            return DateTime.Today;
        }

        private void SetSelectedDate(DateTime? date)
        {
            if (!date.HasValue)
                _txtDate.Text = "";
            else
            {
                _txtDate.Text = date.Value.ToString(DateFormat);
            }
        }

        private string GetInitializationScript()
        {
            var result = Selector + ".pickmeup({";
            result += "hide_on_select : '" + _hideAfterSelect.ToString().ToLower() + "',";
            result += "format : 'm.d.Y',";
            result += "language : '" + Language.ToString().ToLower() + "'";

            return result + "});";
        }

        protected override void OnLoad(EventArgs e)
        {
            PlaceHolder = DateFormat;
            this.Font.Size = 10;
            base.OnLoad(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            const string cssResourceName = "Controls.DatePicker.css.DatePicker.css";
            const string datePickerResourceName = "Controls.DatePicker.js.DatePicker.js";
          
            IncludeJavaScript(datePickerResourceName);
            IncludeStyle(cssResourceName);
            
            var script = GetInitializationScript();
            Page.ClientScript.RegisterStartupScript(GetType(), "css" + UniqueID, script, true);

            base.OnPreRender(e);
        }

        protected override void CreateChildControls()
        {
           // txtDate.ID = "txt";
            if (_txtDate.Width.IsEmpty)
            {
                //txtDate.Width = Unit.Pixel(70);
            }
          //  txtDate.Attributes.Add("OnKeyPress", "return checkAllowedKey(event);");

            Controls.Add(_txtDate);
        }

        private void IncludeStyle(string resourceName)
        {
            var cssUrl = Page.ClientScript.GetWebResourceUrl(this.GetType(), resourceName);
            var cssLink = new HtmlLink {Href = cssUrl};
           
            cssLink.Attributes.Add("rel", "stylesheet");
            cssLink.Attributes.Add("type", "text/css");
           
            Page.Header.Controls.Add(cssLink);
        }

        private void IncludeJavaScript(IEnumerable<string> resourceNames)
        {
            foreach (var resourceName in resourceNames)
            {
                IncludeJavaScript(resourceName);
            }
        }

        private void IncludeJavaScript(string resourceName)
        {
            var urlScript = Page.ClientScript.GetWebResourceUrl(GetType(), resourceName);

            if (!Page.ClientScript.IsClientScriptIncludeRegistered(GetType(), resourceName))
                Page.ClientScript.RegisterClientScriptInclude(GetType(), resourceName, urlScript);
        }
    }
}