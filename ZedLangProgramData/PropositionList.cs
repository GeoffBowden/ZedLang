using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZedLangSyntacticElements;

namespace ZedLangProgramData
{
    public class PropositionList
    {
        List<PropositionExpression> Propositions;
        public PropositionList()
        {
            Propositions = new List<PropositionExpression>();
        }

        public Boolean AddProposition(PropositionExpression proposition)
        {
            if (proposition == null) throw new ArgumentNullException("PropositionList.AddProposition: cannot add nulls");
            Propositions.Add(proposition);
            return true;
        }

        public PropositionExpression GetPropositionAt(int i)
        {
            if (i >= 0 && i < Propositions.Count())
            {
                return Propositions[i];
            }
            return null;
        }
        public int PropositionCount()
        {
            return Propositions.Count();
        }

        public List<string> Output()
        {
            List<String> programListing = new List<string>();
            programListing.Add("Propositions:");
            foreach (PropositionExpression pse in Propositions)
            {
                programListing.Add("\t" + pse.Text );
            }
            return programListing;
        }
    }
}
