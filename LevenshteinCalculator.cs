using System;
using System.Collections.Generic;
using System.Linq;

// Каждый документ — это список токенов. То есть List<string>.
// Вместо этого будем использовать псевдоним DocumentTokens.
// Это поможет избежать сложных конструкций:
// вместо List<List<string>> будет List<DocumentTokens>
using DocumentTokens = System.Collections.Generic.List<string>;

namespace Antiplagiarism
{
    public class LevenshteinCalculator
    {
        public List<ComparisonResult> CompareDocumentsPairwise(List<DocumentTokens> documents)
        {
            var result = new List<ComparisonResult>();
            for (int i = 0; i < documents.Count; i++)
                for (int j = i+1; j < documents.Count; j++)
                        result.Add(Levenshtein(documents[i], documents[j]));
            return result;
        }

        public static ComparisonResult Levenshtein(DocumentTokens doc1, DocumentTokens doc2)
        {
            var opt = new double[doc1.Count + 1, doc2.Count + 1];
            for (int i = 1; i <= doc1.Count; i++) opt[i, 0] = i;
            for (int i = 1; i <= doc2.Count; i++) opt[0, i] = i;

            for (var i = 1; i <= doc1.Count; ++i)
                for (var j = 1; j <= doc2.Count; ++j)
                    if (doc1[i - 1] == doc2[j - 1])
                        opt[i, j] = opt[i - 1, j - 1];
                    else
                    {
                        var d = TokenDistanceCalculator.GetTokenDistance(doc1[i - 1], doc2[j - 1]);
                        opt[i, j] = Math.Min(Math.Min(
                            1 + opt[i - 1, j], 
                            1 + opt[i, j - 1]),
                            d + opt[i - 1, j - 1]);
                    }

            return new ComparisonResult(
                    doc1,
                    doc2,
                    opt[doc1.Count, doc2.Count]);
        }
    }
}