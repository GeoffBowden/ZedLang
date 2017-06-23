using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZedLangSyntacticElements;

namespace ZedLangProgramData
{
    public class LogicArgument
    {
        public PredicateList predicateList;
        public PropositionList propositionList;
        public PredicateSetterList predicateSetterList;
        public ThereforeSyntacticElement thereforeSyntacticElement;
        public bool? Conclusion;

        public LogicArgument()
        {
            predicateList = new PredicateList();
            propositionList = new PropositionList();
            predicateSetterList = new PredicateSetterList();
            thereforeSyntacticElement = null;
            Conclusion = null;
        }

        public void Clear()
        {
            predicateList = new PredicateList();
            propositionList = new PropositionList();
            predicateSetterList = new PredicateSetterList();
            thereforeSyntacticElement = null;
        }

        public bool Add(PredicateSyntacticElement pse)
        {
            return predicateList.AddPredicate(pse);
        }

        public bool Add(ThereforeSyntacticElement tse)
        {
            // only one allowed
            if (thereforeSyntacticElement == null)
            {
                thereforeSyntacticElement = tse;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Add(PredicateSetterSyntacticElement pse)
        {
            return predicateSetterList.AddPredicateSetter(pse);
        }

        public bool Add(PropositionExpression pse)
        {
            propositionList.AddProposition(pse);
            return true;
        }

        public bool Add( SyntacticElement se )
        {
            if (se is PropositionExpression) {return Add((PropositionExpression)se);}
            if (se is PredicateSyntacticElement) { return Add((PredicateSyntacticElement)se);}
            if (se is ThereforeSyntacticElement) { return Add((ThereforeSyntacticElement)se);}
            if (se is PredicateSetterSyntacticElement) { return Add((PredicateSetterSyntacticElement)se); }
            throw new NotImplementedException("Logic Argument: public bool Add( SyntacticElement se )");
        }

        public List<string> OutputAsStringList()
        {
            List<string> output = predicateList.Output().
                    Concat(propositionList.Output()).
                    Concat(predicateSetterList.Output()).ToList();
            if (thereforeSyntacticElement == null)
                output.Add("no conclusuion yet");
            else
                output.Add(thereforeSyntacticElement.Text);
            return output;
        }
    }
}