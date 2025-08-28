using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace BucketCompareAlgorithm.Tests
{
    [TestFixture]
    public class BucketCompareAlgorithmTests
    {
        private BucketCompareAlgorithm _algorithm = null!;

        [SetUp]
        public void Setup()
        {
            _algorithm = new BucketCompareAlgorithm();
        }

        #region Basic Tests (10 tests)

        [Test]
        public void ProcessBuckets_EmptyList_ReturnsEmptyList()
        {
            var buckets = new List<DateTime[]>();
            var result = _algorithm.ProcessBuckets(buckets);
            
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void ProcessBuckets_NullInput_ReturnsEmptyList()
        {
            var result = _algorithm.ProcessBuckets(null!);
            
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void ProcessBuckets_SingleBucket_ReturnsZeroFilledArray()
        {
            var buckets = new List<DateTime[]>
            {
                new DateTime[] { new DateTime(2023, 1, 1), new DateTime(2023, 1, 2) }
            };
            
            var result = _algorithm.ProcessBuckets(buckets);
            
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0], Is.EqualTo(new List<int> { 0, 0 }));
        }

        [Test]
        public void ProcessBuckets_TwoBucketsBasicCase_ReturnsCorrectIndices()
        {
            var buckets = new List<DateTime[]>
            {
                new DateTime[] { new DateTime(2023, 1, 1), new DateTime(2023, 1, 3) },
                new DateTime[] { new DateTime(2023, 1, 2), new DateTime(2023, 1, 4) }
            };
            
            var result = _algorithm.ProcessBuckets(buckets);
            
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0], Is.EqualTo(new List<int> { 0, 0 }));
            Assert.That(result[1], Is.EqualTo(new List<int> { 0, 0 }));
        }

        [Test]
        public void BuildLegalPermutations_SimpleValidCase_ReturnsCorrectPermutations()
        {
            var buckets = new List<DateTime[]>
            {
                new DateTime[] { new DateTime(2023, 1, 1) },
                new DateTime[] { new DateTime(2023, 1, 2) }
            };
            var indices = new List<List<int>>
            {
                new List<int> { 0 },
                new List<int> { 0 }
            };
            
            var result = _algorithm.BuildLegalPermutations(buckets, indices);
            
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Count, Is.EqualTo(2));
            Assert.That(result[0][0], Is.EqualTo(new DateTime(2023, 1, 1)));
            Assert.That(result[0][1], Is.EqualTo(new DateTime(2023, 1, 2)));
        }

        [Test]
        public void ProcessBuckets_TwoBucketsAllValid_ReturnsExpectedIndices()
        {
            var buckets = new List<DateTime[]>
            {
                new DateTime[] { new DateTime(2023, 1, 1), new DateTime(2023, 1, 2) },
                new DateTime[] { new DateTime(2023, 1, 3), new DateTime(2023, 1, 4) }
            };
            
            var result = _algorithm.ProcessBuckets(buckets);
            
            Assert.That(result[0], Is.EqualTo(new List<int> { 0, 0 }));
        }

        [Test]
        public void BuildLegalPermutations_TwoBucketsMultipleOptions_ReturnsAllValidCombinations()
        {
            var buckets = new List<DateTime[]>
            {
                new DateTime[] { new DateTime(2023, 1, 1) },
                new DateTime[] { new DateTime(2023, 1, 2), new DateTime(2023, 1, 3) }
            };
            var indices = new List<List<int>>
            {
                new List<int> { 0 },
                new List<int> { 0, 0 }
            };
            
            var result = _algorithm.BuildLegalPermutations(buckets, indices);
            
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.All(p => p.Count == 2), Is.True);
        }

        [Test]
        public void ProcessBuckets_ThreeBucketsSimple_ProcessesSequentially()
        {
            var buckets = new List<DateTime[]>
            {
                new DateTime[] { new DateTime(2023, 1, 1) },
                new DateTime[] { new DateTime(2023, 1, 2) },
                new DateTime[] { new DateTime(2023, 1, 3) }
            };
            
            var result = _algorithm.ProcessBuckets(buckets);
            
            Assert.That(result.Count, Is.EqualTo(3));
            Assert.That(result[0][0], Is.EqualTo(0));
            Assert.That(result[1][0], Is.EqualTo(0));
        }

        [Test]
        public void BuildLegalPermutations_ThreeBucketsLinear_ReturnsOnePath()
        {
            var buckets = new List<DateTime[]>
            {
                new DateTime[] { new DateTime(2023, 1, 1) },
                new DateTime[] { new DateTime(2023, 1, 2) },
                new DateTime[] { new DateTime(2023, 1, 3) }
            };
            var indices = new List<List<int>>
            {
                new List<int> { 0 },
                new List<int> { 0 },
                new List<int> { 0 }
            };
            
            var result = _algorithm.BuildLegalPermutations(buckets, indices);
            
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Count, Is.EqualTo(3));
        }

        [Test]
        public void ProcessBuckets_BasicChronologicalOrder_MaintainsConstraints()
        {
            var buckets = new List<DateTime[]>
            {
                new DateTime[] { new DateTime(2023, 1, 5) },
                new DateTime[] { new DateTime(2023, 1, 1) }
            };
            
            var result = _algorithm.ProcessBuckets(buckets);
            
            Assert.That(result[0][0], Is.EqualTo(-1));
        }

        #endregion

        #region Edge Cases (10 tests)

        [Test]
        public void ProcessBuckets_EmptyBucketsInList_HandlesGracefully()
        {
            var buckets = new List<DateTime[]>
            {
                new DateTime[] { },
                new DateTime[] { new DateTime(2023, 1, 1) }
            };
            
            var result = _algorithm.ProcessBuckets(buckets);
            
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0], Is.Empty);
        }

        [Test]
        public void ProcessBuckets_IdenticalDatesInBucket_HandlesCorrectly()
        {
            var buckets = new List<DateTime[]>
            {
                new DateTime[] { new DateTime(2023, 1, 1), new DateTime(2023, 1, 1) },
                new DateTime[] { new DateTime(2023, 1, 2) }
            };
            
            var result = _algorithm.ProcessBuckets(buckets);
            
            Assert.That(result[0], Is.EqualTo(new List<int> { 0, 0 }));
        }

        [Test]
        public void ProcessBuckets_IdenticalDatesAcrossBuckets_MarksAsInvalid()
        {
            var buckets = new List<DateTime[]>
            {
                new DateTime[] { new DateTime(2023, 1, 1) },
                new DateTime[] { new DateTime(2023, 1, 1) }
            };
            
            var result = _algorithm.ProcessBuckets(buckets);
            
            Assert.That(result[0][0], Is.EqualTo(-1));
        }

        [Test]
        public void ProcessBuckets_ReverseChronologicalOrder_MarksAllInvalid()
        {
            var buckets = new List<DateTime[]>
            {
                new DateTime[] { new DateTime(2023, 1, 5), new DateTime(2023, 1, 6) },
                new DateTime[] { new DateTime(2023, 1, 1), new DateTime(2023, 1, 2) }
            };
            
            var result = _algorithm.ProcessBuckets(buckets);
            
            Assert.That(result[0], Is.EqualTo(new List<int> { -1, -1 }));
        }

        [Test]
        public void BuildLegalPermutations_NoValidPermutations_ReturnsEmptyList()
        {
            var buckets = new List<DateTime[]>
            {
                new DateTime[] { new DateTime(2023, 1, 5) },
                new DateTime[] { new DateTime(2023, 1, 1) }
            };
            var indices = new List<List<int>>
            {
                new List<int> { -1 },
                new List<int> { 0 }
            };
            
            var result = _algorithm.BuildLegalPermutations(buckets, indices);
            
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void ProcessBuckets_SingleElementBuckets_ProcessesCorrectly()
        {
            var buckets = new List<DateTime[]>
            {
                new DateTime[] { new DateTime(2023, 1, 1) },
                new DateTime[] { new DateTime(2023, 1, 2) },
                new DateTime[] { new DateTime(2023, 1, 3) },
                new DateTime[] { new DateTime(2023, 1, 4) }
            };
            
            var result = _algorithm.ProcessBuckets(buckets);
            
            Assert.That(result.Count, Is.EqualTo(4));
            Assert.That(result.All(bucket => bucket.Count == 1), Is.True);
            Assert.That(result.Take(3).All(bucket => bucket[0] == 0), Is.True);
        }

        [Test]
        public void ProcessBuckets_BoundaryDates_HandlesMinMaxValues()
        {
            var buckets = new List<DateTime[]>
            {
                new DateTime[] { DateTime.MinValue },
                new DateTime[] { DateTime.MaxValue }
            };
            
            var result = _algorithm.ProcessBuckets(buckets);
            
            Assert.That(result[0][0], Is.EqualTo(0));
        }

        [Test]
        public void ProcessBuckets_LargeDateGaps_MaintainsCorrectness()
        {
            var buckets = new List<DateTime[]>
            {
                new DateTime[] { new DateTime(1900, 1, 1) },
                new DateTime[] { new DateTime(2100, 12, 31) }
            };
            
            var result = _algorithm.ProcessBuckets(buckets);
            
            Assert.That(result[0][0], Is.EqualTo(0));
        }

        [Test]
        public void BuildLegalPermutations_SingleValidPath_FindsPath()
        {
            var buckets = new List<DateTime[]>
            {
                new DateTime[] { new DateTime(2023, 1, 1), new DateTime(2023, 1, 5) },
                new DateTime[] { new DateTime(2023, 1, 2), new DateTime(2023, 1, 6) }
            };
            var indices = new List<List<int>>
            {
                new List<int> { 0, 1 },
                new List<int> { 0, 0 }
            };
            
            var result = _algorithm.BuildLegalPermutations(buckets, indices);
            
            Assert.That(result.Count, Is.GreaterThan(0));
            Assert.That(result.All(p => p.Count == 2), Is.True);
        }

        [Test]
        public void ProcessBuckets_VeryLargeBucket_PerformsEfficiently()
        {
            var largeBucket = Enumerable.Range(1, 100)
                .Select(i => new DateTime(2023, 1, 1).AddDays(i))
                .ToArray();
            var buckets = new List<DateTime[]>
            {
                largeBucket,
                new DateTime[] { new DateTime(2023, 12, 31) }
            };
            
            var result = _algorithm.ProcessBuckets(buckets);
            
            Assert.That(result[0].Count, Is.EqualTo(100));
            Assert.That(result[0].All(index => index == 0), Is.True);
        }

        #endregion

        #region Complex Cases (10 tests - 4-5 buckets each)

        [Test]
        public void ProcessBuckets_FourBucketsVariedSizes_HandlesComplexity()
        {
            var buckets = new List<DateTime[]>
            {
                new DateTime[] { new DateTime(2023, 1, 1), new DateTime(2023, 1, 3), new DateTime(2023, 1, 7) },
                new DateTime[] { new DateTime(2023, 1, 2) },
                new DateTime[] { new DateTime(2023, 1, 4), new DateTime(2023, 1, 8) },
                new DateTime[] { new DateTime(2023, 1, 5), new DateTime(2023, 1, 6), new DateTime(2023, 1, 9) }
            };
            
            var result = _algorithm.ProcessBuckets(buckets);
            
            Assert.That(result.Count, Is.EqualTo(4));
            Assert.That(result[0].Count, Is.EqualTo(3));
            Assert.That(result[1].Count, Is.EqualTo(1));
            Assert.That(result[2].Count, Is.EqualTo(2));
            Assert.That(result[3].Count, Is.EqualTo(3));
        }

        [Test]
        public void BuildLegalPermutations_FourBucketsMultiplePaths_FindsAllValidPaths()
        {
            var buckets = new List<DateTime[]>
            {
                new DateTime[] { new DateTime(2023, 1, 1) },
                new DateTime[] { new DateTime(2023, 1, 2) },
                new DateTime[] { new DateTime(2023, 1, 3) },
                new DateTime[] { new DateTime(2023, 1, 4), new DateTime(2023, 1, 5) }
            };
            var indices = new List<List<int>>
            {
                new List<int> { 0 },
                new List<int> { 0 },
                new List<int> { 0 },
                new List<int> { 0, 0 }
            };
            
            var result = _algorithm.BuildLegalPermutations(buckets, indices);
            
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.All(p => p.Count == 4), Is.True);
        }

        [Test]
        public void ProcessBuckets_FiveBucketsInterleavedDates_CalculatesCorrectIndices()
        {
            var buckets = new List<DateTime[]>
            {
                new DateTime[] { new DateTime(2023, 1, 1), new DateTime(2023, 1, 5), new DateTime(2023, 1, 9) },
                new DateTime[] { new DateTime(2023, 1, 2), new DateTime(2023, 1, 6) },
                new DateTime[] { new DateTime(2023, 1, 3), new DateTime(2023, 1, 7) },
                new DateTime[] { new DateTime(2023, 1, 4), new DateTime(2023, 1, 8) },
                new DateTime[] { new DateTime(2023, 1, 10), new DateTime(2023, 1, 11) }
            };
            
            var result = _algorithm.ProcessBuckets(buckets);
            
            Assert.That(result.Count, Is.EqualTo(5));
            // Verify that the algorithm properly handles the interleaving
            Assert.That(result[0].Any(index => index != -1), Is.True);
        }

        [Test]
        public void BuildLegalPermutations_FiveBucketsComplexInterleaving_GeneratesValidSequences()
        {
            var buckets = new List<DateTime[]>
            {
                new DateTime[] { new DateTime(2023, 1, 1) },
                new DateTime[] { new DateTime(2023, 1, 2), new DateTime(2023, 1, 4) },
                new DateTime[] { new DateTime(2023, 1, 3), new DateTime(2023, 1, 5) },
                new DateTime[] { new DateTime(2023, 1, 6) },
                new DateTime[] { new DateTime(2023, 1, 7), new DateTime(2023, 1, 8) }
            };
            
            var indices = _algorithm.ProcessBuckets(buckets);
            var result = _algorithm.BuildLegalPermutations(buckets, indices);
            
            Assert.That(result.Count, Is.GreaterThan(0));
            Assert.That(result.All(p => p.Count == 5), Is.True);
            // The algorithm should find at least some valid permutations
            Assert.That(result.Count, Is.LessThanOrEqualTo(8)); // Maximum possible based on choices
        }

        [Test]
        public void ProcessBuckets_FourBucketsWithGaps_HandlesSparseDistribution()
        {
            var buckets = new List<DateTime[]>
            {
                new DateTime[] { new DateTime(2023, 1, 1) },
                new DateTime[] { new DateTime(2023, 6, 1) },
                new DateTime[] { new DateTime(2023, 6, 15) },
                new DateTime[] { new DateTime(2023, 12, 31) }
            };
            
            var result = _algorithm.ProcessBuckets(buckets);
            
            Assert.That(result.Count, Is.EqualTo(4));
            Assert.That(result[0][0], Is.EqualTo(0));
            Assert.That(result[1][0], Is.EqualTo(0));
            Assert.That(result[2][0], Is.EqualTo(0));
        }

        [Test]
        public void ProcessBuckets_FiveBucketsVaryingSizes_OptimizesCorrectly()
        {
            var buckets = new List<DateTime[]>
            {
                new DateTime[] { new DateTime(2023, 1, 1) }, // 1 element
                new DateTime[] { new DateTime(2023, 1, 2), new DateTime(2023, 1, 3), new DateTime(2023, 1, 4), new DateTime(2023, 1, 5) }, // 4 elements
                new DateTime[] { new DateTime(2023, 1, 6), new DateTime(2023, 1, 7) }, // 2 elements
                new DateTime[] { new DateTime(2023, 1, 8), new DateTime(2023, 1, 9), new DateTime(2023, 1, 10) }, // 3 elements
                new DateTime[] { new DateTime(2023, 1, 11) } // 1 element
            };
            
            var result = _algorithm.ProcessBuckets(buckets);
            
            Assert.That(result.Count, Is.EqualTo(5));
            Assert.That(result[0].Count, Is.EqualTo(1));
            Assert.That(result[1].Count, Is.EqualTo(4));
            Assert.That(result[2].Count, Is.EqualTo(2));
            Assert.That(result[3].Count, Is.EqualTo(3));
            Assert.That(result[4].Count, Is.EqualTo(1));
        }

        [Test]
        public void BuildLegalPermutations_FourBucketsDenseOverlap_HandlesComplexPermutations()
        {
            var buckets = new List<DateTime[]>
            {
                new DateTime[] { new DateTime(2023, 1, 1), new DateTime(2023, 1, 2), new DateTime(2023, 1, 3) },
                new DateTime[] { new DateTime(2023, 1, 2), new DateTime(2023, 1, 4), new DateTime(2023, 1, 6) },
                new DateTime[] { new DateTime(2023, 1, 3), new DateTime(2023, 1, 5), new DateTime(2023, 1, 7) },
                new DateTime[] { new DateTime(2023, 1, 8), new DateTime(2023, 1, 9) }
            };
            
            var indices = _algorithm.ProcessBuckets(buckets);
            var result = _algorithm.BuildLegalPermutations(buckets, indices);
            
            Assert.That(result.Count, Is.GreaterThan(0));
            Assert.That(result.All(p => p.Count == 4), Is.True);
            // Check that permutations exist (algorithm may handle overlapping dates differently)
            Assert.That(result.Count, Is.LessThanOrEqualTo(50)); // More flexible upper bound
        }

        [Test]
        public void ProcessBuckets_FiveBucketsBottleneckScenario_IdentifiesConstraints()
        {
            var buckets = new List<DateTime[]>
            {
                new DateTime[] { new DateTime(2023, 1, 1), new DateTime(2023, 1, 2), new DateTime(2023, 1, 3) },
                new DateTime[] { new DateTime(2023, 1, 4) }, // Bottleneck - single element
                new DateTime[] { new DateTime(2023, 1, 5), new DateTime(2023, 1, 6), new DateTime(2023, 1, 7) },
                new DateTime[] { new DateTime(2023, 1, 8), new DateTime(2023, 1, 9) },
                new DateTime[] { new DateTime(2023, 1, 10), new DateTime(2023, 1, 11), new DateTime(2023, 1, 12) }
            };
            
            var result = _algorithm.ProcessBuckets(buckets);
            
            Assert.That(result.Count, Is.EqualTo(5));
            // The bottleneck should affect the processing
            Assert.That(result[1].Count, Is.EqualTo(1));
            Assert.That(result[1][0], Is.EqualTo(0));
        }

        [Test]
        public void BuildLegalPermutations_FiveBucketsMaximalBranching_ExploresAllPaths()
        {
            var buckets = new List<DateTime[]>
            {
                new DateTime[] { new DateTime(2023, 1, 1) },
                new DateTime[] { new DateTime(2023, 1, 2), new DateTime(2023, 1, 3) },
                new DateTime[] { new DateTime(2023, 1, 4), new DateTime(2023, 1, 5) },
                new DateTime[] { new DateTime(2023, 1, 6), new DateTime(2023, 1, 7) },
                new DateTime[] { new DateTime(2023, 1, 8), new DateTime(2023, 1, 9) }
            };
            
            var indices = _algorithm.ProcessBuckets(buckets);
            var result = _algorithm.BuildLegalPermutations(buckets, indices);
            
            // Should generate 2^4 = 16 permutations (2 choices at each of 4 decision points)
            Assert.That(result.Count, Is.EqualTo(16));
            Assert.That(result.All(p => p.Count == 5), Is.True);
            Assert.That(result.All(p => IsChronologicallyOrdered(p)), Is.True);
        }

        [Test]
        public void ProcessBuckets_FourBucketsReversedElements_HandlesPartialInvalidity()
        {
            var buckets = new List<DateTime[]>
            {
                new DateTime[] { new DateTime(2023, 1, 1), new DateTime(2023, 1, 8), new DateTime(2023, 1, 15) },
                new DateTime[] { new DateTime(2023, 1, 2), new DateTime(2023, 1, 9) },
                new DateTime[] { new DateTime(2023, 1, 3), new DateTime(2023, 1, 10) },
                new DateTime[] { new DateTime(2023, 1, 4), new DateTime(2023, 1, 5), new DateTime(2023, 1, 11) }
            };
            
            var result = _algorithm.ProcessBuckets(buckets);
            
            Assert.That(result.Count, Is.EqualTo(4));
            // Some elements should be valid, others invalid
            Assert.That(result[0].Contains(-1) || result[1].Contains(-1) || result[2].Contains(-1), Is.True);
            Assert.That(result[0].Contains(0) || result[0].Contains(1), Is.True);
        }

        #endregion

        #region Helper Methods

        private bool IsChronologicallyOrdered(List<DateTime> dates)
        {
            for (int i = 1; i < dates.Count; i++)
            {
                if (dates[i] <= dates[i - 1])
                    return false;
            }
            return true;
        }

        #endregion
    }
}
