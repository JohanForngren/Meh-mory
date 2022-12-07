using UnityEngine;

public static class Helpers
{
    /// <summary>
    ///     Knuth shuffle from https://rosettacode.org/wiki/Knuth_shuffle#C#
    /// </summary>
    /// <param name="array"></param>
    /// <typeparam name="T"></typeparam>
    public static T[] KnuthShuffle<T>(T[] array)
    {
        for (var i = 0; i < array.Length; i++)
        {
            var j = Random.Range(i, array.Length); // Don't select from the entire array on subsequent loops
            (array[i], array[j]) = (array[j], array[i]);
        }

        return array;
    }
}