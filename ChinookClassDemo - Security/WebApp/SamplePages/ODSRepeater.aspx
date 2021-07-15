<%@ Page Title="Repeater Control" Language="C#" MasterPageFile="~/Site.Master" 
    AutoEventWireup="true" CodeBehind="ODSRepeater.aspx.cs" 
    Inherits="WebApp.SamplePages.ODSRepeater" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Repeater Control displaying Nested Query</h1>
    <div class="row">
        <div class="offset-1">
            <uc1:MessageUserControl runat="server" ID="MessageUserControl" />
        </div>
    </div>
    <div class="row">
        <div class="offset-2">
            <asp:Repeater ID="EmployeeCustomers" runat="server" 
                DataSourceID="EmployeeCustomersODS"
                  ItemType="ChinookSystem.ViewModels.EmployeeItem">
                <HeaderTemplate>
                    <h3>Sales Support Employees</h3> 
                </HeaderTemplate>
                <ItemTemplate>
                    <br />
                    <%# Item.FullName %> (<%# Item.Title %>) has
                     <%# Item.NumberOfCustomers %> customers
                    <br /><br />
                   <%-- <asp:GridView ID="CustomersOfEmployee" runat="server"
                        DataSource='<%# Item.CustomerList %>'
                         ItemType="ChinookSystem.ViewModels.CustomerItem">
                    </asp:GridView>--%>
                    <asp:Repeater ID="CustomersOfEmployee" runat="server"
                        DataSource='<%# Item.CustomerList %>'
                         ItemType="ChinookSystem.ViewModels.CustomerItem">
                        <ItemTemplate>
                            Name: <%# Item.FullName %>&nbsp;&nbsp;
                            Phone: <%# Item.Phone %>&nbsp;&nbsp;
                            City: <%# Item.City %>&nbsp;&nbsp;
                            State: <%# Item.State == null ? "Unknown" : Item.State %><br />
                        </ItemTemplate>
                    </asp:Repeater>
                    
                </ItemTemplate>
                <SeparatorTemplate>
                    <hr style="height:8px; background-color:black;" />
                </SeparatorTemplate>
            </asp:Repeater>
        </div>
    </div>
    <asp:ObjectDataSource ID="EmployeeCustomersODS" runat="server" 
        OldValuesParameterFormatString="original_{0}" 
        SelectMethod="Employee_EmployeeCustomers" 
        TypeName="ChinookSystem.BLL.EmployeeController"
         OnSelected="SelectCheckForException">
    </asp:ObjectDataSource>
</asp:Content>
