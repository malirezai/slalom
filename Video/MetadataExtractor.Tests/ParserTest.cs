using System.IO;
using System.Collections.Generic;
using SlalomTracker;
using MetadataExtractor;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace MetadataExtractor.Tests
{
    [TestClass]
    public class ParserTest
    {
        [TestMethod]
        public void TestLoadFromFile()
        {
            const string csvPath = "../../../GOPR0194.csv";
            string csv = "";
            using (var sr = File.OpenText(csvPath))
                csv = sr.ReadToEnd();
            Parser parser = new Parser();
            List<Measurement> measurements = parser.LoadFromCsv(csv);
            AssertMeasurements(measurements);
            Storage storage = Program.ConnectToStorage();
            storage.AddMetadata("2018-08-21/GOPR0194.MP4", measurements);
        }

        [TestMethod]
        public void TestLoadFromMp4()
        {
            const string mp4Path = "GOPR0194.MP4";
            Parser parser = new Parser();
            List<Measurement> measurements = parser.LoadFromMp4(mp4Path);
            AssertMeasurements(measurements);
        }

        private void AssertMeasurements(List<Measurement> measurements)
        {
            Assert.AreEqual(measurements.Count, 1177);
            Assert.AreEqual(measurements[1000].BoatSpeedMps, 8.164);
        }
    }
}