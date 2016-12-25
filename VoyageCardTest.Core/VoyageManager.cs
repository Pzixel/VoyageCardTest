using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace VoyageCardTest.Core
{
    public static class VoyageManager
    {
        /// <summary>
        /// Collection of cards whichs describes origin and destination of one voyage
        /// </summary>
        /// <param name="voyageCards">Collection of cards, which should have exatly N+1 points without loops and spaces (guaranteed by task definition)</param>
        /// <returns>Sequence of cards which produces a continuous Hamiltonian path across all locations</returns>
        [NotNull]
        public static IEnumerable<VoyageCard> GetVoyagePlan([NotNull] IReadOnlyCollection<VoyageCard> voyageCards)
        {
            if (voyageCards == null)
            {
                throw new ArgumentNullException(nameof(voyageCards));
            }
            if (voyageCards.Count <= 1)
            {
                return voyageCards;
            }
            return GetVoyagePlanInternal(voyageCards); // we want to be sure that user will get exception when query is created, not when it is iterated first time
        }

        // Method complexity: O(N)
        private static IEnumerable<VoyageCard> GetVoyagePlanInternal(IReadOnlyCollection<VoyageCard> voyageCards)
        {
            int totalNodeCount = voyageCards.Count + 1;
            var resultPath = new Tuple<int, VoyageCard>[totalNodeCount];
            var indexDictionary = new Dictionary<string, int>(totalNodeCount);
            var locationsWithoutOrigin = new Dictionary<int, bool>(totalNodeCount);
            int i = 0;
            foreach (var currentCard in voyageCards) // O(N)
            {
                int originIndex = indexDictionary.GetLocationIndex(currentCard.Origin, ref i);
                int destIndex = indexDictionary.GetLocationIndex(currentCard.Destination, ref i);

                resultPath[originIndex] = new Tuple<int, VoyageCard>(destIndex, currentCard);

                locationsWithoutOrigin[destIndex] = false;
                if (!locationsWithoutOrigin.ContainsKey(originIndex))
                {
                    locationsWithoutOrigin[originIndex] = true;
                }
            }

            int currentOriginIndex = locationsWithoutOrigin.Where(x => x.Value).Single().Key; // should be exactly one which is our path start, O(N) lookup
            // I do not use Single(x => x.Value) because this overload is inefficient thus sequence of `Where` and `Single` is better

            while (true) // O(N)
            {
                var tuple = resultPath[currentOriginIndex];
                if (tuple == null)
                    break;
                yield return tuple.Item2;
                currentOriginIndex = tuple.Item1;
            }
        }

        // Method complexity: O(1)
        private static int GetLocationIndex(this Dictionary<string, int> indexDictionary, string key, ref int currentIndex)
        {
            int locationIndex;
            if (!indexDictionary.TryGetValue(key, out locationIndex))
            {
                locationIndex = currentIndex;
                indexDictionary.Add(key, currentIndex);
                currentIndex++;
            }
            return locationIndex;
        }
    }
}