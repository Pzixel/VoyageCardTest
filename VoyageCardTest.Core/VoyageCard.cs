namespace VoyageCardTest.Core
{
    public struct VoyageCard
    {
        public string Origin { get; }
        public string Destination { get; }

        public VoyageCard(string origin, string destination)
        {
            Origin = origin;
            Destination = destination;
        }

        public override string ToString()
        {
            return $"{Origin} -> {Destination}";
        }
    }
}
