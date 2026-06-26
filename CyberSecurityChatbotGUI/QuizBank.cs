using System.Collections.Generic;

namespace CyberSecurityChatbot
{
    public static class QuizBank
    {
        // All quiz questions stored in a List
        public static List<QuizQuestion> GetAllQuestions()
        {
            List<QuizQuestion> questions = new List<QuizQuestion>();

            // ── Question 1 — Multiple Choice ─────────────────────────────────
            QuizQuestion q1 = new QuizQuestion();
            q1.QuestionText = "What should you do if you receive an email\nasking for your password?";
            q1.Type = QuestionType.MultipleChoice;
            q1.Options = new List<string> { "A) Reply with your password", "B) Delete the email", "C) Report it as phishing", "D) Ignore it" };
            q1.CorrectAnswer = "C";
            q1.Explanation = "Always report phishing emails. Reporting helps protect others from the same scam.";
            questions.Add(q1);

            // ── Question 2 — True/False ──────────────────────────────────────
            QuizQuestion q2 = new QuizQuestion();
            q2.QuestionText = "True or False:\nUsing the same password for multiple accounts is safe\nas long as it is a strong password.";
            q2.Type = QuestionType.TrueFalse;
            q2.Options = new List<string> { "A) True", "B) False" };
            q2.CorrectAnswer = "B";
            q2.Explanation = "False! If one account is breached, all accounts sharing that password are at risk.";
            questions.Add(q2);

            // ── Question 3 — Multiple Choice ─────────────────────────────────
            QuizQuestion q3 = new QuizQuestion();
            q3.QuestionText = "What does 'https://' in a website URL indicate?";
            q3.Type = QuestionType.MultipleChoice;
            q3.Options = new List<string> { "A) The site is fast", "B) The site uses encryption", "C) The site is government-owned", "D) The site is free" };
            q3.CorrectAnswer = "B";
            q3.Explanation = "HTTPS means the connection is encrypted, protecting data between you and the site.";
            questions.Add(q3);

            // ── Question 4 — True/False ──────────────────────────────────────
            QuizQuestion q4 = new QuizQuestion();
            q4.QuestionText = "True or False:\nTwo-factor authentication (2FA) makes your\naccount significantly more secure.";
            q4.Type = QuestionType.TrueFalse;
            q4.Options = new List<string> { "A) True", "B) False" };
            q4.CorrectAnswer = "A";
            q4.Explanation = "True! 2FA adds a second layer of security. Even if your password is stolen, attackers cannot log in without the second factor.";
            questions.Add(q4);

            // ── Question 5 — Multiple Choice ─────────────────────────────────
            QuizQuestion q5 = new QuizQuestion();
            q5.QuestionText = "Which of the following is the safest password?";
            q5.Type = QuestionType.MultipleChoice;
            q5.Options = new List<string> { "A) password123", "B) John1990", "C) T!g3r-L@mp-R!v3r", "D) 123456" };
            q5.CorrectAnswer = "C";
            q5.Explanation = "A strong password uses a mix of uppercase, lowercase, numbers and symbols. Avoid personal info and common words.";
            questions.Add(q5);

            // ── Question 6 — Multiple Choice ─────────────────────────────────
            QuizQuestion q6 = new QuizQuestion();
            q6.QuestionText = "What is social engineering in cybersecurity?";
            q6.Type = QuestionType.MultipleChoice;
            q6.Options = new List<string> { "A) Building social media apps", "B) Manipulating people to reveal information", "C) Engineering social networks", "D) Hacking using code only" };
            q6.CorrectAnswer = "B";
            q6.Explanation = "Social engineering uses psychological manipulation rather than technical hacking to steal information.";
            questions.Add(q6);

            // ── Question 7 — True/False ──────────────────────────────────────
            QuizQuestion q7 = new QuizQuestion();
            q7.QuestionText = "True or False:\nIt is safe to use public Wi-Fi for\nonline banking without a VPN.";
            q7.Type = QuestionType.TrueFalse;
            q7.Options = new List<string> { "A) True", "B) False" };
            q7.CorrectAnswer = "B";
            q7.Explanation = "False! Public Wi-Fi is unencrypted. Attackers can intercept your data. Always use a VPN on public networks.";
            questions.Add(q7);

            // ── Question 8 — Multiple Choice ─────────────────────────────────
            QuizQuestion q8 = new QuizQuestion();
            q8.QuestionText = "What is ransomware?";
            q8.Type = QuestionType.MultipleChoice;
            q8.Options = new List<string> { "A) Software that speeds up your PC", "B) A type of antivirus", "C) Malware that encrypts your files and demands payment", "D) A firewall application" };
            q8.CorrectAnswer = "C";
            q8.Explanation = "Ransomware encrypts your files and demands a ransom to restore access. Regular backups are your best defence.";
            questions.Add(q8);

            // ── Question 9 — True/False ──────────────────────────────────────
            QuizQuestion q9 = new QuizQuestion();
            q9.QuestionText = "True or False:\nLegitimate organisations will sometimes\nask for your password via email.";
            q9.Type = QuestionType.TrueFalse;
            q9.Options = new List<string> { "A) True", "B) False" };
            q9.CorrectAnswer = "B";
            q9.Explanation = "False! No legitimate organisation will ever ask for your password via email. This is always a phishing attempt.";
            questions.Add(q9);

            // ── Question 10 — Multiple Choice ────────────────────────────────
            QuizQuestion q10 = new QuizQuestion();
            q10.QuestionText = "Which method of 2FA is considered most secure?";
            q10.Type = QuestionType.MultipleChoice;
            q10.Options = new List<string> { "A) SMS text message", "B) Email code", "C) Authenticator app", "D) Security question" };
            q10.CorrectAnswer = "C";
            q10.Explanation = "Authenticator apps are more secure than SMS because SIM-swap attacks can intercept text messages.";
            questions.Add(q10);

            // ── Question 11 — Multiple Choice ────────────────────────────────
            QuizQuestion q11 = new QuizQuestion();
            q11.QuestionText = "What should you check before clicking\na link in an email?";
            q11.Type = QuestionType.MultipleChoice;
            q11.Options = new List<string> { "A) The font size of the email", "B) How many images the email has", "C) The actual sender email address and URL", "D) Whether the email has a signature" };
            q11.CorrectAnswer = "C";
            q11.Explanation = "Always hover over links to preview the URL and check the sender's actual email address, not just the display name.";
            questions.Add(q11);

            // ── Question 12 — True/False ─────────────────────────────────────
            QuizQuestion q12 = new QuizQuestion();
            q12.QuestionText = "True or False:\nKeeping your software and apps updated\nhelps protect against cyberattacks.";
            q12.Type = QuestionType.TrueFalse;
            q12.Options = new List<string> { "A) True", "B) False" };
            q12.CorrectAnswer = "A";
            q12.Explanation = "True! Updates often contain security patches that fix vulnerabilities attackers could exploit.";
            questions.Add(q12);

            return questions;
        }
    }
}