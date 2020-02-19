<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="ranger_view.aspx.cs" Inherits="AppBoxPro.Dapper.admin.business.ranger_view" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
        <f:Panel ID="Panel1" ShowBorder="false" ShowHeader="false"  AutoScroll="true" runat="server">
            <Toolbars>
                <f:Toolbar ID="Toolbar1" runat="server">
                    <Items>
                        <f:Button ID="btnClose" Icon="SystemClose" EnablePostBack="false" runat="server"
                            Text="关闭">
                        </f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Items>
                <f:Form ID="Form2" ShowBorder="false" ShowHeader="false"  runat="server" BodyPadding="10px"
                    Title="SimpleForm" LabelAlign="Left">
                    <Rows>
                        <f:FormRow ID="FormRow1" runat="server">
                            <Items>
                                <f:Label ID="labID" runat="server" Label="编号">
                                </f:Label>
                                <f:Label ID="labName" runat="server" Label="姓名">
                                </f:Label>
                            </Items>
                        </f:FormRow>
                        <f:FormRow ID="FormRow2" runat="server">
                            <Items>
                                <f:Label ID="labTel" runat="server" Label="手机号码">
                                </f:Label>
                                <f:Label runat="server"></f:Label>
                            </Items>
                        </f:FormRow>
                        <f:FormRow ID="FormRow3" runat="server">
                            <Items>
                                <f:Label ID="labTown" runat="server" Label="乡镇名">
                                </f:Label>
                                <f:Label ID="LabVillage" runat="server" Label="村名">
                                </f:Label>
                            </Items>
                        </f:FormRow>
                    </Rows>
                </f:Form>
            </Items>
        </f:Panel>
    </form>
</body>
</html>
