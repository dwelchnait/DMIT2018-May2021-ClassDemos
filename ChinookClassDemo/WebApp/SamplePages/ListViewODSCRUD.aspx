<%@ Page Title="ListView ODS CRUD" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ListViewODSCRUD.aspx.cs" Inherits="WebApp.SamplePages.ListViewODSCRUD" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>ListView ODS CRUD of Albums</h1>
    <div class="row">
        <div class="offset-2">
            <blockquote class="alert alert-info">
                This sample will use the asp:ListView control<br/>
                This sample will use the ObjectDataSource for the ListView control<br />
                This sample will use minimal code behind<br />
                This sample will use the course's MessageUserControl for erroring handling
            </blockquote>
        </div>
    </div>
    <div class="row">
        <uc1:MessageUserControl runat="server" ID="MessageUserControl" />

        <%-- Validation Summary is setup ONE for EACH validation group --%>
        <asp:ValidationSummary ID="ValidationSummaryEdit" runat="server" 
             HeaderText="Following are concerns with your data while editting"
             ValidationGroup="egroup"/>
         <asp:ValidationSummary ID="ValidationSummaryInsert" runat="server" 
             HeaderText="Following are concerns with your data while inserting"
             ValidationGroup="igroup"/>
    </div>
    <div class="row">
        <%-- IF, you can not delete, CHECK to see if you are missing the DataKeyNames parameter
             The parameter is needed to identify the pkey value of the record collection--%>
        <asp:ListView ID="AlbumList" runat="server" 
            DataSourceID="AlbumListODS" 
            InsertItemPosition="FirstItem"
            DataKeyNames="AlbumId">

            <AlternatingItemTemplate>
                <tr style="background-color: #FFFFFF; color: #284775;">
                    <td>
                        <asp:Button runat="server" CommandName="Delete" Text="Delete" ID="DeleteButton" />
                        <asp:Button runat="server" CommandName="Edit" Text="Edit" ID="EditButton" />
                    </td>
                    <td>
                        <asp:Label Text='<%# Eval("AlbumId") %>' runat="server" 
                            ID="AlbumIdLabel" /></td>
                    <td>
                        <asp:Label Text='<%# Eval("Title") %>' runat="server" ID="TitleLabel" /></td>
                    <td>
                        <asp:Label Text='<%# Eval("ArtistId") %>' runat="server" ID="ArtistIdLabel" /></td>
                    <td>
                        <asp:Label Text='<%# Eval("ReleaseYear") %>' runat="server" ID="ReleaseYearLabel" /></td>
                    <td>
                        <asp:Label Text='<%# Eval("ReleaseLabel") %>' runat="server" ID="ReleaseLabelLabel" /></td>
                </tr>
            </AlternatingItemTemplate>
            <EditItemTemplate>
                <%-- validation controls will be placed inside the associate template
                     The ID of the validated control needs to be unique
                     The validation controls for a particular template needs to be grouped *****
                     The validation executes ONLY on the use of the template where the group is
                        tied to the Button--%>
                <asp:RequiredFieldValidator ID="RequiredTitleE" runat="server" 
                    ErrorMessage="Album title is required" Display="None"
                     ControlToValidate="TitleTextBoxE" ValidationGroup="egroup" >
                </asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegExTitleE" runat="server" 
                    ErrorMessage="Album title is limited to 160 characters" Display="None"
                     ControlToValidate="TitleTextBoxE" ValidationGroup="egroup"
                     ValidationExpression="^.{1,10}$">
                </asp:RegularExpressionValidator>
                <asp:RegularExpressionValidator ID="RegExReleaseLabelE" runat="server" 
                    ErrorMessage="Album label is limited to 50 characters" Display="None"
                     ControlToValidate="ReleaseLabelTextBoxE" ValidationGroup="egroup"
                     ValidationExpression="^.{0,5}$">
                </asp:RegularExpressionValidator>
                <asp:RangeValidator ID="RangeReleaseYearE" runat="server" 
                    ErrorMessage="Release year must between 1950 and this year" Display="None"
                     ControlToValidate="ReleaseYearTextBoxE" MinimumValue="1950"
                     MaximumValue="<%# DateTime.Today.Year %>" ValidationGroup="egroup">
                </asp:RangeValidator>
                <tr style="background-color: #999999;">
                    <td>
                        <%-- tie the button to the appropriate validation group
                            FAILURE to tie the button to the appropriate group will cause
                            ALL validation on the page to execute--%>
                        <asp:Button runat="server" CommandName="Update" Text="Update" 
                            ID="UpdateButton"  ValidationGroup="egroup"/>
                        <asp:Button runat="server" CommandName="Cancel" Text="Cancel" 
                            ID="CancelButton" />
                    </td>
                    <td>
                        <asp:TextBox Text='<%# Bind("AlbumId") %>' runat="server" ID="AlbumIdTextBox"
                             Enabled="false"/></td>
                    <td>
                        <asp:TextBox Text='<%# Bind("Title") %>' runat="server" 
                            ID="TitleTextBoxE" /></td>
                    <td>
                        <asp:TextBox Text='<%# Bind("ArtistId") %>' runat="server" ID="ArtistIdTextBox" /></td>
                    <td>
                        <asp:TextBox Text='<%# Bind("ReleaseYear") %>' runat="server" 
                            ID="ReleaseYearTextBoxE" 
                            TextMode="Number" step="1" min="1950" required="true" /></td>
                    <td>
                        <asp:TextBox Text='<%# Bind("ReleaseLabel") %>' runat="server" 
                            ID="ReleaseLabelTextBoxE" /></td>
                </tr>
            </EditItemTemplate>
            <EmptyDataTemplate>
                <table runat="server" style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px;">
                    <tr>
                        <td>No data was returned.</td>
                    </tr>
                </table>
            </EmptyDataTemplate>
            <InsertItemTemplate>
                 <asp:RequiredFieldValidator ID="RequiredTitleI" runat="server" 
                    ErrorMessage="Album title is required" Display="None"
                     ControlToValidate="TitleTextBoxI" ValidationGroup="igroup" >
                </asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegExTitleI" runat="server" 
                    ErrorMessage="Album title is limited to 160 characters" Display="None"
                     ControlToValidate="TitleTextBoxI" ValidationGroup="igroup"
                     ValidationExpression="^.{1,10}$">
                </asp:RegularExpressionValidator>
                <asp:RegularExpressionValidator ID="RegExReleaseLabelI" runat="server" 
                    ErrorMessage="Album label is limited to 50 characters" Display="None"
                     ControlToValidate="ReleaseLabelTextBoxI" ValidationGroup="igroup"
                     ValidationExpression="^.{0,5}$">
                </asp:RegularExpressionValidator>
                <asp:RangeValidator ID="RangeReleaseYearI" runat="server" 
                    ErrorMessage="Release year must between 1950 and this year" Display="None"
                     ControlToValidate="ReleaseYearTextBoxI" MinimumValue="1950"
                     MaximumValue="<%# DateTime.Today.Year %>" ValidationGroup="igroup">
                </asp:RangeValidator>
                <tr style="">
                    <td>
                        <asp:Button runat="server" CommandName="Insert" Text="Insert" 
                            ID="InsertButton"  ValidationGroup="igroup"/>
                        <asp:Button runat="server" CommandName="Cancel" Text="Clear" 
                            ID="CancelButton" />
                    </td>
                    <td>
                        <asp:TextBox Text='<%# Bind("AlbumId") %>' runat="server" 
                            ID="AlbumIdTextBox"  Enabled="false"/></td>
                    <td>
                        <asp:TextBox Text='<%# Bind("Title") %>' runat="server" 
                            ID="TitleTextBoxI" /></td>
                    <td>
                        <asp:TextBox Text='<%# Bind("ArtistId") %>' runat="server" ID="ArtistIdTextBox" /></td>
                    <td>
                        <asp:TextBox Text='<%# Bind("ReleaseYear") %>' runat="server" 
                            ID="ReleaseYearTextBoxI" 
                             TextMode="Number" step="1" min="1950" required="true"/></td>
                    <td>
                        <asp:TextBox Text='<%# Bind("ReleaseLabel") %>' runat="server" 
                            ID="ReleaseLabelTextBoxI" /></td>
                </tr>
            </InsertItemTemplate>
            <ItemTemplate>
                <tr style="background-color: #E0FFFF; color: #333333;">
                    <td>
                        <asp:Button runat="server" CommandName="Delete" Text="Delete" ID="DeleteButton" />
                        <asp:Button runat="server" CommandName="Edit" Text="Edit" ID="EditButton" />
                    </td>
                    <td>
                        <asp:Label Text='<%# Eval("AlbumId") %>' runat="server" ID="AlbumIdLabel" /></td>
                    <td>
                        <asp:Label Text='<%# Eval("Title") %>' runat="server" ID="TitleLabel" /></td>
                    <td>
                        <asp:Label Text='<%# Eval("ArtistId") %>' runat="server" ID="ArtistIdLabel" /></td>
                    <td>
                        <asp:Label Text='<%# Eval("ReleaseYear") %>' runat="server" ID="ReleaseYearLabel" /></td>
                    <td>
                        <asp:Label Text='<%# Eval("ReleaseLabel") %>' runat="server" ID="ReleaseLabelLabel" /></td>
                </tr>
            </ItemTemplate>
            <LayoutTemplate>
                <table runat="server">
                    <tr runat="server">
                        <td runat="server">
                            <table runat="server" id="itemPlaceholderContainer" style="background-color: #FFFFFF; border-collapse: collapse; border-color: #999999; border-style: none; border-width: 1px; font-family: Verdana, Arial, Helvetica, sans-serif;" border="1">
                                <tr runat="server" style="background-color: #E0FFFF; color: #333333;">
                                    <th runat="server"></th>
                                    <th runat="server">AlbumId</th>
                                    <th runat="server">Title</th>
                                    <th runat="server">ArtistId</th>
                                    <th runat="server">ReleaseYear</th>
                                    <th runat="server">ReleaseLabel</th>
                                </tr>
                                <tr runat="server" id="itemPlaceholder"></tr>
                            </table>
                        </td>
                    </tr>
                    <tr runat="server">
                        <td runat="server" style="text-align: center; background-color: #5D7B9D; font-family: Verdana, Arial, Helvetica, sans-serif; color: #FFFFFF">
                            <asp:DataPager runat="server" ID="DataPager1">
                                <Fields>
                                    <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="True" ShowNextPageButton="False" ShowPreviousPageButton="False"></asp:NextPreviousPagerField>
                                    <asp:NumericPagerField></asp:NumericPagerField>
                                    <asp:NextPreviousPagerField ButtonType="Button" ShowLastPageButton="True" ShowNextPageButton="False" ShowPreviousPageButton="False"></asp:NextPreviousPagerField>
                                </Fields>
                            </asp:DataPager>
                        </td>
                    </tr>
                </table>
            </LayoutTemplate>
            <SelectedItemTemplate>
                <tr style="background-color: #E2DED6; font-weight: bold; color: #333333;">
                    <td>
                        <asp:Button runat="server" CommandName="Delete" Text="Delete" ID="DeleteButton" />
                        <asp:Button runat="server" CommandName="Edit" Text="Edit" ID="EditButton" />
                    </td>
                    <td>
                        <asp:Label Text='<%# Eval("AlbumId") %>' runat="server" ID="AlbumIdLabel" /></td>
                    <td>
                        <asp:Label Text='<%# Eval("Title") %>' runat="server" ID="TitleLabel" /></td>
                    <td>
                        <asp:Label Text='<%# Eval("ArtistId") %>' runat="server" ID="ArtistIdLabel" /></td>
                    <td>
                        <asp:Label Text='<%# Eval("ReleaseYear") %>' runat="server" ID="ReleaseYearLabel" /></td>
                    <td>
                        <asp:Label Text='<%# Eval("ReleaseLabel") %>' runat="server" ID="ReleaseLabelLabel" /></td>
                </tr>
            </SelectedItemTemplate>
        </asp:ListView>
        <asp:ObjectDataSource ID="AlbumListODS" runat="server" 
            DataObjectTypeName="ChinookSystem.ViewModels.AlbumItem" 
            DeleteMethod="Albums_Delete" 
            InsertMethod="Albums_Add" 
            SelectMethod="Albums_List" 
            UpdateMethod="Albums_Update"
             OnDeleted="DeleteCheckForException"
             OnInserted="InsertCheckForException"
             OnSelected="SelectCheckForException"
             OnUpdated="UpdateCheckForException"
            OldValuesParameterFormatString="original_{0}" 
            TypeName="ChinookSystem.BLL.AlbumController" >
        </asp:ObjectDataSource>
    </div>
</asp:Content>
