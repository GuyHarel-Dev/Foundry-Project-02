namespace DemoAzAI.Application.AI
{
    public static class VectorMath
    {
        public static float CosineSimilarity(
            IReadOnlyList<float> a,
            IReadOnlyList<float> b)
        {
            if (a.Count != b.Count)
                throw new ArgumentException(
                    "Vectors must have same dimensions.");

            double dot = 0;
            double magA = 0;
            double magB = 0;

            for (int i = 0; i < a.Count; i++)
            {
                dot += a[i] * b[i];
                magA += a[i] * a[i];
                magB += b[i] * b[i];
            }

            return (float)(dot /
                (Math.Sqrt(magA) * Math.Sqrt(magB)));
        }
    }
}
