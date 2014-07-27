using System;

public static class ArrayExtensions
{

    public static T[] RemoveAt<T> (this T[] source, int index)
    {
        if (source == null)
            return null;
        
        T[] dest = new T[source.Length - 1];
        if (index > 0)
            Array.Copy (source, 0, dest, 0, index);
 
        if (index < source.Length - 1)
            Array.Copy (source, index + 1, dest, index, source.Length - index - 1);
 
        return dest;
    }
}
