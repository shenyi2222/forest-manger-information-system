<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="parea.aspx.cs" Inherits="AppBoxPro.Dapper.admin.business.parea" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <meta charset="utf-8">
    <title></title>
    <link rel="stylesheet" href="https://unpkg.com/leaflet@1.5.1/dist/leaflet.css" />
    <!-- Make sure you put this AFTER Leaflet's CSS -->
    <script src="https://unpkg.com/leaflet@1.5.1/dist/leaflet.js"></script>
    <script src="../../res/js/leaflet.ChineseTmsProviders.js"></script>
    <link rel="stylesheet" href="../../res/css/leaflet.measure.css" />
    <script src="../../res/js/leaflet.measure.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" />
        <f:Panel ID="Panel1" ShowHeader="false" ShowBorder="false"
            BodyPadding="5px" AutoScroll="true" runat="server">
            <Items>
                <f:Form ID="Form2" runat="server" ShowHeader="false" ShowBorder="false" LabelAlign="Right">
                    <Rows>
                        <f:FormRow ID="FormRow1" runat="server">
                            <Items>
                                <f:DropDownList ID="ddlTown" Label="街道/镇" runat="server" AutoPostBack="true" DataTextField="Town">
                                    <%--<f:ListItem Text="锦城街道" Selected="true" Value="tree" />--%>
                                </f:DropDownList>
                                <f:DropDownList ID="ddlVillage" Label="村" runat="server" AutoPostBack="true" DataTextField="Village">
                                    <%--<f:ListItem Text="横街村" Selected="true" Value="tree" />--%>
                                </f:DropDownList>
                                <f:DropDownList ID="ddlRanger" Label="护林人员" DataTextField="Name" runat="server">
                                    <%--<f:ListItem Text="陈银松" Selected="true" Value="tree" />--%>
                                </f:DropDownList>
                            </Items>
                        </f:FormRow>
                    </Rows>
                </f:Form>
            </Items>
        </f:Panel>
    </form>
    <div id="map" style="padding-bottom:50%"></div>

    <script>

        var normalm = L.tileLayer.chinaProvider('TianDiTu.Normal.Map', {
            maxZoom: 18,
            minZoom: 5
        }),
            normala = L.tileLayer.chinaProvider('TianDiTu.Normal.Annotion', {
                maxZoom: 18,
                minZoom: 5
            }),
            imgm = L.tileLayer.chinaProvider('TianDiTu.Satellite.Map', {
                maxZoom: 18,
                minZoom: 5
            }),
            imga = L.tileLayer.chinaProvider('TianDiTu.Satellite.Annotion', {
                maxZoom: 18,
                minZoom: 5
            });

        var normal = L.layerGroup([normalm, normala]),
            image = L.layerGroup([imgm, imga]);
        var units = L.tileLayer.wms('http://39.106.41.205:8080/geoserver/PFM/wms', {
            layers: 'PFM:units',
            maxZoom: 18,
            format: 'image/png',
            transparent: true,
            opacity: 0.5
        });
        var ckpoint = L.tileLayer.wms('http://39.106.41.205:8080/geoserver/PFM/wms', {
            layers: 'PFM:unitrangers',
            maxZoom: 18,
            format: 'image/png',
            transparent: true,
            opacity: 0.5
        });

        var baseLayers = {
            "地图": normal,
            "影像": image
        }
        var overlayMaps = {
            "巡护区": units,
            "打卡点": ckpoint
        };
        var map = L.map("map", {
            center: [30.25, 119.75],
            zoom: 13,
            layers: [units, image],
            zoomControl: false
        });
        L.control.scale().addTo(map);
        L.control.layers(baseLayers, overlayMaps).addTo(map);
        L.control.zoom({
            zoomInTitle: '放大',
            zoomOutTitle: '缩小',
            position: 'bottomright'
        }).addTo(map);
    </script>
</body>
</html>

