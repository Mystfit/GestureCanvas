using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GestureRecognizerDotNet
{
    public class DataQuantizer
    {
        public static int quantize(int value)
        {
            int result;
            if(value > 10)
            {
                if(value > 20)
                {
                    result = 16;
                } else
                {
                    result = 10 + (value - 10) / 10 * 5;
                }
            
            } else if (value< -10) {
                if (value< -20) {
                  result = -16;
                } else {
                  result = -10 + (value + 10) / 10 * 5;
                }
              } else {
                result = value;
              }

            return result;
        }
    }
}
