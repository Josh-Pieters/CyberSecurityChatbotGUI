using System.Collections.Generic;

namespace CyberSecurityChatbot
{
    // The two types of questions supported
    public enum QuestionType
    {
        MultipleChoice,
        TrueFalse
    }

    public class QuizQuestion
    {
        // The question text shown to the user
        public string QuestionText { get; set; } = string.Empty;

        // The list of answer options e.g. A, B, C, D or True, False
        public List<string> Options { get; set; } = new List<string>();

        // The correct answer e.g. "C" or "True"
        public string CorrectAnswer { get; set; } = string.Empty;

        // Brief explanation shown after the user answers
        public string Explanation { get; set; } = string.Empty;

        // Whether this is multiple choice or true/false
        public QuestionType Type { get; set; } = QuestionType.MultipleChoice;
    }
}