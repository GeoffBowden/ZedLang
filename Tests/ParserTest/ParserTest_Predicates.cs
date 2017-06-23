using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZedLang;
using ZedLangSyntacticElements;

namespace ParserTest
{
    [TestClass]
    public class LineParserTest
    {
        [TestMethod]
        public void LineParser_BlankNullOrQhiteSpace_ReturnsNullSyntacticElement()
        {
            var parseResult = LineParser.Parse("");
            Assert.AreEqual(parseResult.GetType(), typeof(NullSyntacticElement));
            parseResult = LineParser.Parse(null);
            Assert.AreEqual(parseResult.GetType(), typeof(NullSyntacticElement));
            parseResult = LineParser.Parse("   ");
            Assert.AreEqual(parseResult.GetType(), typeof(NullSyntacticElement));
            parseResult = LineParser.Parse("\t");
            Assert.AreEqual(parseResult.GetType(), typeof(NullSyntacticElement));
        }
        [TestMethod]
        public void LineParser_ValidPredicateDefinition_NoValue_ReturnsPredicate_Unknown()
        {
            string line;
            line = "\tpredicate p > raining in chicago";
            SyntacticElement parseResult;
            parseResult = LineParser.Parse(line);
            Assert.AreEqual(parseResult.GetType(), typeof(PredicateSyntacticElement));
            Assert.AreEqual(parseResult.State, State.Unknown);
            PredicateSyntacticElement pe = (PredicateSyntacticElement)parseResult;
            Assert.AreEqual("p", pe.Name);
            Assert.AreEqual("raining in chicago", pe.Predicate);
        }
        [TestMethod]
        public void LineParser_ValidPredicateDefinition_TrueValue_ReturnsPredicate_KnownTrue()
        {
            string line;
            line = "\tpredicate p > raining in chicago = true";
            SyntacticElement parseResult;
            parseResult = LineParser.Parse(line);
            Assert.AreEqual(parseResult.GetType(), typeof(PredicateSyntacticElement));
            Assert.AreEqual(parseResult.State, State.Known);
            Assert.AreEqual(parseResult.Value, true);
            PredicateSyntacticElement pe = (PredicateSyntacticElement)parseResult;
            Assert.AreEqual(pe.Name, "p");
            Assert.AreEqual("raining in chicago", pe.Predicate);
        }
        [TestMethod]
        public void LineParser_ValidPredicateDefinition_FalseValue_ReturnsPredicate_KnownFalse()
        {
            string line;
            line = "\tpredicate p > raining in chicago = false";
            SyntacticElement parseResult;
            parseResult = LineParser.Parse(line);
            Assert.AreEqual(parseResult.GetType(), typeof(PredicateSyntacticElement));
            Assert.AreEqual(parseResult.State, State.Known);
            Assert.AreEqual(parseResult.Value, false);
            PredicateSyntacticElement pe = (PredicateSyntacticElement)parseResult;
            Assert.AreEqual(pe.Name, "p");
            Assert.AreEqual("raining in chicago",pe.Predicate);
        }
        [TestMethod]
        public void LineParser_InValidPredicateDefinition_NameHasAspace_ReturnsInvalidSyntacticElement()
        {
            string line;
            line = "\tpredicate p p > raining in chicago = false";
            SyntacticElement parseResult;
            parseResult = LineParser.Parse(line);
            Assert.AreEqual(parseResult.GetType(), typeof(InvalidSyntacticElement));
            InvalidSyntacticElement ise = (InvalidSyntacticElement)parseResult;
            Assert.AreEqual(ise.Title, "A predicate name can only be one word");
        }
        // no predicate name
        [TestMethod]
        public void LineParser_InValidPredicateDefinition_NoName_ReturnsInvalidSyntacticElement()
        {
            string line;
            line = "\tpredicate > raining in chicago = false";
            SyntacticElement parseResult;
            parseResult = LineParser.Parse(line);
            Assert.AreEqual(parseResult.GetType(), typeof(InvalidSyntacticElement));
            InvalidSyntacticElement ise = (InvalidSyntacticElement)parseResult;
            Assert.AreEqual(ise.Title, "No predicate name");
        }
        // no predicate text
        [TestMethod]
        public void LineParser_InValidPredicateDefinition_NoPredicate_ReturnsInvalidSyntacticElement()
        {
            string line;
            line = "\tpredicate p > = false";
            SyntacticElement parseResult;
            parseResult = LineParser.Parse(line);
            Assert.AreEqual(parseResult.GetType(), typeof(InvalidSyntacticElement));
            InvalidSyntacticElement ise = (InvalidSyntacticElement)parseResult;
            Assert.AreEqual(ise.Title, "No predicate text found");
        }
        // no predicate definition symbol
        [TestMethod]
        public void LineParser_InValidPredicateDefinition_NoPredicateSymbol_ReturnsInvalidSyntacticElement()
        {
            string line;
            line = "\tpredicate p p = false";
            SyntacticElement parseResult;
            parseResult = LineParser.Parse(line);
            Assert.AreEqual(parseResult.GetType(), typeof(InvalidSyntacticElement));
            InvalidSyntacticElement ise = (InvalidSyntacticElement)parseResult;
            Assert.AreEqual(ise.Title, "predicate symbol not found");
        }
        // more than one value
        [TestMethod]
        public void LineParser_InValidPredicateDefinition_MoreThanOneValue_ReturnsInvalidSyntacticElement()
        {
            string line;
            line = "\tpredicate p > p = false true";
            SyntacticElement parseResult;
            parseResult = LineParser.Parse(line);
            Assert.AreEqual(parseResult.GetType(), typeof(InvalidSyntacticElement));
            InvalidSyntacticElement ise = (InvalidSyntacticElement)parseResult;
            Assert.AreEqual(ise.Title, "Equals sign not in the expected place");
        }
        // more than one equals sign
        [TestMethod]
        public void LineParser_InValidPredicateDefinition_MoreThanOneEqualsValue_ReturnsInvalidSyntacticElement()
        {
            string line;
            line = "\tpredicate p > p = true = false";
            SyntacticElement parseResult;
            parseResult = LineParser.Parse(line);
            Assert.AreEqual(parseResult.GetType(), typeof(InvalidSyntacticElement));
            InvalidSyntacticElement ise = (InvalidSyntacticElement)parseResult;
            Assert.AreEqual(ise.Title, "Equals sign not in the expected place");
        }
        // more than one equals sign
        [TestMethod]
        public void LineParser_InValidPredicateDefinition_MoreThanOneEqualsSign_ReturnsInvalidSyntacticElement()
        {
            string line;
            line = "\tpredicate p > p = = false";
            SyntacticElement parseResult;
            parseResult = LineParser.Parse(line);
            Assert.AreEqual(parseResult.GetType(), typeof(InvalidSyntacticElement));
            InvalidSyntacticElement ise = (InvalidSyntacticElement)parseResult;
            Assert.AreEqual(ise.Title, "Equals sign not in the expected place");
        }
        // invalid value
        [TestMethod]
        public void LineParser_InValidPredicateDefinition_InvalidValue_ReturnsInvalidSyntacticElement()
        {
            string line;
            line = "\tpredicate p > p = grapes";
            SyntacticElement parseResult;
            parseResult = LineParser.Parse(line);
            Assert.AreEqual(parseResult.GetType(), typeof(InvalidSyntacticElement));
            InvalidSyntacticElement ise = (InvalidSyntacticElement)parseResult;
            Assert.AreEqual(ise.Title, "Invalid value");
        }
        [TestMethod]
        public void LineParser_InValidPredicateDefinition_IntegerValue5_ReturnsInvalidSyntacticElement()
        {
            string line;
            line = "\tpredicate p > p = 5";
            SyntacticElement parseResult;
            parseResult = LineParser.Parse(line);
            Assert.AreEqual(parseResult.GetType(), typeof(InvalidSyntacticElement));
            InvalidSyntacticElement ise = (InvalidSyntacticElement)parseResult;
            Assert.AreEqual(ise.Title, "Invalid value");
        }
        [TestMethod]
        public void LineParser_ValidPredicateDefinition_IntegerValue1_ReturnsPredicate_KnownTrue()
        {
            string line;
            line = "\tpredicate p > raining in chicago = 1";
            SyntacticElement parseResult;
            parseResult = LineParser.Parse(line);
            Assert.AreEqual(parseResult.GetType(), typeof(PredicateSyntacticElement));
            Assert.AreEqual(parseResult.State, State.Known);
            Assert.AreEqual(parseResult.Value, true);
            PredicateSyntacticElement pe = (PredicateSyntacticElement)parseResult;
            Assert.AreEqual(pe.Name, "p");
            Assert.AreEqual("raining in chicago", pe.Predicate);
        }
        [TestMethod]
        public void LineParser_ValidPredicateDefinition_IntegerValue0_ReturnsPredicate_KnownFalse()
        {
            string line;
            line = "\tpredicate p > raining in chicago = 0";
            SyntacticElement parseResult;
            parseResult = LineParser.Parse(line);
            Assert.AreEqual(parseResult.GetType(), typeof(PredicateSyntacticElement));
            Assert.AreEqual(parseResult.State, State.Known);
            Assert.AreEqual(parseResult.Value, false);
            PredicateSyntacticElement pe = (PredicateSyntacticElement)parseResult;
            Assert.AreEqual(pe.Name, "p");
            Assert.AreEqual("raining in chicago", pe.Predicate);
        }
        // predicate contains a keyword
        [TestMethod]
        public void LineParser_InValidPredicateDefinition_ReservedWordAnd_ReturnsInvalidSyntacticElement()
        {
            string line;
            line = "\tpredicate p > raining in chicago and illinois = false";
            SyntacticElement parseResult;
            parseResult = LineParser.Parse(line);
            Assert.AreEqual(parseResult.GetType(), typeof(InvalidSyntacticElement));
            InvalidSyntacticElement ise = (InvalidSyntacticElement)parseResult;
            Assert.AreEqual(ise.Title, "Predicate may not contain reserved words");
        }
        [TestMethod]
        public void LineParser_InValidPredicateDefinition_ReservedWordOr_ReturnsInvalidSyntacticElement()
        {
            string line;
            line = "\tpredicate p > raining in chicago or illinois = false";
            SyntacticElement parseResult;
            parseResult = LineParser.Parse(line);
            Assert.AreEqual(parseResult.GetType(), typeof(InvalidSyntacticElement));
            InvalidSyntacticElement ise = (InvalidSyntacticElement)parseResult;
            Assert.AreEqual(ise.Title, "Predicate may not contain reserved words");
        }
        [TestMethod]
        public void LineParser_InValidPredicateDefinition_ReservedWordIf_ReturnsInvalidSyntacticElement()
        {
            string line;
            line = "\tpredicate p > raining in chicago if you are in illinois = false";
            SyntacticElement parseResult;
            parseResult = LineParser.Parse(line);
            Assert.AreEqual(parseResult.GetType(), typeof(InvalidSyntacticElement));
            InvalidSyntacticElement ise = (InvalidSyntacticElement)parseResult;
            Assert.AreEqual(ise.Title, "Predicate may not contain reserved words");
        }
        [TestMethod]
        public void LineParser_InValidPredicateDefinition_ReservedWordThen_ReturnsInvalidSyntacticElement()
        {
            string line;
            line = "\tpredicate p > raining in chicago then you are in illinois = false";
            SyntacticElement parseResult;
            parseResult = LineParser.Parse(line);
            Assert.AreEqual(parseResult.GetType(), typeof(InvalidSyntacticElement));
            InvalidSyntacticElement ise = (InvalidSyntacticElement)parseResult;
            Assert.AreEqual(ise.Title, "Predicate may not contain reserved words");
        }
        [TestMethod]
        public void LineParser_InValidPredicateDefinition_ReservedWordifandonlyif_ReturnsInvalidSyntacticElement()
        {
            string line;
            line = "\tpredicate p > raining in chicago ifandonlyif you are in illinois = false";
            SyntacticElement parseResult;
            parseResult = LineParser.Parse(line);
            Assert.AreEqual(parseResult.GetType(), typeof(InvalidSyntacticElement));
            InvalidSyntacticElement ise = (InvalidSyntacticElement)parseResult;
            Assert.AreEqual(ise.Title, "Predicate may not contain reserved words");
        }
        [TestMethod]
        public void LineParser_ValidPredicateDefinition_nospaces_ReturnsPredicate()
        {
            string line;
            line = "\tpredicate p>raining in chicago=false";
            SyntacticElement parseResult;
            parseResult = LineParser.Parse(line);
            Assert.AreEqual(parseResult.GetType(), typeof(PredicateSyntacticElement));
            Assert.AreEqual(parseResult.State, State.Known);
            Assert.AreEqual(parseResult.Value, false);
            PredicateSyntacticElement pe = (PredicateSyntacticElement)parseResult;
            Assert.AreEqual(pe.Name, "p");
            Assert.AreEqual(pe.Predicate, "raining in chicago");
        }
    }
}
