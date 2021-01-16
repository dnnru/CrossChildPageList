<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ListStandard.ascx.cs" Inherits="Cross.Modules.ChildPageList.Template.ListStandard" %>
<asp:DataList ID="dlSubTabList" runat="server" RepeatColumns="<%# ColumnPerRow %>" >
    <ItemTemplate >
        &nbsp;&nbsp;
        <asp:HyperLink ID="hypTab" runat="server" Target="<%#LinkTarget%>" CssClass="child-page-link"  ToolTip='<%# Convert.ToString(Eval("Description")) %>'
            Text='<%# Server.HtmlDecode(Convert.ToString(Eval("TabName"))) %>' NavigateUrl='<%# NavigationManager.NavigateURL(Convert.ToInt32(Eval("TabID"))) %>' />
    </ItemTemplate>
    <ItemStyle Wrap="false"></ItemStyle>
</asp:DataList>