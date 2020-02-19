<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="parea.aspx.cs" Inherits="AppBoxPro.Dapper.admin.business.parea" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <meta charset="utf-8">
    <title></title>
    <link href="../../res/css/ol.css" rel="stylesheet" />
    <script src="../../res/js/jquery-3.2.1.js"></script>
    <script src="../../res/js/jquery.min.js"></script>
    <script src="../../res/js/proj4.js"></script>
    <script src="../../res/js/ol.js"></script>

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
                                <f:DropDownList ID="ddlTown" Label="乡镇" ShowRedStar="true" CompareType="String" DataTextField="town" DataValueField="town"
                                    CompareValue="-1" CompareOperator="NotEqual" CompareMessage="请选择乡镇！" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTown_SelectedIndexChanged">
                                </f:DropDownList>
                                <f:DropDownList ID="ddlVillage" Label="村" ShowRedStar="true" CompareType="String" DataTextField="village" DataValueField="village"
                                    CompareValue="-1" CompareOperator="NotEqual" CompareMessage="请选择村！" runat="server"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlVillage_SelectedIndexChanged" Enabled="false">
                                </f:DropDownList>
                                <f:DropDownList ID="ddlRanger" Label="护林人员" ShowRedStar="true" CompareType="String" DataTextField="name" DataValueField="name"
                                    CompareValue="-1" CompareOperator="NotEqual" CompareMessage="请选择护林人员！" runat="server"
                                    AutoPostBack="true"  OnSelectedIndexChanged="ddlRanger_SelectedIndexChanged" Enabled="false">
                                </f:DropDownList>
                                   
                            </Items>
                        </f:FormRow>
                    </Rows>
                </f:Form>
            </Items>
        </f:Panel>
    </form>
    <div id="map" style="padding-bottom: 50%"></div>
    <script>

        var intervald = null;
        ////定义投影
        proj4.defs('EPSG:4549', '+proj=tmerc +lat_0=0 +lon_0=120 +k=1 +x_0=500000 +y_0=0 +ellps=GRS80 +units=m +no_defs');
        ol.proj.proj4.register(proj4);
        var projection = new ol.proj.Projection({
            code: 'EPSG:4549',
            extent: [290000, 3000000, 550000, 3560000]
        });
        //Add a Projection object to the list of supported projections that can be looked up by their code.
        ol.proj.addProjection(projection);
        var map = new ol.Map({
            layers: [new ol.layer.Tile({
               source: new ol.source.OSM({
                   //zhuanhuan

                projection:ol.proj.getTransform('EPSG:3857','EPSG:4549')

            })
            })],
            target: 'map',
            controls: ol.control.defaults({
             attributionOptions: { collapsible: false }
            }),
            view: new ol.View({

                projection: 'EPSG:4549',

                center: [464567.96875, 3346559.25],

                zoom: 6

            })
        });
        //加载wfs图层
        var wfsVectorLayer; 
        wfsVectorLayer = new ol.layer.Vector({
            source: new ol.source.Vector({
                format: new ol.format.GeoJSON({
                    geometryName: 'the_geom'
                }),
                url: 'http://localhost:8080/geoserver/PFM/ows?service=WFS&version=1.0.0&request=GetFeature&typeName=PFM%3Aunits&maxFeatures=50&outputFormat=application%2Fjson&srsname=EPSG:4549'
            }),
            style: function (feature, resolution) {
                return new ol.style.Style({
                    stroke: new ol.style.Stroke({
                        color: 'black',
                        width: 1
                    })
                });
            }

        });
        
        map.addLayer(wfsVectorLayer);
        //根据条件查询
        function a()
        {
            var town = $("#<%=ddlTown.ClientID%> option:selected").text();
           // var town = $("#<%=ddlTown.ClientID %>").find(":selected").text();
            //var town = $("#<%=ddlTown.ClientID%>").text();
            //var town = $("#ddlTown option:selected").text();
        var village= $("#ddlVillage").val();
         var ranger =  $("#ddlRanger").val();
            alert(town);
            if (town != null)
            {
                if (village = null)
                {
                    //只根据town进行查询
                    var filter = new OpenLayers.Filter.Comparison({
                        type: OpenLayers.Filter.Comparison.EQUAL_TO,
                        xiangname: town
                    });
                    wfsVectorLayer.filter = filter;
                    wfsVectorLayer.refresh();
                }
                else if (ranger = null)
                {
                    //根据2个条件查询
                    var filter = new OpenLayers.Filter.Comparison({
                        type: OpenLayers.Filter.Comparison.EQUAL_TO,
                        xiangname: town,
                        cunname:village
                    });
                    wfsVectorLayer.filter = filter;
                    wfsVectorLayer.refresh();
                }
                else
                {
                    //根据3个条件查询
                }
            }
        }
        intervald = setInterval(a, 5000);

       

    </script>
</body>
</html>

