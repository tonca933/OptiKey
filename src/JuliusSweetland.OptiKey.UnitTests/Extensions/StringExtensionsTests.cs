﻿using System;
using System.Collections.Generic;
using JuliusSweetland.OptiKey.Extensions;
using NUnit.Framework;

namespace JuliusSweetland.OptiKey.UnitTests.Extensions
{
    [TestFixture]
    public class StringExtensionsTests
    {
        private const string NullString = null;

        [Test]
        public void TestFirstCharToUpper()
        {
            Assert.AreEqual("Optikey", "optikey".FirstCharToUpper());
            Assert.AreEqual("Optikey", "Optikey".FirstCharToUpper());
            Assert.AreEqual("OptiKey", "OptiKey".FirstCharToUpper());
            Assert.AreEqual("Sam Was Here", "sam Was Here".FirstCharToUpper());
            Assert.AreEqual("%$%@$#%#@#$&^%$&#$", "%$%@$#%#@#$&^%$&#$".FirstCharToUpper());
            Assert.AreEqual("  ", "  ".FirstCharToUpper());
            Assert.AreEqual(null, NullString.FirstCharToUpper());
        }

        [Test]
        public void TestConvertEscapedCharToLiteral()
        {
            Assert.AreEqual(@"\0", "\0".ConvertEscapedCharsToLiterals());
            Assert.AreEqual(@"\a", "\a".ConvertEscapedCharsToLiterals());
            Assert.AreEqual(@"\b", "\b".ConvertEscapedCharsToLiterals());
            Assert.AreEqual(@"\t", "\t".ConvertEscapedCharsToLiterals());
            Assert.AreEqual(@"\f", "\f".ConvertEscapedCharsToLiterals());
            Assert.AreEqual(@"\n", "\n".ConvertEscapedCharsToLiterals());
            Assert.AreEqual(@"\r", "\r".ConvertEscapedCharsToLiterals());
            Assert.AreEqual(@"s", "s".ConvertEscapedCharsToLiterals());
            Assert.AreEqual(@" ", " ".ConvertEscapedCharsToLiterals());
            Assert.AreEqual(null, NullString.ConvertEscapedCharsToLiterals());
        }
        
        [Test]
        public void TestNextCharacterWouldBeStartOfNewSentence()
        {
            Assert.AreEqual(true, NullString.NextCharacterWouldBeStartOfNewSentence());
            Assert.AreEqual(true, "".NextCharacterWouldBeStartOfNewSentence());
            Assert.AreEqual(true, "This is a sentence. ".NextCharacterWouldBeStartOfNewSentence());
            Assert.AreEqual(true, "This is a sentence! ".NextCharacterWouldBeStartOfNewSentence());
            Assert.AreEqual(true, "This is a sentence? ".NextCharacterWouldBeStartOfNewSentence());
            Assert.AreEqual(true, "This is a sentence\n".NextCharacterWouldBeStartOfNewSentence());
            Assert.AreEqual(false, "This is a sentence where i forgot my punctuation ".NextCharacterWouldBeStartOfNewSentence());
            Assert.AreEqual(false, "This is a sentence.".NextCharacterWouldBeStartOfNewSentence()); //Does not have trailing space

        }
        
        [Test]
        public void TestInProgressWord()
        {
            var sentence = "This is a sentence.";
            Assert.AreEqual(null, sentence.InProgressWord(0));
            Assert.AreEqual("This", sentence.InProgressWord(1));
            Assert.AreEqual("This", sentence.InProgressWord(2));
            Assert.AreEqual("This", sentence.InProgressWord(3));
            Assert.AreEqual("This", sentence.InProgressWord(4));
            Assert.AreEqual(null, sentence.InProgressWord(5));
            Assert.AreEqual("is", sentence.InProgressWord(6));
            Assert.AreEqual("is", sentence.InProgressWord(7));
            Assert.AreEqual(null, sentence.InProgressWord(8));
            Assert.AreEqual("a", sentence.InProgressWord(9));
            Assert.AreEqual(null, sentence.InProgressWord(10));
            Assert.AreEqual("sentence.", sentence.InProgressWord(11));
        }

        [Test]
        public void TestRemoveDiacritics()
        {
            Assert.AreEqual("aAa", "aÀå".RemoveDiacritics());
            Assert.AreEqual("CEISe", "ČĔĨŞē".RemoveDiacritics());
            Assert.AreEqual("CEISe", "ČĔĨŞē".RemoveDiacritics(false));
           
        }

        [Test]
        public void TestLongestCommonSubsequence()
        {
            Assert.AreEqual(19, "This is a sentence.".LongestCommonSubsequence("This is another sentence."));
            Assert.AreEqual(7, "cat hat".LongestCommonSubsequence("cat in the hat"));
            Assert.AreEqual(3, "cat".LongestCommonSubsequence("cat in the hat"));
            Assert.AreEqual(0, "CAT".LongestCommonSubsequence("cat in the hat"));
        }

        [Test]
        public void TestToString()
        {
            var stringCollection = new List<string>
            {
                 "This",
                 "is",
                 "a",
                 "comma",
                 "seperated",
                 "list",

            };

            var emptyStringCollection = new List<string>();
            List<string> nullStringCollection = null;
            Assert.AreEqual("This,is,a,comma,seperated,list", stringCollection.ToString(""));
            Assert.AreEqual("This,is,a,comma,seperated,list", stringCollection.ToString("Base string: "));
            Assert.AreEqual("", emptyStringCollection.ToString("EMPTY"));
            // ReSharper disable once ExpressionIsAlwaysNull
            Assert.AreEqual("EMPTY", nullStringCollection.ToString("EMPTY"));
        }

        [Test]
        public void TestExtraWordsAndLines()
        {
            List<string> finalList = new List<string>
            {
                "This",
                "is",
                "sentence",
                "This is a sentence"
            };

            Assert.AreEqual(null, "  ".ExtractWordsAndLines());
            Assert.AreEqual(null, NullString.ExtractWordsAndLines());

            var list = "This is a sentence".ExtractWordsAndLines();
            Assert.AreEqual(4, list.Count);
            CollectionAssert.AreEqual(finalList, list);

            var list2 = "This is a sentence\nThis is a sentence".ExtractWordsAndLines();
            Assert.AreEqual(4, list2.Count);
            CollectionAssert.AreEqual(finalList, list2);
        }

        [Test]
        public void TestCreateDictionaryEntryHash()
        {
            Assert.AreEqual(null, "  ".CreateDictionaryEntryHash());
            Assert.AreEqual(null, NullString.CreateDictionaryEntryHash());
            Assert.AreEqual("ENTRYWITHSPACEATEND", "EntryWithSpaceAtEnd   ".CreateDictionaryEntryHash());
            Assert.AreEqual("CEISE", "ČĔĨŞē".CreateDictionaryEntryHash());
            Assert.AreEqual("THS", "This has spaces".CreateDictionaryEntryHash());
            Assert.AreEqual("THS", "This has spaces".CreateDictionaryEntryHash(false));
            Assert.AreEqual(null, "5Cats 6dogs 7Goats".CreateDictionaryEntryHash());
            Assert.AreEqual("OIAIWC", "Optikey is awesome. I want 2 copies".CreateDictionaryEntryHash());
        }
        
        [Test]
        public void TestCreateAutoCompleteDictionaryEntryHash()
        {
            Assert.AreEqual(null, "  ".CreateAutoCompleteDictionaryEntryHash());
            Assert.AreEqual(null, NullString.CreateAutoCompleteDictionaryEntryHash());
            Assert.AreEqual("ENTRYWITHSPACEATEND", "EntryWithSpaceAtEnd   ".CreateAutoCompleteDictionaryEntryHash());
            Assert.AreEqual("CEISE", "ČĔĨŞē".CreateAutoCompleteDictionaryEntryHash());
            Assert.AreEqual("THIS HAS SPACES", "This has spaces".CreateAutoCompleteDictionaryEntryHash());
            Assert.AreEqual("THIS HAS SPACES", "This has spaces".CreateAutoCompleteDictionaryEntryHash(false));
            Assert.AreEqual("5CATS 6DOGS 7GOATS", "5Cats 6dogs 7Goats".CreateAutoCompleteDictionaryEntryHash());
            Assert.AreEqual("OPTIKEY IS AWESOME. I WANT 2 COPIES", "Optikey is awesome. I want 2 copies".CreateAutoCompleteDictionaryEntryHash());
        }

        [Test]
        public void TestCleanupPossibleDictionaryEntry()
        {
            Assert.AreEqual(null, "  ".CleanupPossibleDictionaryEntry());
            Assert.AreEqual(null, NullString.CleanupPossibleDictionaryEntry());
            Assert.AreEqual("Word", "Word!".CleanupPossibleDictionaryEntry());
            Assert.AreEqual("Cat", "Cat".CleanupPossibleDictionaryEntry());
            Assert.AreEqual("This is a sentence", "This is a sentence".CleanupPossibleDictionaryEntry());
            Assert.AreEqual("Čats", "Čats!".CleanupPossibleDictionaryEntry());
        }

        [Test]
        public void TestToCharListWithCounts()
        {
            var strings = new List<string>
            {
                "Čats",
                "Word!",
                "THIS HAS SPACES",
                "OPTIKEY IS AWESOME. I WANT 2 COPIES"
            };

          var charListWithCounts = strings.ToCharListWithCounts();
          Assert.AreEqual(charListWithCounts.Count,4);
          AssertCharTuple(charListWithCounts[0], 'C', 'Č', 1);
          AssertCharTuple(charListWithCounts[1], 'W', 'W', 1);
          AssertCharTuple(charListWithCounts[2], 'T', 'T', 1);
          AssertCharTuple(charListWithCounts[3], 'O', 'O', 1);

        }

        private void AssertCharTuple(Tuple<char,char,int> tuple, char expectedItem1, char expectedItem2, int expectedItem3)
        {
            Assert.AreEqual(expectedItem1, tuple.Item1);
            Assert.AreEqual(expectedItem2, tuple.Item2);
            Assert.AreEqual(expectedItem3, tuple.Item3);
        }
    }
}
