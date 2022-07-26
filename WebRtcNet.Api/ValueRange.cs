using System;

namespace WebRtcNet
{
    public class ValueRange<T> where T : struct
    {
        public T Max;
        public T Min;

        public ValueRange(T value)
        {
            Min = Max = value;
        }

        public ValueRange(T min, T max)
        {
            Min = min;
            Max = max;
        }

        public static implicit operator T(ValueRange<T> from)
        {
            return from.Max;
        }

        public static implicit operator ValueRange<T>(T from)
        {
            return new ValueRange<T>(from);
        }
    }
}