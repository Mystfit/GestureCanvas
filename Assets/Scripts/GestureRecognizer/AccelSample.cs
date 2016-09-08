using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace GestureRecognizerDotNet
{
    [JsonObject(MemberSerialization.OptOut)]
    public class AccelSample
    {
        public AccelSample(){}
        public AccelSample(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public enum Axis
        {
            X, Y, Z
        }

        [JsonProperty]
        public int x, y, z;

        public void setAxisValue(Axis axis, int value)
        {
            switch (axis)
            {
                case Axis.X:
                    x = value;
                    break;
                case Axis.Y:
                    y = value;
                    break;
                case Axis.Z:
                    z = value;
                    break;
            }
        }

        public int getAxisValue(Axis axis)
        {
            switch (axis)
            {
                case Axis.X:
                    return x;
                case Axis.Y:
                    return y;
                case Axis.Z:
                    return z;
            }
            return 0;
        }

        public void scale(int divider)
        {
            x /= divider;
            y /= divider;
            z /= divider;
        }
    }
}
