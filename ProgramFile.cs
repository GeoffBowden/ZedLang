using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZedLangProgramData;
using ZedLangSyntacticElements;
using ZedLang;
using ZedlangExecutor;
using System.IO;

namespace ZedLang
{  
    public class ProgramFile
    {
        public List<string> Listing;
        LogicArgument programData;

        public ProgramFile()
        {
            Listing = new List<string>();
            programData=new LogicArgument();
        }

        public ProgramFile Clear()
        {
            Listing.Clear();
            programData.Clear();
            return this;
        }

        // assumes a valid syntactic element
        public ProgramFile Add(string line)
        {
            SyntacticElement se = LineParser.Parse(line);
            if ( se is InvalidSyntacticElement ) { throw new ArgumentException("Unable to add invalid syntactic element! "+se.Text); }
            if ( se is PredicateSyntacticElement )
            {
                PredicateSyntacticElement p = (PredicateSyntacticElement)se;
                if (!programData.Add(p))
                {
                    throw new ArgumentException("Predicate {0} already exists", p.Name);
                }
            }
            if (se is PropositionExpression)
            {
                PropositionExpression p = (PropositionExpression)se;
                programData.Add(p);
            }
            if (se is PredicateSetterSyntacticElement )
            {
                programData.Add((PredicateSetterSyntacticElement)se);
            }
            if (se is ThereforeSyntacticElement)
            {
                ThereforeSyntacticElement t = (ThereforeSyntacticElement)se;
                programData.Add(t);
            }
            if (!(se is NullSyntacticElement) ) { Listing.Add(line); }
            return this;
        }

        public List<string> OutputAsStringList()
        {
            List<string> output = new List<string>();
            output.Add("___________________________________________________________");
            int lineNumber = 1;
            foreach( string line in Listing)
            {
                output.Add(lineNumber++.ToString("d5") + " " + line);
            }
            output.Add("___________________________________________________________");
            output.Add(" ");
            return output.Concat( programData.OutputAsStringList()).ToList();
        }

        public List<string> run(out ProgramReturnCode code)
        {
            return ZedLangProgramExecutor.Execute(programData, out code);
        }
    } // program file
}
