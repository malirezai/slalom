﻿using System;
using GeoCoordinatePortable;

namespace SlalomTracker
{
    // struct CourseDimensions { double Width = 23; Height = 259; }

    /// <summary>
    /// X,Y coorinates in relative meters to the rectangle that represents the ski course.
    /// https://www.thinkwaterski.com/dox/slalom_tolerances.pdf
    /// Lower left of course is 0,0 meters.  Upper right is 23,259.
    /// </summary>
    public class CoursePosition
    {
        public double X, Y;

        public CoursePosition(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Overrides the + operator between two CoursePositions and returns a new one 
        /// representing the sum of both X & Y.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static CoursePosition operator +(CoursePosition a, CoursePosition b)
        {
            return new CoursePosition(a.X + b.X, a.Y + b.Y);
        }
    }

    //public struct GeoCoordinate
    //{
    //    public double Latitude, Longitude;
    //}

    /// <summary>
    /// Represents the entry & exit gates for a course and the rectangle that surrounds.
    /// </summary>
    public class Course
    {
        // The default course width and length, there is a possibility that these 
        // are slightly off.
        public static readonly double WidthM = 23;
        public static readonly double LengthM = 259;

        /// <summary>
        /// Lat/Long of the pilon as you enter & exit the course.
        /// </summary>
        public GeoCoordinate CourseEntryCL { get; set; }
        public GeoCoordinate CourseExitCL { get; set; }

        /// <summary>
        /// The heading in degrees for a straight line from course entry to exit 
        /// along the center line of the wake.
        /// </summary>
        /// <remarks>
        /// Only available after Course Entry & Exit have been set.
        /// </remarks>
        public double CourseHeadingDeg { get; }

        public Course()
        {
            CourseEntryCL = new GeoCoordinate();
            CourseExitCL = new GeoCoordinate();
        }

        /// <summary>
        /// Generates Balls, BoatMarkers, Gates once Course Entry & Exit coordinates are available.
        /// </summary>
        public void GenerateCourseFeatures()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Calculates the heading straight through the course from Entry to Exit Center Line.
        /// </summary>
        /// <returns></returns>
        public double GetCourseHeadingDeg()
        {
            double dLongitude = Util.DegToRad(CourseExitCL.Longitude -
                CourseEntryCL.Longitude);

            double dPhi = Math.Log(
                      Math.Tan(Util.DegToRad(CourseExitCL.Latitude)/2+Math.PI/4) /
                        Math.Tan(Util.DegToRad(CourseEntryCL.Latitude)/2+Math.PI/4));

            if (Math.Abs(dLongitude) > Math.PI) 
                dLongitude = dLongitude > 0 ? -(2*Math.PI- dLongitude) : (2*Math.PI+ dLongitude);

            double heading = Util.RadToDeg(Math.Atan2(dLongitude, dPhi));

            return heading;
        }

        public void SetCourseEntry(double latitude, double longitude)
        {
            CourseEntryCL.Latitude = latitude;
            CourseEntryCL.Longitude = longitude;
        }

        public void SetCourseExit(double latitude, double longitude)
        {
            CourseExitCL.Latitude = latitude;
            CourseExitCL.Longitude = longitude;

            double length = CourseEntryCL.GetDistanceTo(CourseExitCL);

        }

        //
        // Maintain a virtual course map in x,y coordinates for all course elements (entry/exit gates, balls[1-6], boat markers)
        // 
        // Entry Gate Center Line Position (lat/long)
        // Exit Gate Center Line Position (lat/long)
        // EntryGates[2] (x,y coordinates) 
        // ExitGates[2] (x,y coordiantes)
        // Balls[5] (x,y coordinates)
        // BoatMarkers[10] (x,y coordiantes)
        // BoatGuides[4] -- the course entry / exit guides, what are these called? Green balls..

        public CoursePosition[] Balls { get; private set; }

        public CoursePosition[] BoatMarkers { get; private set; }

        public CoursePosition[] Gates { get; private set; }

        /// <summary>
        /// Given the boat's position, calculate in the matrix (x,y) relative to the course. 
        /// Where 0,0 represents Center Line at course entry.
        /// </summary>
        /// <param name="boatPosition"></param>
        /// <returns></returns>
        public CoursePosition CoursePositionFromGeo(double latitude, double longitude)
        {
            return CoursePositionFromGeo(new GeoCoordinate(latitude, longitude));
        }

        /// <summary>
        /// Given the boat's position, calculate in the matrix (x,y) relative to the course. 
        /// Where 0,0 represents Center Line at course entry.
        /// </summary>
        /// <param name="boatPosition"></param>
        /// <returns></returns>
        public CoursePosition CoursePositionFromGeo(GeoCoordinate boatPosition)
        {
            double distance = boatPosition.GetDistanceTo(CourseEntryCL);

            // TODO: Right now we're hardcoded to center of the course.
            return new CoursePosition(11.5, distance);
        }
    }
}
