// Copyright (c) 2003, Paul Welter
// All rights reserved.

using System;
using NetSpell.SpellChecker;
using NetSpell.SpellChecker.Dictionary;
using NetSpell.SpellChecker.Dictionary.Phonetic;
using NetSpell.SpellChecker.Dictionary.Affix;
using Xunit;

namespace NetSpell.Tests
{
	/// <summary>
	/// Summary description for EventTest.
	/// </summary>
	public class EventTest
	{

		WordDictionary _dictionary = new WordDictionary();
		SpellingEventArgs _lastSpellingEvent;
		ReplaceWordEventArgs _lastReplaceEvent;
		EventNames _lastEvent = EventNames.None; 

		public enum EventNames
		{
			None,
			DeletedWord,
			DoubledWord,
			EndOfText,
			IgnoredWord,
			MisspelledWord,
			ReplacedWord,
		};

		public EventTest()
		{
			_dictionary.DictionaryFolder = @"..\..\..\..\dic";
			_dictionary.Initialize();
		}

		private Spelling NewSpellChecker()
		{
			Spelling _SpellChecker = new Spelling();
			_SpellChecker.Dictionary = _dictionary;
			
			_SpellChecker.DeletedWord += new Spelling.DeletedWordEventHandler(SpellChecker_DeletedWord);
			_SpellChecker.DoubledWord +=new Spelling.DoubledWordEventHandler(SpellChecker_DoubledWord);
			_SpellChecker.EndOfText +=new Spelling.EndOfTextEventHandler(SpellChecker_EndOfText);
			_SpellChecker.IgnoredWord +=new Spelling.IgnoredWordEventHandler(SpellChecker_IgnoredWord);
			_SpellChecker.MisspelledWord +=new Spelling.MisspelledWordEventHandler(SpellChecker_MisspelledWord);
			_SpellChecker.ReplacedWord +=new Spelling.ReplacedWordEventHandler(SpellChecker_ReplacedWord);

			return _SpellChecker;
		}

		private void ResetEvents()
		{
			// reset event data
			_lastSpellingEvent = null;
			_lastReplaceEvent = null;
			_lastEvent = EventNames.None;
		}
		private void SpellChecker_DeletedWord(object sender, SpellingEventArgs e)
		{
			_lastSpellingEvent = e;
			_lastEvent = EventNames.DeletedWord;
		}

		private void SpellChecker_DoubledWord(object sender, SpellingEventArgs e)
		{
			_lastSpellingEvent = e;
			_lastEvent = EventNames.DoubledWord;
		}

		private void SpellChecker_EndOfText(object sender, EventArgs e)
		{
			_lastEvent = EventNames.EndOfText;
		}

		private void SpellChecker_IgnoredWord(object sender, SpellingEventArgs e)
		{
			_lastSpellingEvent = e;
			_lastEvent = EventNames.IgnoredWord;
		}

		private void SpellChecker_MisspelledWord(object sender, SpellingEventArgs e)
		{
			_lastSpellingEvent = e;
			_lastEvent = EventNames.MisspelledWord;
		}

		private void SpellChecker_ReplacedWord(object sender, ReplaceWordEventArgs e)
		{
			_lastReplaceEvent = e;
			_lastEvent = EventNames.ReplacedWord;
		}

		[Fact]
		public void TestEvents()
		{
			Spelling _SpellChecker = NewSpellChecker();

			_SpellChecker.Text = "ths is is a tst.";
			
			ResetEvents();
			_SpellChecker.SpellCheck();
			//spelling check
			Assert.True(0 == _SpellChecker.WordIndex, "Incorrect WordOffset");
			Assert.True("ths" == _SpellChecker.CurrentWord, "Incorrect CurrentWord");
			// event check
			Assert.True(EventNames.MisspelledWord == _lastEvent, "Incorrect Event");
			Assert.NotNull(_lastSpellingEvent);//, "Event not fired");
			Assert.True(0 == _lastSpellingEvent.WordIndex, "Incorrect Event Word Index");
			Assert.True(0 == _lastSpellingEvent.TextIndex, "Incorrect Event Text Index");
			Assert.True("ths" == _lastSpellingEvent.Word, "Incorrect Event Word");
			
			ResetEvents();
			_SpellChecker.ReplaceWord("this");
			//spelling check
			Assert.True("this is is a tst." == _SpellChecker.Text, "Incorrect Text");
			// event check
			Assert.True(EventNames.ReplacedWord == _lastEvent, "Incorrect Event");
			Assert.NotNull(_lastReplaceEvent);//, "Null Event object fired");
			Assert.True(0 == _lastReplaceEvent.WordIndex, "Incorrect Event Word Index");
			Assert.True(0 == _lastReplaceEvent.TextIndex, "Incorrect Event Text Index");
			Assert.True("ths" == _lastReplaceEvent.Word, "Incorrect Event Word");
			Assert.True("this" == _lastReplaceEvent.ReplacementWord, "Incorrect Event Replacement Word");
			
			ResetEvents();
			_SpellChecker.SpellCheck();
			//spelling check
			Assert.True(2 == _SpellChecker.WordIndex, "Incorrect WordOffset");
			Assert.True("is" == _SpellChecker.CurrentWord, "Incorrect CurrentWord");
			// event check
			Assert.True(EventNames.DoubledWord == _lastEvent, "Incorrect Event");
			Assert.NotNull(_lastSpellingEvent);//, "Null Event object fired");
			Assert.True(2 == _lastSpellingEvent.WordIndex, "Incorrect Event Word Index");
			Assert.True(8 == _lastSpellingEvent.TextIndex, "Incorrect Event Text Index");
			Assert.True("is" == _lastSpellingEvent.Word, "Incorrect Event Word");
			
			ResetEvents();
			_SpellChecker.DeleteWord();
			//spelling check
			Assert.True("this is a tst." == _SpellChecker.Text, "Incorrect Text");
			// event check
			Assert.True(EventNames.DeletedWord == _lastEvent, "Incorrect Event");
			Assert.NotNull(_lastSpellingEvent);//, "Null Event object fired");
			Assert.True(2 == _lastSpellingEvent.WordIndex, "Incorrect Event Word Index");
			Assert.True(8 == _lastSpellingEvent.TextIndex, "Incorrect Event Text Index");
			Assert.True("is " == _lastSpellingEvent.Word, "Incorrect Event Word");
		
			ResetEvents();
			_SpellChecker.SpellCheck();
			//spelling check
			Assert.True(3 == _SpellChecker.WordIndex, "Incorrect WordOffset");
			Assert.True("tst" == _SpellChecker.CurrentWord, "Incorrect CurrentWord");
			// event check
			Assert.True(EventNames.MisspelledWord == _lastEvent, "Incorrect Event");
			Assert.NotNull(_lastSpellingEvent);//, "Null Event object fired");
			Assert.True(3 == _lastSpellingEvent.WordIndex, "Incorrect Event Word Index");
			Assert.True(10 == _lastSpellingEvent.TextIndex, "Incorrect Event Text Index");
			Assert.True("tst" == _lastSpellingEvent.Word, "Incorrect Event Word");
			
			ResetEvents();
			_SpellChecker.IgnoreWord();
			//spelling check
			Assert.True("this is a tst." == _SpellChecker.Text, "Incorrect Text");
			// event check
			Assert.True(EventNames.IgnoredWord == _lastEvent, "Incorrect Event");
			Assert.NotNull(_lastSpellingEvent);//, "Null Event object fired");
			Assert.True(3 == _lastSpellingEvent.WordIndex, "Incorrect Event Word Index");
			Assert.True(10 == _lastSpellingEvent.TextIndex, "Incorrect Event Text Index");
			Assert.True("tst" == _lastSpellingEvent.Word, "Incorrect Event Word");
		
			ResetEvents();
			_SpellChecker.SpellCheck();
			// event check
			Assert.True(EventNames.EndOfText == _lastEvent, "Incorrect Event");
			
		}
	}
}
