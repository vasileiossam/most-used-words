// Copyright (c) 2003, Paul Welter
// All rights reserved.

using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Text;
using NetSpell.SpellChecker.Dictionary;
using Xunit;

namespace NetSpell.Tests
{
    /// <summary>
    /// Summary description for DictionaryTest.
    /// </summary>
    public class DictionaryTest
	{
		WordDictionary _WordDictionary = new WordDictionary();
		PerformanceTimer _timer = new PerformanceTimer();


		public DictionaryTest()
		{
			_WordDictionary.DictionaryFolder = @"..\..\..\..\dic";
			_WordDictionary.Initialize();
		}

		[Fact]
		public void Contains() 
		{
			var validFile = Path.Combine(
				Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Data", "ValidWords.txt");
			var invalidFile = Path.Combine(
				Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Data", "SuggestionTest.txt");

			// open file
			FileStream fs = new FileStream(validFile, FileMode.Open, FileAccess.Read, FileShare.Read);
			StreamReader sr = new StreamReader(fs, Encoding.UTF7);
			
			_timer.StartTimer();

			// read line by line
			while (sr.Peek() >= 0) 
			{
				string tempLine = sr.ReadLine().Trim();
				if (tempLine.Length > 0)
				{
					if (tempLine.IndexOf(' ') > 0)
					{
						tempLine = tempLine.Substring(0, tempLine.IndexOf(' '));
					}

					if (!_WordDictionary.Contains(tempLine)) 
					{
						Console.WriteLine(string.Format("Did not find word: {0}" , tempLine));
					}
				}
			}
			float checkTime = _timer.StopTimer();
			Console.WriteLine("Valid words check time:" + checkTime.ToString());

			sr.Close();
			fs.Close();

			
			// open file
			fs = new FileStream(invalidFile, FileMode.Open, FileAccess.Read, FileShare.Read);
			sr = new StreamReader(fs, Encoding.UTF7);
			
			_timer.StartTimer();
			// read line by line
			while (sr.Peek() >= 0) 
			{
				string tempLine = sr.ReadLine().Trim();
				if (tempLine.Length > 0)
				{
					if (tempLine.IndexOf(' ') > 0)
					{
						tempLine = tempLine.Substring(0, tempLine.IndexOf(' '));
					}

					if (_WordDictionary.Contains(tempLine)) 
					{
						Console.WriteLine(string.Format("Word found that should not be: {0}" , tempLine));
					}
				}

			}
			float invalidTime = _timer.StopTimer();
			Console.WriteLine("Invalid words check time:" + invalidTime.ToString());
			
			sr.Close();
			fs.Close();
		}

		[Fact]
		public void PhoneticCode()
		{
			string code = _WordDictionary.PhoneticCode("test");

			Assert.True("*BRFTT" == _WordDictionary.PhoneticCode("abbreviated"), "Incorrect Phonitic Code");
			Assert.True("*BLT" == _WordDictionary.PhoneticCode("ability"), "Incorrect Phonitic Code");
			Assert.True("NMNT" == _WordDictionary.PhoneticCode("nominate"), "Incorrect Phonitic Code");
			Assert.True("NN" == _WordDictionary.PhoneticCode("noun"), "Incorrect Phonitic Code");
			Assert.True("*BKKT" == _WordDictionary.PhoneticCode("object"), "Incorrect Phonitic Code");
			Assert.True("*TKR" == _WordDictionary.PhoneticCode("outgrow"), "Incorrect Phonitic Code");
			Assert.True("*TLNTX" == _WordDictionary.PhoneticCode("outlandish"), "Incorrect Phonitic Code");
			Assert.True("PBLX" == _WordDictionary.PhoneticCode("publish"), "Incorrect Phonitic Code");
			Assert.True("STL" == _WordDictionary.PhoneticCode("sightly"), "Incorrect Phonitic Code");
			Assert.True("SPL" == _WordDictionary.PhoneticCode("supple"), "Incorrect Phonitic Code");
			Assert.True("TRTNS" == _WordDictionary.PhoneticCode("triteness"), "Incorrect Phonitic Code");

		}

		[Fact]
		public void ExpandWord()
		{
			
			ArrayList words = new ArrayList();

			words = _WordDictionary.ExpandWord(new Word("abbreviated", "UA"));
			Assert.True(3 == words.Count, "Incorrect Number of expanded words");

            words = _WordDictionary.ExpandWord(new Word("ability", "IMES"));
            Assert.True(9 == words.Count, "Incorrect Number of expanded words");

            words = _WordDictionary.ExpandWord(new Word("nominate", "CDSAXNG"));
            Assert.True(18 == words.Count, "Incorrect Number of expanded words");

            words = _WordDictionary.ExpandWord(new Word("noun", "SMK"));
            Assert.True(6 == words.Count, "Incorrect Number of expanded words");

            words = _WordDictionary.ExpandWord(new Word("object", "SGVMD"));
            Assert.True(6 == words.Count, "Incorrect Number of expanded words");

            words = _WordDictionary.ExpandWord(new Word("outgrow", "GSH"));
            Assert.True(4 == words.Count, "Incorrect Number of expanded words");

            words = _WordDictionary.ExpandWord(new Word("outlandish", "PY"));
            Assert.True( 3 == words.Count, "Incorrect Number of expanded words");

            words = _WordDictionary.ExpandWord(new Word("publish", "JDRSBZG"));
            Assert.True(8 == words.Count, "Incorrect Number of expanded words");

            words = _WordDictionary.ExpandWord(new Word("sightly", "TURP"));
            Assert.True(8 == words.Count, "Incorrect Number of expanded words");

            words = _WordDictionary.ExpandWord(new Word("supple", "SPLY"));
            Assert.True(5 == words.Count, "Incorrect Number of expanded words");

            words = _WordDictionary.ExpandWord(new Word("triteness", "SF"));
            Assert.True(4 == words.Count, "Incorrect Number of expanded words");

        }

	}
}
