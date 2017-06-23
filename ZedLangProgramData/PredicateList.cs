using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZedLangSyntacticElements;

namespace ZedLangProgramData
{
    public class PredicateList
    {
        List<PredicateSyntacticElement> Predicates;
        public PredicateList()
        {
            Predicates = new List<PredicateSyntacticElement>();
        }

        public Boolean IsPredicate(string PredicateName)
        {
            foreach (PredicateSyntacticElement pse in Predicates)
            {
                if (string.Compare(PredicateName, pse.Name, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public Boolean AddPredicate(PredicateSyntacticElement predicate)
        {
            if (IsPredicate(predicate.Name))
            {
                return false;
            }
            else
            {
                Predicates.Add(predicate);
                return true;
            }
        }

        public PredicateSyntacticElement GetPredicate(string PredicateName)
        {
            foreach (PredicateSyntacticElement pse in Predicates)
            {
                if (string.Compare(PredicateName, pse.Name, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return pse;
                }
            }
            return null;
        }

        public PredicateSyntacticElement GetPredicateAt(int i)
        {
            if (i >= 0 && i < Predicates.Count())
            {
                return Predicates[i];
            }
            return null;
        }

        public int PredicateCount()
        {
            return Predicates.Count();
        }

        public List<string> Output()
        {
            List<String> programListing = new List<string>();
            programListing.Add("Propositions:");
            foreach (PredicateSyntacticElement pse in Predicates)
            {
                programListing.Add("\t" + pse.Name + ">" + pse.Predicate + "=" + pse.Value.ToString());
            }
            return programListing;
        }
    }
}

