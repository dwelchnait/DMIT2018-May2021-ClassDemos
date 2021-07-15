<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MessageUserControl.ascx.cs" Inherits="FreeCode.WebApp.UserControls.MessageUserControl" %>

<asp:Panel ID="MessagePanel" runat="server" CssClass="card">
    <asp:Panel ID="CardHeader" runat="server">
        <asp:Label ID="MessageTitleIcon" runat="server"> </asp:Label>
        <asp:Label ID="MessageTitle" runat="server" />
    </asp:Panel>
    <div class="card-body">
        <asp:Label ID="MessageLabel" runat="server" />
        <asp:Repeater ID="MessageDetailsRepeater" runat="server" EnableViewState="false">
            <headertemplate>
                <ul>
            </headertemplate>
            <footertemplate>
                </ul>
            </footertemplate>
            <itemtemplate>
                <li><%# Eval("Error") %></li>
            </itemtemplate>
        </asp:Repeater>
    </div>
</asp:Panel>