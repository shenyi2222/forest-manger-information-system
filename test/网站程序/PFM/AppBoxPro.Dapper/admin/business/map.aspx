<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="map.aspx.cs" Inherits="AppBoxPro.Dapper.admin.business.map" %>

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

    <div id="map" style="padding-bottom:50%"></div>

    <script>
        var intervald = null;
        //定义图标
        var renIcon = L.icon({
            iconUrl: 'images/cliprenyuan.jpg', //图标地址
            iconSize: [25, 30], // 图标宽高
        });


        //定义基础图层
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
        var em = null,
            map = null;
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
            "巡护区": units,
        };
        var map = L.map("map", {
            zoomControl: true,
            attributionControl: false,
            center: [30.260863, 119.72331],
            zoom: 18,
            layers: [units, image] //默认图层
        });
        L.control.scale().addTo(map);
        var marker = L.marker([30.260863, 119.72331]).addTo(map);
        L.control.layers(baseLayers, overlayMaps).addTo(map);
        marker.bindPopup("<table><tr><td>锦城街道</td></tr ><tr><td>横街村</td></tr><tr><td>陈银松</td></tr></table > ");
        //L.control.zoom({
        //    zoomInTitle: '放大',
        //    zoomOutTitle: '缩小',
        //    position: 'bottomright'
        //}).addTo(map);
    </script>
</body>
</html>
