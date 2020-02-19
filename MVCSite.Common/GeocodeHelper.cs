using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Collections;
using HtmlAgilityPack;

namespace MVCSite.Common
{
    public class Geocoordinates
    {
        public float Latitude { get; set; }
        public float Longitude { get; set; }
    }
    public class RealGeocoordinates
    {
        public int Latitude { get; set; }
        public int Longitude { get; set; }
        public string Address { get; set; }
    }
    public static class GeoCodeHelper
    {
        private const float _earthRadiusKm = 6371.009f;
        public const float _latitudeGapFor1Km = 6359.1554f;//63591.553574496618f;
        public const int RatioToMagnify = 1000000;

        private static float _calculationMedium = (float)(180.0f * RatioToMagnify / (_earthRadiusKm * Math.PI));
        public static int MinLongitude = RatioToMagnify * 70;
        public static int MaxLongitude = RatioToMagnify * 136;
        public static int MinLatitude = RatioToMagnify * 17;
        public static int MaxLatitude = RatioToMagnify * 54;
        public const int MaxDistance = 25;
        public static float PiDividedBy180 = (float)Math.PI / 180.0f;
        public static int RatioToMagnify2Times = 2 * RatioToMagnify;
        public static int RatioToMagnify4BaiduGeo =10;//Baidu geo=100000

        public const int GeoFailedCoordinateValue = 5;
        public const int MercatorEarthRadius = 6378137; //Equatorial Radius, WGS84
        public static float MercatorShift = (float)Math.PI * MercatorEarthRadius;

        public static float DistanceInKm(Geocoordinates coord1, Geocoordinates coord2)
        {
            float deltaLat = (coord1.Latitude - coord2.Latitude) * (float)Math.PI / 180.0f;
            float deltaLng = (coord1.Longitude - coord2.Longitude) * (float)Math.PI / 180.0f;
            float meanLat = ((coord1.Latitude + coord2.Latitude) / 2) * (float)Math.PI / 180.0f;
            float convTerm = (float)Math.Cos(meanLat) * deltaLng;
            return _earthRadiusKm * (float)Math.Sqrt(deltaLat * deltaLat + convTerm * convTerm);
        }
        public static float DistanceInKm(int x1,int y1,int x2,int y2)
        {
            float deltaLat = y1 - y2;
            float deltaLng = x1 - x2;
            float meanLat = (y1 + y2) * PiDividedBy180 / RatioToMagnify2Times;
            float convTerm = (float)Math.Cos(meanLat) * deltaLng;
            return _earthRadiusKm * (float)Math.Sqrt(deltaLat * deltaLat + convTerm * convTerm) * PiDividedBy180 / RatioToMagnify;
        }

        public static float LatitudeDeltaOfKm(float distance = 1.0f)
        {
            return distance * 180.0f / _earthRadiusKm / (float)Math.PI;
        }
        public static float LongitudeDeltaOfKm(float latitude, float distance = 1.0f)
        {
            return distance * 180.0f / _earthRadiusKm / (float)Math.Cos(latitude * (float)Math.PI / 180.0f) / (float)Math.PI;
        }
        public static int LatitudeIntDeltaOfKm(float distance = 1.0f)
        {
            return (int)(distance * _calculationMedium);
        }                                  
        public static int LongitudeIntDeltaOfKm(float latitude, float distance = 1.0f)
        {
            return (int)Math.Abs((distance * _calculationMedium / Math.Cos(latitude * (float)Math.PI / 180.0f)));
        }

        public static Geocoordinates ConvertMercatorToLonLat(Geocoordinates coords)
        {
            var lon = coords.Longitude / MercatorShift * 180.0;
            var lat = coords.Latitude / MercatorShift * 180.0;
            lat = 180 / Math.PI * (2 * Math.Atan(Math.Exp(lat * Math.PI / 180.0)) - Math.PI / 2.0);
            return new Geocoordinates { Longitude = (float)lon, Latitude = (float)lat };
        }
        public static Geocoordinates ConvertLonLatToMercator(Geocoordinates coords)
        {
            float lon = coords.Longitude, lat = coords.Latitude;
            var x = lon * MercatorShift / 180;
            var y = Math.Log(Math.Tan((90 + lat) * Math.PI / 360)) / (Math.PI / 180);
            y = y * MercatorShift / 180;
            return new Geocoordinates { Longitude = x, Latitude = (float)y };
        }
    }
}