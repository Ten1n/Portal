<%@ Control Language="C#" AutoEventWireup="true" Inherits="Controls_UsersList" CodeBehind="UsersList.ascx.cs" %>

<asp:ObjectDataSource ID="UserNamesAndStatusesObjectDataSource" runat="server"
            TypeName="Controls_UsersList" SelectMethod="GetUsersStatusInfo" />

<%--<asp:DataGrid ID="grdUsersList" runat="server"
    DataKeyField="UserID"
    AllowSorting="true"
    DataSourceID="UserNamesAndStatusesObjectDataSource" AutoGenerateColumns="False" OnSortCommand="SortingCommandDataGrid_Click">
    <ItemStyle Width="5px" />
    <Columns>
        <asp:BoundColumn DataField="UserName" HeaderText="User Name:" SortExpression="UserNameSortingExpression"/>
        <asp:BoundColumn DataField="Status" HeaderText="Status:" SortExpression="StatusSortingExpression"/>

        <asp:TemplateColumn HeaderText="Operations"
            ItemStyle-HorizontalAlign="Right"
            meta:resourcekey="hOperations">
            <ItemTemplate>
                <asp:ImageButton ID="btnIll" runat="server"
                    meta:resourcekey="btnIll"
                    OnClick="OnSetIll"
                    ImageUrl="~/Images/Ill.ico"
                    Height="17" Width="17" />
                <asp:ImageButton ID="btnTrustIll" runat="server"
                    meta:resourcekey="btnTrustIll"
                    OnClick="OnSetTrustIll"
                    ImageUrl="~/Images/TrustIll.ico"
                    Height="17" Width="17" />
                <asp:ImageButton ID="btnBusinessTrip" runat="server"
                    meta:resourcekey="btnBusinessTrip"
                    OnClick="OnSetBusinessTrip"
                    ImageUrl="~/Images/BusinessTrip.ico"
                    Height="17" Width="17" />
                <asp:ImageButton ID="btnVacation" runat="server"
                    meta:resourcekey="btnVacation"
                    OnClick="OnSetVacation"
                    ImageUrl="~/Images/Vacation.ico"
                    Height="17" Width="17" />
                <asp:ImageButton ID="btnLesson" runat="server"
                    meta:resourcekey="btnLesson"
                    OnClick="OnSetLesson"
                    ImageUrl="~/Images/lesson.png"
                    Height="17" Width="17" />

                <asp:LinkButton ID="lbtnEdit" runat="server"
                    CssClass="control-hyperlink-big"
                    meta:resourcekey="lbtnEdit" />
            </ItemTemplate>
            <HeaderStyle Width="30%" HorizontalAlign="Center" Font-Bold="true" />
        </asp:TemplateColumn>
    </Columns>
    <ItemStyle CssClass="gridview-row" />
    <AlternatingItemStyle CssClass="gridview-alternatingrow" />
</asp:DataGrid>--%>

<asp:GridView ID="grdUsersList" runat="server" AllowSorting="true"
    DataSourceID="UserNamesAndStatusesObjectDataSource" AutoGenerateColumns="False" OnSorting="SortingCommand_Click">
    <Columns>
         <asp:BoundField DataField="UserName" HeaderText="User Name:" SortExpression="UserNameSortingExpression"/>
         <asp:BoundField DataField="Status" HeaderText="Status:" SortExpression="StatusSortingExpression"/>
         <asp:TemplateField HeaderText="Userprofile">
            <ItemTemplate>
               <asp:ImageButton ID="btnIll" runat="server"
                    meta:resourcekey="btnIll"
                    OnClick="OnSetIll"
                    ImageUrl="~/Images/Ill.ico"
                    Height="17" Width="17" />
                <asp:ImageButton ID="btnTrustIll" runat="server"
                    meta:resourcekey="btnTrustIll"
                    OnClick="OnSetTrustIll"
                    ImageUrl="~/Images/TrustIll.ico"
                    Height="17" Width="17" />
                <asp:ImageButton ID="btnBusinessTrip" runat="server"
                    meta:resourcekey="btnBusinessTrip"
                    OnClick="OnSetBusinessTrip"
                    ImageUrl="~/Images/BusinessTrip.ico"
                    Height="17" Width="17" />
                <asp:ImageButton ID="btnVacation" runat="server"
                    meta:resourcekey="btnVacation"
                    OnClick="OnSetVacation"
                    ImageUrl="~/Images/Vacation.ico"
                    Height="17" Width="17" />
                <asp:ImageButton ID="btnLesson" runat="server"
                    meta:resourcekey="btnLesson"
                    OnClick="OnSetLesson"
                    ImageUrl="~/Images/lesson.png"
                    Height="17" Width="17" />
                <asp:LinkButton ID="lbtnEdit" runat="server"
                    CssClass="control-hyperlink-big"
                    meta:resourcekey="lbtnEdit" />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
    <AlternatingRowStyle CssClass="gridview-alternatingrow" />
    <RowStyle HorizontalAlign="Center" />
</asp:GridView>
<asp:Label runat="server" ID="lblException" CssClass="control-errorlabel" Visible="false" />