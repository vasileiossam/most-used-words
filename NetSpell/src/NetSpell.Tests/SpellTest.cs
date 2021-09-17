// Copyright (c) 2003, Paul Welter
// All rights reserved.

using NetSpell.SpellChecker;
using NetSpell.SpellChecker.Dictionary;
using Xunit;

namespace NetSpell.Tests
{

    /// <summary>
    ///     This is the spell checker test fixture for NUnit
    /// </summary>
	public class SpellTest
    {

        PerformanceTimer _timer = new PerformanceTimer();
        WordDictionary _dictionary = new WordDictionary();

        public SpellTest()
        {
            _dictionary.DictionaryFolder = @"..\..\..\..\dic";
            _dictionary.Initialize();
        }

        private Spelling NewSpellChecker()
        {
            Spelling _SpellChecker = new Spelling();
            _SpellChecker.Dictionary = _dictionary;

            return _SpellChecker;
        }

        /// <summary>
        ///		NUnit Test Function for DeleteWord
        /// </summary>
        [Fact]
        public void DeleteWord()
        {
            Spelling _SpellChecker = NewSpellChecker();

            _SpellChecker.Text = "this is is a tst.";
            _SpellChecker.SpellCheck();
            Assert.True(2 == _SpellChecker.WordIndex, "Incorrect WordOffset");
            Assert.True("is" == _SpellChecker.CurrentWord, "Incorrect CurrentWord");

            // basic delete test
            _SpellChecker.DeleteWord();
            Assert.True("this is a tst." == _SpellChecker.Text, "Incorrect Text");

            _SpellChecker.SpellCheck();
            Assert.True(3 == _SpellChecker.WordIndex, "Incorrect WordOffset");
            Assert.True("tst" == _SpellChecker.CurrentWord, "Incorrect CurrentWord");

            // before punctuation delete test
            _SpellChecker.DeleteWord();
            Assert.True("this is a." == _SpellChecker.Text, "Incorrect Text");


            _SpellChecker.Text = "Becuase people are realy bad spelers";
            _SpellChecker.SpellCheck();

            Assert.True(0 == _SpellChecker.WordIndex, "Incorrect WordOffset");
            Assert.True("Becuase" == _SpellChecker.CurrentWord, "Incorrect CurrentWord");

            //delete first word test
            _SpellChecker.DeleteWord();
            Assert.True("people are realy bad spelers" == _SpellChecker.Text, "Incorrect Text");

            //_SpellChecker.SpellCheck();
            //Assert.True(2 == _SpellChecker.WordIndex, "Incorrect WordOffset");
            //Assert.True("realy" == _SpellChecker.CurrentWord, "Incorrect CurrentWord");

            ////delete first word test
            //_SpellChecker.DeleteWord();
            //Assert.True("people are bad spelers" == _SpellChecker.Text, "Incorrect Text");

            //_SpellChecker.SpellCheck();
            //Assert.True(3 == _SpellChecker.WordIndex, "Incorrect WordOffset");
            //Assert.True("spelers" == _SpellChecker.CurrentWord, "Incorrect CurrentWord");

            ////delete last word test
            //_SpellChecker.DeleteWord();
            //Assert.True("people are bad" == _SpellChecker.Text, "Incorrect Text");


        }

        /// <summary>
        ///		NUnit Test Function for DeleteWord
        /// </summary>
        [Fact]
        public void NoText()
        {
            Spelling _SpellChecker = NewSpellChecker();

            Assert.True(string.Empty == _SpellChecker.CurrentWord, "Incorrect Current Word");

            _SpellChecker.WordIndex = 1;
            Assert.True(0 == _SpellChecker.WordIndex, "Incorrect Word Index");

            Assert.True(0 == _SpellChecker.WordCount, "Incorrect Word Count");

            Assert.True(0 == _SpellChecker.TextIndex, "Incorrect Text Index");

            _SpellChecker.DeleteWord();
            Assert.True(string.Empty == _SpellChecker.Text, "Incorrect Text");

            _SpellChecker.IgnoreWord();
            Assert.True(string.Empty == _SpellChecker.Text, "Incorrect Text");

            _SpellChecker.ReplaceWord("Test");
            Assert.True(string.Empty == _SpellChecker.Text, "Incorrect Text");

            Assert.False(_SpellChecker.SpellCheck(), "Spell Check not false");

            _SpellChecker.Suggest();
            Assert.True(0 == _SpellChecker.Suggestions.Count, "Generated Suggestions with no text");

        }

        /// <summary>
        ///		NUnit Test Function for IgnoreWord
        /// </summary>
        [Fact]
        public void IgnoreWord()
        {
            Spelling _SpellChecker = NewSpellChecker();

            _SpellChecker.Text = "this is an errr tst";

            _SpellChecker.SpellCheck();
            Assert.True(3 == _SpellChecker.WordIndex, "Incorrect WordOffset");
            Assert.True("errr" == _SpellChecker.CurrentWord, "Incorrect CurrentWord");
            _SpellChecker.IgnoreWord();

            _SpellChecker.SpellCheck();
            Assert.True(4 == _SpellChecker.WordIndex, "Incorrect WordOffset");
            Assert.True("tst" == _SpellChecker.CurrentWord, "Incorrect CurrentWord");

        }

        /// <summary>
        ///		NUnit Test Function for IgnoreWord
        /// </summary>
        [Fact]
        public void IgnoreAllWord()
        {
            Spelling _SpellChecker = NewSpellChecker();

            _SpellChecker.Text = "this is a tst of a tst errr";

            _SpellChecker.SpellCheck();
            Assert.True(3 == _SpellChecker.WordIndex, "Incorrect WordOffset");
            Assert.True("tst" == _SpellChecker.CurrentWord, "Incorrect CurrentWord");
            _SpellChecker.IgnoreAllWord();

            _SpellChecker.SpellCheck();
            Assert.True(7 == _SpellChecker.WordIndex, "Incorrect WordOffset");
            Assert.True("errr" == _SpellChecker.CurrentWord, "Incorrect CurrentWord");

        }

        /// <summary>
        ///		NUnit Test Function for ReplaceWord
        /// </summary>
        [Fact]
        public void ReplaceWord()
        {
            Spelling _SpellChecker = NewSpellChecker();

            _SpellChecker.Text = "ths is an errr tst";

            _SpellChecker.SpellCheck();
            Assert.True(0 == _SpellChecker.WordIndex, "Incorrect WordOffset");
            Assert.True("ths" == _SpellChecker.CurrentWord, "Incorrect CurrentWord");
            _SpellChecker.ReplacementWord = "this";
            _SpellChecker.ReplaceWord();
            Assert.True("this is an errr tst" == _SpellChecker.Text, "Incorrect Text");

            //replace with empty string
            _SpellChecker.SpellCheck();
            Assert.True(3 == _SpellChecker.WordIndex, "Incorrect WordOffset");
            Assert.True("errr" == _SpellChecker.CurrentWord, "Incorrect CurrentWord");
            _SpellChecker.ReplaceWord("");
            Assert.True("this is an tst" == _SpellChecker.Text, "Incorrect Text");


            _SpellChecker.Text = "Becuase people are realy bad spelers, \r\nths produc was desinged to prevent spelling errors in a text area like ths.";

            _SpellChecker.SpellCheck();
            Assert.True(0 == _SpellChecker.WordIndex, "Incorrect WordOffset");
            Assert.True("Becuase" == _SpellChecker.CurrentWord, "Incorrect CurrentWord");
            _SpellChecker.ReplaceWord("because");
            Assert.True("Because people are realy bad spelers, \r\nths produc was desinged to prevent spelling errors in a text area like ths." == _SpellChecker.Text, "Incorrect Text");

            //_SpellChecker.SpellCheck();
            //Assert.True(3 == _SpellChecker.WordIndex, "Incorrect WordOffset");
            //Assert.True("realy" == _SpellChecker.CurrentWord, "Incorrect CurrentWord");
            //_SpellChecker.ReplaceWord("really");
            //Assert.True("Because people are really bad spelers, \r\nths produc was desinged to prevent spelling errors in a text area like ths." == _SpellChecker.Text, "Incorrect Text");

            //_SpellChecker.SpellCheck();
            //Assert.True(5 == _SpellChecker.WordIndex, "Incorrect WordOffset");
            //Assert.True("spelers" == _SpellChecker.CurrentWord, "Incorrect CurrentWord");
            //_SpellChecker.ReplaceWord("spellers");
            //Assert.True("Because people are really bad spellers, \r\nths produc was desinged to prevent spelling errors in a text area like ths." == _SpellChecker.Text, "Incorrect Text");

            //_SpellChecker.SpellCheck();
            //Assert.True(6 == _SpellChecker.WordIndex, "Incorrect WordOffset");
            //Assert.True("ths" == _SpellChecker.CurrentWord, "Incorrect CurrentWord");
            //_SpellChecker.ReplaceWord("this");
            //Assert.True("Because people are really bad spellers, \r\nthis produc was desinged to prevent spelling errors in a text area like ths." == _SpellChecker.Text, "Incorrect Text");

            //_SpellChecker.SpellCheck();
            //Assert.True(7 == _SpellChecker.WordIndex, "Incorrect WordOffset");
            //Assert.True("produc" == _SpellChecker.CurrentWord, "Incorrect CurrentWord");
            //_SpellChecker.ReplaceWord("product");
            //Assert.True("Because people are really bad spellers, \r\nthis product was desinged to prevent spelling errors in a text area like ths." == _SpellChecker.Text, "Incorrect Text");

            //_SpellChecker.SpellCheck();
            //Assert.True(9 == _SpellChecker.WordIndex, "Incorrect WordOffset");
            //Assert.True("desinged" == _SpellChecker.CurrentWord, "Incorrect CurrentWord");
            //_SpellChecker.ReplaceWord("designed");
            //Assert.True("Because people are really bad spellers, \r\nthis product was designed to prevent spelling errors in a text area like ths." == _SpellChecker.Text, "Incorrect Text");

            //_SpellChecker.SpellCheck();
            //Assert.True(19 == _SpellChecker.WordIndex, "Incorrect WordOffset");
            //Assert.True("ths" == _SpellChecker.CurrentWord, "Incorrect CurrentWord");
            //_SpellChecker.ReplaceWord("this");
            //Assert.True("Because people are really bad spellers, \r\nthis product was designed to prevent spelling errors in a text area like this." == _SpellChecker.Text, "Incorrect Text");

        }

        /// <summary>
        ///		NUnit Test Function for ReplaceWord
        /// </summary>
        [Fact]
        public void ReplaceAllWord()
        {
            Spelling _SpellChecker = NewSpellChecker();

            _SpellChecker.Text = "this is a tst of a tst errr";
            _SpellChecker.IgnoreList.Clear();
            _SpellChecker.ReplaceList.Clear();

            _SpellChecker.SpellCheck();
            Assert.True(3 == _SpellChecker.WordIndex, "Incorrect WordOffset");
            Assert.True("tst" == _SpellChecker.CurrentWord, "Incorrect CurrentWord");
            _SpellChecker.ReplaceAllWord("test");
            Assert.True("this is a test of a tst errr" == _SpellChecker.Text, "Incorrect Text");

            _SpellChecker.SpellCheck();
            Assert.True(7 == _SpellChecker.WordIndex, "Incorrect WordOffset");
            Assert.True("errr" == _SpellChecker.CurrentWord, "Incorrect CurrentWord");
            Assert.True("this is a test of a test errr" == _SpellChecker.Text, "Incorrect Text");


        }

        /// <summary>
        ///		NUnit Test Function for SpellCheck
        /// </summary>
        [Fact]
        public void SpellCheck()
        {
            Spelling _SpellChecker = NewSpellChecker();

            _SpellChecker.Text = "this is an errr tst";

            _SpellChecker.SpellCheck();
            Assert.True(3 == _SpellChecker.WordIndex, "Incorrect WordOffset");
            Assert.True("errr" == _SpellChecker.CurrentWord, "Incorrect CurrentWord");

        }

        /// <summary>
        ///		NUnit Test Function for SpellCheck
        /// </summary>
        [Fact]
        public void HtmlSpellCheck()
        {
            Spelling _SpellChecker = NewSpellChecker();

            _SpellChecker.IgnoreHtml = true;
            _SpellChecker.Text = "<a href=\"#\">this <span id=\"txt\">is</span> an errr tst</a>";

            _SpellChecker.SpellCheck();
            Assert.True(9 == _SpellChecker.WordIndex, "Incorrect WordOffset");
            Assert.True("errr" == _SpellChecker.CurrentWord, "Incorrect CurrentWord");

        }


        /// <summary>
        ///		NUnit Test Function for Suggest
        /// </summary>
        [Fact]
        public void Suggest()
        {
            Spelling _SpellChecker = NewSpellChecker();

            _SpellChecker.Text = "this is tst";
            _SpellChecker.SpellCheck();
            Assert.True(2 == _SpellChecker.WordIndex, "Incorrect WordOffset");
            Assert.True("tst" == _SpellChecker.CurrentWord, "Incorrect CurrentWord");

            _SpellChecker.Suggest();
            Assert.True(25 == _SpellChecker.Suggestions.Count, "Incorrect Suggestion Count");
            Assert.True(true == _SpellChecker.Suggestions.Contains("test"), "Could not find 'test' in suggestions");

        }

        /// <summary>
        ///		NUnit Test Function for TestWord
        /// </summary>
        [Fact]
        public void TestWord()
        {
            Spelling _SpellChecker = NewSpellChecker();

            Assert.True(_SpellChecker.TestWord("test"), "Did not find test word");
            Assert.False(_SpellChecker.TestWord("tst"), "Found tst word and shouldn't have");

        }

        /// <summary>
        ///		NUnit Test Function for WordSimilarity
        /// </summary>
        [Fact]
        public void EditDistance()
        {
            Spelling _SpellChecker = NewSpellChecker();

            Assert.True(1 == _SpellChecker.EditDistance("test", "tst"), "Incorrect EditDistance");
            Assert.True(2 == _SpellChecker.EditDistance("test", "tes"), "Incorrect EditDistance");
            Assert.True(0 == _SpellChecker.EditDistance("test", "test"), "Incorrect EditDistance");

        }


        [Fact]
        public void GetWordIndexFromTextIndex()
        {
            Spelling _SpellChecker = NewSpellChecker();

            _SpellChecker.Text = "This is a test ";
            Assert.True(0 == _SpellChecker.GetWordIndexFromTextIndex(1));
            Assert.True(0 == _SpellChecker.GetWordIndexFromTextIndex(4));
            Assert.True(1 == _SpellChecker.GetWordIndexFromTextIndex(5));
            Assert.True(2 == _SpellChecker.GetWordIndexFromTextIndex(9));
            Assert.True(3 == _SpellChecker.GetWordIndexFromTextIndex(12));
            Assert.True(3 == _SpellChecker.GetWordIndexFromTextIndex(15));
            Assert.True(3 == _SpellChecker.GetWordIndexFromTextIndex(20));
        }

    }
}
