using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Collections;

namespace GestureRecognizerDotNet
{

    public class Gesture : IEnumerable
    {
        private List<AccelSample> _samples;
        public List<AccelSample> samples { get { return _samples; } }
        public IEnumerator GetEnumerator() { return ((IEnumerable)_samples).GetEnumerator(); }
   
        private int _id;
        public int id { get { return _id; } set { _id = value; } }

        public Gesture()
        {
            id = 0;
            _samples = new List<AccelSample>();
        }

        public void add(AccelSample accelerationData)
        {
            if (_samples.Count == Config.MAX_SAMPLES)
            {
                _samples.RemoveAt(0);
            }

            _samples.Add(accelerationData);
        }


        public void load()
        {
            load(string.Format(Path.Combine(Config.GESTURE_PATH, "{0}.ges"), id));
        }

        public void load(string filename)
        {
            StreamReader reader = new StreamReader(Path.Combine(Config.GESTURE_PATH, filename));
        }

        public void save(string name)
        {
            string filename = Path.Combine(Config.GESTURE_PATH, string.Format("{0}.ges", name));
            string json = JsonConvert.SerializeObject(samples, Formatting.Indented);
        }

        public bool isValid { get { return _samples.Count > 0; } }

        public long calculateDistanceBetween(Gesture other)
        {
            if (!isValid && other.isValid)
                return int.MaxValue;

            AccelSample otherAccelerationData = other.samples[0];
            long[] table = buildTable(other.samples.Count);

            long distance = calculateDTWDistance(
                other.samples,
                samples.Count - 1,
                other.samples.Count - 1,
                table);

            distance /= (samples.Count + other.samples.Count);

            return distance;
        }

        private long[] buildTable(int itemsInOtherGestureToCompare)
        {
            int tableSize = samples.Count * itemsInOtherGestureToCompare;

            long[] table = new long[tableSize];

            for (int i = 0; i < tableSize; i++)
                table[i] = -1L;
            return table;
        }

        private long calculateDTWDistance(List<AccelSample> otherSamples, int compareIndex, int otherCompareIndex, long[] table)
        {
            if (compareIndex < 0 || otherCompareIndex < 0)
                return int.MaxValue;

            int tableWidth = otherSamples.Count;
            long localDistance = 0;

            for(uint axis = 0; axis < 3; axis++)
            {
                int a = samples[compareIndex].getAxisValue((AccelSample.Axis)axis);
                int b = otherSamples[compareIndex].getAxisValue((AccelSample.Axis)axis);
                localDistance += ((a - b) * (a - b));
            }

            long sdistance = 0;
            long s1, s2, s3;

            if(compareIndex == 0 && otherCompareIndex == 0)
            {
                if( table[compareIndex * tableWidth + otherCompareIndex] < 0)
                {
                    table[compareIndex * tableWidth + otherCompareIndex] = localDistance;
                }

                return localDistance;
            }

            if (compareIndex == 0)
            {
                if (table[compareIndex * tableWidth + (otherCompareIndex - 1)] < 0)
                    sdistance = calculateDTWDistance(otherSamples, compareIndex, otherCompareIndex - 1, table);
                else
                    sdistance = table[compareIndex * tableWidth + otherCompareIndex - 1];
            }
            else if (otherCompareIndex == 0)
            {
                if (table[(compareIndex - 1) * tableWidth + otherCompareIndex] < 0)
                    sdistance = calculateDTWDistance(otherSamples, compareIndex - 1, otherCompareIndex, table);
                else
                    sdistance = table[(compareIndex - 1) * tableWidth + otherCompareIndex];
            }
            else
            {

                if (table[compareIndex * tableWidth + (otherCompareIndex - 1)] < 0)
                    s1 = calculateDTWDistance(otherSamples, compareIndex, otherCompareIndex - 1, table);
                else
                    s1 = table[compareIndex * tableWidth + (otherCompareIndex - 1)];

                if (table[(compareIndex - 1) * tableWidth + otherCompareIndex] < 0)
                    s2 = calculateDTWDistance(otherSamples, compareIndex - 1, otherCompareIndex, table);
                else
                    s2 = table[(compareIndex - 1) * tableWidth + otherCompareIndex];

                if (table[(compareIndex - 1) * tableWidth + otherCompareIndex - 1] < 0)
                    s3 = calculateDTWDistance(otherSamples, compareIndex - 1, otherCompareIndex - 1, table);
                else
                    s3 = table[(compareIndex - 1) * tableWidth + otherCompareIndex - 1];

                sdistance = s1 < s2 ? s1 : s2;
                sdistance = sdistance < s3 ? sdistance : s3;
            }
            table[compareIndex * tableWidth + otherCompareIndex] = localDistance + sdistance;
            return table[compareIndex * tableWidth + otherCompareIndex];
        }
    }
}