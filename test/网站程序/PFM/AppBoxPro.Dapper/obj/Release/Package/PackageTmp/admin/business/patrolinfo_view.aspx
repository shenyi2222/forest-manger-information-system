<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="patrolinfo_view.aspx.cs" Inherits="AppBoxPro.Dapper.admin.business.patrolinfo_view" %>

<!DOCTYPE html>
<html>
<head runat="server">
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
        <div id="map" style="padding-bottom:60%"></div>
    </form>
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

        var baseLayers = {
            "地图": normal,
            "影像": image
        }
        var overlayMaps = {
            "巡护区": units
        };

        var map = L.map("map", {
            center: [<%=point.Y%>, <%=point.X%>],
            zoom: 15,
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
        var marker = L.marker([<%=point.Y%>, <%=point.X%>]).addTo(map);
        marker.bindPopup("<table><tr><td><%=patrolinfo.Town + " " + patrolinfo.Village%></td></tr ><tr><td><%=patrolinfo.Name%></td></tr><tr><td><%=patrolinfo.Time%></td></tr></table > ").openPopup();
    </script>
</body>
</html>
