/*
    Syntactic Element (abstract)
	Null Syntactic Element
	Invalid Syntactic Element
	Predicate Syntactic Element
	Expression Syntactic Element (abstract)
		Predicate Expression
		Unary Expression (abstract)
			Not Expression
			Therefore syntactic Element
		Binary Expression (abstract)
			And Expression
			Or Expression
			Xor Expression
			Propsition Expression (abstract)
				Implication Expression
				IfAndOnlyIf Expression
		proposition setter (abstract)
			Proposition Syntactic Element
			Proposition Negation Syntactic Element
	Comment Syntactic Element
    */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZedLangSyntacticElements
{
    public abstract class SyntacticElement
    {
        private ExecutableValue value; 
        public SyntacticElement () {  value = new ExecutableValue(); }
        public virtual bool? Value { get { return value.value; } set { this.value.value = value; } }
        public virtual State State { get { return value.State; } }
        public virtual bool IsKnown { get { return value.IsKnown; } }
        public abstract string Text { get; }
    }
    public class NullSyntacticElement : SyntacticElement
    {
        public NullSyntacticElement() : base() { }
        public override string Text { get { return ""; } }
    }
    public class InvalidSyntacticElement : SyntacticElement
    {
        public InvalidSyntacticElement( string title, string message ):base ()
        {
            Title = title;
            Message = message;
        }
        public readonly string Message;
        public readonly string Title;
        public override string Text { get { return Title + " : " + Message; } }
    }
    public class PredicateSyntacticElement : SyntacticElement
    {
        public string Predicate;
        public string Name;
        public override string Text { get { return Name + " : " + Predicate; } }
    }
        //father of predicate and <name>expressions
        public abstract class ExpressionSyntacticElement : SyntacticElement
    {
    }
    public class PredicateExpression : ExpressionSyntacticElement
    {
        public string PredicateName;
        public PredicateExpression(string PredicateName):base ()
        {
            this.PredicateName = PredicateName;
        }
        public override string Text { get { return PredicateName; } }
    }
    // expressions are recursive data structures ending in predecate expressions
    public abstract class UnaryExpression : ExpressionSyntacticElement
    {
        public ExpressionSyntacticElement expression;
    }
    public class NotExpression : UnaryExpression 
    {
        public override string Text { get { return "(not " + expression.Text + ")"; } }
    }

    public class ThereforeSyntacticElement : UnaryExpression
    {
        public override string Text { get { return "therefore " + expression.Text; } }
    }

    public abstract class BinaryExpression : ExpressionSyntacticElement
    {
        // a predicate expression is a recursive data structure
        // a predicate expression is a [predicate|predicate expression] operation [predicate|predicate expression]  
        public ExpressionSyntacticElement lhs;
        public ExpressionSyntacticElement rhs;
    }
    public class AndExpression : BinaryExpression
    {
        public override string Text { get { return "(" + lhs.Text + " and " + rhs.Text + ")"; } }
    } // and
    public class OrExpression : BinaryExpression
    {
        public override string Text { get { return "(" + lhs.Text + " or " + rhs.Text + ")"; } }
    } // or
    public class XorExpression : BinaryExpression
    {
        public override string Text { get { return "(" + lhs.Text + " xor " + rhs.Text + ")"; } }
    } // xor
    public abstract class PredicateSetterSyntacticElement : ExpressionSyntacticElement {}
    public class PropositionSyntacticElement : PredicateSetterSyntacticElement
    {
        public readonly string PredicateName;
        public PropositionSyntacticElement( string predicateName )
        {
            PredicateName = predicateName;
            Value = true; // implied value for the predicate? 
        }
        public override string Text { get { return PredicateName; } }
    }
    public class PropositionNegationSyntacticElement : PredicateSetterSyntacticElement
    {
        public readonly string PredicateName;
        public PropositionNegationSyntacticElement(string predicateName)
        {
            PredicateName = predicateName;
            Value = false; // implied value for the predicate? 
        }

        public override string Text { get { return "not ("+PredicateName+")"; } }
    }

    public abstract class PropositionExpression : BinaryExpression
    {
        // syntax is x implies y where x and y are valid predicates
        // similiar are all logical connectives
        // so x could be x and z such that x and z imply y
        // same is ifandonlyif which only differs in the truth table
    }
    public class ImplicationSyntacticExpression : PropositionExpression
    {
        public override string Text { get { return "If " + lhs.Text + " Then " + rhs.Text ; } }
    }
    public class IfAndOnlyIfSyntacticExpression : PropositionExpression
    {
        public override string Text { get { return "If and only if " + lhs.Text + " Then " + rhs.Text ; } }
    }

    public class CommentSyntacticElement : SyntacticElement
    {   // always on a new line, syntax: note <message>
        public CommentSyntacticElement(string text):base ()
        {
            CommentText = text;
        }
        public readonly string CommentText;
        public override string Text { get { return CommentText; } }
    }
}
