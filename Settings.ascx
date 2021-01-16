<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Settings.ascx.cs" Inherits="Cross.Modules.ChildPageList.Settings" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<table id="tblGerneral" cellspacing="0" cellpadding="2" width="90%" border="0" align="center" class="Normal">
    <tr>
        <td style="width:200px">
            <dnn:label ID="plParentTab" runat="server" Text="Parent Tab"></dnn:label>
        </td>
        <td>
            <asp:DropDownList ID="ddlParentTab" DataValueField="TabId" DataTextField="TabName"
                              CssClass="NormalTextBox" runat="server">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td style="width: 200px">
            <dnn:label ID="plIncludeSelf" runat="server" Suffix=":" Text="Include Self" ControlName="chkIncludeSelf">
            </dnn:label>
        </td>
        <td>
            <asp:CheckBox ID="chkIncludeSelf" runat="server"></asp:CheckBox>
        </td>
    </tr>
    <tr>
        <td style="width:200px">
            <dnn:label ID="plIncludeInvisible" runat="server" Suffix=":" Text="Include Hidden Tab"
                       ControlName="chkIncludeInvisible">
            </dnn:label>
        </td>
        <td>
            <asp:CheckBox ID="chkIncludeInvisible" runat="server"></asp:CheckBox>
        </td>
    </tr>
    <tr>
        <td style="width:200px">
            <dnn:label ID="plRecursive" runat="server" Suffix=":" ControlName="chkRecursive">
            </dnn:label>
        </td>
        <td>
            <asp:CheckBox ID="chkRecursive" runat="server"></asp:CheckBox>
        </td>
    </tr>
    <tr>
        <td style="width:200px">
            <dnn:label ID="plDisplayIcon" runat="server" Suffix=":" ControlName="chkDisplayIcon">
            </dnn:label>
        </td>
        <td>
            <asp:CheckBox ID="chkDisplayIcon" runat="server"></asp:CheckBox>
        </td>
    </tr>
    <tr>
        <td style="width:200px">
            <dnn:label ID="plListTemplate" runat="server" Suffix=":" ControlName="ddlListTemplate">
            </dnn:label>
        </td>
        <td valign="top">
            <asp:DropDownList ID="ddlListTemplate" runat="server" Width="60%">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td style="width:200px">
            <dnn:label ID="plColumnCount" runat="server" Suffix=":" ControlName="txtColumnCount">
            </dnn:label>
        </td>
        <td valign="top">
            <asp:TextBox ID="txtColumnCount" runat="server" Columns="4" Text="1"></asp:TextBox>
            <asp:RegularExpressionValidator ID="Regularexpressionvalidator4" resourcekey="MustBeInteger"
                                            runat="server" ValidationExpression="\d*" ControlToValidate="txtColumnCount">
            </asp:RegularExpressionValidator>
        </td>
    </tr>
    <tr>
        <td valign="top" style="width:200px">
            <dnn:label ID="plLinkTarget" ControlName="ddlLinkTarget" Suffix=":" runat="server">
            </dnn:label>
        </td>
        <td valign="top" align="left">
            <asp:DropDownList ID="ddlLinkTarget" runat="server" CssClass="NormalTextBox">
                <asp:ListItem Value="_self" resourcekey="liLinkTarget_Self"></asp:ListItem>
                <asp:ListItem Value="_blank" resourcekey="liLinkTarget_Blank"></asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
</table>
<p align="center">
    <asp:LinkButton CssClass="CommandButton" ID="cmdUpdate" OnClick="cmdUpdate_Click"
                    resourcekey="cmdUpdate" runat="server" BorderStyle="none" Text="Update">
    </asp:LinkButton>&nbsp;
    <asp:LinkButton CssClass="CommandButton" ID="cmdReturn" OnClick="cmdReturn_Click"
                    resourcekey="cmdReturn" runat="server" BorderStyle="none" Text="Return" CausesValidation="False">
    </asp:LinkButton>
</p>