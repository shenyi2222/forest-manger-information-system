<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="patrolinfo.aspx.cs" Inherits="AppBoxPro.Dapper.admin.business.patrolinfo" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
        <f:Panel ID="Panel1" runat="server" BodyPadding="5px"
            ShowBorder="false" Layout="VBox" BoxConfigAlign="Stretch" BoxConfigPosition="Start"
            ShowHeader="false" Title="巡护信息查看">
            <Items>
                <f:Form ID="Form2" runat="server" ShowHeader="false" ShowBorder="false" LabelAlign="Right">
                    <Rows>
                        <f:FormRow ID="FormRow1" runat="server">
                            <Items>
                                <f:DatePicker ID="stDate" runat="server" EmptyText="起始时间"></f:DatePicker>
                                <f:DatePicker ID="etDate" runat="server" EmptyText="截止时间" CompareControl="stDate"
                                    CompareOperator="GreaterThan" CompareMessage="截止时间应该大于起始时间">
                                </f:DatePicker>
                                <f:TwinTriggerBox ID="ttbSearchMessage" runat="server" ShowLabel="false" EmptyText="在列表中搜索"
                                    Trigger1Icon="Clear" Trigger2Icon="Search" ShowTrigger1="false" OnTrigger2Click="ttbSearchMessage_Trigger2Click"
                                    OnTrigger1Click="ttbSearchMessage_Trigger1Click">
                                </f:TwinTriggerBox>
                                <f:Label runat="server"></f:Label>
                            </Items>
                        </f:FormRow>
                    </Rows>
                </f:Form>
                <f:Grid ID="Grid1" runat="server" BoxFlex="1" ShowBorder="true" ShowHeader="false"
                    DataKeyNames="ID,Name" AllowSorting="true" OnSort="Grid1_Sort" SortField="Time"
                    SortDirection="DESC" AllowPaging="true" IsDatabasePaging="true"
                    OnPageIndexChange="Grid1_PageIndexChange">
                    <PageItems>
                        <f:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                        </f:ToolbarSeparator>
                        <f:ToolbarText ID="ToolbarText1" runat="server" Text="每页记录数：">
                        </f:ToolbarText>
                        <f:DropDownList ID="ddlGridPageSize" Width="80px" AutoPostBack="true" OnSelectedIndexChanged="ddlGridPageSize_SelectedIndexChanged"
                            runat="server">
                            <f:ListItem Text="10" Value="10" />
                            <f:ListItem Text="20" Value="20" />
                            <f:ListItem Text="50" Value="50" />
                            <f:ListItem Text="100" Value="100" />
                        </f:DropDownList>
                    </PageItems>
                    <Columns>
                        <f:BoundField DataField="Town" SortField="Town" Width="100px" HeaderText="乡镇名" />
                        <f:BoundField DataField="Village" SortField="Village" Width="100px" HeaderText="村名" />
                        <f:BoundField DataField="Name" SortField="Name" Width="100px" HeaderText="姓名" />
                        <f:BoundField DataField="Tel" SortField="Tel" Width="120px" HeaderText="手机号码" />
                        <f:ImageField DataImageUrlField="Path" HeaderText="图片" Width="222px" ImageWidth="200px" ImageHeight="150px"></f:ImageField>
                        <f:BoundField DataField="Time" SortField="Time" Width="140px" HeaderText="上报时间" DataFormatString="{0:yyyy-MM-dd HH:mm}" />
                        <f:WindowField TextAlign="Center" Icon="Information" ToolTip="查看详细信息" Title="查看详细信息"
                            WindowID="Window1" DataIFrameUrlFields="ID" DataIFrameUrlFormatString="~/admin/business/patrolinfo_view.aspx?id={0}"
                            Width="50px" />
                    </Columns>
                </f:Grid>
            </Items>
        </f:Panel>
        <f:Window ID="Window1" runat="server" IsModal="true" Hidden="true" Target="Top" EnableResize="true"
            EnableMaximize="true" EnableIFrame="true" IFrameUrl="about:blank"
            Width="900px" Height="600px" OnClose="Window1_Close">
        </f:Window>
    </form>
</body>
</html>
