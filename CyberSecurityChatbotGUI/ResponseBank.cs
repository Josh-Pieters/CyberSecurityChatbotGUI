using System;
using System.Collections.Generic;

namespace CyberSecurityChatbot
{
    // This is a method that takes a string and returns a string
    public delegate string ResponseSelector(string input);

    public static class ResponseBank
    {
        // Random number generator for picking random responses later
        private static Random _rng = new Random();

        // ── Exact match responses ────────────────────────────────────────────
        // These are checked first - full question matches
        public static Dictionary<string, string> ExactResponses = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            {
                "how are you",
                "😊 I'm doing great, thank you for asking!\n" +
                "I'm always ready to help you stay safe online.\n" +
                "Is there a cybersecurity topic you'd like to learn about?"
            },
            {
                "what's your purpose",
                "🎯 My Purpose:\n" +
                "I'm CyberBot, your Cybersecurity Awareness Assistant.\n\n" +
                "I can help you with:\n" +
                "• Phishing        - Spotting and avoiding phishing scams\n" +
                "• Password Safety - Creating and managing strong passwords\n" +
                "• Safe Browsing   - How to browse the internet securely\n" +
                "• Malware         - Protecting against malicious software\n" +
                "• Social Engineering - Recognising manipulation tactics\n" +
                "• 2FA             - Setting up two-factor authentication"
            },
            {
                "what can i ask you",
                "📋 You can ask me about:\n" +
                "• Phishing          - Spot and avoid phishing attacks\n" +
                "• Password Safety   - Create and manage strong passwords\n" +
                "• Safe Browsing     - Browse the internet safely\n" +
                "• Malware           - Protect against malicious software\n" +
                "• Social Engineering - Recognise manipulation tactics\n" +
                "• 2FA               - Set up two-factor authentication\n" +
                "• Privacy           - Protect your personal information\n" +
                "• Scam              - Identify and avoid online scams\n\n" +
                "Just type any of these topics and I'll help you out!"
            }
        };

        // ── Keyword responses ────────────────────────────────────────────────
        // Each topic has a list of responses - one is picked randomly
        public static Dictionary<string, List<string>> TopicResponses = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
        {
            {
                "password", new List<string>
                {
                    "🔑 Use at least 12 characters mixing uppercase, lowercase, numbers and symbols. Never reuse passwords across sites.",
                    "🔑 Consider a passphrase — three random words joined together are both strong and memorable. E.g. 'Tiger-Lamp-River7!'",
                    "🔑 A password manager like Bitwarden or 1Password can generate and store unique passwords for every site automatically.",
                    "🔑 Change your passwords immediately if a service you use announces a data breach. Check haveibeenpwned.com regularly."
                }
            },
            {
                "phishing", new List<string>
                {
                    "🎣 Always check the sender's actual email address — scammers use addresses like 'support@paypa1.com' (note the '1').",
                    "🎣 Hover over links before clicking to preview the real URL. If it looks odd or mismatched, don't click.",
                    "🎣 Legitimate banks and organisations will NEVER ask for your password, PIN, or OTP via email or SMS.",
                    "🎣 Be cautious of urgency tactics — 'Your account will be closed in 24 hours!' is a classic phishing pressure technique."
                }
            },
            {
                "safe browsing", new List<string>
                {
                    "🌐 Always look for 'https://' and the padlock icon before entering any personal information on a website.",
                    "🌐 Avoid using public Wi-Fi for banking or sensitive activities. Use a reputable VPN if you must.",
                    "🌐 Keep your browser and its extensions up to date — updates often patch critical security vulnerabilities.",
                    "🌐 Use a privacy-focused browser extension like uBlock Origin to block malicious ads and trackers."
                }
            },
            {
                "malware", new List<string>
                {
                    "🦠 Install reputable antivirus software such as Malwarebytes or Windows Defender and keep it updated.",
                    "🦠 Never download software from unofficial or suspicious websites — stick to official sources and app stores.",
                    "🦠 Be cautious with email attachments, even from known contacts — their account may have been compromised.",
                    "🦠 Regularly back up your data to an external drive or cloud storage so ransomware cannot hold you hostage."
                }
            },
            {
                "social engineering", new List<string>
                {
                    "🎭 Attackers manipulate people psychologically. Always verify the identity of anyone requesting sensitive info.",
                    "🎭 Be sceptical of unsolicited calls claiming to be from your bank, SARS, or Microsoft — hang up and call back on official numbers.",
                    "🎭 Scammers create urgency and fear to bypass your rational thinking. Slow down and verify before acting.",
                    "🎭 Report suspicious contacts to your IT department or the South African Police Service (SAPS)."
                }
            },
            {
                "2fa", new List<string>
                {
                    "📱 2FA adds a second layer of security. Even if your password is stolen, attackers cannot log in without the second factor.",
                    "📱 Use an authenticator app like Google Authenticator or Authy rather than SMS — SIM-swap attacks can intercept SMS codes.",
                    "📱 Enable 2FA on your email first — it is the master key to all your other accounts.",
                    "📱 Never share your 2FA codes with anyone, not even someone claiming to be technical support."
                }
            },
            {
                "privacy", new List<string>
                {
                    "🔒 Review your social media privacy settings regularly — limit who can see your posts and personal details.",
                    "🔒 Avoid oversharing personal information online. Your date of birth and ID number are gold for identity thieves.",
                    "🔒 Read app permissions carefully before installing. A flashlight app does not need access to your contacts.",
                    "🔒 Use a separate email address for sign-ups and newsletters to protect your primary inbox."
                }
            },
            {
                "scam", new List<string>
                {
                    "⚠️ If an offer sounds too good to be true — a lottery you didn't enter, a Nigerian prince — it's a scam.",
                    "⚠️ Romance scammers build emotional connections over weeks before asking for money. Be cautious of online relationships that quickly turn financial.",
                    "⚠️ Never pay upfront fees to receive a prize or inheritance. Legitimate winnings do not require payment first.",
                    "⚠️ Verify charity requests before donating. Scammers exploit disasters and tragedies to solicit fake donations."
                }
            }
        };

        // ── Help response ────────────────────────────────────────────────────
        public static string HelpResponse =
            "📋 Topics I can help you with:\n" +
            "• password           - Password safety tips\n" +
            "• phishing           - Spot phishing attacks\n" +
            "• safe browsing      - Browse safely\n" +
            "• malware            - Fight malicious software\n" +
            "• social engineering - Recognise manipulation\n" +
            "• 2fa                - Two-factor authentication\n" +
            "• privacy            - Protect your personal info\n" +
            "• scam               - Identify online scams\n\n" +
            "You can also ask:\n" +
            "• 'How are you?'\n" +
            "• 'What is your purpose?'\n" +
            "• 'What can I ask you?'\n\n" +
            "After any response try: 'tell me more' or 'give me another tip'!";

        // ── Follow-up keywords ───────────────────────────────────────────────
        // If the user types any of these, give another tip on the last topic
        public static List<string> FollowUpKeywords = new List<string>
        {
            "tell me more",
            "more",
            "another tip",
            "give me another",
            "explain more",
            "go on",
            "continue",
            "what else",
            "keep going"
        };

        // ── Pick a random response for a topic ───────────────────────────────
        public static string PickRandom(string topic)
        {
            if (TopicResponses.ContainsKey(topic))
            {
                List<string> responses = TopicResponses[topic];
                int index = _rng.Next(responses.Count);
                return responses[index];
            }

            return string.Empty;
        }

        // ── Delegate-based selector ──────────────────────────────────────────
        // Returns a ResponseSelector delegate for a given topic
        // The delegate is a method that can be stored and called later
        public static ResponseSelector GetRandomPicker(string topic)
        {
            ResponseSelector selector = delegate (string input)
            {
                return PickRandom(topic);
            };

            return selector;
        }
    }
}