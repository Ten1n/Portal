<%@ Page Language="C#" MasterPageFile="~/MasterPages/Main.master" AutoEventWireup="True" Inherits="UsersInfo" Codebehind="UsersInfo.aspx.cs" %>

<%@ Register Src="~/UsersList/UserInfoView.ascx" TagPrefix="uc1" TagName="UserInfoView" %>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <asp:HyperLink ID="hlMain" runat="server" Visible="false" NavigateUrl="~/Default.aspx" />
   
    <div style="width: 60%;" >
        <uc1:UserInfoView ID="userInfoView" runat="server"/>
    </div>
 </asp:Content>