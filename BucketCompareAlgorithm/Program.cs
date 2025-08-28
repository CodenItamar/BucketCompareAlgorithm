using System;
using System.Collections.Generic;
using System.Linq;

namespace BucketCompareAlgorithm;

/// <summary>
/// Implements a bucket comparison algorithm for comparing sorted date arrays
/// and builds all legal permutations.
/// </summary>
public class BucketCompareAlgorithm
{

    /// <summary>
    /// Processes a list of individually sorted buckets using the bucket comparison algorithm.
    /// </summary>
    public List<List<int>> ProcessBuckets(List<DateTime[]> buckets)
    {
        if (buckets == null || buckets.Count == 0)
            return new List<List<int>>();
        
        var bucketIndicesArray = InitializeIndicesArray(buckets);
        int outerBucketPointer = buckets.Count - 2;
        
        while (outerBucketPointer >= 0)
        {
            ProcessBucketPair(buckets, bucketIndicesArray, outerBucketPointer);
            outerBucketPointer--;
        }
        
        return bucketIndicesArray;
    }

    /// <summary>
    /// Processes a pair of adjacent buckets in the algorithm.
    /// </summary>
    private void ProcessBucketPair(List<DateTime[]> buckets, List<List<int>> bucketIndicesArray, int outerBucketPointer)
    {
        var bucket1 = buckets[outerBucketPointer];
        var bucket2 = buckets[outerBucketPointer + 1];
        
        int innerBucket1Pointer = bucket1.Length - 1;
        int innerBucket2Pointer = bucket2.Length - 1;
        
        while (innerBucket1Pointer >= 0)
        {
            // If bucket1 element >= bucket2 element, mark as -1
            while (innerBucket1Pointer >= 0 && bucket1[innerBucket1Pointer] >= bucket2[innerBucket2Pointer])
            {
                bucketIndicesArray[outerBucketPointer][innerBucket1Pointer] = -1;
                innerBucket1Pointer--;
            }
            
            if (innerBucket1Pointer < 0)
                break;

            // Move bucket2 pointer left until condition is satisfied
            while (innerBucket2Pointer > 0 && bucket2[innerBucket2Pointer] >= bucket1[innerBucket1Pointer])
            {
                innerBucket2Pointer--;
            }
            
            bucketIndicesArray[outerBucketPointer][innerBucket1Pointer] = innerBucket2Pointer;
            innerBucket1Pointer--;
        }
    }

    /// <summary>
    /// Initializes the indices array with the same dimensions as the input buckets, filled with zeros.
    /// </summary>
    private List<List<int>> InitializeIndicesArray(List<DateTime[]> buckets)
    {
        var indicesArray = new List<List<int>>();
        
        foreach (var bucket in buckets)
        {
            indicesArray.Add(Enumerable.Repeat(0, bucket.Length).ToList());
        }
        
        return indicesArray;
    }

    /// <summary>
    /// Backtracking: Builds all legal permutations using buckets + indicesArray.
    /// </summary>
    public List<List<DateTime>> BuildLegalPermutations(
        List<DateTime[]> buckets, 
        List<List<int>> indicesArray)
    {
        var results = new List<List<DateTime>>();

        // Try every starting element in the first bucket
        for (int startIndex = 0; startIndex < buckets[0].Length; startIndex++)
        {
            Backtrack(buckets, indicesArray, 0, startIndex, new List<DateTime>(), results);
        }

        return results;
    }

    private void Backtrack(
        List<DateTime[]> buckets,
        List<List<int>> indicesArray,
        int bucketIndex,
        int elementIndex,
        List<DateTime> current,
        List<List<DateTime>> results)
    {
        current.Add(buckets[bucketIndex][elementIndex]);

        // Base case: reached last bucket
        if (bucketIndex == buckets.Count - 1)
        {
            results.Add(new List<DateTime>(current));
        }
        else
        {
            int nextIndex = indicesArray[bucketIndex][elementIndex];
            if (nextIndex != -1)
            {
                // Always explore the direct nextIndex
                Backtrack(buckets, indicesArray, bucketIndex + 1, nextIndex, current, results);

                // Explore all higher indices in the next bucket
                for (int i = nextIndex + 1; i < buckets[bucketIndex + 1].Length; i++)
                {
                    Backtrack(buckets, indicesArray, bucketIndex + 1, i, current, results);
                }
            }
        }

        current.RemoveAt(current.Count - 1); // backtrack
    }

    /// <summary>
    /// Creates sample buckets for demonstration purposes.
    /// </summary>
    public List<DateTime[]> CreateSampleBuckets()
    {
        return new List<DateTime[]>
        {
            new DateTime[] { new DateTime(2023, 1, 1), new DateTime(2023, 1, 4), new DateTime(2023, 1, 5) },
            new DateTime[] { new DateTime(2023, 1, 2), new DateTime(2023, 1, 3) },
            new DateTime[] { new DateTime(2023, 1, 6), new DateTime(2023, 1, 7) }
        };
    }

    private void PrintBuckets(List<DateTime[]> buckets)
    {
        for (int i = 0; i < buckets.Count; i++)
        {
            Console.WriteLine($"Bucket {i}: [{string.Join(", ", buckets[i].Select(d => d.ToString("yyyy-MM-dd")))}]");
        }
    }

    private void PrintIndicesArray(List<List<int>> indicesArray)
    {
        for (int i = 0; i < indicesArray.Count; i++)
        {
            Console.WriteLine($"Bucket {i}: [{string.Join(", ", indicesArray[i])}]");
        }
    }

    private void PrintPermutations(List<List<DateTime>> permutations)
    {
        foreach (var perm in permutations)
        {
            Console.WriteLine($"[{string.Join(", ", perm.Select(d => d.ToString("yyyy-MM-dd")))}]");
        }
    }
}
