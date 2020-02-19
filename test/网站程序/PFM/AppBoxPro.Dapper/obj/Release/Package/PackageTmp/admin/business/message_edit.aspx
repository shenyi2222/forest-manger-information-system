﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="message_edit.aspx.cs" Inherits="AppBoxPro.Dapper.admin.business.message_edit" %>

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
                <f:Toolbar ID="Toolbar1" runat="server" Position="Bottom">
                    <Items>
                        <f:Button ID="btnClose" Icon="SystemClose" EnablePostBack="false" runat="server"
                            Text="关闭">
                        </f:Button>
                        <f:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
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
                                <f:Label ID="labRecipients" runat="server" Label="收信方">
                                </f:Label>
                            </Items>
                        </f:FormRow>
                        <f:FormRow runat="server">
                            <Items>
                                <f:TextArea ID="tbxContent" runat="server" Label="发送内容" EmptyText="添加文本" Height="300px">
                                </f:TextArea>
                            </Items>
                        </f:FormRow>
                        <f:FormRow ID="FormRow4" runat="server">
                            <Items>
                                <f:CheckBox ID="cbxEnabled" runat="server" Label="定时发送" Checked="true" OnCheckedChanged="cbxEnabled_CheckedChanged" AutoPostBack="true">
                                </f:CheckBox>
                            </Items>
                        </f:FormRow>
                        <f:FormRow ID="FormRow1" runat="server">
                            <Items>
                                <f:DatePicker ID="pickerDate" runat="server" Enabled="true"></f:DatePicker>
                                <f:Label runat="server" Width="40px"></f:Label>
                            </Items>
                        </f:FormRow>
                        <f:FormRow ID="FormRow2" runat="server">
                            <Items>
                                <f:TimePicker ID="pickerTime" runat="server" Enabled="true"></f:TimePicker>
                                <f:Label runat="server"></f:Label>
                            </Items>
                        </f:FormRow>
                    </Rows>
                </f:Form>
            </Items>
        </f:Panel>
    </form>
</body>
</html>
