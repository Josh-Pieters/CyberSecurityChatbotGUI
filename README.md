# Cybersecurity Awareness Chatbot — Part 2
A C# WPF desktop application that expands the Part 1 console chatbot into a fully interactive graphical cybersecurity awareness assistant for South African citizens.

---

## GUI Application (Question 1)
The app launches a styled WPF window with a dark cybersecurity theme.

**On startup the app will:**
1. Play a WAV voice greeting automatically
2. Show a name entry dialog
3. Load the main chat window with your name personalised throughout

> **Note:** This is a Windows-only WPF application. `System.Media.SoundPlayer` requires Windows.

---

## Features Implemented

| Question | Feature | File |
|----------|---------|------|
| Q1 | WPF GUI with dark theme, ASCII banner, voice greeting, name dialog | `MainWindow.xaml`, `MainWindow.xaml.cs`, `NameDialog.xaml` |
| Q2 | Keyword recognition for 8 cybersecurity topics | `ResponseBank.cs`, `ChatbotEngine.cs` |
| Q3 | Random responses using `List<string>` per topic | `ResponseBank.cs` |
| Q4 | Conversation flow — follow-ups like "tell me more" | `ChatbotEngine.cs` |
| Q5 | Memory and recall — remembers favourite topic | `MemoryStore.cs` |
| Q6 | Sentiment detection — detects worried, curious, frustrated | `SentimentDetector.cs` |
| Q7 | Error handling — typo detection, input validation, fallbacks | `ErrorHandler.cs` |
| Q8 | OOP, dictionaries, lists, delegates throughout | All files |

---

## Topics the Chatbot Can Discuss
Type any of the following keywords or click the quick-topic chips in the UI:

- `password` — strong password tips
- `phishing` — how to spot phishing attacks
- `safe browsing` — browsing the internet safely
- `malware` — protecting against malicious software
- `social engineering` — recognising manipulation tactics
- `2fa` — setting up two-factor authentication
- `privacy` — protecting your personal information
- `scam` — identifying and avoiding online scams
- `help` — shows all available topics

**You can also ask:**
- `How are you?`
- `What is your purpose?`
- `What can I ask you?`

**After any response try:**
- `tell me more` — get another tip on the same topic
- `give me another tip` — same as above

---

## Sentiment Detection
The chatbot detects your mood and responds empathetically:

| Mood | Example Input | Bot Reaction |
|------|--------------|--------------|
| Worried | "I'm worried about scams" | Reassuring prefix + tip |
| Curious | "I'm curious about phishing" | Enthusiastic prefix + tip |
| Frustrated | "I'm frustrated with passwords" | Empathetic prefix + tip |
| Confused | "I don't understand 2fa" | Clear explanation prefix + tip |
| Happy | "Thanks, that's great!" | Positive prefix + tip |

---

## Memory and Recall
The chatbot remembers information you share during the conversation:

## Project Structure
```
CybersecurityChatbot/
├── App.xaml                  # WPF application entry point
├── App.xaml.cs               # App code-behind
├── MainWindow.xaml           # Main chat window UI layout
├── MainWindow.xaml.cs        # Main window events and bubble rendering
├── NameDialog.xaml           # Startup name entry dialog UI
├── NameDialog.xaml.cs        # Name dialog logic
├── ChatbotEngine.cs          # Core response logic — orchestrates all features
├── ResponseBank.cs           # All responses, delegate, random picker
├── MemoryStore.cs            # User memory and recall
├── SentimentDetector.cs      # Mood detection and empathetic prefixes
├── ErrorHandler.cs           # Input validation, typo detection, fallbacks
├── CybersecurityChatbot.csproj
└── README.md
```
## Workflow Fully working
<img width="1268" height="583" alt="image" src="https://github.com/user-attachments/assets/99f8634d-b291-46ed-99b4-b5249cf70344" />
