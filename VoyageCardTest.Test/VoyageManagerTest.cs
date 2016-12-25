using System;
using System.Linq;
using NUnit.Framework;
using VoyageCardTest.Core;

namespace VoyageCardTest.Test
{
    [TestFixture]
    public class VoyageManagerTest
    {
        [Test]
        public void TestSimple()
        {
            VoyageCard[] voyages =
            {
                new VoyageCard("Мельбурн", "Кельн"),
                new VoyageCard("Москва", "Париж"),
                new VoyageCard("Кельн", "Москва"),
            };

            VoyageCard[] sortedVoyages =
            {
                new VoyageCard("Мельбурн", "Кельн"),
                new VoyageCard("Кельн", "Москва"),
                new VoyageCard("Москва", "Париж"),
            };

            var voyagePlan = VoyageManager.GetVoyagePlan(voyages);

            Assert.IsTrue(sortedVoyages.SequenceEqual(voyagePlan));
        }

        [Test]
        public void TestNull()
        {
            Assert.Throws<ArgumentNullException>(() => VoyageManager.GetVoyagePlan(null));
        }

        [Test]
        public void TestEmpty()
        {
            var voyages = new VoyageCard[0];
            var voyagePlan = VoyageManager.GetVoyagePlan(voyages);
            Assert.IsEmpty(voyagePlan);
        }

        [Test]
        public void TestSingle()
        {
            VoyageCard[] voyages = {new VoyageCard("A", "B")};
            var voyagePlan = VoyageManager.GetVoyagePlan(voyages);
            Assert.AreEqual(voyagePlan.Single(), voyages[0]);
        }

        [Test]
        public void TestLargeTrip()
        {
            var sortedVoyages = Enumerable.Range(1, 1000 * 1000).Select(x => new VoyageCard(x.ToString(), (x + 1).ToString())).ToArray();
            var voyages = (VoyageCard[]) sortedVoyages.Clone();
            Shuffle(voyages);

            var voyagePlan = VoyageManager.GetVoyagePlan(voyages);

            Assert.IsFalse(sortedVoyages.SequenceEqual(voyages));
            Assert.IsTrue(sortedVoyages.SequenceEqual(voyagePlan));
        }

        private static void Shuffle<T>(T[] a)
        {
            Random rand = new Random();
            for (int i = a.Length - 1; i > 0; i--)
            {
                int j = rand.Next(0, i + 1);
                T tmp = a[i];
                a[i] = a[j];
                a[j] = tmp;
            }
        }
    }
}
