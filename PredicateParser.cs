using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZedLangSyntacticElements;

namespace ZedLang
{
    public static class PredicateParser 
    {
        public static SyntacticElement Parse(List<string> lineElements)
        {
            // syntax is
            // <predicate> ::= predicate <name> > <predicate text> [= <value>]
            // <name>      ::= (<letter>|<number>|<symbol>)*
            // <symbol>    ::= _@&^*$£!
            // <value>     ::= true|false
            // <predicate text> may not contain a reserved word
            //string word = line.ContainsaReservedWord();
            InvalidSyntacticElement ise = null;
            ise = CheckForPredicateSymbol(lineElements);
            if (ise != null) { return ise; }
            ise = CheckForReservedWordsUsage(lineElements);
            if ( ise != null ) { return ise; }
            PredicateSyntacticElement pe = new PredicateSyntacticElement();
            pe.Name = lineElements[1];
            int equalitySymbolPosition = lineElements.IndexOf( "=" );
            if (equalitySymbolPosition < 0)
            {
                pe.Value = null;
                pe.Predicate = string.Join(" ", lineElements.Skip(3).ToArray());
            }
            else if (equalitySymbolPosition == lineElements.Count() - 2)// 0 1 2 3 4 5 cnt=6 
            {   // p p > t = v
                pe.Predicate = string.Join(" ", lineElements.Skip(3).
                                       Take(lineElements.Count()-((lineElements.Count()- equalitySymbolPosition) + 3)).ToArray());
                int valueElement = equalitySymbolPosition + 1;
                if ((lineElements.Count()-1) == valueElement)
                {
                    bool result;
                    if (bool.TryParse(lineElements.Last(), out result))
                    {
                        pe.Value = result;
                    }
                    else
                    {
                        int iResult = 0;
                        if (int.TryParse(lineElements.Last(), out iResult))
                        {
                            if (iResult == 0)
                            {
                                pe.Value = false;
                            }
                            else if (iResult == 1)
                            {
                                pe.Value = true;
                            }
                            else
                            {
                                ise = new InvalidSyntacticElement("Invalid value", "value must be [true|false], numbers or letters are non truth values and aliases for true or false cannot be used");
                                return ise;
                            }
                        }
                        else
                        {
                            ise = new InvalidSyntacticElement("Invalid value", "value must be [true|false], numbers or letters are non truth values and aliases for true or false cannot be used");
                            return ise;
                        }
                    }
                }
            }
            else
            {
                ise = new InvalidSyntacticElement("Equals sign not in the expected place", "the value must be [true|false], numbers or letters are non truth values and aliases for true or false cannot be used and the value must be the last thing on the line");
                return ise;
            }
            if ( string.IsNullOrWhiteSpace( pe.Predicate) )
            {
                ise = new InvalidSyntacticElement("No predicate text found", "a predicate with no words, you must be a buddha if you think that is sensical");
                return ise;
            }
            return pe;
        }

        private static InvalidSyntacticElement CheckForPredicateSymbol(List<string> lineElements)
        {
            InvalidSyntacticElement ise = null;
            int predicateElementIndex = lineElements.IndexOf(">");
            if ( predicateElementIndex == -1 )
            {
                ise = new InvalidSyntacticElement("predicate symbol not found", "predicate symbol not found");
            }
            if ( predicateElementIndex == 1 )
            {
                ise = new InvalidSyntacticElement("No predicate name", "the predicate symbol must be in position just after predicate 'name' ");
            }
            if (predicateElementIndex > 2)
            {
                ise = new InvalidSyntacticElement("A predicate name can only be one word", "the predicate symbol must be in position just after predicate 'name' ");
            }
            return ise;
        }

        private static InvalidSyntacticElement CheckForReservedWordsUsage(List<string> lineElements)
        {
            InvalidSyntacticElement ise = null;
            foreach ( string word in lineElements )
            {
                if (word.IsaReservedWord())
                {
                    ise = new InvalidSyntacticElement("Predicate may not contain reserved words", 
                                                       word + " Found in predicate " + string.Join( " ", lineElements.ToList() ) );
                    break;
                }
            }
            return ise;
        }
    }
}
 