<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ranger_new.aspx.cs" Inherits="AppBoxPro.Dapper.admin.business.ranger_new" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
        <f:Panel ID="Panel1" ShowBorder="false" ShowHeader="false" AutoScroll="true" runat="server">
            <Toolbars>
                <f:Toolbar ID="Toolbar1" runat="server">
                    <Items>
                        <f:Button ID="btnClose" Icon="SystemClose" EnablePostBack="false" runat="server"
                            Text="关闭">
                        </f:Button>
                        <f:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                        </f:ToolbarSeparator>
                        <f:Button ID="btnSaveClose" ValidateForms="SimpleForm1" Icon="SystemSaveClose" OnClick="btnSaveClose_Click"
                            runat="server" Text="保存后关闭">
                        </f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Items>
                <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="false" runat="server" BodyPadding="10px"
                    Title="SimpleForm">
                    <Rows>
                        <f:FormRow runat="server">
                            <Items>
                                <f:TextBox ID="tbxName" runat="server" Label="姓名" Required="true" ShowRedStar="true">
                                </f:TextBox>
                                <f:TextBox ID="tbxTel" runat="server" Label="手机号码" Required="true" ShowRedStar="true" RegexPattern="NUMBER">
                                </f:TextBox>
                            </Items>
                        </f:FormRow>
                        <f:FormRow runat="server">
                            <Items>
                                <f:DropDownList ID="ddlCharacter" Label="角色" Required="true" ShowRedStar="true" runat="server" AutoSelectFirstItem="true">
                                    <f:ListItem Text="护林人员" Value="护林人员" />
                                    <f:ListItem Text="领导" Value="领导" />
                                </f:DropDownList>
                                <f:Label runat="server"></f:Label>
                            </Items>
                        </f:FormRow>
                        <f:FormRow ID="FormRow5" runat="server">
                            <Items>
                                <f:TextBox ID="tbxPassword" runat="server" TextMode="Password" Label="登录密码" Required="true" ShowRedStar="true">
                                </f:TextBox>
                                <f:Label runat="server"></f:Label>
                            </Items>
                        </f:FormRow>
                        <f:FormRow runat="server">
                            <Items>
                                <f:TextBox ID="tbxTown" runat="server" Label="乡镇名">
                                </f:TextBox>
                                <f:TextBox ID="tbxVillage" runat="server" Label="村名">
                                </f:TextBox>
                            </Items>
                        </f:FormRow>
                    </Rows>
                </f:Form>
            </Items>
        </f:Panel>
    </form>
</body>
</html>
