using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZedLangSyntacticElements;

namespace ZedLangProgramData
{
    public class PredicateSetterList
    {
        List<PredicateSetterSyntacticElement> Propositions;
        public PredicateSetterList()
        {
            Propositions = new List<PredicateSetterSyntacticElement>();
        }

        public Boolean AddPredicateSetter(PredicateSetterSyntacticElement proposition)
        {
            Propositions.Add(proposition);
            return true;
        }

        public PredicateSetterSyntacticElement GetPredicateSetterAt(int i)
        {
            if (i >= 0 && i < Propositions.Count())
            {
                return Propositions[i];
            }
            return null;
        }

        public int PredicateSetterCount()
        {
            return Propositions.Count();
        }

        public List<string> Output()
        {
            List<String> programListing = new List<string>();
            programListing.Add("Predicate Value Setters:");
            foreach (PredicateSetterSyntacticElement pse in Propositions)
            {
                programListing.Add("\t" + pse.Text);
            }
            return programListing;
        }
    }
}
