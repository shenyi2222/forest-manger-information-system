﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="user_select_role.aspx.cs"
    Inherits="AppBoxPro.Dapper.admin.user_select_role" %>

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
                        <f:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                        </f:ToolbarSeparator>
                        <f:Button ID="btnSaveClose" ValidateForms="SimpleForm1" Icon="SystemSaveClose" EnablePostBack="false"
                            runat="server" Text="选择后关闭">
                            <Listeners>
                                <f:Listener Event="click" Handler="onSaveCloseClick" />
                            </Listeners>
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
                                <f:CheckBoxList ID="cblRole" ColumnNumber="4" Label="所属角色" ShowLabel="false" runat="server">
                                </f:CheckBoxList>
                            </Items>
                        </f:FormRow>
                    </Rows>
                </f:Form>
            </Items>
        </f:Panel>
    </form>

    <script>

        var cblRoleClientID = '<%= cblRole.ClientID %>';

        function onSaveCloseClick() {
            // 数据源 - 复选框列表
            var cblRole = F(cblRoleClientID);

            var roleNames = [], roleIds = [];
            $.each(cblRole.getValue(), function (index, item) {
                roleNames.push(cblRole.getTextByValue(item));
                roleIds.push(item);
            });

            // 返回当前活动Window对象（浏览器窗口对象通过F.getActiveWindow().window获取）
            var activeWindow = F.getActiveWindow();
            activeWindow.window.updateSelectedRole(roleNames, roleIds);
            activeWindow.hide();
        }

    </script>
</body>
</html>
