using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GestureRecognizerDotNet
{
    public class GestureRecognizer
    {
        public GestureRecognizer()
        {

        }

        public void load(string replayFilename)
        {
            _possibleGesture.load(replayFilename);
        }

        public void loadGestures()
        {
            string[] files = System.IO.Directory.GetFiles(Config.GESTURE_PATH, "*.ges");
            foreach(String path in files)
            {
                Gesture gesture = new Gesture();
                gesture.load(path);
                _gestures.Add(gesture);
            }
        }

        public void addAccelerationData(AccelSample accelerationData)
        {

        }

        public Gesture recognize()
        {
            //_possibleGesture.save();

            long shortestDistance = int.MaxValue;
            long longestDistance = 0;
            Gesture bestMatch = null;

            long totalDistance = 0;
            uint gesturesChecked = 0;

            foreach(Gesture gesture in _gestures)
            {
                if (!gesture.isValid)
                    continue;

                long distance = _possibleGesture.calculateDistanceBetween(gesture);

                if(distance < shortestDistance)
                {
                    shortestDistance = distance;
                    bestMatch = gesture;
                }

                if(distance > longestDistance)
                {
                    longestDistance = distance;
                }

                totalDistance += distance;
                gesturesChecked++;
            }

            long averageDistance = totalDistance / gesturesChecked;
            long variance = longestDistance - shortestDistance;

            if (shortestDistance > averageDistance / 2)
                return null;

            return bestMatch;
        }

        private List<Gesture> _gestures;
        private Gesture _possibleGesture;
    }
}
