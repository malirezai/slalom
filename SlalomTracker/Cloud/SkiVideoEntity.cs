﻿using System;
using System.IO;
using Microsoft.WindowsAzure.Storage.Table;

namespace SlalomTracker.Cloud
{
    public class SkiVideoEntity : TableEntity
    {
        public SkiVideoEntity()
        {
            
        }

        public SkiVideoEntity(string videoUrl, DateTime creationTime)
        {
            this.Url = videoUrl;
            this.RecordedTime = creationTime;
            this.PartitionKey = creationTime.ToString("yyyy-MM-dd");
            this.RowKey = GetFilenameFromUrl(videoUrl); 
            this.SlalomTrackerVersion = GetVersion();
        }


        public string Url { get; set; }
        
        /// <summary>
        /// Url for hot storage (currently Google storage).  Only most recent 
        /// videos stored here.
        /// </summary>
        public string HotUrl { get; set; }

        public string ThumbnailUrl { get; set; }
        
        public string JsonUrl { get; set; }
        
        public string Skier { get; set; }

        public double CenterLineDegreeOffset { get; set; }

        /// <summary>
        /// Rope Length in canonical "Off" format; 15,22,28,32, etc...
        /// </summary>
        public double RopeLengthM { get; set; }

        public double BoatSpeedMph { get; set; }

        public bool HasCrash { get; set; }

        public bool All6Balls { get; set; }

        public string CourseName { get; set; }

        public double EntryTime { get; set; }

        public string Notes { get; set; }

        public string SlalomTrackerVersion { get; set; }

        public DateTime RecordedTime { get; set; }

        public bool MarkedForDelete { get; set; } = false;

        public bool Starred { get; set; } = false;

        private string GetFilenameFromUrl(string videoUrl)
        {
            Uri uri = new Uri(videoUrl);
            string filename = System.IO.Path.GetFileName(uri.LocalPath);
            return filename;
        }

        private string GetVersion()
        {
            var extractor = typeof(MetadataExtractor.Extract).Assembly.GetName();
            var assembly = System.Reflection.Assembly.GetEntryAssembly().GetName();
            string version = $"{assembly.Name}:v{assembly.Version.ToString()}, " +
                $"{extractor.Name}:v{extractor.Version.ToString()}";

            return version;
        }
    }
}
