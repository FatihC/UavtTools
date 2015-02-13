var app = angular.module('app', ['uiGmapgoogle-maps', 'angularFileUpload', 'ngResource']);

app.controller('indexController', ["$scope", "$resource", "$http", "$fileUploader",
    function indexController($scope, $resource, $http, $fileUploader) {
        var uploader = $scope.uploader = $fileUploader.create({
            scope: $scope,
            autoUpload: true,
            url: '/ModemMap/api/workapi/upload'
        });
        $scope.markers = [];
        $scope.map = { center: { latitude: 39.995185, longitude: 28.452582 }, zoom: 10 };
        $scope.options = { scrollwheel: true };

        uploader.bind('complete', function (event, xhr, item, response) {
            $scope.excelData = response;
            var markers = [];
            var i = 0;
            angular.forEach(response, function (dt) {

                var lat = dt.boylam.replace(',', '.');
                var dLat = parseFloat(lat);

                var longi = dt.enlem.replace(',', '.');
                var dLong = parseFloat(longi);

                markers.push({
                    latitude: dLong,
                    longitude: dLat,
                    title:"",
                    id: i,
                    options: {
                        labelContent: "",
                        labelAnchor: "22 0",
                        labelClass: "marker-labels"
                    }
                });
                i++;
            });

            if (!$scope.$$phase) {
                $scope.$apply(function () {
                    $scope.markers = markers;
                });
            }
        });
    }]);