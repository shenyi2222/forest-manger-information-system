<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="message_new.aspx.cs" Inherits="AppBoxPro.Dapper.admin.business.message_new" %>

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
                        <f:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                        </f:ToolbarSeparator>
                        <f:Button ID="btnSaveClose" ValidateForms="SimpleForm1" Icon="SystemSaveClose" OnClick="btnSaveClose_Click"
                            runat="server" Text="发送并关闭">
                        </f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Items>
                <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="false" runat="server" BodyPadding="10px"
                    Title="SimpleForm">
                    <Rows>
                        <f:FormRow runat="server">
                            <%--<Items>
                                <f:TwinTriggerBox ID="tbSelectedRecipients" EnableEdit="false" EnableTrigger1PostBack="false" EnableTrigger2PostBack="false"
                                    Trigger1Icon="Clear" Trigger2Icon="Search" ShowTrigger1="false" ShowTrigger2="true"
                                    OnClientTrigger1Click="onSelectedRecipientsTrigger1Click();" OnClientTrigger2Click="onSelectedRecipientsTrigger2Click();"
                                    Label="收信方" EmptyText="默认发送至全体" runat="server">
                                </f:TwinTriggerBox>
                            </Items>--%>
                            <Items>
                                <f:DropDownList ID="ddlVillage" Label="收信方" runat="server" DataTextField="Village"></f:DropDownList>
                            </Items>
                        </f:FormRow>
                        <f:FormRow runat="server">
                            <Items>
                                <f:TextArea ID="tbxContent" runat="server" Label="发送内容" EmptyText="添加文本" Required="true" ShowRedStar="true" Height="300px">
                                </f:TextArea>
                            </Items>
                        </f:FormRow>
                        <f:FormRow ID="FormRow5" runat="server">
                            <Items>
                                <f:CheckBox ID="cbxEnabled" runat="server" Label="定时发送" Checked="false" OnCheckedChanged="cbxEnabled_CheckedChanged" AutoPostBack="true">
                                </f:CheckBox>
                                <f:Label runat="server"></f:Label>
                            </Items>
                        </f:FormRow>
                        <f:FormRow ID="FormRow1" runat="server">
                            <Items>
                                <f:DatePicker ID="pickerDate" runat="server" Enabled="false"></f:DatePicker>
                                <f:Label runat="server"></f:Label>
                            </Items>
                        </f:FormRow>
                        <f:FormRow ID="FormRow2" runat="server">
                            <Items>
                                <f:TimePicker ID="pickerTime" runat="server" Enabled="false"></f:TimePicker>
                                <f:Label runat="server"></f:Label>
                            </Items>
                        </f:FormRow>
                    </Rows>
                </f:Form>
            </Items>
        </f:Panel>
        <f:HiddenField ID="hfSelectedRecipients" runat="server">
        </f:HiddenField>
        <f:Window ID="Window1" Title="编辑" Hidden="true" EnableIFrame="true" runat="server"
            EnableMaximize="true" EnableResize="true" Target="Top" IsModal="True" Width="550px"
            Height="350px">
        </f:Window>
    </form>
</body>
</html>
