using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ChartBase.utils;

public static class Extensions
{
    /// <summary>
    /// AddRange method to HashSet
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="items"></param>
    /// <returns></returns>
    public static bool AddRange<T>(this HashSet<T> source, IEnumerable<T> items)
    {
        bool allAdded = true;
        foreach (T item in items)
        {
            allAdded &= source.Add(item);
        }
        return allAdded;
    }

    public static void AddRange<T>(this ObservableCollection<T> source, IEnumerable<T> items)
    {
        foreach (T item in items)
        {
            source.Add(item);
        }
    }

    public static void AddRange<T>(this ObservableCollection<T> source, T[] items)
    {
        foreach (T item in items)
        {
            source.Add(item);
        }
    }

    /// <summary>
    /// Helper method for splitting a list into a few of small sub lists
    /// </summary>
    public static List<List<T>> SplitBy<T>(this List<T> source, int chunkSize)
    {
        return source
            .Select((x, i) => new { Index = i, Value = x })
            .GroupBy(x => x.Index / chunkSize)
            .Select(x => x.Select(v => v.Value).ToList())
            .ToList();
    }

}

