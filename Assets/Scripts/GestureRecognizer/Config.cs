namespace GestureRecognizerDotNet
{
    public class Config
    {
        public static int MAX_GESTURES = 32;
        public static int SAMPLE_FREQUENCY_HZ = 100;
        public static int SAMPLE_WINDOW = ((SAMPLE_FREQUENCY_HZ / 25) * 2);
        public static int SAMPLE_STEP = (SAMPLE_WINDOW / 2);
        public static int MAX_SAMPLES = ((SAMPLE_FREQUENCY_HZ / 25) * SAMPLE_STEP);
        public static bool USE_DATA_QUANTIZER = false;
        public static string GESTURE_PATH = "gestures";
    }
}
